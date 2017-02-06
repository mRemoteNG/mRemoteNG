using System;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class ConfConsEnsureConnectionsHaveIdsTests
    {
        private ConfConsEnsureConnectionsHaveIds _consEnsureConnectionsHaveIds;

        [SetUp]
        public void Setup()
        {
            _consEnsureConnectionsHaveIds = new ConfConsEnsureConnectionsHaveIds();
        }

        [Test]
        public void IdAttributeIsAddedIfItDidntExist()
        {
            var xdoc = CreateTestDocument();
            _consEnsureConnectionsHaveIds.EnsureElementsHaveIds(xdoc);
            var attribute = xdoc.Root?.Element("Node")?.Attribute("Id");
            Assert.That(attribute, Is.Not.Null);
        }

        [Test]
        public void NewIdAttributeShouldNotBeAnEmptyGuid()
        {
            var xdoc = CreateTestDocument();
            _consEnsureConnectionsHaveIds.EnsureElementsHaveIds(xdoc);
            var attribute = xdoc.Root?.Element("Node")?.Attribute("Id");
            Assert.That(attribute?.Value, Is.Not.EqualTo(Guid.Empty.ToString()));
        }

        private XDocument CreateTestDocument()
        {
            var xdoc = new XDocument();
            xdoc.Add(new XElement("Root", 
                new XElement("Node",
                    new XAttribute("Thingy",""))));
            return xdoc;
        }
    }
}
