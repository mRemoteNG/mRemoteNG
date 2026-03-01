using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;
using mRemoteNG.Tools.CustomCollections;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.Connection.Protocol;

[NonParallelizable] // Uses shared Runtime.ExternalToolsService singleton
public class IntegratedProgramTests
{
    private readonly ExternalTool _extTool = new()
    {
        DisplayName = "notepad",
        FileName = @"%windir%\system32\notepad.exe",
        Arguments = "",
        TryIntegrate = true
    };

    [Test]
    public void InitializeSucceedsWhenExternalToolExists()
    {
        SetExternalToolList(_extTool);
        var sut = new IntegratedProgram();
        sut.InterfaceControl = BuildInterfaceControl("notepad", sut);

        var initialized = sut.Initialize();

        Assert.That(initialized, Is.True);
    }

    [TestCase("cmd")]
    [TestCase("cmd.exe")]
    [TestCase(@"C:\Windows\System32\cmd.exe")]
    [TestCase("pwsh")]
    [TestCase("pwsh.exe")]
    [TestCase("powershell")]
    [TestCase("powershell.exe")]
    [TestCase("wsl")]
    [TestCase("wsl.exe")]
    [TestCase(@"C:\Windows\System32\wsl.exe")]
    // WSL distribution launchers (issue #693: ubuntu terminal as external tool)
    [TestCase("ubuntu")]
    [TestCase("ubuntu.exe")]
    [TestCase("ubuntu2004")]
    [TestCase("ubuntu2004.exe")]
    [TestCase("ubuntu2204")]
    [TestCase("ubuntu2204.exe")]
    [TestCase("debian")]
    [TestCase("debian.exe")]
    [TestCase("kali-linux")]
    [TestCase("kali-linux.exe")]
    [TestCase("kali")]
    [TestCase("kali.exe")]
    public void InitializeSucceedsForBuiltInShellPresetWhenExternalToolIsNotConfigured(string extAppName)
    {
        SetExternalToolList();
        var sut = new IntegratedProgram();
        sut.InterfaceControl = BuildInterfaceControl(extAppName, sut);

        var initialized = sut.Initialize();

        Assert.That(initialized, Is.True);
    }

    [Test]
    public void ConnectingToExternalAppThatDoesntExistDoesNothing()
    {
        SetExternalToolList(_extTool);
        var sut = new IntegratedProgram();
        sut.InterfaceControl = BuildInterfaceControl("doesntExist", sut);
        var appInitialized = sut.Initialize();
        Assert.That(appInitialized, Is.False);
    }

    private static void SetExternalToolList(params ExternalTool[] externalTools)
    {
        Runtime.ExternalToolsService.ExternalTools = new FullyObservableCollection<ExternalTool>(externalTools);
    }

    private static InterfaceControl BuildInterfaceControl(string extAppName, ProtocolBase sut)
    {
        var connectionWindow = new ConnectionWindow(new DockContent());
        var connectionInfo = new ConnectionInfo { ExtApp = extAppName, Protocol = ProtocolType.IntApp };
        return new InterfaceControl(connectionWindow, sut, connectionInfo);
    }
}
