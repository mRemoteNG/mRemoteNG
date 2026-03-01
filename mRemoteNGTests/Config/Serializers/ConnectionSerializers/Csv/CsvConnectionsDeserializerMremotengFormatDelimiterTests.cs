using NUnit.Framework;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Csv;
using System.Linq;
using mRemoteNG.Tree.Root;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Csv
{
    [TestFixture]
    public class CsvConnectionsDeserializerMremotengFormatDelimiterTests
    {
        [Test]
        public void Deserialize_CommaSeparatedCsv_ReturnsCorrectModel()
        {
            // Arrange
            var csv = "Name,Hostname,Protocol,Port,Parent\nTestConnection,example.com,RDP,3389,";
            var deserializer = new CsvConnectionsDeserializerMremotengFormat();

            // Act
            var model = deserializer.Deserialize(csv);

            // Assert
            var connections = model.GetRecursiveChildList();
            Assert.That(connections.Count, Is.EqualTo(1));
            var connection = connections.First();
            Assert.That(connection.Name, Is.EqualTo("TestConnection"));
            Assert.That(connection.Hostname, Is.EqualTo("example.com"));
            Assert.That(connection.Port, Is.EqualTo(3389));
        }

        [Test]
        public void Deserialize_SemicolonSeparatedCsv_ReturnsCorrectModel()
        {
             // Arrange
            var csv = "Name;Hostname;Protocol;Port;Parent\nTestConnection;example.com;RDP;3389;";
            var deserializer = new CsvConnectionsDeserializerMremotengFormat();

            // Act
            var model = deserializer.Deserialize(csv);

            // Assert
            var connections = model.GetRecursiveChildList();
            Assert.That(connections.Count, Is.EqualTo(1));
            var connection = connections.First();
            Assert.That(connection.Name, Is.EqualTo("TestConnection"));
            Assert.That(connection.Hostname, Is.EqualTo("example.com"));
            Assert.That(connection.Port, Is.EqualTo(3389));
        }
    }
}
