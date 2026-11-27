using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using mRemoteNG.Config.DataProviders;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders;

[Platform("Win")]
public class PrivateStagingFileTests
{
    private string _directory;

    [SetUp]
    public void SetUp()
    {
        _directory = Path.Combine(Path.GetTempPath(), "mRemoteNG-acl-" + Guid.NewGuid().ToString("N"));
        DirectoryInfo directory = Directory.CreateDirectory(_directory);
        DirectorySecurity security = new();
        security.SetAccessRuleProtection(true, false);
        security.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().User,
            FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
            PropagationFlags.None, AccessControlType.Allow));
        security.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
            FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
            PropagationFlags.None, AccessControlType.Allow));
        directory.SetAccessControl(security);
    }

    [TearDown]
    public void TearDown() => Directory.Delete(_directory, true);

    [Test]
    public void CreatePrivateStagingFile_RestrictsAccessBeforeWritingAnyBytes()
    {
        string path = Path.Combine(_directory, "staging.tmp");
        using FileStream stream = AtomicFile.CreatePrivateStagingFile(path);
        AssertPrivate(path);
        Assert.That(stream.Length, Is.Zero);
        Assert.Throws<IOException>(() => AtomicFile.CreatePrivateStagingFile(path));
    }

    [Test]
    public void WriteAllText_FirstCreation_DoesNotInheritPublicReadAccess()
    {
        string path = Path.Combine(_directory, "new.xml");
        AtomicFile.WriteAllText(path, "synthetic");
        AssertPrivate(path);
        Assert.That(File.ReadAllText(path), Is.EqualTo("synthetic"));
    }

    [Test]
    public void WriteAllText_Replacement_PreservesExistingRestrictedAcl()
    {
        string path = Path.Combine(_directory, "existing.xml");
        AtomicFile.WriteAllText(path, "original");
        FileSecurity original = new FileInfo(path).GetAccessControl();
        SecurityIdentifier authorizedReader = new(WellKnownSidType.AuthenticatedUserSid, null);
        original.AddAccessRule(new FileSystemAccessRule(authorizedReader, FileSystemRights.Read, AccessControlType.Allow));
        new FileInfo(path).SetAccessControl(original);
        var before = AccessRules(path);
        AtomicFile.WriteAllText(path, "replacement");
        Assert.That(AccessRules(path), Is.EquivalentTo(before));
        Assert.That(new FileInfo(path).GetAccessControl().AreAccessRulesProtected, Is.True);
        Assert.That(File.ReadAllText(path), Is.EqualTo("replacement"));
    }

    [Test]
    public void Restore_StagedAndPreservedFiles_RemainPrivate()
    {
        string primary = Path.Combine(_directory, "connections.xml");
        string backup = Path.Combine(_directory, "connections.backup");
        AtomicFile.WriteAllText(primary, "broken original");
        AtomicFile.WriteAllText(backup, "valid generation");
        FileRecoveryCandidate candidate = new(backup, DateTime.UtcNow, new FileInfo(backup).Length,
            FileRecoveryRestorer.ComputeSha256(backup));
        FileRecoveryRestorer restorer = new((staged, destination, preserved) =>
        {
            AssertPrivate(staged);
            File.Replace(staged, destination, preserved);
        });
        string preserved = restorer.Restore(primary, candidate, candidate.Sha256,
            FileRecoveryRestorer.ComputeSha256(primary), _ => true);
        AssertPrivate(primary);
        AssertPrivate(preserved);
        Assert.That(File.ReadAllText(primary), Is.EqualTo("valid generation"));
        Assert.That(File.ReadAllText(preserved), Is.EqualTo("broken original"));
    }

    private static string[] AccessRules(string path) => new FileInfo(path).GetAccessControl()
        .GetAccessRules(true, true, typeof(SecurityIdentifier)).Cast<FileSystemAccessRule>()
        .Select(rule => $"{rule.IdentityReference.Value}:{rule.AccessControlType}:{rule.FileSystemRights}:{rule.IsInherited}").ToArray();

    private static void AssertPrivate(string path)
    {
        FileSecurity security = new FileInfo(path).GetAccessControl();
        Assert.That(security.AreAccessRulesProtected, Is.True);
        string currentUser = WindowsIdentity.GetCurrent().User.Value;
        string system = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null).Value;
        var allowed = security.GetAccessRules(true, true, typeof(SecurityIdentifier)).Cast<FileSystemAccessRule>()
            .Where(rule => rule.AccessControlType == AccessControlType.Allow)
            .Select(rule => rule.IdentityReference.Value);
        Assert.That(allowed, Is.EquivalentTo(new[] { currentUser, system }));
    }
}
