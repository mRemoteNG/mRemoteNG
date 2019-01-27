using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;


namespace mRemoteNGTests.Tools
{
    public class ExternalToolsArgumentParserTests
    {
        private ExternalToolArgumentParser _argumentParser;
        private const string TestString = @"()%!^abc123*<>&|""'\";
        private const string StringAfterMetacharacterEscaping = @"^(^)^%^!^^abc123*^<^>^&^|^""'\";
        private const string StringAfterAllEscaping = @"^(^)^%^!^^abc123*^<^>^&^|\^""'\";
        private const string StringAfterNoEscaping = TestString;
        private const int Port = 9933;
        private const string PortAsString = "9933";
        private const string SampleCommandString = @"/k echo ()%!^abc123*<>&|""'\";
        private const string Username = "myuser1";
        private const string Domain = "dom1";
        private const string Password = "pass123";
        private static readonly Guid CredentialGuid = Guid.Parse("D1C198F4-57D7-4F48-808F-E1724BECB291");


        [OneTimeSetUp]
        public void Setup()
        {
            var creds = new[]
            {
                new CredentialRecord
                {
                    Username = TestString,
                    Domain = TestString,
                    Password = TestString.ConvertToSecureString()
                },
                new CredentialRecord(CredentialGuid)
                {
                    Username = Username,
                    Domain = Domain,
                    Password = Password.ConvertToSecureString()
                }
            };

            var credentialService = Substitute.For<ICredentialService>();
            credentialService.GetEffectiveCredentialRecord(null)
                .ReturnsForAnyArgs(info => creds.FirstOrDefault(r => r.Id == info.Arg<Optional<Guid>>().FirstOrDefault()));
            credentialService.GetCredentialRecords()
                .Returns(info => creds);

            var connectionInfo = new ConnectionInfo
            {
                Name = TestString,
                Hostname = TestString,
                Port = Port,
                CredentialRecord = creds[0],
                Description = TestString,
                MacAddress = TestString,
                UserField = TestString
            };
            _argumentParser = new ExternalToolArgumentParser(connectionInfo, credentialService);
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
            var parser = new ExternalToolArgumentParser(null, Substitute.For<ICredentialService>());
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
                    yield return new TestCaseData("%HOSTNAME%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-HOSTNAME%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!HOSTNAME%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%PORT%").Returns(PortAsString);
                    yield return new TestCaseData("%-PORT%").Returns(PortAsString);
                    yield return new TestCaseData("%!PORT%").Returns(PortAsString);
                    yield return new TestCaseData("%USERNAME%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-USERNAME%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!USERNAME%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%PASSWORD%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-PASSWORD%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!PASSWORD%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%DOMAIN%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-DOMAIN%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!DOMAIN%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%DESCRIPTION%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-DESCRIPTION%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!DESCRIPTION%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%MACADDRESS%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-MACADDRESS%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!MACADDRESS%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%USERFIELD%").Returns(StringAfterAllEscaping);
                    yield return new TestCaseData("%-USERFIELD%").Returns(StringAfterMetacharacterEscaping);
                    yield return new TestCaseData("%!USERFIELD%").Returns(StringAfterNoEscaping);
                    yield return new TestCaseData("%%") {TestName = "EmptyVariableTagsNotParsed" }.Returns("%%");
                    yield return new TestCaseData("/k echo %!USERNAME%") { TestName = "ParsingWorksWhenVariableIsNotInFirstPosition" }.Returns(SampleCommandString);
                    yield return new TestCaseData("%COMSPEC%") { TestName = "EnvironmentVariablesParsed" }.Returns(Environment.GetEnvironmentVariable("comspec"));
                    yield return new TestCaseData("%UNSUPPORTEDPARAMETER%") { TestName = "UnsupportedParametersNotParsed" }.Returns("%UNSUPPORTEDPARAMETER%");
                    yield return new TestCaseData(@"\%COMSPEC\%") { TestName = "BackslashEscapedEnvironmentVariablesParsed" }.Returns(Environment.GetEnvironmentVariable("comspec"));
                    yield return new TestCaseData(@"^%COMSPEC^%") { TestName = "ChevronEscapedEnvironmentVariablesNotParsed" }.Returns("%COMSPEC%");

                    // specific credential record tests
                    yield return new TestCaseData(@"%USERNAME:D1C198F4-57D7-4F48-808F-E1724BECB291%").Returns(Username);
                    yield return new TestCaseData(@"%USERNAME:D1C198F457D74F48808FE1724BECB291%").Returns(Username);
                    yield return new TestCaseData(@"%USERNAME:D1C198F%").Returns(Username);
                    yield return new TestCaseData(@"%DOMAIN:D1C198F4-57D7-4F48-808F-E1724BECB291%").Returns(Domain);
                    yield return new TestCaseData(@"%DOMAIN:D1C198F457D74F48808FE1724BECB291%").Returns(Domain);
                    yield return new TestCaseData(@"%DOMAIN:D1C198F%").Returns(Domain);
                    yield return new TestCaseData(@"%PASSWORD:D1C198F4-57D7-4F48-808F-E1724BECB291%").Returns(Password);
                    yield return new TestCaseData(@"%PASSWORD:D1C198F457D74F48808FE1724BECB291%").Returns(Password);
                    yield return new TestCaseData(@"%PASSWORD:D1C198F%").Returns(Password);
                }
            }
        }
    }
}