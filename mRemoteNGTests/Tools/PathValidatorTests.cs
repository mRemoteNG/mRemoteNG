using System;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools;

public class PathValidatorTests
{
    [Test]
    public void ValidPath_ReturnsTrue()
    {
        string validPath = @"C:\Users\TestUser\Documents\test.xml";
        Assert.That(PathValidator.IsValidPath(validPath), Is.True);
    }

    [Test]
    public void PathWithForwardSlashTraversal_ReturnsFalse()
    {
        string maliciousPath = @"C:\Users\TestUser\Documents\..\..\..\Windows\System32\test.xml";
        Assert.That(PathValidator.IsValidPath(maliciousPath), Is.False);
    }

    [Test]
    public void PathWithBackslashTraversal_ReturnsFalse()
    {
        string maliciousPath = @"C:\Users\TestUser\Documents\..\..\test.xml";
        Assert.That(PathValidator.IsValidPath(maliciousPath), Is.False);
    }

    [Test]
    public void PathWithMixedTraversal_ReturnsFalse()
    {
        string maliciousPath = @"C:\Users\TestUser\Documents\.././..\test.xml";
        Assert.That(PathValidator.IsValidPath(maliciousPath), Is.False);
    }

    [Test]
    public void PathWithEncodedTraversal_ReturnsFalse()
    {
        string maliciousPath = @"C:\Users\TestUser\Documents\%2e%2e\test.xml";
        Assert.That(PathValidator.IsValidPath(maliciousPath), Is.False);
    }

    [Test]
    public void PathWithUppercaseEncodedTraversal_ReturnsFalse()
    {
        string maliciousPath = @"C:\Users\TestUser\Documents\%2E%2E\test.xml";
        Assert.That(PathValidator.IsValidPath(maliciousPath), Is.False);
    }

    [Test]
    public void NullPath_ReturnsFalse()
    {
        Assert.That(PathValidator.IsValidPath(null), Is.False);
    }

    [Test]
    public void EmptyPath_ReturnsFalse()
    {
        Assert.That(PathValidator.IsValidPath(""), Is.False);
    }

    [Test]
    public void ValidatePathOrThrow_WithValidPath_DoesNotThrow()
    {
        string validPath = @"C:\Users\TestUser\Documents\test.xml";
        Assert.DoesNotThrow(() => PathValidator.ValidatePathOrThrow(validPath));
    }

    [Test]
    public void ValidatePathOrThrow_WithTraversalPath_ThrowsArgumentException()
    {
        string maliciousPath = @"C:\Users\TestUser\Documents\..\..\..\test.xml";
        var exception = Assert.Throws<ArgumentException>(() => PathValidator.ValidatePathOrThrow(maliciousPath));
        Assert.That(exception.Message, Does.Contain("path traversal"));
    }

    [Test]
    public void ValidatePathOrThrow_WithNullPath_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => PathValidator.ValidatePathOrThrow(null));
    }

    [Test]
    public void ValidatePathOrThrow_WithCustomParameterName_IncludesParameterName()
    {
        string maliciousPath = @"..\..\..\test.xml";
        var exception = Assert.Throws<ArgumentException>(() => PathValidator.ValidatePathOrThrow(maliciousPath, "customParam"));
        Assert.That(exception.ParamName, Is.EqualTo("customParam"));
    }

    #region IsValidExecutablePath Tests

    [Test]
    public void IsValidExecutablePath_ValidWindowsPath_ReturnsTrue()
    {
        string validPath = @"C:\Windows\System32\notepad.exe";
        Assert.That(PathValidator.IsValidExecutablePath(validPath), Is.True);
    }

    [Test]
    public void IsValidExecutablePath_ValidRelativePath_ReturnsTrue()
    {
        string validPath = @"app.exe";
        Assert.That(PathValidator.IsValidExecutablePath(validPath), Is.True);
    }

    [Test]
    public void IsValidExecutablePath_PathWithSpaces_ReturnsTrue()
    {
        string validPath = @"C:\Program Files\App\app.exe";
        Assert.That(PathValidator.IsValidExecutablePath(validPath), Is.True);
    }

    [Test]
    public void IsValidExecutablePath_PathWithCommandInjectionAmpersand_ReturnsFalse()
    {
        string maliciousPath = @"notepad.exe & calc.exe";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_PathWithCommandInjectionPipe_ReturnsFalse()
    {
        string maliciousPath = @"notepad.exe | calc.exe";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_PathWithCommandInjectionSemicolon_ReturnsFalse()
    {
        string maliciousPath = @"notepad.exe; calc.exe";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_PathWithRedirection_ReturnsFalse()
    {
        string maliciousPath = @"notepad.exe > output.txt";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_PathWithInputRedirection_ReturnsFalse()
    {
        string maliciousPath = @"notepad.exe < input.txt";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_ProgramFilesX86Path_ReturnsTrue()
    {
        string validPath = @"C:\Program Files (x86)\PuTTY\putty.exe";
        Assert.That(PathValidator.IsValidExecutablePath(validPath), Is.True);
    }

    [Test]
    public void IsValidExecutablePath_PathWithCaret_ReturnsFalse()
    {
        string maliciousPath = @"notepad.exe^calc.exe";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_PathWithNewline_ReturnsFalse()
    {
        string maliciousPath = "notepad.exe\ncalc.exe";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_PathWithCarriageReturn_ReturnsFalse()
    {
        string maliciousPath = "notepad.exe\rcalc.exe";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_PathWithMultipleQuotes_ReturnsFalse()
    {
        string maliciousPath = @"notepad.exe """"";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_PathWithMultipleSingleQuotes_ReturnsFalse()
    {
        string maliciousPath = @"notepad.exe ''";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_EmptyPath_ReturnsFalse()
    {
        Assert.That(PathValidator.IsValidExecutablePath(""), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_NullPath_ReturnsFalse()
    {
        Assert.That(PathValidator.IsValidExecutablePath(null), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_WhitespacePath_ReturnsFalse()
    {
        Assert.That(PathValidator.IsValidExecutablePath("   "), Is.False);
    }

    [Test]
    public void IsValidExecutablePath_PathWithTraversal_ReturnsFalse()
    {
        string maliciousPath = @"..\notepad.exe";
        Assert.That(PathValidator.IsValidExecutablePath(maliciousPath), Is.False);
    }

    [Test]
    public void ValidateExecutablePathOrThrow_ValidPath_DoesNotThrow()
    {
        string validPath = @"C:\Windows\System32\notepad.exe";
        Assert.DoesNotThrow(() => PathValidator.ValidateExecutablePathOrThrow(validPath));
    }

    [Test]
    public void ValidateExecutablePathOrThrow_PathWithCommandInjection_ThrowsArgumentException()
    {
        string maliciousPath = @"notepad.exe & calc.exe";
        var exception = Assert.Throws<ArgumentException>(() => PathValidator.ValidateExecutablePathOrThrow(maliciousPath));
        Assert.That(exception.Message, Does.Contain("dangerous characters"));
    }

    [Test]
    public void ValidateExecutablePathOrThrow_PathWithTraversal_ThrowsArgumentException()
    {
        string maliciousPath = @"..\notepad.exe";
        Assert.Throws<ArgumentException>(() => PathValidator.ValidateExecutablePathOrThrow(maliciousPath));
    }

    [Test]
    public void ValidateExecutablePathOrThrow_WithCustomParameterName_IncludesParameterName()
    {
        string maliciousPath = @"notepad.exe & calc.exe";
        var exception = Assert.Throws<ArgumentException>(() => PathValidator.ValidateExecutablePathOrThrow(maliciousPath, "executablePath"));
        Assert.That(exception.ParamName, Is.EqualTo("executablePath"));
    }

    #endregion
}
