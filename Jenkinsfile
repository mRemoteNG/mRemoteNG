node('windows') {
	def jobDir = pwd()
	def solutionFilePath = "\"${jobDir}\\mRemoteV1.sln\""
	def vsToolsDir = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\Tools"
	def vsExtensionsDir = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow"
	
	stage 'Checkout Branch'
	checkout scm

	stage 'Restore NuGet Packages'
	def nugetPath = "C:\\nuget.exe"
	bat "${nugetPath} restore ${solutionFilePath}"

	stage 'Build mRemoteNG (Normal)'
	bat "\"${vsToolsDir}\\VsDevCmd.bat\" && msbuild.exe /nologo /p:Platform=x86 \"${jobDir}\\mRemoteV1.sln\""

	stage 'Build mRemoteNG (Portable)'
	bat "\"${vsToolsDir}\\VsDevCmd.bat\" && msbuild.exe /nologo /p:Configuration=\"Debug Portable\";Platform=x86 \"${jobDir}\\mRemoteV1.sln\""

	stage 'Run Unit Tests'
	def nunitTestAdapterPath = "C:\\Users\\Administrator\\AppData\\Local\\Microsoft\\VisualStudio\\14.0\\Extensions"
	bat "\"${vsToolsDir}\\VsDevCmd.bat\" && VSTest.Console.exe /TestAdapterPath:${nunitTestAdapterPath} \"${jobDir}\\mRemoteNGTests\\bin\\debug\\mRemoteNGTests.dll\""
}
