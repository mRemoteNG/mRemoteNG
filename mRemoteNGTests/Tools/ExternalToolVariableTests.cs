using mRemoteNG.Connection;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    public class ExternalToolVariableTests
    {
        [Test]
        public void SupportedVariablesIsNotEmpty()
        {
            Assert.That(ExternalToolVariable.SupportedVariables, Is.Not.Empty);
        }

        [Test]
        public void EveryVariableHasNameAndToken()
        {
            foreach (ExternalToolVariable variable in ExternalToolVariable.SupportedVariables)
            {
                Assert.That(variable.Name, Is.Not.Empty);
                Assert.That(variable.Token, Is.EqualTo($"%{variable.Name}%"));
            }
        }

        [Test]
        public void EverySupportedVariableIsResolvedByTheParser()
        {
            var connectionInfo = new ConnectionInfo
            {
                Name = "name",
                Hostname = "host",
                Port = 1234,
                Username = "user",
                Password = "pass",
                Domain = "domain",
                Description = "desc",
                MacAddress = "mac",
                UserField = "userfield"
            };
            var parser = new ExternalToolArgumentParser(connectionInfo);

            foreach (ExternalToolVariable variable in ExternalToolVariable.SupportedVariables)
            {
                // A resolved variable produces output different from the raw token.
                string parsed = parser.ParseArguments(variable.Token);
                Assert.That(parsed, Is.Not.EqualTo(variable.Token),
                    $"Variable {variable.Token} was not resolved by the parser.");
            }
        }

        [Test]
        public void GetVariablePreviewReturnsResolvedValue()
        {
            var connectionInfo = new ConnectionInfo { Hostname = "example.com" };
            var parser = new ExternalToolArgumentParser(connectionInfo);

            Assert.That(parser.GetVariablePreview("Hostname"), Is.EqualTo("example.com"));
        }

        [Test]
        public void GetVariablePreviewReturnsEmptyForUnknownVariable()
        {
            var parser = new ExternalToolArgumentParser(new ConnectionInfo());
            Assert.That(parser.GetVariablePreview("NotARealVariable"), Is.Empty);
        }

        [Test]
        public void GetVariablePreviewReturnsEmptyWhenNoConnection()
        {
            var parser = new ExternalToolArgumentParser(null);
            Assert.That(parser.GetVariablePreview("Hostname"), Is.Empty);
        }
    }
}
