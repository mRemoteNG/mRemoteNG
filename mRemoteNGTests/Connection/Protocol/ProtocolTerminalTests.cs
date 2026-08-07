using System;
using System.Reflection;
using mRemoteNG.Connection.Protocol.Terminal;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol;

// Regression coverage for issue #3335: the Terminal protocol used to build "cmd.exe /K ssh <host>"
// by concatenating the unsanitized Hostname/Username connection fields, letting a malicious .xml
// inject arbitrary commands through cmd.exe metacharacters. It now launches ssh.exe directly and
// rejects any host/user value that ssh could mis-parse as an extra argument.
[TestFixture]
public class ProtocolTerminalTests
{
    #region Valid targets

    [Test]
    public void BuildSshArguments_HostnameOnly_ReturnsHost()
    {
        Assert.That(InvokeBuild("server.example.com", "", 22), Is.EqualTo("server.example.com"));
    }

    [Test]
    public void BuildSshArguments_WithUsername_ReturnsUserAtHost()
    {
        Assert.That(InvokeBuild("server.example.com", "admin", 22), Is.EqualTo("admin@server.example.com"));
    }

    [Test]
    public void BuildSshArguments_DefaultPort_OmitsPortFlag()
    {
        Assert.That(InvokeBuild("host", "", 22), Is.EqualTo("host"));
    }

    [Test]
    public void BuildSshArguments_ZeroPort_OmitsPortFlag()
    {
        Assert.That(InvokeBuild("host", "", 0), Is.EqualTo("host"));
    }

    [Test]
    public void BuildSshArguments_NonDefaultPort_PrependsPortFlag()
    {
        Assert.That(InvokeBuild("host", "admin", 2222), Is.EqualTo("-p 2222 admin@host"));
    }

    [Test]
    public void BuildSshArguments_TrimsWhitespaceAroundValues()
    {
        Assert.That(InvokeBuild("  host  ", "  admin  ", 22), Is.EqualTo("admin@host"));
    }

    [Test]
    public void BuildSshArguments_Ipv6Address_IsAllowed()
    {
        Assert.That(InvokeBuild("[fe80::1]", "root", 22), Is.EqualTo("root@[fe80::1]"));
    }

    #endregion

    #region Injection attempts are rejected

    [Test]
    public void BuildSshArguments_AmpersandCommandInjection_Throws()
    {
        // The exact payload from issue #3335.
        AssertRejected(@"a & cmd /c echo pwned > C:\Users\Public\poc.txt & rem", "");
    }

    [TestCase("host | calc.exe")]
    [TestCase("host & calc.exe")]
    [TestCase("host > out.txt")]
    [TestCase("host < in.txt")]
    [TestCase("host ^ escape")]
    [TestCase("host with space")]
    [TestCase("host\ttab")]
    [TestCase("host\nnewline")]
    [TestCase("host\rreturn")]
    public void BuildSshArguments_HostWithShellMetacharactersOrWhitespace_Throws(string hostname)
    {
        AssertRejected(hostname, "");
    }

    [Test]
    public void BuildSshArguments_HostStartingWithDash_Throws()
    {
        // ssh would treat a leading '-' value as an option (e.g. -oProxyCommand=...) — argument injection.
        AssertRejected("-oProxyCommand=calc.exe", "");
    }

    [Test]
    public void BuildSshArguments_UsernameStartingWithDash_Throws()
    {
        AssertRejected("host", "-oProxyCommand=calc.exe");
    }

    [Test]
    public void BuildSshArguments_UsernameWithSpace_Throws()
    {
        AssertRejected("host", "admin -oProxyCommand=calc.exe");
    }

    [Test]
    public void BuildSshArguments_EmptyHostname_Throws()
    {
        AssertRejected("", "admin");
    }

    [Test]
    public void BuildSshArguments_QuotedLeadingDashHostname_Throws()
    {
        // A leading '-' hidden behind double quotes: Windows CommandLineToArgvW strips the quotes, so
        // ssh.exe still receives an argv token starting with '-'. The quote must be rejected.
        AssertRejected("\"-oProxyCommand=calc.exe\"", "");
    }

    [Test]
    public void BuildSshArguments_QuotedLeadingDashUsername_Throws()
    {
        AssertRejected("host", "\"-oProxyCommand=calc.exe\"");
    }

    [TestCase("ho\"st")]
    [TestCase("\"host")]
    [TestCase("host\"")]
    public void BuildSshArguments_HostWithDoubleQuote_Throws(string hostname)
    {
        AssertRejected(hostname, "");
    }

    [Test]
    public void BuildSshArguments_MessageDoesNotEchoOffendingValue()
    {
        // The exception must not embed the attacker-controlled token (control chars/newlines would
        // otherwise reach logs and UI verbatim — log forging / message spoofing).
        MethodInfo method = GetBuildMethod();
        var ex = Assert.Throws<TargetInvocationException>(
            () => method.Invoke(null, new object[] { "evil\r\nInjected-Log-Line", "", 22 }));
        Assert.That(ex.InnerException, Is.TypeOf<ArgumentException>());
        Assert.That(ex.InnerException.Message, Does.Not.Contain("Injected-Log-Line"));
        Assert.That(ex.InnerException.Message, Does.Not.Contain("\n"));
    }

    #endregion

    #region Helpers

    private static string InvokeBuild(string hostname, string username, int port)
    {
        MethodInfo method = GetBuildMethod();
        return (string)method.Invoke(null, new object[] { hostname, username, port });
    }

    private static void AssertRejected(string hostname, string username)
    {
        MethodInfo method = GetBuildMethod();
        TargetInvocationException ex = Assert.Throws<TargetInvocationException>(
            () => method.Invoke(null, new object[] { hostname, username, 22 }));
        Assert.That(ex.InnerException, Is.TypeOf<ArgumentException>());
    }

    private static MethodInfo GetBuildMethod()
    {
        MethodInfo method = typeof(ProtocolTerminal).GetMethod("BuildSshArguments",
            BindingFlags.NonPublic | BindingFlags.Static);

        if (method == null)
            throw new InvalidOperationException("BuildSshArguments method not found. The method may have been renamed or removed.");

        return method;
    }

    #endregion
}
