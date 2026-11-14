using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Web;

namespace ExternalConnectors.OP;

public class OnePasswordCliException(string message, string arguments) : Exception(message)
{
	public string Arguments { get; set; } = arguments;
}

public class OnePasswordCli
{
	private const string OnePasswordCliExecutable = "op.exe";

	// A last resort if op never finishes. op gives up by itself on an authorization nobody answers, after
	// about two minutes (#3628), and its own message says more than this timeout would.
	private const int CommandTimeoutMilliseconds = 180_000;

	// Reads run one at a time, so that connections opened together do not start op in parallel.
	private static readonly SemaphoreSlim AsyncReads = new(1, 1);

	// Username / password purpose metadata is used on Login category item fields
	private const string UserNamePurpose = "USERNAME";
	private const string PasswordPurpose = "PASSWORD";
	
	// Server category items (and perhaps others) do have a built-in username/password field but don't have the `purpose` set
	// and because it's a built-in field this can't be set afterwards.
	// We use the label for as fallback because that can be user-modified to fit this convention in all cases.
	private const string UserNameLabel = "username";
	private const string PasswordLabel = "password";
	
	
	private const string StringType = "STRING";
	private const string SshKeyType = "SSHKEY";
	private const string DomainLabel = "domain";

	private record VaultUrl(string Label, string Href);

	private record VaultField(string Id, string Label, string Type, string Purpose, string Value);

	private record VaultItem(VaultUrl[]? Urls, VaultField[]? Fields);

	private static readonly JsonSerializerOptions JsonSerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	public static void ReadPassword(string input, out string username, out string password, out string domain, out string privateKey)
	{
		var inputUrl = new Uri(input);
		var vault = WebUtility.UrlDecode(inputUrl.Host);
		var queryParams = HttpUtility.ParseQueryString(inputUrl.Query);
		var account = queryParams["account"];
		var item = WebUtility.UrlDecode(inputUrl.AbsolutePath.TrimStart('/'));
		ItemGet(item, vault, account, out username, out password, out domain, out privateKey);
	}

	/// <summary>
	/// Reads <paramref name="input"/> like <see cref="ReadPassword(string, out string, out string, out string, out string)"/>,
	/// on a thread pool thread, so that the caller's thread is free while op waits, for instance for the
	/// 1Password authorization prompt, which stays blank while the UI thread of mRemoteNG is blocked (#3628).
	/// It does not throw: what ReadPassword throws is kept in the result, and thrown when the result is used.
	/// </summary>
	public static async Task<OnePasswordRead> ReadPasswordAsync(string input)
	{
		await AsyncReads.WaitAsync().ConfigureAwait(false);
		try
		{
			return await Task.Run(() => new OnePasswordRead(input)).ConfigureAwait(false);
		}
		finally
		{
			AsyncReads.Release();
		}
	}

	/// <summary>
	/// Reads <paramref name="input"/> like <see cref="ReadPassword(string, out string, out string, out string, out string)"/>,
	/// unless there is a <paramref name="readAhead"/>, which is used even if the reference was changed while it was read:
	/// the connection keeps the reference it had when it was opened, and reading again here would run op on the UI
	/// thread (#3628).
	/// </summary>
	public static void ReadPassword(OnePasswordRead? readAhead, string input, ref string username, ref string password, ref string domain, ref string privateKey)
	{
		if (readAhead != null)
			readAhead.Apply(ref username, ref password, ref domain, ref privateKey);
		else
			ReadPassword(input, out username, out password, out domain, out privateKey);
	}

	private static void ItemGet(string item, string? vault, string? account, out string username, out string password, out string domain, out string privateKey)
    {
        var args = new List<string> { "item", "get", item };

        if (!string.IsNullOrEmpty(account))
        {
            args.Add("--account");
            args.Add(account);
        }

        if (!string.IsNullOrEmpty(vault))
        {
            args.Add("--vault");
            args.Add(vault);
        }

        args.Add("--format");
        args.Add("json");

		string commandLine = OnePasswordCliExecutable + " " + string.Join(' ', args);
            
        var exitCode = RunCommand(OnePasswordCliExecutable, args, out var output, out var error);
        if (exitCode != 0)
        {
            username = string.Empty;
            password = string.Empty;
            privateKey = string.Empty;
            domain = string.Empty;
            throw new OnePasswordCliException($"Error running op item get: {error}",
                commandLine);
        }

        var items = JsonSerializer.Deserialize<VaultItem>(output, JsonSerializerOptions) ??
                    throw new OnePasswordCliException("1Password returned null",
                        commandLine);
        username = FindField(items, UserNamePurpose, UserNameLabel);
        password = FindField(items, PasswordPurpose, PasswordLabel);
        privateKey = items.Fields?.FirstOrDefault(x => x.Type == SshKeyType)?.Value ?? string.Empty;
        domain = items.Fields?.FirstOrDefault(x => x.Type == StringType && x.Label == DomainLabel)?.Value ?? string.Empty;
		if(string.IsNullOrEmpty(password) && string.IsNullOrEmpty(privateKey))
		{
			throw new OnePasswordCliException("No secret found in 1Password. At least fields with labels username/password or a SshKey are expected.", commandLine);
		}
    }

    private static string FindField(VaultItem items, string purpose, string fallbackLabel)
    {
        return items.Fields?.FirstOrDefault(x => x.Purpose == purpose)?.Value ??
			items.Fields?.FirstOrDefault(x => x.Type == StringType && string.Equals(x.Id, fallbackLabel, StringComparison.InvariantCultureIgnoreCase))?.Value ??
		 	items.Fields?.FirstOrDefault(x => x.Type == StringType && string.Equals(x.Label, fallbackLabel, StringComparison.InvariantCultureIgnoreCase))?.Value ??
		 	string.Empty;
    }

    internal static int RunCommand(string command, IReadOnlyCollection<string> arguments, out string output,
		out string error, int timeoutMilliseconds = CommandTimeoutMilliseconds)
	{
		var processStartInfo = new ProcessStartInfo
		{
			FileName = command,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};

		foreach (var argument in arguments)
		{
			processStartInfo.ArgumentList.Add(argument);
		}

		using var process = new Process();
		process.StartInfo = processStartInfo;
		process.Start();
		// Both pipes are read at once: reading one to the end first hangs if op fills the other one meanwhile.
		Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
		Task<string> errorTask = process.StandardError.ReadToEndAsync();
		if (!process.WaitForExit(timeoutMilliseconds))
		{
			process.Kill(entireProcessTree: true);
			throw new OnePasswordCliException($"{command} did not finish within {timeoutMilliseconds / 1000} seconds and was stopped",
				command + " " + string.Join(' ', arguments));
		}
		output = outputTask.GetAwaiter().GetResult();
		error = errorTask.GetAwaiter().GetResult();
		return process.ExitCode;
	}
}