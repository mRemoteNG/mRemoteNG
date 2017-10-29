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
	def coverageReport = "code_coverage_report.xml"
	def codeCoverageHtml = "CodeCoverageReport.html"
	
	stage ('Checkout Branch') {
		checkout scm
		bat "del /Q \"${jobDir}\\${testResultFilePrefix}*.xml\""
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

	stage ('Run Unit Tests (Normal, w/coverage)') {
		try {
			bat "\"${openCoverPath}\" -register:user -target:\"${nunitConsolePath}\" -targetargs:\"\"${jobDir}\\mRemoteNGTests\\bin\\debug\\mRemoteNGTests.dll\" --result=${testResultFileNormal} --x86\" -output:\"${coverageReport}\""
		}
		catch (ex) {
			nunit testResultsPattern: "${testResultFilePrefix}*.xml"
			throw ex
		}
	}
	
	stage ('Run Unit Tests (Portable)') {
		try {
			bat "\"${nunitConsolePath}\" \"${jobDir}\\mRemoteNGTests\\bin\\debug portable\\mRemoteNGTests.dll\" --result=${testResultFilePortable} --x86"
		}
		catch (ex) {
			nunit testResultsPattern: "${testResultFilePrefix}*.xml"
			throw ex
		}
	}
	
	stage ('Run Acceptance Tests') {
		try {
			bat "\"${nunitConsolePath}\" \"${jobDir}\\mRemoteNG.Specs\\bin\\debug\\mRemoteNG.Specs.dll\" --result=${testResultFileAcceptance} --x86"
		}
		catch (ex) {
			nunit testResultsPattern: "${testResultFilePrefix}*.xml"
			throw ex
		}
	}
}
