using System;
using mRemoteNG.Config.DatabaseConnectors;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DatabaseConnectors;

/// <summary>
/// Test connection names the reason a connection failed: a server certificate this computer does
/// not trust must be told apart from a server that cannot be reached or a login that is refused.
/// </summary>
public class DatabaseConnectionTesterTests
{
    [TestCase("A connection was successfully established with the server, but then an error occurred during the pre-login handshake. (provider: SSL Provider, error: 0 - The certificate chain was issued by an authority that is not trusted.)")]
    [TestCase("A connection was successfully established with the server, but then an error occurred during the pre-login handshake. (provider: SSL Provider, error: 0 - The target principal name is incorrect.)")]
    [TestCase("The remote certificate was rejected by the provided RemoteCertificateValidationCallback.")]
    [TestCase("Certificate failed chain validation. Error(s): 'RemoteCertificateChainErrors'.")]
    [TestCase("Certificate name mismatch. The provided 'DataSource' or 'HostNameInCertificate' does not match the name in the certificate.")]
    public void UntrustedServerCertificateIsReportedAsSuch(string message)
    {
        Assert.That(DatabaseConnectionTester.ResultFor(new Exception(message)), Is.EqualTo(ConnectionTestResult.ServerCertificateNotTrusted));
    }

    [Test]
    public void UntrustedServerCertificateIsFoundInTheInnerException()
    {
        Exception ex = new("A connection was successfully established with the server, but then an error occurred during the pre-login handshake. (provider: TCP Provider, error: 35 - An internal exception was caught)",
            new System.Security.Authentication.AuthenticationException("The remote certificate was rejected by the provided RemoteCertificateValidationCallback."));

        Assert.That(DatabaseConnectionTester.ResultFor(ex), Is.EqualTo(ConnectionTestResult.ServerCertificateNotTrusted));
    }

    [TestCase("A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible.", ConnectionTestResult.ServerNotAccessible)]
    [TestCase("Cannot open database \"mRemoteNG\" requested by the login. The login failed.", ConnectionTestResult.UnknownDatabase)]
    [TestCase("Unknown database 'mremoteng'", ConnectionTestResult.UnknownDatabase)]
    [TestCase("Login failed for user 'mrng_login'.", ConnectionTestResult.CredentialsRejected)]
    [TestCase("Login failed for user 'certificate_reader'.", ConnectionTestResult.CredentialsRejected)]
    [TestCase("Cannot open database \"certificates\" requested by the login. The login failed.", ConnectionTestResult.UnknownDatabase)]
    [TestCase("The target principal name is incorrect.  Cannot generate SSPI context.", ConnectionTestResult.UnknownError)]
    [TestCase("Access denied for user 'mrng_login'@'localhost' (using password: YES)", ConnectionTestResult.CredentialsRejected)]
    [TestCase("Something else went wrong.", ConnectionTestResult.UnknownError)]
    public void OtherFailuresKeepTheirResult(string message, ConnectionTestResult expected)
    {
        Assert.That(DatabaseConnectionTester.ResultFor(new Exception(message)), Is.EqualTo(expected));
    }
}
