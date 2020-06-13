<p align="center">
  <img width="500" src="https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/develop/.github/logo.png">
</p>
  
<p align="center">
  mRemoteNG is an open source, multi-protocol, remote connections manager that allows you to view all of your remote connections in a simple, yet powerful tabbed interface
</p>

<p align="center">
  <img alt="GitHub All Releases" src="https://img.shields.io/github/downloads/mremoteng/mremoteng/total?label=Overall%20Downloads&style=for-the-badge">
</p>

<p align="center">
  <a href="https://www.reddit.com/r/mRemoteNG/">
    <img alt="Subreddit subscribers" src="https://img.shields.io/reddit/subreddit-subscribers/mremoteng?label=Reddit&logo=Reddit&style=flat-square">
  </a>
  <a href="https://twitter.com/mremoteng">
    <img alt="Twitter Follow" src="https://img.shields.io/twitter/follow/mremoteng?color=%231DA1F2&label=Twitter&logo=Twitter&style=flat-square">
  </a>
  <a href="https://gitter.im/mRemoteNG/PublicChat">
    <img alt="Gitter" src="https://img.shields.io/gitter/room/mRemoteNG/PublicChat?label=Join%20the%20Chat&logo=Gitter&style=flat-square">
  </a>  
  <a href="https://www.paypal.me/DavidSparer">
    <img alt="PayPal" src="https://img.shields.io/badge/%24-PayPal-blue.svg?label=Donate&logo=PayPal&style=flat-square">
  </a>
</p>

<p align="center">
  <a href="https://github.com/mRemoteNG/mRemoteNG/blob/develop/COPYING.TXT">
    <img alt="License" src="https://img.shields.io/github/license/mremoteng/mremoteng?label=License&style=flat">
  </a>
  <a href="https://bestpractices.coreinfrastructure.org/projects/529">
    <img alt="CII Best Practices" src="https://bestpractices.coreinfrastructure.org/projects/529/badge?style=flat">
  </a>
  <a href='https://mremoteng.readthedocs.io/en/latest/?badge=latest'>
    <img src='https://readthedocs.org/projects/mremoteng/badge/?version=latest' alt='Documentation Status' />
  </a>
</p>

---

| Update Channel | Build Status | Downloads |
| ---------------|--------------|-----------|
| Stable | [![Build status](https://ci.appveyor.com/api/projects/status/k0sdbxmq90fgdmj6/branch/master?svg=true)](https://ci.appveyor.com/project/mremoteng/mremoteng/branch/master) | [![Github Releases (by Release)](https://img.shields.io/github/downloads/mRemoteNG/mRemoteNG/v1.76.15/total.svg)](https://github.com/mRemoteNG/mRemoteNG/releases/tag/v1.76.15) |
| Prerelease | [![Build status](https://ci.appveyor.com/api/projects/status/k0sdbxmq90fgdmj6/branch/develop?svg=true)](https://ci.appveyor.com/project/mremoteng/mremoteng/branch/develop) | [![Github Releases (by Release)](https://img.shields.io/github/downloads/mRemoteNG/mRemoteNG/v1.77.1/total.svg)](https://github.com/mRemoteNG/mRemoteNG/releases/tag/v1.77.1) |

## Features

The following protocols are supported:

* RDP (Remote Desktop/Terminal Server)
* VNC (Virtual Network Computing)
* SSH (Secure Shell)
* Telnet (TELecommunication NETwork)
* HTTP/HTTPS (Hypertext Transfer Protocol)
* rlogin (Remote Login)
* Raw Socket Connections

For a detailed feature list and general usage support, refer to the [Documentation](https://mremoteng.readthedocs.io/en/latest/).

## Installation

### Supported Operating Systems

- [Windows 10](https://en.wikipedia.org/wiki/Windows_10)
- [Windows 8.1](https://en.wikipedia.org/wiki/Windows_8.1)
- [Windows 7](https://en.wikipedia.org/wiki/Windows_7) (See: _Minimum Requirements_)
- [Windows Server 2019](https://en.wikipedia.org/wiki/Windows_Server_2019)
- [Windows Server 2016](https://en.wikipedia.org/wiki/Windows_Server_2016)
- [Windows Server 2012 R2](https://en.wikipedia.org/wiki/Windows_Server_2012_R2)
- [Windows Server 2012](https://en.wikipedia.org/wiki/Windows_Server_2012)
- [Windows Server 2008 R2](https://en.wikipedia.org/wiki/Windows_Server_2008_R2) (See: _Minimum Requirements_)

### Packaging

Downloads are provided in three different packages.

#### Binary package

The binary package of mRemoteNG is a compiled version of mRemoteNG which comes in an MSI installer.
This is the most common way to install mRemoteNG and get up and running. For more custom and advanced
installs of mRemoteNG then continue reading further down this page.

#### Portable package

The portable package consists of the same files as the binary package but contains a modified version
of the executable which stores and loads all your settings from files in the application's directory.
This package can be used to run mRemoteNG from a USB stick and preserve your configuration wherever
you go.

#### Source package

This package is a package provided to be launched within Visual Studio. The only way to launch
mRemoteNG using this package is by compiling it from inside Visual Studio.

### Minimum Requirements

* [Microsoft Visual C++ Redistributable for Visual Studio 2015, 2017 and 2019](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)
* [Microsoft .NET Framework 4.0](https://www.microsoft.com/en-us/download/details.aspx?id=17851)
* Microsoft Terminal Service Client 6.0 or later
  * Needed if you use RDP. mstscax.dll and/or msrdp.ocx be registered.
* [PuTTY](http://www.chiark.greenend.org.uk/~sgtatham/putty/download.html)
  * Needed if you use Telnet, SSH, Rlogin or RAW. Included in all packages
  * An appropriate and integrated version is included with mRemoteNG
* [Citrix ICA Client](https://www.citrix.com/downloads/citrix-receiver.html)
  * Needed if you use ICA. wfica.ocx must be registered

### Suggested for Windows 7 and Windows 2008 R2 Clients

_You can use powershell to check if the hotfixes are installed. Example: `Get-HotFix | where {$_.HotFixID -eq "KB2574819" -and $_.HotFixID -eq "KB2592687"}`_

The following updates are suggested (but not required) for Windows 7 / Windows Server 2008 clients:

- [KB2857650](https://support.microsoft.com/en-us/kb/2857650) - Update that improves the RemoteApp and Desktop Connections features is available for Windows 7
- [KB2830477](https://support.microsoft.com/en-us/kb/2830477) - Update for RemoteApp and Desktop Connections feature is available for Windows
- [KB2913751](https://support.microsoft.com/en-us/kb/2913751) - Smart card redirection in remote sessions fails in a Windows 7 SP1-based RDP 8.1 client
- [KB2923545](https://support.microsoft.com/en-us/kb/2923545) - Update for RDP 8.1 is available for Windows 7 SP1
- [KB2574819](https://support.microsoft.com/en-us/kb/2574819) - Adds support for DTLS in Windows 7 SP1 and Windows Server 2008 R2 SP1
- [KB2592687](https://support.microsoft.com/en-us/kb/2592687) - RDP 8.0 update for Windows 7 and Windows Server 2008 R2
- [KB2965788](https://support.microsoft.com/en-us/kb/2965788) - MS14-030: Description of the security update for Remote Desktop Security Release for Windows: June 10, 2014
- [KB2985461](https://support.microsoft.com/en-us/kb/2985461) - Error 0x800401f0 when you update RemoteApp and Desktop Connections feeds in Windows 7 or Windows Server 2008 R2
- [KB2984972](https://support.microsoft.com/en-us/kb/2984972) - Update for RDC 7.1to support restricted administration logons on Windows 7 and Windows Server 2008 R2
- [KB2984976](https://support.microsoft.com/en-us/kb/2984976) - RDP 8.0 update for restricted administration on Windows 7 or Windows Server 2008 R2

### Download

mRemoteNG is available as a redistributable MSI package or as a portable ZIP package and can be downloaded from the following locations:
* [GitHub](https://github.com/mRemoteNG/mRemoteNG/releases)
* [Project Website](https://mremoteng.org/download)

### Command line install

There is also the possibility to install mRemoteNG through command line with the binary package:

`msiexec /i C:\Path\To\mRemoteNG-Installer.exe PROPERTY1=value PROPERTY2=value`

To explain the properties that can be set during install we will go into details of them below.

#### Extend Installer Properties

The following extended properties can be provided to the installer when running it from the
command line.

| Property | Accepted Values | Description |
|-|-|-|
| INSTALLDIR | Any valid folder path | This allows you to set the installation directory from the command line. For paths that contain spaces, enclose the path in double quotes (""). This overrides any value found in the registry. Introduced in v1.75 beta 2. |
| IGNOREPREREQUISITES | `0` or `1` | When the ``IGNOREPREREQUISITES`` property is set to ``1``, the installer will not be halted if any prerequisite check is not met. You must still run the installer as Admin - this flag will not remove that requirement. Introduced in v1.74. |

#### Examples

##### Install to a custom folder**

`msiexec /i C:\Path\To\mRemoteNG-Installer.msi INSTALLDIR="D:\Work Apps\mRemoteNG"`

##### Ignore prerequisites during a normal install

`msiexec /i C:\Path\To\mRemoteNG-Installer.msi IGNOREPREREQUISITES=1`

##### Ignore prerequisites during a silent install

`msiexec /i C:\Path\To\mRemoteNG-Installer.msi /qn IGNOREPREREQUISITES=1`

### Troubleshooting installation

Turn on verbose logging by using the `/lv* <log path>` argument at the command line.

`msiexec /i C:\Path\To\mRemoteNG-Installer.msi /l*v C:\mremoteng_install.log`

## Uninstall

### Standard Uninstall

mRemoteNG basic binary package can be uninstalled with Windows Control Panel. If for some reason it does not work please
follow information provided below for Manual Uninstall.

### Manual Uninstall

_If you are using the Portable version, simply deleting the folder that contains mRemoteNG should be sufficient. These uninstall instructions are only necessary for the normal binary .MSI installed version of mRemoteNG_

* Delete the folder where mRemoteNG was installed. By default, this is:
	`%PROGRAMFILES%\mRemoteNG`

* Delete the mRemoteNG install entry from one of the following locations. Search for "mRemoteNG" in the DisplayName field:
  * x86: ``HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\``
  * x64: ``HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\``

* (Optional) If you would also like to delete user data remove `%LOCALAPPDATA%\mRemoteNG`

## Contribute

If you find mRemoteNG useful and would like to contribute, it would be greatly appreciated. When you contribute, you make it possible for the team to cover the costs of producing mRemoteNG.

### Submit Code
Check out the [Wiki page](https://github.com/mRemoteNG/mRemoteNG/wiki) on how to configure your development environment and submit a pull request.

### Translate
Check out the [Wiki page](https://github.com/mRemoteNG/mRemoteNG/wiki) on how to help make mRemoteNG a polyglot.

<p align="center">
  <img alt="Developed with ReSharper" src="https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/develop/.github/icon_ReSharper.png">
</p>
