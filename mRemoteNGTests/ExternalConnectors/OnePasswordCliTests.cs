using System;
using System.Reflection;
using System.Text.Json;
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

	[Test]
	public void ExtractCredentialsFromJson_ExtractsUsernameAndPasswordByPurpose()
	{
		const string json = """
		                    {
		                      "fields": [
		                        { "id": "username", "label": "user", "type": "STRING", "purpose": "USERNAME", "value": "admin" },
		                        { "id": "password", "label": "pass", "type": "CONCEALED", "purpose": "PASSWORD", "value": "hunter2" }
		                      ]
		                    }
		                    """;

		var result = InvokePrivate<(string Username, string Password, string Domain, string PrivateKey)>(
			"ExtractCredentialsFromJson", json, "op.exe item get");

		Assert.That(result.Username, Is.EqualTo("admin"));
		Assert.That(result.Password, Is.EqualTo("hunter2"));
		Assert.That(result.Domain, Is.EqualTo(string.Empty));
		Assert.That(result.PrivateKey, Is.EqualTo(string.Empty));
	}

	[Test]
	public void ExtractCredentialsFromJson_FallsBackToLabelWhenPurposeMissing()
	{
		const string json = """
		                    {
		                      "fields": [
		                        { "id": "username", "label": "username", "type": "STRING", "purpose": "", "value": "svc_acct" },
		                        { "id": "password", "label": "password", "type": "CONCEALED", "purpose": "", "value": "Pa$$w0rd" }
		                      ]
		                    }
		                    """;

		var result = InvokePrivate<(string Username, string Password, string Domain, string PrivateKey)>(
			"ExtractCredentialsFromJson", json, "op.exe item get");

		Assert.That(result.Username, Is.EqualTo("svc_acct"));
		Assert.That(result.Password, Is.EqualTo("Pa$$w0rd"));
	}

	[Test]
	public void ExtractCredentialsFromJson_ExtractsSshKeyByType()
	{
		const string json = """
		                    {
		                      "fields": [
		                        { "id": "username", "label": "username", "type": "STRING", "purpose": "USERNAME", "value": "git" },
		                        { "id": "privkey", "label": "private key", "type": "SSHKEY", "purpose": "", "value": "-----BEGIN OPENSSH PRIVATE KEY-----\ntest\n-----END OPENSSH PRIVATE KEY-----" }
		                      ]
		                    }
		                    """;

		var result = InvokePrivate<(string Username, string Password, string Domain, string PrivateKey)>(
			"ExtractCredentialsFromJson", json, "op.exe item get");

		Assert.That(result.Username, Is.EqualTo("git"));
		Assert.That(result.Password, Is.EqualTo(string.Empty));
		Assert.That(result.PrivateKey, Does.StartWith("-----BEGIN OPENSSH PRIVATE KEY-----"));
	}

	[Test]
	public void ExtractCredentialsFromJson_ThrowsWhenNoPasswordOrKey()
	{
		const string json = """
		                    {
		                      "fields": [
		                        { "id": "username", "label": "username", "type": "STRING", "purpose": "USERNAME", "value": "admin" }
		                      ]
		                    }
		                    """;

		var ex = Assert.Throws<TargetInvocationException>(() =>
			InvokePrivate<(string, string, string, string)>("ExtractCredentialsFromJson", json, "op.exe item get"));

		Assert.That(ex?.InnerException, Is.TypeOf<OnePasswordCliException>());
	}

	[Test]
	public void ExtractCredentialsFromJson_ThrowsForNullDeserialization()
	{
		const string json = "null";

		var ex = Assert.Throws<TargetInvocationException>(() =>
			InvokePrivate<(string, string, string, string)>("ExtractCredentialsFromJson", json, "op.exe item get"));

		Assert.That(ex?.InnerException, Is.TypeOf<OnePasswordCliException>());
	}

	[Test]
	public void ExtractCredentialsFromJson_ExtractsDomain()
	{
		const string json = """
		                    {
		                      "fields": [
		                        { "id": "username", "label": "username", "type": "STRING", "purpose": "USERNAME", "value": "admin" },
		                        { "id": "password", "label": "password", "type": "CONCEALED", "purpose": "PASSWORD", "value": "secret" },
		                        { "id": "domain", "label": "domain", "type": "STRING", "purpose": "", "value": "CORP" }
		                      ]
		                    }
		                    """;

		var result = InvokePrivate<(string Username, string Password, string Domain, string PrivateKey)>(
			"ExtractCredentialsFromJson", json, "op.exe item get");

		Assert.That(result.Domain, Is.EqualTo("CORP"));
	}

	[Test]
	public void ExtractCredentialsFromJson_EmptyFieldsArrayThrows()
	{
		const string json = """{ "fields": [] }""";

		var ex = Assert.Throws<TargetInvocationException>(() =>
			InvokePrivate<(string, string, string, string)>("ExtractCredentialsFromJson", json, "op.exe item get"));

		Assert.That(ex?.InnerException, Is.TypeOf<OnePasswordCliException>());
	}

	[Test]
	public void ExtractCredentialsFromJson_SkipsFieldsWithEmptyValues()
	{
		const string json = """
		                    {
		                      "fields": [
		                        { "id": "username", "label": "username", "type": "STRING", "purpose": "USERNAME", "value": "" },
		                        { "id": "username2", "label": "username", "type": "STRING", "purpose": "", "value": "fallback_user" },
		                        { "id": "password", "label": "password", "type": "CONCEALED", "purpose": "PASSWORD", "value": "" },
		                        { "id": "password2", "label": "password", "type": "CONCEALED", "purpose": "", "value": "fallback_pass" }
		                      ]
		                    }
		                    """;

		var result = InvokePrivate<(string Username, string Password, string Domain, string PrivateKey)>(
			"ExtractCredentialsFromJson", json, "op.exe item get");

		Assert.That(result.Username, Is.EqualTo("fallback_user"));
		Assert.That(result.Password, Is.EqualTo("fallback_pass"));
	}

	[Test]
	public void ExtractCredentialsFromJson_ThrowsWhenFieldsKeyIsMissing()
	{
		const string json = """{ "urls": [{ "label": "website", "href": "https://example.com" }] }""";

		var ex = Assert.Throws<TargetInvocationException>(() =>
			InvokePrivate<(string, string, string, string)>("ExtractCredentialsFromJson", json, "op.exe item get"));

		Assert.That(ex?.InnerException, Is.TypeOf<OnePasswordCliException>());
	}

	[Test]
	public void ExtractCredentialsFromJson_ThrowsForMalformedJson()
	{
		const string json = "this is not valid json at all";

		var ex = Assert.Throws<TargetInvocationException>(() =>
			InvokePrivate<(string, string, string, string)>("ExtractCredentialsFromJson", json, "op.exe item get"));

		Assert.That(ex?.InnerException, Is.TypeOf<JsonException>());
	}

	[Test]
	public void ExtractCredentialsFromJson_FallsBackToLabelWhenIdDoesNotMatch()
	{
		const string json = """
		                    {
		                      "fields": [
		                        { "id": "login_user", "label": "username", "type": "STRING", "purpose": "", "value": "alice" },
		                        { "id": "login_pass", "label": "password", "type": "CONCEALED", "purpose": "", "value": "secret123" }
		                      ]
		                    }
		                    """;

		var result = InvokePrivate<(string Username, string Password, string Domain, string PrivateKey)>(
			"ExtractCredentialsFromJson", json, "op.exe item get");

		Assert.That(result.Username, Is.EqualTo("alice"));
		Assert.That(result.Password, Is.EqualTo("secret123"));
	}

	private static T InvokePrivate<T>(string methodName, params object[] args)
	{
		var method = typeof(OnePasswordCli).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
		Assert.That(method, Is.Not.Null, $"Method '{methodName}' was not found.");
		return (T)method!.Invoke(null, args)!;
	}
}
