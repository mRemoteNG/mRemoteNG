
# Load Assembly
Add-Type -AssemblyName System.Windows.Forms

# Load custom functions

#region New-MessageBox()
function New-MessageBox {
<#
	.SYNOPSIS
		Open a new windows forms message box dialog

	.DESCRIPTION
		Ability to open a new windows message box dialog

	.PARAMETER Message
		Specifies the window message

	.PARAMETER Title
		Specifies the window title

	.PARAMETER Button
	.PARAMETER Buttons		
		Specifies the window button(s)

	.PARAMETER Icon
		Specifies the window message box icon

	.EXAMPLE
		New-WFMessageBox -Title 'Test' -Message 'just a message'
		New-WFMessageBox -Title 'Test' -Message 'just a message' -Buttons OKCancel -Icon Error
		New-WFMessageBox -Title 'Test' -Message 'just a message' -Buttons YesNo -Icon Information
#>
	PARAM
	(
		[Parameter(Mandatory = $true,  Position=0, HelpMessage='Specifies the window message')]
		[String]$Message,

		[Parameter(Mandatory = $false,  Position=1, HelpMessage='Specifies the window title')]
		[String]$Title,

		[alias("Buttons")]
		[Parameter(Mandatory = $false,  Position=2, HelpMessage='Specifies the window button(s)')]
		[System.Windows.Forms.MessageBoxButtons] $Button = "OK",

		[Parameter(Mandatory = $false,  Position=3, HelpMessage='Specifies the window message box icon')]
		[System.Windows.Forms.MessageBoxIcon] $Icon = "None"
		

	) #end Param
	BEGIN {
		Add-Type -AssemblyName System.Windows.Forms
	} #end Begin
	PROCESS {
		$Dialog = $null

		$Dialog = [System.Windows.Forms.MessageBox]::Show($Message, $Title, $Button, $Icon)

	} #end Process
	END	{
		$Dialog.ToString() 
	} #end End
}
#endregion New-MessageBox()

#region Wait-KeyDown()
function Wait-KeyDown {
<#
SYNOPSIS
    Waits for the user to press any key before exiting.

SYNTAX
    Wait-KeyDown

DESCRIPTION
    The Wait-KeyDown function displays a message prompting the user to press any key. Once a key is pressed, the function exits.
#>
	param ()
	process {
		Write-Host 
		Write-Host "Press any key to exit..." -ForegroundColor White
		$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
	}
} 
#endregion Wait-KeyDown()

#region Test-Python()
function Test-Python {
<#
SYNOPSIS
    Tests if Python is installed and accessible in the current environment.

SYNTAX
    Test-Python

DESCRIPTION
    The Test-Python function checks if Python is installed and available in the current environment. It attempts to retrieve the Python version information and returns $true if Python is found, otherwise $false.

OUTPUTS
    System.Boolean
        Returns $true if Python is installed and accessible; otherwise, returns $false.

#>
	param ()
	process {
		$version  = $null
		$version = Python --version

		if ($version) {
			return $true
		} 
		return $false
	}
}
#endregion Test-Python()

#region Test-PythonPip()
function Test-PythonPip {
<#
SYNOPSIS
    Tests if the Python pip package manager is installed and available. Optionally attempts to upgrade pip if not found.

SYNTAX
    Test-PythonPip [[-attemptUpgrade] <SwitchParameter>]

DESCRIPTION
    The Test-PythonPip function checks if the Python pip package manager is installed and accessible in the current environment. If pip is not found, the function prompts the user to upgrade pip. Optionally, the function can automatically attempt to upgrade pip without user interaction.

PARAMETERS
    -attemptUpgrade [<SwitchParameter>]
        If specified, the function attempts to upgrade pip without user interaction. Default is to prompt the user for confirmation.
#>
	param (
		[switch] $attemptUpgrade
	)
	process {
		$pythonModules  = $null
		$pythonModules = (Python -m pip list)
		if ( -not $pythonModules) {
			if ($attemptUpgrade) {
				return $false
			}

			$result = New-MessageBox -Message "Can't read pip list, need to check for updates and install first. Do you want to continue?" -Title "PIP upgrade?" -Button YesNo -Icon Question
			if ($result -eq "No") {
				return $false
			}

			python -m pip install --upgrade pip
			return $(Test-PythonPip -attemptUpgrade)
		}	

		return $true
	}
}
#endregion Test-PythonPip()

#region Test-PythonSphinx()
function Test-PythonSphinx {
<#
SYNOPSIS
    Tests if the Sphinx Python module is installed and available. Optionally attempts to install Sphinx if not found.

SYNTAX
    Test-PythonSphinx [[-attemptInstall] <SwitchParameter>]

DESCRIPTION
    The Test-PythonSphinx function checks if the Sphinx Python module is installed and accessible in the current environment. If Sphinx is not found, the function prompts the user to install it. Optionally, the function can automatically attempt to install Sphinx without user interaction.

PARAMETERS
    -attemptInstall [<SwitchParameter>]
        If specified, the function attempts to install Sphinx without user interaction. Default is to prompt the user for confirmation.
#>
	param (
		[switch] $attemptInstall
	)
	process {
		if ((python -c "import pkgutil; print(1 if pkgutil.find_loader('sphinx') else 0)") -eq 0) {
			if ($attemptInstall) {
				return $false
			}

			$result = New-MessageBox -Message "Module 'sphinx' not found. It needs to be installed before continuing.`n`r`n`rDo you want to install Sphinx now?" -Title "sphinx install?" -Button YesNo -Icon Question
			if ($result -eq "No") {
				return $false
			}

			python -m pip install sphinx
			return $(Test-PythonSphinx -attemptInstall)
		}

		return $true
	}
}
#endregion Test-PythonSphinx()

#region Test-PythonSphinxRtdTheme()
function Test-PythonSphinxRtdTheme {
<#
SYNOPSIS
    Tests if the Sphinx Read the Docs (RTD) theme Python module is installed and available. Optionally attempts to install the RTD theme if not found.

SYNTAX
    Test-PythonSphinxRtdTheme [[-attemptInstall] <SwitchParameter>]

DESCRIPTION
    The Test-PythonSphinxRtdTheme function checks if the Sphinx RTD theme Python module is installed and accessible in the current environment. If the RTD theme is not found, the function prompts the user to install it. Optionally, the function can automatically attempt to install the RTD theme without user interaction.

PARAMETERS
    -attemptInstall [<SwitchParameter>]
        If specified, the function attempts to install the RTD theme without user interaction. Default is to prompt the user for confirmation.
#>
	param (
		[switch] $attemptInstall
	)
	process {
		if ((python -c "import pkgutil; print(1 if pkgutil.find_loader('sphinx_rtd_theme') else 0)") -eq 0) {
			if ($attemptInstall) {
				return $false
			}

			$result = New-MessageBox -Message "Module 'sphinx-rtd-theme' not found. It needs to be installed before continuing.`n`r`n`rDo you want to install sphinx-rtd-theme now?" -Title "sphinx-rtd-theme install?" -Button YesNo -Icon Question
			if ($result -eq "No") {
				return $false
			}

			python -m pip install sphinx-rtd-theme
			return $(Test-PythonSphinxRtdTheme -attemptInstall)
		}

		return $true
	}
}
#endregion Test-PythonSphinxRtdTheme()


if (-not (Test-Python)) {
	Write-Error "Python is not detected on your system."
	Write-Host
    Write-Host "Please install Python from the Microsoft Store before continuing."
    Write-Host "You can download Python from the Microsoft Store at https://www.microsoft.com/store/apps/9p7qfzlmrfp7."
    Write-Host "Once Python is installed, restart this script."

	Wait-KeyDown
	exit
}

if (-not (Test-PythonPip)) {
	Write-Error "PIP needs to be updated."
	Write-Host
	Write-Host "Please check for updates and install before continuing." -ForegroundColor White

	Wait-KeyDown
	exit
}

if (-not (Test-PythonSphinx)) {
	Write-Error "Required module 'sphinx' is not installed on the system."
	Write-Host
	Write-Host "Please install 'sphinx' before continuing." -ForegroundColor White

	Wait-KeyDown
	exit
}

if (-not (Test-PythonSphinxRtdTheme)) {
	Write-Error "Required module 'sphinx_rtd_theme' is not installed on the system."
	Write-Host
	Write-Host "Please install 'sphinx_rtd_theme' before continuing." -ForegroundColor White

	Wait-KeyDown
	exit
}

# Define defaults
$rootDir = $PSScriptroot | Split-Path | Split-Path 
$sourceDir = Join-Path $rootDir "mRemoteNGDocumentation"
$buildDir = Join-Path $PSScriptroot "html"

# gona check
if (-not (Test-Path $sourceDir)) {
    throw "$sourceDir not found"
}

if ((Test-Path $buildDir) -and (Get-ChildItem $buildDir -Recurse).Count -gt 0) {
    $result = New-MessageBox -Message "Rebuild documentation?`n`r`n`rYes to rebuild`n`rNo to Update." -Title "Build documentation" -Button YesNo -Icon Question

    if ($result -eq "Yes") {
        # Remove dir to rebuid
        $null = Remove-Item $buildDir -Recurse -Force
    }
}

if (-not (Test-Path $buildDir)) {
    $null = New-Item $buildDir -ItemType Directory -ErrorAction Stop
}

python -m sphinx.cmd.build -b html "$sourceDir" "$buildDir"

Wait-KeyDown