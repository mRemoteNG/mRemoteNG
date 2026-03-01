using System;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers;

public class ConfConsEnsureConnectionsHaveIdsTests
{

    [Test]
    public void IdAttributeIsAddedIfItDidntExist()
    {
        var xdoc = CreateTestDocument();
        ConfConsEnsureConnectionsHaveIds.EnsureElementsHaveIds(xdoc);
        var attribute = xdoc.Root?.Element("Node")?.Attribute("Id");
        Assert.That(attribute, Is.Not.Null);
    }

    [Test]
    public void NewIdAttributeShouldNotBeAnEmptyGuid()
    {
        var xdoc = CreateTestDocument();
        ConfConsEnsureConnectionsHaveIds.EnsureElementsHaveIds(xdoc);
        var attribute = xdoc.Root?.Element("Node")?.Attribute("Id");
        Assert.That(attribute?.Value, Is.Not.EqualTo(Guid.Empty.ToString()));
    }

    private static XDocument CreateTestDocument()
    {
        var xdoc = new XDocument();
        xdoc.Add(new XElement("Root",
            new XElement("Node",
                new XAttribute("Thingy", ""))));
        return xdoc;
    }
}