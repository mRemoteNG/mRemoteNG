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

    private static int RunCommand(string command, IReadOnlyCollection<string> arguments, out string output,
		out string error)
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
		output = process.StandardOutput.ReadToEnd();
		error = process.StandardError.ReadToEnd();
		process.WaitForExit();
		return process.ExitCode;
	}
}