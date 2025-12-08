using System;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.AnyDesk;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol;

public class ProtocolAnydeskTests
{
    private ProtocolAnyDesk _protocolAnydesk;
    private ConnectionInfo _connectionInfo;

    [SetUp]
    public void Setup()
    {
        _connectionInfo = new ConnectionInfo();
        _protocolAnydesk = new ProtocolAnyDesk(_connectionInfo);
    }

    [TearDown]
    public void Teardown()
    {
        _protocolAnydesk?.Close();
        _protocolAnydesk = null;
        _connectionInfo = null;
    }

    #region IsValidAnydeskId Tests

    [Test]
    public void IsValidAnydeskId_NumericId_ReturnsTrue()
    {
        // Valid numeric AnyDesk ID
        bool result = InvokeIsValidAnydeskId("123456789");
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsValidAnydeskId_AlphanumericWithAt_ReturnsTrue()
    {
        // Valid alias format: alias@ad
        bool result = InvokeIsValidAnydeskId("myalias@ad");
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsValidAnydeskId_WithHyphen_ReturnsTrue()
    {
        // Valid ID with hyphen
        bool result = InvokeIsValidAnydeskId("my-alias@ad");
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsValidAnydeskId_WithUnderscore_ReturnsTrue()
    {
        // Valid ID with underscore
        bool result = InvokeIsValidAnydeskId("my_alias@ad");
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsValidAnydeskId_WithDot_ReturnsTrue()
    {
        // Valid ID with dot
        bool result = InvokeIsValidAnydeskId("alias.name@ad");
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsValidAnydeskId_WithSemicolon_ReturnsFalse()
    {
        // Command injection attempt with semicolon
        bool result = InvokeIsValidAnydeskId("123456789; calc.exe");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithAmpersand_ReturnsFalse()
    {
        // Command injection attempt with ampersand
        bool result = InvokeIsValidAnydeskId("123456789 & calc.exe");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithPipe_ReturnsFalse()
    {
        // Command injection attempt with pipe
        bool result = InvokeIsValidAnydeskId("123456789 | calc.exe");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithRedirection_ReturnsFalse()
    {
        // Command injection attempt with redirection
        bool result = InvokeIsValidAnydeskId("123456789 > output.txt");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithBacktick_ReturnsFalse()
    {
        // PowerShell escape character
        bool result = InvokeIsValidAnydeskId("123456789`calc");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithDollarSign_ReturnsFalse()
    {
        // PowerShell variable indicator
        bool result = InvokeIsValidAnydeskId("123456789$var");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithParentheses_ReturnsFalse()
    {
        // Command substitution
        bool result = InvokeIsValidAnydeskId("123456789(calc)");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithNewline_ReturnsFalse()
    {
        // Newline injection
        bool result = InvokeIsValidAnydeskId("123456789\ncalc");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithCarriageReturn_ReturnsFalse()
    {
        // Carriage return injection
        bool result = InvokeIsValidAnydeskId("123456789\rcalc");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithQuotes_ReturnsFalse()
    {
        // Quote escape attempt
        bool result = InvokeIsValidAnydeskId("123456789\"calc\"");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_WithSingleQuotes_ReturnsFalse()
    {
        // Single quote escape attempt
        bool result = InvokeIsValidAnydeskId("123456789'calc'");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_EmptyString_ReturnsFalse()
    {
        // Empty string
        bool result = InvokeIsValidAnydeskId("");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_Whitespace_ReturnsFalse()
    {
        // Only whitespace
        bool result = InvokeIsValidAnydeskId("   ");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidAnydeskId_Null_ReturnsFalse()
    {
        // Null string
        bool result = InvokeIsValidAnydeskId(null);
        Assert.That(result, Is.False);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Uses reflection to invoke the private IsValidAnydeskId method
    /// </summary>
    private bool InvokeIsValidAnydeskId(string anydeskId)
    {
        var method = typeof(ProtocolAnyDesk).GetMethod("IsValidAnydeskId",
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (method == null)
        {
            throw new Exception("IsValidAnydeskId method not found. The method may have been renamed or removed.");
        }

        return (bool)method.Invoke(_protocolAnydesk, new object[] { anydeskId });
    }

    #endregion
}
