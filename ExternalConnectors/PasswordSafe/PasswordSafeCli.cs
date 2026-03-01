using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Web;

namespace ExternalConnectors.PasswordSafe;

public class PasswordSafeCliException(string message, string arguments) : Exception(message)
{
    public string Arguments { get; set; } = arguments;
}

public class PasswordSafeCli
{
    private const string PwSafeCliExecutable = "pwsafe-cli.exe"; // NOSONAR — S2068 false positive: CLI executable name, not a credential
    private const string PwSafeScheme = "pwsafe://"; // NOSONAR — S2068 false positive: URI scheme, not a credential

    public static void ReadPassword(string input, out string username, out string password, out string domain, out string privateKey)
    {
        var (dbPath, group, title, dbUser) = ParseSecretReference(input);
        
        // We will execute pwsafe-cli to get the password
        // Assumed syntax: pwsafe-cli --file <db> --group <group> --title <title> --show-password
        
        var args = new List<string>();
        
        if (!string.IsNullOrEmpty(dbPath))
        {
            args.Add("--file");
            args.Add(dbPath);
        }
        
        if (!string.IsNullOrEmpty(group))
        {
            args.Add("--group");
            args.Add(group);
        }
        
        if (!string.IsNullOrEmpty(title))
        {
            args.Add("--title");
            args.Add(title);
        }

        if (!string.IsNullOrEmpty(dbUser))
        {
             args.Add("--user"); 
             args.Add(dbUser);
        }
        
        args.Add("--show-password"); // Request password output to stdout

        string commandLine = PwSafeCliExecutable + " " + string.Join(' ', args);
            
        var exitCode = RunCommand(PwSafeCliExecutable, args, out var output, out var error);
        
        if (exitCode != 0)
        {
            username = string.Empty;
            password = string.Empty;
            privateKey = string.Empty;
            domain = string.Empty;
            throw new PasswordSafeCliException($"Error running pwsafe-cli: {error}", commandLine);
        }

        output = output.Trim();
        password = output;
        username = dbUser ?? string.Empty;
        domain = string.Empty;
        privateKey = string.Empty;

        // Attempt to parse JSON if the output looks like JSON
        if (output.StartsWith("{") && output.EndsWith("}"))
        {
             try
             {
                 var json = JsonSerializer.Deserialize<Dictionary<string, string>>(output);
                 if (json != null)
                 {
                     if (json.TryGetValue("password", out var pwd)) password = pwd;
                     if (json.TryGetValue("username", out var usr)) username = usr;
                     if (json.TryGetValue("domain", out var dom)) domain = dom;
                     if (json.TryGetValue("privateKey", out var pk)) privateKey = pk;
                 }
             }
             catch
             {
                 // Ignore JSON error, treat as raw password
             }
        }
    }

    private static (string DbPath, string Group, string Title, string? DbUser) ParseSecretReference(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new PasswordSafeCliException("PasswordSafe secret reference is empty.", input);
        }

        string normalizedInput = input.Trim();
        if (!normalizedInput.StartsWith(PwSafeScheme, StringComparison.OrdinalIgnoreCase))
        {
             throw new PasswordSafeCliException($"Invalid PasswordSafe secret reference. Expected format {PwSafeScheme}path/to/db?group=...&title=...", input);
        }

        string secret = normalizedInput[PwSafeScheme.Length..];
        int querySeparator = secret.IndexOf('?', StringComparison.Ordinal);
        string pathPart = querySeparator >= 0 ? secret[..querySeparator] : secret;
        string queryPart = querySeparator >= 0 ? secret[(querySeparator + 1)..] : string.Empty;

        string dbPath = WebUtility.UrlDecode(pathPart);

        string group = string.Empty;
        string title = string.Empty;
        string? user = null;

        if (!string.IsNullOrEmpty(queryPart))
        {
            var queryParams = HttpUtility.ParseQueryString(queryPart);
            group = queryParams["group"] ?? string.Empty;
            title = queryParams["title"] ?? string.Empty;
            user = queryParams["username"];
        }

        if (string.IsNullOrEmpty(group) && string.IsNullOrEmpty(title))
        {
             throw new PasswordSafeCliException("PasswordSafe group or title is missing in secret reference.", input);
        }

        return (dbPath, group, title, user);
    }

    private static int RunCommand(string command, IReadOnlyCollection<string> arguments, out string output, out string error)
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
        try 
        {
            process.Start();
        }
        catch (Exception ex)
        {
            output = string.Empty;
            error = ex.Message;
            return -1;
        }
        
        output = process.StandardOutput.ReadToEnd();
        error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        return process.ExitCode;
    }
}
