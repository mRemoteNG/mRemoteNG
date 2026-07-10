using System.Linq;
using mRemoteNG.Connection.Protocol.SshDotNet;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol.SshDotNet
{
    [TestFixture]
    [Category("Unit")]
    public class PortForwardRuleParserTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void ParseRules_ReturnsEmpty_WhenNullOrWhitespace(string input)
        {
            Assert.That(PortForwardRuleParser.ParseRules(input), Is.Empty);
        }

        [Test]
        public void ParseRules_ParsesLocalForward()
        {
            var rules = PortForwardRuleParser.ParseRules("L:8080:example.com:80");

            Assert.That(rules, Has.Count.EqualTo(1));
            var rule = rules[0];
            Assert.That(rule.Kind, Is.EqualTo(PortForwardKind.Local));
            Assert.That(rule.BindHost, Is.EqualTo("127.0.0.1"));
            Assert.That(rule.BindPort, Is.EqualTo(8080u));
            Assert.That(rule.Host, Is.EqualTo("example.com"));
            Assert.That(rule.Port, Is.EqualTo(80u));
        }

        [Test]
        public void ParseRules_ParsesRemoteForward()
        {
            var rules = PortForwardRuleParser.ParseRules("R:9090:localhost:3000");

            Assert.That(rules, Has.Count.EqualTo(1));
            var rule = rules[0];
            Assert.That(rule.Kind, Is.EqualTo(PortForwardKind.Remote));
            Assert.That(rule.BindHost, Is.EqualTo("0.0.0.0"));
            Assert.That(rule.BindPort, Is.EqualTo(9090u));
            Assert.That(rule.Host, Is.EqualTo("localhost"));
            Assert.That(rule.Port, Is.EqualTo(3000u));
        }

        [Test]
        public void ParseRules_ParsesDynamicForward()
        {
            var rules = PortForwardRuleParser.ParseRules("D:1080");

            Assert.That(rules, Has.Count.EqualTo(1));
            var rule = rules[0];
            Assert.That(rule.Kind, Is.EqualTo(PortForwardKind.Dynamic));
            Assert.That(rule.BindHost, Is.EqualTo("127.0.0.1"));
            Assert.That(rule.BindPort, Is.EqualTo(1080u));
            Assert.That(rule.Host, Is.Null);
        }

        [Test]
        public void ParseRules_IsCaseInsensitiveForRuleType()
        {
            var rules = PortForwardRuleParser.ParseRules("l:8080:example.com:80;d:1080");

            Assert.That(rules.Select(r => r.Kind),
                Is.EqualTo(new[] { PortForwardKind.Local, PortForwardKind.Dynamic }));
        }

        [Test]
        public void ParseRules_ParsesMultipleRules_AndTrimsAndIgnoresEmptyEntries()
        {
            var rules = PortForwardRuleParser.ParseRules(" L:8080:a:80 ; ; R:9090:b:90 ;D:1080;");

            Assert.That(rules.Select(r => r.Kind),
                Is.EqualTo(new[] { PortForwardKind.Local, PortForwardKind.Remote, PortForwardKind.Dynamic }));
        }

        [TestCase("X:1:2:3")]            // unrecognized type
        [TestCase("L:8080")]            // too few parts for L
        [TestCase("L:notaport:a:80")]   // non-numeric port
        [TestCase("D:notaport")]        // non-numeric dynamic port
        [TestCase("D:1080:extra")]      // too many parts for D
        [TestCase("nopartsep")]         // no ':' separator
        public void ParseRules_SkipsInvalidRules(string invalidRule)
        {
            Assert.That(PortForwardRuleParser.ParseRules(invalidRule), Is.Empty);
        }

        [Test]
        public void ParseRules_KeepsValidRules_AndSkipsInvalidOnes()
        {
            var rules = PortForwardRuleParser.ParseRules("L:8080:a:80;BOGUS;D:1080");

            Assert.That(rules.Select(r => r.Kind),
                Is.EqualTo(new[] { PortForwardKind.Local, PortForwardKind.Dynamic }));
        }
    }
}
