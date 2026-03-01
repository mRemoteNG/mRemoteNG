using System;
using System.Reflection;
using ExternalConnectors.PasswordSafe;
using NUnit.Framework;

namespace mRemoteNGTests.ExternalConnectors;

[TestFixture]
public class PasswordSafeCliTests
{
    [Test]
    public void ParseSecretReference_ParsesDbPathGroupAndTitle()
    {
        var result = InvokePrivate<(string DbPath, string Group, string Title, string? DbUser)>(
            "ParseSecretReference",
            "pwsafe://C:\\MySafe.psafe3?group=MyGroup&title=MyTitle");

        Assert.That(result.DbPath, Is.EqualTo("C:\\MySafe.psafe3"));
        Assert.That(result.Group, Is.EqualTo("MyGroup"));
        Assert.That(result.Title, Is.EqualTo("MyTitle"));
        Assert.That(result.DbUser, Is.Null);
    }

    [Test]
    public void ParseSecretReference_ParsesDbUser()
    {
        var result = InvokePrivate<(string DbPath, string Group, string Title, string? DbUser)>(
            "ParseSecretReference",
            "pwsafe://C:\\Safe.psafe3?group=G&title=T&username=Admin");

        Assert.That(result.DbUser, Is.EqualTo("Admin"));
    }

    [Test]
    public void ParseSecretReference_DecodesDbPath()
    {
        var result = InvokePrivate<(string DbPath, string Group, string Title, string? DbUser)>(
            "ParseSecretReference",
            "pwsafe://C:\\My%20Data\\Safe.psafe3?group=G&title=T");

        Assert.That(result.DbPath, Is.EqualTo("C:\\My Data\\Safe.psafe3"));
    }

    [Test]
    public void ParseSecretReference_ThrowsIfSchemeMissing()
    {
        var ex = Assert.Throws<TargetInvocationException>(() =>
            InvokePrivate<(string, string, string, string?)>("ParseSecretReference", "invalid"));

        Assert.That(ex?.InnerException, Is.TypeOf<PasswordSafeCliException>());
        Assert.That(ex?.InnerException?.Message, Does.Contain("Expected format pwsafe://"));
    }

    [Test]
    public void ParseSecretReference_ThrowsIfGroupAndTitleMissing()
    {
        var ex = Assert.Throws<TargetInvocationException>(() =>
            InvokePrivate<(string, string, string, string?)>("ParseSecretReference", "pwsafe://C:\\Safe.psafe3"));

        Assert.That(ex?.InnerException, Is.TypeOf<PasswordSafeCliException>());
        Assert.That(ex?.InnerException?.Message, Does.Contain("group or title is missing"));
    }

    private static T InvokePrivate<T>(string methodName, params object[] args)
    {
        var method = typeof(PasswordSafeCli).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
        Assert.That(method, Is.Not.Null, $"Method '{methodName}' was not found.");
        return (T)method!.Invoke(null, args)!;
    }
}
