using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;


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


        [OneTimeSetUp]
        public void Setup()
        {
            var credential = new CredentialRecord
            {
                Username = TestString,
                Domain = TestString,
                Password = TestString.ConvertToSecureString()
            };

            var credRepo = Substitute.For<ICredentialRepository>();
            credRepo.CredentialRecords.Returns(info => new List<ICredentialRecord> {credential});
            Runtime.CredentialService.AddRepository(credRepo);

            var connectionInfo = new ConnectionInfo
            {
                Name = TestString,
                Hostname = TestString,
                Port = Port,
                CredentialRecord = credential,
                Description = TestString,
                MacAddress = TestString,
                UserField = TestString
            };
            _argumentParser = new ExternalToolArgumentParser(connectionInfo);
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
                }
            }
    }
    }
}