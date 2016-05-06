node('windows') {
	def jobDir = pwd()
	def vsToolsDir = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\Tools"
	def vsExtensionsDir = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow"
	
	stage 'Checkout Branch'
	def gitUrl = "https://github.com/mRemoteNG/mRemoteNG.git"
	def branchName = GetBranchName()
	echo "BranchName: ${branchName}"
	git([url: gitUrl, branch: branchName])

	stage 'Build mRemoteNG'
	bat "\"${vsToolsDir}\\VsDevCmd.bat\" && msbuild.exe /nologo \"${jobDir}\\mRemoteV1.sln\""

	stage 'Run Unit Tests'
	def nunitTestAdapterPath = "C:\\Users\\Administrator\\AppData\\Local\\Microsoft\\VisualStudio\\14.0\\Extensions"
	bat "\"${vsToolsDir}\\VsDevCmd.bat\" && VSTest.Console.exe /TestAdapterPath:${nunitTestAdapterPath} \"${jobDir}\\mRemoteNGTests\\bin\\debug\\mRemoteNGTests.dll\""
}
def GetBranchName() {
	def jobDir = pwd()
	echo "JobDir: ${jobDir}"
	def patternToUse = ""
	def linuxPattern = "/([a-zA-Z0-9\\-]*)(@[0-9])*\$"
	def windowsPattern = "\\\\([a-zA-Z0-9\\-]*)(@[0-9])*\$"
	echo "isUnix: ${isUnix()}"
	if (isUnix()) {
		patternToUse = linuxPattern
	} else {
		patternToUse =  windowsPattern
	}
	echo "PatternToUse: ${patternToUse}"
	java.util.regex.Matcher matcher = jobDir =~ patternToUse
	echo "Ran the matcher"
	matcher ? matcher[0][1] : null
}