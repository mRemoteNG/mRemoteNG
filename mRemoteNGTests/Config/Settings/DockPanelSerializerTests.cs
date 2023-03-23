using System;
using System.Xml.Linq;
using mRemoteNG.Config.Settings;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.Config.Settings;

public class DockPanelSerializerTests
{
    private DockPanelLayoutSerializer _dockPanelLayoutSerializer;
    private DockPanel _dockPanel;

    [SetUp]
    public void Setup()
    {
        _dockPanelLayoutSerializer = new DockPanelLayoutSerializer();
        _dockPanel = new DockPanel();
    }

    [Test]
    public void SerializerProducesValidDockPanelXml()
    {
        var serializedData = _dockPanelLayoutSerializer.Serialize(_dockPanel);
        var serializedDataAsXDoc = XDocument.Parse(serializedData);
        Assert.That(serializedDataAsXDoc.Root?.Name.ToString(), Is.EqualTo("DockPanel"));
    }

    [Test]
    public void CantProvideNullDockPanel()
    {
        Assert.Throws<ArgumentNullException>(() => _dockPanelLayoutSerializer.Serialize(null));
    }
}