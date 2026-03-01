using System;
using System.Collections;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using NUnit.Framework;


namespace mRemoteNGTests.Tools
{
    public class ExternalToolsArgumentParserTests
    {
        private ExternalToolArgumentParser _argumentParser;
        private const string TestString = @"()%!^abc123*<>&|""'\";
        private const string StringAfterMetacharacterEscaping = @"^(^)^%^!^^abc123*^<^>^&^|^""'\";
        private const string StringAfterAllEscaping = @"^(^)^%^!^^abc123*^<^>^&^|\^""'\";
        private const string StringAfterNoEscaping = TestString;
        private const string StringAfterUrlEncoding = @"%28%29%25%21%5Eabc123%2A%3C%3E%26%7C%22%27%5C";
        private const int Port = 9933;
        private const string PortAsString = "9933";
        private const string ProtocolAsString = "RDP";
        private const string SampleCommandString = @"/k echo ()%!^abc123*<>&|""'\";


        [OneTimeSetUp]
        public void Setup()
        {
            var connectionInfo = new ConnectionInfo
            {
                Name = TestString,
                Hostname = TestString,
                Port = Port,
                Protocol = ProtocolType.RDP,
                Username = TestString,
                //Password = TestString.ConvertToSecureString(),
                Password = TestString,
                Domain = TestString,
                Description = TestString,
                MacAddress = TestString,
                UserField = TestString,
                UserField1 = TestString + "1",
                UserField2 = TestString + "2",
                UserField3 = TestString + "3",
                UserField4 = TestString + "4",
                UserField5 = TestString + "5",
                UserField6 = TestString + "6",
                UserField7 = TestString + "7",
                UserField8 = TestString + "8",
                UserField9 = TestString + "9",
                UserField10 = TestString + "10",
                EnvironmentTags = TestString,
                SSHOptions = TestString,
                PuttySession = TestString,
                IPAddress = TestString,
                LoadBalanceInfo = TestString,
                PrivateKeyPath = TestString,
                RDPStartProgram = TestString,
                RDPStartProgramWorkDir = TestString,
                Notes = TestString,
                Panel = TestString,
                OpeningCommand = TestString
            };
            var externalTool = new ExternalTool
            {
                AuthenticationType = TestString,
                AuthenticationUsername = TestString,
                AuthenticationPassword = TestString,
                PrivateKeyFile = TestString,
                Passphrase = TestString
            };
            _argumentParser = new ExternalToolArgumentParser(connectionInfo, externalTool);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _argumentParser = null;
        }

        [TestCaseSource(typeof(ParserTestsDataSource), nameof(ParserTestsDataSource.TestCases))]
        public string ParserTests(string argumentString)
        {
            return _argumentParser.ParseArguments(argumentString);
        }

        [Test]
        public void NullConnectionInfoResultsInEmptyVariables()
        {
            var parser = new ExternalToolArgumentParser(null);
            var parsedText = parser.ParseArguments("test %USERNAME% test");
            Assert.That(parsedText, Is.EqualTo("test  test"));
        }



        private class ParserTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData("%NAME%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-NAME%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!NAME%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+NAME%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%HOSTNAME%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-HOSTNAME%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!HOSTNAME%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+HOSTNAME%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%PORT%").Returns(PortAsString);
                    yield return new TestCaseData("%-PORT%").Returns(PortAsString);
                    yield return new TestCaseData("%!PORT%").Returns(PortAsString);
                    yield return new TestCaseData("%+PORT%").Returns(PortAsString);
                    yield return new TestCaseData("%USERNAME%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-USERNAME%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!USERNAME%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+USERNAME%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%PASSWORD%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-PASSWORD%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!PASSWORD%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+PASSWORD%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%DOMAIN%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-DOMAIN%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!DOMAIN%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+DOMAIN%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%DESCRIPTION%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-DESCRIPTION%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!DESCRIPTION%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+DESCRIPTION%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%MACADDRESS%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-MACADDRESS%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!MACADDRESS%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+MACADDRESS%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%USERFIELD%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-USERFIELD%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!USERFIELD%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+USERFIELD%").Returns(StringAfterUrlEncoding);
                    for (int userFieldNumber = 1; userFieldNumber <= 10; userFieldNumber++)
                    {
                        string suffix = userFieldNumber.ToString();
                        yield return new TestCaseData($"%USERFIELD{suffix}%").Returns(StringAfterAllEscaping + suffix);
                        yield return new TestCaseData($"%-USERFIELD{suffix}%").Returns(StringAfterMetacharacterEscaping + suffix);
                        yield return new TestCaseData($"%!USERFIELD{suffix}%").Returns(StringAfterNoEscaping + suffix);
                        yield return new TestCaseData($"%+USERFIELD{suffix}%").Returns(StringAfterUrlEncoding + suffix);
                    }
                    yield return new TestCaseData("%PROTOCOL%").Returns(ProtocolAsString);
                    yield return new TestCaseData("%-PROTOCOL%").Returns(ProtocolAsString);
                    yield return new TestCaseData("%!PROTOCOL%").Returns(ProtocolAsString);
                    yield return new TestCaseData("%+PROTOCOL%").Returns(ProtocolAsString);
                    yield return new TestCaseData("%ENVIRONMENTTAGS%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-ENVIRONMENTTAGS%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!ENVIRONMENTTAGS%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+ENVIRONMENTTAGS%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%SSHOPTIONS%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-SSHOPTIONS%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!SSHOPTIONS%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+SSHOPTIONS%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%PUTTYSESSION%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-PUTTYSESSION%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!PUTTYSESSION%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+PUTTYSESSION%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%AUTHTYPE%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-AUTHTYPE%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!AUTHTYPE%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+AUTHTYPE%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%AUTHUSERNAME%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-AUTHUSERNAME%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!AUTHUSERNAME%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+AUTHUSERNAME%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%AUTHPASSWORD%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-AUTHPASSWORD%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!AUTHPASSWORD%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+AUTHPASSWORD%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%PRIVATEKEYFILE%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-PRIVATEKEYFILE%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!PRIVATEKEYFILE%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+PRIVATEKEYFILE%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%PASSPHRASE%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-PASSPHRASE%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!PASSPHRASE%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+PASSPHRASE%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%IPADDRESS%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-IPADDRESS%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!IPADDRESS%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+IPADDRESS%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%LOADBALANCEINFO%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-LOADBALANCEINFO%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!LOADBALANCEINFO%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+LOADBALANCEINFO%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%PRIVATEKEYPATH%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-PRIVATEKEYPATH%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!PRIVATEKEYPATH%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+PRIVATEKEYPATH%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%RDPSTARTPROGRAM%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-RDPSTARTPROGRAM%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!RDPSTARTPROGRAM%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+RDPSTARTPROGRAM%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%RDPSTARTPROGRAMWORKDIR%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-RDPSTARTPROGRAMWORKDIR%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!RDPSTARTPROGRAMWORKDIR%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+RDPSTARTPROGRAMWORKDIR%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%NOTES%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-NOTES%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!NOTES%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+NOTES%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%PANEL%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-PANEL%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!PANEL%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+PANEL%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%OPENINGCOMMAND%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-OPENINGCOMMAND%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!OPENINGCOMMAND%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%+OPENINGCOMMAND%").Returns(StringAfterUrlEncoding);
                    yield return new TestCaseData("%%") {TestName = "EmptyVariableTagsNotParsed" }.Returns("%%");
                    yield return new TestCaseData("/k echo %!USERNAME%") { TestName = "ParsingWorksWhenVariableIsNotInFirstPosition" }.Returns(SampleCommandString);
                    yield return new TestCaseData("%COMSPEC%") { TestName = "EnvironmentVariablesParsed" }.Returns(Environment.GetEnvironmentVariable("comspec"));
                    yield return new TestCaseData("%UNSUPPORTEDPARAMETER%") { TestName = "UnsupportedParametersNotParsed" }.Returns("%UNSUPPORTEDPARAMETER%");
                    yield return new TestCaseData(@"\%COMSPEC\%") { TestName = "BackslashEscapedEnvironmentVariablesParsed" }.Returns(Environment.GetEnvironmentVariable("comspec"));
                    yield return new TestCaseData(@"^%COMSPEC^%") { TestName = "ChevronEscapedEnvironmentVariablesNotParsed" }.Returns("%COMSPEC%");
                }
            }
        }

        [Test]
        public void PasswordWithCommaIsNotCaretEscaped()
        {
            // Commas are cmd.exe weak delimiters — caret escaping never protected them.
            // They are now left unescaped; callers must use double-quoting instead.
            var connectionInfo = new ConnectionInfo
            {
                Password = "1234,56789"
            };
            var parser = new ExternalToolArgumentParser(connectionInfo);
            var result = parser.ParseArguments("%PASSWORD%");
            Assert.That(result, Is.EqualTo("1234,56789"));
        }

        [Test]
        public void PasswordWithSemicolonIsNotCaretEscaped()
        {
            // Semicolons are cmd.exe weak delimiters — caret escaping never protected them.
            var connectionInfo = new ConnectionInfo
            {
                Password = "1234;56789"
            };
            var parser = new ExternalToolArgumentParser(connectionInfo);
            var result = parser.ParseArguments("%PASSWORD%");
            Assert.That(result, Is.EqualTo("1234;56789"));
        }

        [Test]
        public void PasswordWithMultipleSpecialCharsIsEscaped()
        {
            // Only & is still caret-escaped (strong shell metacharacter).
            // Comma and semicolon are left raw — protected by quoting at the caller level.
            var connectionInfo = new ConnectionInfo
            {
                Password = "pass,word;test&more"
            };
            var parser = new ExternalToolArgumentParser(connectionInfo);
            var result = parser.ParseArguments("%PASSWORD%");
            Assert.That(result, Is.EqualTo("pass,word;test^&more"));
        }

        [Test]
        public void UserFieldWithNestedVariableIsExpanded()
        {
            var connectionInfo = new ConnectionInfo
            {
                Hostname = "myserver",
                Port = 3389,
                UserField = "%HOSTNAME%:%PORT%"
            };
            var parser = new ExternalToolArgumentParser(connectionInfo);
            var result = parser.ParseArguments("%!USERFIELD%");
            Assert.That(result, Is.EqualTo("myserver:3389"));
        }

        [Test]
        public void UserField1WithNestedVariableIsExpanded()
        {
            var connectionInfo = new ConnectionInfo
            {
                Username = "admin",
                UserField1 = "user=%USERNAME%"
            };
            var parser = new ExternalToolArgumentParser(connectionInfo);
            var result = parser.ParseArguments("%!USERFIELD1%");
            Assert.That(result, Is.EqualTo("user=admin"));
        }

        [Test]
        public void UserFieldWithNoVariablesIsUnchanged()
        {
            var connectionInfo = new ConnectionInfo
            {
                UserField = "plain-value"
            };
            var parser = new ExternalToolArgumentParser(connectionInfo);
            var result = parser.ParseArguments("%!USERFIELD%");
            Assert.That(result, Is.EqualTo("plain-value"));
        }

        [Test]
        public void UserFieldWithSelfReferenceDoesNotInfiniteLoop()
        {
            var connectionInfo = new ConnectionInfo
            {
                UserField = "%USERFIELD%"
            };
            var parser = new ExternalToolArgumentParser(connectionInfo);
            var result = parser.ParseArguments("%!USERFIELD%");
            // Should stop expanding after reaching max depth, returning the last unexpanded value
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void UrlEncodePrefixEncodesSpecialCharactersInPassword()
        {
            // Issue #1515: passwords with :;=/>  break WinSCP URLs without URL encoding
            var connectionInfo = new ConnectionInfo
            {
                Username = "root",
                Hostname = "172.28.3.151",
                Password = "J4Lk:;7=0!S>>/"
            };
            var parser = new ExternalToolArgumentParser(connectionInfo);
            var result = parser.ParseArguments("scp://%+USERNAME%:%+PASSWORD%@%+HOSTNAME%", escapeForShell: false);
            Assert.That(result, Is.EqualTo("scp://root:J4Lk%3A%3B7%3D0%21S%3E%3E%2F@172.28.3.151"));
        }

        [TestCase(ProtocolType.SSH2, "SSH2")]
        [TestCase(ProtocolType.VNC, "VNC")]
        [TestCase(ProtocolType.Telnet, "Telnet")]
        [TestCase(ProtocolType.HTTP, "HTTP")]
        [TestCase(ProtocolType.HTTPS, "HTTPS")]
        [TestCase(ProtocolType.SSH1, "SSH1")]
        [TestCase(ProtocolType.Rlogin, "Rlogin")]
        [TestCase(ProtocolType.RAW, "RAW")]
        [TestCase(ProtocolType.IntApp, "IntApp")]
        [TestCase(ProtocolType.ARD, "ARD")]
        [TestCase(ProtocolType.AnyDesk, "AnyDesk")]
        public void ProtocolTokenReturnsCorrectValueForEachProtocol(ProtocolType protocol, string expected)
        {
            var connectionInfo = new ConnectionInfo { Protocol = protocol };
            var parser = new ExternalToolArgumentParser(connectionInfo);
            var result = parser.ParseArguments("%PROTOCOL%");
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
