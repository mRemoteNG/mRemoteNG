using mRemoteNG.Connection;
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
        private const int Port = 9933;
        private const string PortAsString = "9933";
        private const string SampleCommandString = @"/k echo ()%!^abc123*<>&|""'\";


        [OneTimeSetUp]
        public void Setup()
        {
            var connectionInfo = new ConnectionInfo
            {
                Name = TestString,
                Hostname = TestString,
                Port = Port,
                Username = TestString,
                Password = TestString,
                Domain = TestString,
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


        [TestCase("%NAME%", ExpectedResult = StringAfterAllEscaping)]
        [TestCase("%-NAME%", ExpectedResult = StringAfterMetacharacterEscaping)]
        [TestCase("%!NAME%", ExpectedResult = StringAfterNoEscaping)]

        [TestCase("%HOSTNAME%", ExpectedResult = StringAfterAllEscaping)]
        [TestCase("%-HOSTNAME%", ExpectedResult = StringAfterMetacharacterEscaping)]
        [TestCase("%!HOSTNAME%", ExpectedResult = StringAfterNoEscaping)]

        [TestCase("%PORT%", ExpectedResult = PortAsString)]
        [TestCase("%-PORT%", ExpectedResult = PortAsString)]
        [TestCase("%!PORT%", ExpectedResult = PortAsString)]

        [TestCase("%USERNAME%", ExpectedResult = StringAfterAllEscaping)]
        [TestCase("%-USERNAME%", ExpectedResult = StringAfterMetacharacterEscaping)]
        [TestCase("%!USERNAME%", ExpectedResult = StringAfterNoEscaping)]

        [TestCase("%PASSWORD%", ExpectedResult = StringAfterAllEscaping)]
        [TestCase("%-PASSWORD%", ExpectedResult = StringAfterMetacharacterEscaping)]
        [TestCase("%!PASSWORD%", ExpectedResult = StringAfterNoEscaping)]

        [TestCase("%DOMAIN%", ExpectedResult = StringAfterAllEscaping)]
        [TestCase("%-DOMAIN%", ExpectedResult = StringAfterMetacharacterEscaping)]
        [TestCase("%!DOMAIN%", ExpectedResult = StringAfterNoEscaping)]

        [TestCase("%DESCRIPTION%", ExpectedResult = StringAfterAllEscaping)]
        [TestCase("%-DESCRIPTION%", ExpectedResult = StringAfterMetacharacterEscaping)]
        [TestCase("%!DESCRIPTION%", ExpectedResult = StringAfterNoEscaping)]

        [TestCase("%MACADDRESS%", ExpectedResult = StringAfterAllEscaping)]
        [TestCase("%-MACADDRESS%", ExpectedResult = StringAfterMetacharacterEscaping)]
        [TestCase("%!MACADDRESS%", ExpectedResult = StringAfterNoEscaping)]

        [TestCase("%USERFIELD%", ExpectedResult = StringAfterAllEscaping)]
        [TestCase("%-USERFIELD%", ExpectedResult = StringAfterMetacharacterEscaping)]
        [TestCase("%!USERFIELD%", ExpectedResult = StringAfterNoEscaping)]

        [TestCase("%%", ExpectedResult = "%%", TestName = "EmptyVariableTagsNotParsed")]
        [TestCase("/k echo %!USERNAME%", ExpectedResult = SampleCommandString, TestName = "ParsingWorksWhenVariableIsNotInFirstPosition")]
        [TestCase("%COMSPEC%", ExpectedResult = @"C:\Windows\system32\cmd.exe", TestName = "EnvironmentVariablesParsed")]
        [TestCase("%UNSUPPORTEDPARAMETER%", ExpectedResult = "%UNSUPPORTEDPARAMETER%", TestName = "UnsupportedParametersNotParsed")]
        [TestCase(@"\%COMSPEC\%", ExpectedResult = @"C:\Windows\system32\cmd.exe", TestName = "BackslashEscapedEnvironmentVariablesParsed")]
        [TestCase(@"^%COMSPEC^%", ExpectedResult = "%COMSPEC%", TestName = "ChevronEscapedEnvironmentVariablesNotParsed")]
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
    }
}