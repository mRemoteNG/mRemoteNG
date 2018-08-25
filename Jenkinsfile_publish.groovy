node('windows') {
	def jobDir = pwd()
	def solutionFilePath = "\"${jobDir}\\mRemoteV1.sln\""
    def msBuild = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Community\\MSBuild\\15.0\\Bin\\msbuild.exe"
    def nunitConsolePath = "${jobDir}\\packages\\NUnit.ConsoleRunner.3.7.0\\tools\\nunit3-console.exe"
    def openCoverPath = "${jobDir}\\packages\\OpenCover.4.6.519\\tools\\OpenCover.Console.exe"
    def testResultFilePrefix = "TestResult"
    def testResultFileNormal = "${testResultFilePrefix}_UnitTests_normal.xml"
    def testResultFilePortable = "${testResultFilePrefix}_UnitTests_portable.xml"
    def coverageReport = "code_coverage_report.xml"

	
	stage ('Clean output dir') {
		bat script: "rmdir /S /Q \"${jobDir}\\Release\" 2>nul", returnStatus: true
	}

	stage ('Checkout Branch') {
    	checkout([
    	    $class: 'GitSCM',
    	    branches: [[name: '*/${TargetBranch}']],
            doGenerateSubmoduleConfigurations: false, 
            extensions: [],
            submoduleCfg: [], 
            userRemoteConfigs: [[
                credentialsId: '9c3fbff4-5b90-402f-a298-00e607fcec87', 
                url: 'https://github.com/mRemoteNG/mRemoteNG.git'
            ]]
        ])
	}

	stage ('Restore NuGet Packages') {
    	def nugetPath = "C:\\nuget.exe"
    	bat "${nugetPath} restore ${solutionFilePath}"
	}

    withCredentials([file(credentialsId: '9b674d57-6792-48e3-984a-4d1bab2abb64', variable: 'CODE_SIGNING_CERT')]) {
        withCredentials([usernamePassword(credentialsId: '05b7449b-05c0-490f-8661-236242526e62', passwordVariable: 'MRNG_CERT_PASSWORD', usernameVariable: 'NO_USERNAME')]) {
            stage ('Build mRemoteNG (Normal - MSI)') {
                bat "\"${msBuild}\" /nologo /t:Clean,Build /p:Configuration=\"Release Installer\" /p:Platform=x86 /p:CertPath=\"${env.CODE_SIGNING_CERT}\" /p:CertPassword=${env.MRNG_CERT_PASSWORD} \"${jobDir}\\mRemoteV1.sln\""
                archiveArtifacts artifacts: "Release\\*.msi", caseSensitive: false, onlyIfSuccessful: true, fingerprint: true
            }
        
            stage ('Build mRemoteNG (Portable)') {
                bat "\"${msBuild}\" /nologo /t:Clean,Build /p:Configuration=\"Release Portable\" /p:Platform=x86 /p:CertPath=\"${env.CODE_SIGNING_CERT}\" /p:CertPassword=${env.MRNG_CERT_PASSWORD} \"${jobDir}\\mRemoteV1.sln\""
                archiveArtifacts artifacts: "Release\\*.zip", caseSensitive: false, onlyIfSuccessful: true, fingerprint: true
            }
        }
    }
	
    stage ('Run Unit Tests (Normal - MSI)') {
        bat "\"${nunitConsolePath}\" \"${jobDir}\\mRemoteNGTests\\bin\\release\\mRemoteNGTests.dll\" --result=${testResultFileNormal} --x86"
    }
    
    stage ('Run Unit Tests (Portable)') {
        bat "\"${nunitConsolePath}\" \"${jobDir}\\mRemoteNGTests\\bin\\release portable\\mRemoteNGTests.dll\" --result=${testResultFilePortable} --x86"
    }

    stage ('Generate UpdateCheck Files') {
        bat "powershell -ExecutionPolicy Bypass -File \"${jobDir}\\Tools\\create_upg_chk_files.ps1\" -TagName \"${env.TagName}\" -UpdateChannel \"${env.UpdateChannel}\""
        archiveArtifacts artifacts: "Release\\*.txt", caseSensitive: false, onlyIfSuccessful: true
    }
	
    stage ('Publish to GitHub') {
        withCredentials([string(credentialsId: '5443a369-dbe8-42d3-b4e8-04d0b4e9039a', variable: 'GH_AUTH_TOKEN')]) {
            def releaseFolder = "${jobDir}\\Release"
            // because batch files suck at handling newline characters, we have to convert to base64 in groovy and back to text in powershell
            def base64Description = env.ReleaseDescription.bytes.encodeBase64().toString()
            bat "powershell -ExecutionPolicy Bypass -File \"${jobDir}\\Tools\\publish_to_github.ps1\" -Owner \"mRemoteNG\" -Repository \"mRemoteNG\" -ReleaseTitle \"${env.ReleaseTitle}\" -TagName \"${env.TagName}\" -TargetCommitish \"${env.TargetBranch}\" -Description \"${base64Description}\" -IsDraft ${env.IsDraft} -IsPrerelease ${env.IsPreRelease} -ReleaseFolderPath \"${releaseFolder}\" -AuthToken \"${env.GH_AUTH_TOKEN}\" -DescriptionIsBase64Encoded"
        }
    }
}