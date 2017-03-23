node('windows') {
	def jobDir = pwd()
	def solutionFilePath = "\"${jobDir}\\mRemoteV1.sln\""
	def vsToolsDir = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\Tools"
	def vsExtensionsDir = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow"
	def nunitTestAdapterPath = "C:\\Users\\Administrator\\AppData\\Local\\Microsoft\\VisualStudio\\14.0\\Extensions"

	
	stage ('Clean output dir') {
		bat script: "rmdir /S /Q \"${jobDir}\\Release\" 2>nul", returnStatus: true
	}

	stage ('Checkout Branch') {
    	checkout([
    	    $class: 'GitSCM',
    	    branches: [[name: '*/pipeline_tests']],
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
                bat "\"${vsToolsDir}\\VsDevCmd.bat\" && msbuild.exe /nologo /p:Configuration=\"Release Installer\" /p:Platform=x86 /p:CertPath=\"${env.CODE_SIGNING_CERT}\" /p:CertPassword=${env.MRNG_CERT_PASSWORD} \"${jobDir}\\mRemoteV1.sln\""
                archiveArtifacts artifacts: "Release\\*.msi", caseSensitive: false, onlyIfSuccessful: true, fingerprint: true
            }
        
            stage ('Build mRemoteNG (Portable)') {
                bat "\"${vsToolsDir}\\VsDevCmd.bat\" && msbuild.exe /nologo /p:Configuration=\"Release Portable\" /p:Platform=x86 /p:CertPath=\"${env.CODE_SIGNING_CERT}\" /p:CertPassword=${env.MRNG_CERT_PASSWORD} \"${jobDir}\\mRemoteV1.sln\""
                archiveArtifacts artifacts: "Release\\*.zip", caseSensitive: false, onlyIfSuccessful: true, fingerprint: true
            }
        }
    }
	
	stage ('Run Unit Tests (Normal - MSI)') {
    	bat "\"${vsToolsDir}\\VsDevCmd.bat\" && VSTest.Console.exe /TestAdapterPath:${nunitTestAdapterPath} \"${jobDir}\\mRemoteNGTests\\bin\\Release\\mRemoteNGTests.dll\""
	}

	stage ('Run Unit Tests (Portable)') {
    	bat "\"${vsToolsDir}\\VsDevCmd.bat\" && VSTest.Console.exe /TestAdapterPath:${nunitTestAdapterPath} \"${jobDir}\\mRemoteNGTests\\bin\\Release Portable\\mRemoteNGTests.dll\""
	}
	
    stage ('Publish to GitHub') {
        withCredentials([string(credentialsId: '5443a369-dbe8-42d3-b4e8-04d0b4e9039a', variable: 'GH_AUTH_TOKEN')]) {
            def zipPath = "${jobDir}\\Release\\*.msi"
            def msiPath = "${jobDir}\\Release\\*.zip"
            // because batch files suck at handling newline characters, we have to convert to base64 in groovy and back to text in powershell
            def base64Description = env.ReleaseDescription.bytes.encodeBase64().toString()
            bat "powershell -ExecutionPolicy Bypass -File \"${jobDir}\\Tools\\publish_to_github.ps1\" -Owner \"mRemoteNG\" -Repository \"mRemoteNG\" -ReleaseTitle \"${env.ReleaseTitle}\" -TagName \"${env.TagName}\" -TargetCommitish \"${env.TargetCommit}\" -Description \"${base64Description}\" -IsDraft ${env.IsDraft} -IsPrerelease ${env.IsPreRelease} -ZipFilePath \"${zipPath}\" -MsiFilePath \"${msiPath}\" -AuthToken \"${env.GH_AUTH_TOKEN}\" -DescriptionIsBase64Encoded"
        }
    }
}