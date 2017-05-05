node('windows') {
	def jobDir = pwd()
	def solutionFilePath = "\"${jobDir}\\mRemoteV1.sln\""
	def vsToolsDir = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\Tools"
	def vsExtensionsDir = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow"
	def nunitConsolePath = "${jobDir}\\packages\\NUnit.ConsoleRunner.3.6.1\\tools\\nunit3-console.exe"
	def openCoverPath = "${jobDir}\\packages\\OpenCover.4.6.519\\tools\\OpenCover.Console.exe"
	def reportGeneratorPath = "${jobDir}\\packages\\ReportGenerator.2.5.7\\tools\\ReportGenerator.exe"
	def testResultFilePrefix = "TestResult"
	def testResultFileNormal = "${testResultFilePrefix}_UnitTests_normal.xml"
	def testResultFilePortable = "${testResultFilePrefix}_UnitTests_portable.xml"
	def testResultFileAcceptance = "${testResultFilePrefix}_AcceptanceTests.xml"
	def codeCoverageHtml = "CodeCoverageReport.html"
	
	stage ('Checkout Branch') {
		checkout scm
	}

	stage ('Restore NuGet Packages') {
		def nugetPath = "C:\\nuget.exe"
		bat "${nugetPath} restore ${solutionFilePath}"
	}

	stage ('Build mRemoteNG (Normal)') {
		bat "\"${vsToolsDir}\\VsDevCmd.bat\" && msbuild.exe /nologo /p:Platform=x86 \"${jobDir}\\mRemoteV1.sln\""
	}

	stage ('Build mRemoteNG (Portable)') {
		bat "\"${vsToolsDir}\\VsDevCmd.bat\" && msbuild.exe /nologo /p:Configuration=\"Debug Portable\";Platform=x86 \"${jobDir}\\mRemoteV1.sln\""
	}

	stage ('Run Unit Tests (Normal)') {
		bat "\"${nunitConsolePath}\" \"${jobDir}\\mRemoteNGTests\\bin\\debug\\mRemoteNGTests.dll\" --result=${testResultFileNormal} --x86"
	}
	
	stage ('Run Unit Tests (Portable)') {
		bat "\"${nunitConsolePath}\" \"${jobDir}\\mRemoteNGTests\\bin\\debug portable\\mRemoteNGTests.dll\" --result=${testResultFilePortable} --x86"
	}
	
	stage ('Run Acceptance Tests') {
		bat "\"${nunitConsolePath}\" \"${jobDir}\\mRemoteNG.Specs\\bin\\debug\\mRemoteNG.Specs.dll\" --result=${testResultFileAcceptance} --x86"
	}
	
	stage ('Generate Code Coverage Report') {
		def coverageReport = "code_coverage_report.xml"
		bat "\"${openCoverPath}\" -register:user -target:\"${nunitConsolePath}\" -targetargs:\"\"${jobDir}\\mRemoteNGTests\\bin\\debug\\mRemoteNGTests.dll\" --x86\" -output:\"${coverageReport}\""
		bat "\"${reportGeneratorPath}\" -reports:\"${jobDir}\\${coverageReport}\" -targetdir:\"${jobDir}\\reports\" -reporttypes:HtmlSummary"
	}

	stage ('Upload test results') {
    	nunit testResultsPattern: "${testResultFilePrefix}*.xml"
    	publishHTML([allowMissing: false, alwaysLinkToLastBuild: false, keepAll: false, reportDir: 'reports', reportFiles: 'summary.htm', reportName: 'Code Coverage Report', reportTitles: ''])
	}
}
