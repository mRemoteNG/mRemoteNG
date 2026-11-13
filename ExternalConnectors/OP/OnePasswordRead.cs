using System.Runtime.ExceptionServices;

namespace ExternalConnectors.OP;

/// <summary>
/// One reference read by <see cref="OnePasswordCli.ReadPasswordAsync"/>: the values that
/// <see cref="OnePasswordCli.ReadPassword(string, out string, out string, out string, out string)"/>
/// assigned, and the exception it threw, if it threw one.
/// </summary>
public sealed class OnePasswordRead
{
	// Null when ReadPassword threw before assigning the value.
	private readonly string? _username;
	private readonly string? _password;
	private readonly string? _domain;
	private readonly string? _privateKey;
	private readonly ExceptionDispatchInfo? _failure;

	internal OnePasswordRead(string input)
	{
		Input = input;
		try
		{
			OnePasswordCli.ReadPassword(input, out _username, out _password, out _domain, out _privateKey);
		}
		catch (Exception ex)
		{
			_failure = ExceptionDispatchInfo.Capture(ex);
		}
	}

	/// <summary>The reference that was read.</summary>
	public string Input { get; }

	/// <summary>
	/// Does to the caller's variables what ReadPassword would have done to them: assigns the values it
	/// assigned, then throws what it threw.
	/// </summary>
	internal void Apply(ref string username, ref string password, ref string domain, ref string privateKey)
	{
		if (_username != null) username = _username;
		if (_password != null) password = _password;
		if (_domain != null) domain = _domain;
		if (_privateKey != null) privateKey = _privateKey;
		_failure?.Throw();
	}
}
