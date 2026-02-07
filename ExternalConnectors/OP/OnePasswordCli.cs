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
	private const string OnePasswordScheme = "op://";

	// Username / password purpose metadata is used on Login category item fields
	private const string UserNamePurpose = "USERNAME";
	private const string PasswordPurpose = "PASSWORD";
	
	// Server category items (and perhaps others) do have a built-in username/password field but don't have the `purpose` set
	// and because it's a built-in field this can't be set afterwards.
	// We use the label for as fallback because that can be user-modified to fit this convention in all cases.
	private const string UserNameLabel = "username";
	private const string PasswordLabel = "password";
	
	
	private const string StringType = "STRING";
	private const string ConcealedType = "CONCEALED";
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
		var (item, vault, account) = ParseSecretReference(input);
		ItemGet(item, vault, account, out username, out password, out domain, out privateKey);
	}

	private static (string Item, string? Vault, string? Account) ParseSecretReference(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			throw new OnePasswordCliException("1Password secret reference is empty.", input);
		}

		string normalizedInput = input.Trim();
		if (!normalizedInput.StartsWith(OnePasswordScheme, StringComparison.OrdinalIgnoreCase))
		{
			throw new OnePasswordCliException($"Invalid 1Password secret reference. Expected format {OnePasswordScheme}vault/item.", input);
		}

		string secret = normalizedInput[OnePasswordScheme.Length..];
		int querySeparator = secret.IndexOf('?', StringComparison.Ordinal);
		string pathPart = querySeparator >= 0 ? secret[..querySeparator] : secret;
		string queryPart = querySeparator >= 0 ? secret[(querySeparator + 1)..] : string.Empty;

		string? account = null;
		if (!string.IsNullOrEmpty(queryPart))
		{
			var queryParams = HttpUtility.ParseQueryString(queryPart);
			account = queryParams["account"];
		}

		string vaultPart = string.Empty;
		string itemPart;

		if (pathPart.StartsWith("/", StringComparison.Ordinal))
		{
			itemPart = pathPart.TrimStart('/');
		}
		else
		{
			int firstSlash = pathPart.IndexOf('/', StringComparison.Ordinal);
			if (firstSlash <= 0 || firstSlash == pathPart.Length - 1)
			{
				throw new OnePasswordCliException($"Invalid 1Password secret reference. Expected format {OnePasswordScheme}vault/item.", input);
			}

			vaultPart = pathPart[..firstSlash];
			itemPart = pathPart[(firstSlash + 1)..];
		}

		string item = WebUtility.UrlDecode(itemPart);
		if (string.IsNullOrWhiteSpace(item))
		{
			throw new OnePasswordCliException("1Password item is missing in secret reference.", input);
		}

		string? vault = string.IsNullOrEmpty(vaultPart) ? null : WebUtility.UrlDecode(vaultPart);
		return (item, vault, account);
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

		(username, password, domain, privateKey) = ExtractCredentialsFromJson(output, commandLine);
    }

	private static (string Username, string Password, string Domain, string PrivateKey) ExtractCredentialsFromJson(
		string output, string commandLine)
	{
		var items = JsonSerializer.Deserialize<VaultItem>(output, JsonSerializerOptions) ??
		            throw new OnePasswordCliException("1Password returned null",
			            commandLine);

		string username = FindField(items, UserNamePurpose, UserNameLabel);
		string password = FindField(items, PasswordPurpose, PasswordLabel);
		string privateKey = items.Fields?.FirstOrDefault(x =>
			string.Equals(x.Type, SshKeyType, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
		string domain = items.Fields?.FirstOrDefault(x =>
			string.Equals(x.Type, StringType, StringComparison.OrdinalIgnoreCase) &&
			string.Equals(x.Label, DomainLabel, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;

		if (string.IsNullOrEmpty(password) && string.IsNullOrEmpty(privateKey))
		{
			throw new OnePasswordCliException("No secret found in 1Password. At least fields with labels username/password or a SshKey are expected.", commandLine);
		}

		return (username, password, domain, privateKey);
	}

    private static string FindField(VaultItem items, string purpose, string fallbackLabel)
    {
	    return items.Fields?.FirstOrDefault(x =>
			string.Equals(x.Purpose, purpose, StringComparison.OrdinalIgnoreCase) &&
			!string.IsNullOrEmpty(x.Value))?.Value ??
			items.Fields?.FirstOrDefault(x =>
				SupportsFallbackType(x.Type) &&
				string.Equals(x.Id, fallbackLabel, StringComparison.InvariantCultureIgnoreCase) &&
				!string.IsNullOrEmpty(x.Value))?.Value ??
			items.Fields?.FirstOrDefault(x =>
				SupportsFallbackType(x.Type) &&
				string.Equals(x.Label, fallbackLabel, StringComparison.InvariantCultureIgnoreCase) &&
				!string.IsNullOrEmpty(x.Value))?.Value ??
			string.Empty;
    }

	private static bool SupportsFallbackType(string type)
	{
		return string.Equals(type, StringType, StringComparison.OrdinalIgnoreCase) ||
		       string.Equals(type, ConcealedType, StringComparison.OrdinalIgnoreCase);
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
