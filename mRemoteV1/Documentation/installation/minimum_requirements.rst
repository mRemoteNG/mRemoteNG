********************
Minimum Requirements
********************

.. note::

	In recent versions of Windows 10 and Windows Server 2016 the below requirements are already provided by the system.

However they are listed below just in case you need to know what mRemoteNG actually needs for different protocols and
error searching on troubles with installing.

- `Microsoft .NET Framework 4.0 <https://www.microsoft.com/en-us/download/details.aspx?id=17851>`_

- Microsoft Terminal Service Client 8.0 or later
   - Needed if you use RDP, mstscax.dll and/or msrdp.ocx be registered.
   - Included with newer Windows versions `KB2574819 <https://support.microsoft.com/en-us/kb/2574819>`_
     AND either `KB2592687 <https://support.microsoft.com/en-us/kb/2592687>`_ or
     `KB2923545 <https://support.microsoft.com/en-us/kb/2923545>`_ is required for Windows 7/Windows Server 2008 R2

- `PuTTY <http://www.chiark.greenend.org.uk/~sgtatham/putty/download.html>`_
   - Needed if you use Telnet, SSH, Rlogin or RAW. Included in all packages.
   - An appropriate and integrated version is included with mRemoteNG.

- `Citrix ICA Client <https://www.citrix.com/downloads/citrix-receiver.html>`_
   - Needed if you use ICA. wfica.ocx must be registered.

.. _requirements:

Windows 7 and Windows 2008 R2 Clients
-------------------------------------
.. tip::

	You can use powershell to check if the hotfixes are installed. Example: ``Get-HotFix | where {$_.HotFixID -eq "KB2574819" -and $_.HotFixID -eq "KB2592687"}``

The following updates, **must be** present on any Windows 7 or Windows Server 2008 client that will be running
mRemoteNG. (They must have been installed in the order provided below):

- `KB2574819 <https://support.microsoft.com/en-us/kb/2574819>`_ - Adds support for DTLS in Windows 7 SP1 and Windows Server 2008 R2 SP1
- `KB2592687 <https://support.microsoft.com/en-us/kb/2592687>`_ - RDP 8.0 update for Windows 7 and Windows Server 2008 R2

The following are suggested (but not required) for Windows 7 / Windows Server 2008 clients:

- `KB2857650 <https://support.microsoft.com/en-us/kb/2857650>`_ - Update that improves the RemoteApp and Desktop Connections features is available for Windows 7
- `KB2830477 <https://support.microsoft.com/en-us/kb/2830477>`_ - Update for RemoteApp and Desktop Connections feature is available for Windows
- `KB2913751 <https://support.microsoft.com/en-us/kb/2913751>`_ - Smart card redirection in remote sessions fails in a Windows 7 SP1-based RDP 8.1 client
- `KB2923545 <https://support.microsoft.com/en-us/kb/2923545>`_ - Update for RDP 8.1 is available for Windows 7 SP1
- `KB2965788 <https://support.microsoft.com/en-us/kb/2965788>`_ - MS14-030: Description of the security update for Remote Desktop Security Release for Windows: June 10, 2014
- `KB2985461 <https://support.microsoft.com/en-us/kb/2985461>`_ - Error 0x800401f0when you update RemoteApp and Desktop Connections feeds in Windows 7 or Windows Server 2008 R2
- `KB2984972 <https://support.microsoft.com/en-us/kb/2984972>`_ - Update for RDC 7.1to support restricted administration logons on Windows 7 and Windows Server 2008 R2
- `KB2984976 <https://support.microsoft.com/en-us/kb/2984976>`_ - RDP 8.0 update for restricted administration on Windows 7 or Windows Server 2008 R2
