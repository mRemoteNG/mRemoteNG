using System;
using System.Reflection;
using ExternalConnectors.OP;
using NUnit.Framework;

namespace mRemoteNGTests.ExternalConnectors;

[TestFixture]
public class OnePasswordCliTests
{
	[Test]
	public void ParseSecretReference_AllowsUnescapedSpacesInVaultAndItem()
	{
		var result = InvokePrivate<(string Item, string? Vault, string? Account)>(
			"ParseSecretReference",
			"op://My Vault/My RDP item");

		Assert.That(result.Item, Is.EqualTo("My RDP item"));
		Assert.That(result.Vault, Is.EqualTo("My Vault"));
		Assert.That(result.Account, Is.Null);
	}

	[Test]
	public void ParseSecretReference_AllowsVaultlessFormat()
	{
		var result = InvokePrivate<(string Item, string? Vault, string? Account)>(
			"ParseSecretReference",
			"op:///Server Login");

		Assert.That(result.Item, Is.EqualTo("Server Login"));
		Assert.That(result.Vault, Is.Null);
		Assert.That(result.Account, Is.Null);
	}

	[Test]
	public void ParseSecretReference_ParsesEncodedValuesAndAccountQuery()
	{
		var result = InvokePrivate<(string Item, string? Vault, string? Account)>(
			"ParseSecretReference",
			"op://My%20Vault/My%20RDP%20item?account=Work%20Account");

		Assert.That(result.Item, Is.EqualTo("My RDP item"));
		Assert.That(result.Vault, Is.EqualTo("My Vault"));
		Assert.That(result.Account, Is.EqualTo("Work Account"));
	}

	[Test]
	public void ParseSecretReference_ThrowsForInvalidInput()
	{
		var ex = Assert.Throws<TargetInvocationException>(() =>
			InvokePrivate<(string Item, string? Vault, string? Account)>("ParseSecretReference", "https://example.com/secret"));

		Assert.That(ex?.InnerException, Is.TypeOf<OnePasswordCliException>());
	}

	[Test]
	public void ExtractCredentialsFromJson_UsesConcealedFallbackWhenPurposeFieldIsEmpty()
	{
		const string json = """
		                    {
		                      "fields": [
		                        { "id": "username", "label": "username", "type": "STRING", "purpose": "USERNAME", "value": "alice" },
		                        { "id": "password", "label": "password", "type": "CONCEALED", "purpose": "PASSWORD", "value": "" },
		                        { "id": "password", "label": "password", "type": "CONCEALED", "purpose": "", "value": "S3cr3t!" },
		                        { "id": "domain", "label": "domain", "type": "STRING", "purpose": "", "value": "CONTOSO" }
		                      ]
		                    }
		                    """;

		var result = InvokePrivate<(string Username, string Password, string Domain, string PrivateKey)>(
			"ExtractCredentialsFromJson",
			json,
			"op.exe item get");

		Assert.That(result.Username, Is.EqualTo("alice"));
		Assert.That(result.Password, Is.EqualTo("S3cr3t!"));
		Assert.That(result.Domain, Is.EqualTo("CONTOSO"));
		Assert.That(result.PrivateKey, Is.EqualTo(string.Empty));
	}

	private static T InvokePrivate<T>(string methodName, params object[] args)
	{
		var method = typeof(OnePasswordCli).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
		Assert.That(method, Is.Not.Null, $"Method '{methodName}' was not found.");
		return (T)method!.Invoke(null, args)!;
	}
}
