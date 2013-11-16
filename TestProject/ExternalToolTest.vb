Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports mRemoteNG.Tools
Imports mRemoteNG.Connection.PuttySession


'''<summary>
'''This is a test class for ExternalToolTest and is intended
'''to contain all ExternalToolTest Unit Tests
'''</summary>
<TestClass()> _
Public Class ExternalToolTest
    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region


    '''<summary>
    '''A test for ParseArguments
    '''</summary>
    <TestMethod(), _
     DeploymentItem("mRemoteNG.exe")> _
    Public Sub ParseArgumentsTest()
        Dim externalTool As New ExternalTool

        ' ReSharper disable StringLiteralTypo
        externalTool.ConnectionInfo = New Info()
        With externalTool.ConnectionInfo
            .Name = "EMAN"
            .Hostname = "EMANTSOH"
            .Port = 9876
            .Username = "EMANRESU"
            .Password = "DROWSSAP"
            .Domain = "NIAMOD"
            .Description = "NOITPIRCSED"
            .MacAddress = "SSERDDACAM"
            .UserField = "DLEIFRESU"
        End With

        Assert.AreEqual("EMAN, EMANTSOH, 9876, EMANRESU, DROWSSAP, NIAMOD, NOITPIRCSED, SSERDDACAM, DLEIFRESU", externalTool.ParseArguments("%NAME%, %HOSTNAME%, %PORT%, %USERNAME%, %PASSWORD%, %DOMAIN%, %DESCRIPTION%, %MACADDRESS%, %USERFIELD%"))
        ' ReSharper restore StringLiteralTypo

        Assert.AreEqual(Environment.ExpandEnvironmentVariables("%PATH%"), externalTool.ParseArguments("%!PATH%"))

        externalTool.ConnectionInfo.Name = "()%!^""<>&|\""\\"

        Assert.AreEqual("%%", externalTool.ParseArguments("%%"))
        Assert.AreEqual("% %", externalTool.ParseArguments("% %"))
        Assert.AreEqual("%-%", externalTool.ParseArguments("%-%"))
        Assert.AreEqual("%!%", externalTool.ParseArguments("%!%"))
        Assert.AreEqual("%^%", externalTool.ParseArguments("%^%"))
        Assert.AreEqual("%%%", externalTool.ParseArguments("%%%"))
        Assert.AreEqual("%foobar%", externalTool.ParseArguments("%foobar%"))
        Assert.AreEqual("%-foobar%", externalTool.ParseArguments("%-foobar%"))
        Assert.AreEqual("%!foobar%", externalTool.ParseArguments("%!foobar%"))
        Assert.AreEqual("%-!^\", externalTool.ParseArguments("%-!^\"))

        Assert.AreEqual("^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("%NAME%"))
        Assert.AreEqual("^(^)^%^!^^^""^<^>^&^|\^""\\", externalTool.ParseArguments("%-NAME%"))
        Assert.AreEqual("()%!^""<>&|\""\\", externalTool.ParseArguments("%!NAME%"))

        Assert.AreEqual("^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("%name%"))
        Assert.AreEqual("%^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("%%name%"))
        Assert.AreEqual("^(^)^%^!^^\^""^<^>^&^|\\\^""\\%", externalTool.ParseArguments("%name%%"))
        Assert.AreEqual("^(^)^%^!^^\^""^<^>^&^|\\\^""\\ ^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("%name% %name%"))

        Assert.AreEqual("%-^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("%-%name%"))
        Assert.AreEqual("^(^)^%^!^^\^""^<^>^&^|\\\^""\\-%", externalTool.ParseArguments("%name%-%"))
        Assert.AreEqual("%!^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("%!%name%"))
        Assert.AreEqual("^(^)^%^!^^\^""^<^>^&^|\\\^""\\!%", externalTool.ParseArguments("%name%!%"))

        Assert.AreEqual("\^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("\%NAME%"))
        Assert.AreEqual("\^(^)^%^!^^\^""^<^>^&^|\\\^""\\NAME%", externalTool.ParseArguments("\%NAME%NAME%"))
        Assert.AreEqual("%NAME\%", externalTool.ParseArguments("%NAME\%"))

        Assert.AreEqual("""^(^)^%^!^^\^""^<^>^&^|\\\^""\\\\""", externalTool.ParseArguments("""%NAME%"""))
        Assert.AreEqual("""^(^)^%^!^^^""^<^>^&^|\^""\\""", externalTool.ParseArguments("""%-NAME%"""))
        Assert.AreEqual("""()%!^""<>&|\""\\""", externalTool.ParseArguments("""%!NAME%"""))

        Assert.AreEqual("""^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("""%NAME%"))
        Assert.AreEqual("""^(^)^%^!^^^""^<^>^&^|\^""\\", externalTool.ParseArguments("""%-NAME%"))
        Assert.AreEqual("""()%!^""<>&|\""\\", externalTool.ParseArguments("""%!NAME%"))

        Assert.AreEqual("^(^)^%^!^^\^""^<^>^&^|\\\^""\\\\""", externalTool.ParseArguments("%NAME%"""))
        Assert.AreEqual("^(^)^%^!^^^""^<^>^&^|\^""\\""", externalTool.ParseArguments("%-NAME%"""))
        Assert.AreEqual("()%!^""<>&|\""\\""", externalTool.ParseArguments("%!NAME%"""))

        Assert.AreEqual(Environment.ExpandEnvironmentVariables("%USERNAME%"), externalTool.ParseArguments("\%USERNAME\%"))
        Assert.AreEqual(Environment.ExpandEnvironmentVariables("%USERNAME%"), externalTool.ParseArguments("\%-USERNAME\%"))
        Assert.AreEqual(Environment.ExpandEnvironmentVariables("%USERNAME%"), externalTool.ParseArguments("\%!USERNAME\%"))

        Assert.AreEqual("\^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("\%NAME%"))
        Assert.AreEqual("%NAME\%", externalTool.ParseArguments("%NAME\%"))
        Assert.AreEqual("\\%NAME\\%", externalTool.ParseArguments("\\%NAME\\%"))
        Assert.AreEqual("\\\%NAME\\\%", externalTool.ParseArguments("\\\%NAME\\\%"))
        Assert.AreEqual("\\\\%NAME\\\\%", externalTool.ParseArguments("\\\\%NAME\\\\%"))
        Assert.AreEqual("\\\\\%NAME\\\\\%", externalTool.ParseArguments("\\\\\%NAME\\\\\%"))

        Assert.AreEqual("%NAME%", externalTool.ParseArguments("^%NAME^%"))
        Assert.AreEqual("%-NAME%", externalTool.ParseArguments("^%-NAME^%"))
        Assert.AreEqual("\%NAME\%", externalTool.ParseArguments("\^%NAME\^%"))
        Assert.AreEqual("\%-NAME\%", externalTool.ParseArguments("\^%-NAME\^%"))

        Assert.AreEqual("^%NAME^%", externalTool.ParseArguments("^^%NAME^^%"))
        Assert.AreEqual("^%-NAME^%", externalTool.ParseArguments("^^%-NAME^^%"))
        Assert.AreEqual("^^^^%NAME^^^^%", externalTool.ParseArguments("^^^^^%NAME^^^^^%"))
        Assert.AreEqual("^^^^%-NAME^^^^%", externalTool.ParseArguments("^^^^^%-NAME^^^^^%"))
        Assert.AreEqual("^^^^%!NAME^^^^%", externalTool.ParseArguments("^^^^^%!NAME^^^^^%"))

        Assert.AreEqual("blah%blah", externalTool.ParseArguments("blah%blah"))
        Assert.AreEqual("blah^%blah", externalTool.ParseArguments("blah^%blah"))
        Assert.AreEqual("blah^^%blah", externalTool.ParseArguments("blah^^%blah"))
        Assert.AreEqual("blah^^^%blah", externalTool.ParseArguments("blah^^^%blah"))

        Assert.AreEqual("^^^^%-NAME^^^^% ^(^)^%^!^^\^""^<^>^&^|\\\^""\\", externalTool.ParseArguments("^^^^^%-NAME^^^^^% %NAME%"))
    End Sub
End Class
