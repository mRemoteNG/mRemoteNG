***********************************
Common External Tool Configurations
***********************************

The list below of various examples is by no means a full list of ways to use
**External Tools** but gives you a idea of how it can be used in different ways.

Ping
====
Ping a server via cmdline.

- Filename: %COMSPEC%
- Arguments: /c ping -t %HostName%
- Can integrate: Unknown

Traceroute
==========
Run a traceroute via cmdline.

- Filename: %COMSPEC%
- Arguments: /c set /P = | tracert %HostName%
- Can integrate: Unknown

`WinSCP <https://winscp.net/eng/index.php>`_
============================================
WinSCP is a free GUI Secure Copy program.

- Filename: C:\\Program Files\\WinSCP\\WinSCP.exe (example path)
- Arguments: scp://%Username%:%Password%@%Hostname%/
- Optional Arguments - turn on compression and ignore any host key errors: -rawsetting Compression=1 -hostkey=*
- Can integrate: No

`FileZilla S/FTP <https://filezilla-project.org/>`_
===================================================
Free and open source FTP client for most platforms.

- Filename: C:\\Program Files\\FileZilla FTP Client\\filezilla.exe (example path)
- Arguments (FTP): ftp://%Username%:%Password%@%Hostname%
- Arguments (SFTP): sftp://%Username%:%Password%@%Hostname%
- Can integrate: Unknown

`Firefox <https://www.mozilla.org/en-US/firefox/new/>`_
=======================================================
Don't like the built-in browser support? Integrate with the Mozilla Firefox browser directly!

- Filename: C:\\Program Files\\Mozilla Firefox\\firefox.exe (example path)
- Arguments: %Hostname%
- Can integrate: Unknown

`Google Chrome <https://www.google.com/chrome/browser/desktop/index.html>`_
===========================================================================
Google Chrome is a freeware web browser developed by Google.

- Filename: C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe (example path)
- Arguments: %Hostname%
- Can integrate: Unknown

`Internet Explorer <http://microsoft.com/ie>`_
==============================================
Standard browser included with Windows installation.

- Filename: C:\\Program Files\\Internet Explorer\\iexplore.exe
- Arguments: %Hostname%
- Can integrate: Unknown

`MySql Workbench <http://www.mysql.com/products/workbench/>`_
=============================================================
MySQL Workbench provides data modeling, SQL development, and comprehensive administration tools for server configuration, user administration, backup, and much more. You will be prompted for a password when starting the connection.

- Filename: C:\\Program Files\\MySQL\\MySQL Workbench 6.3 CE\\MySQLWorkbench.exe (example path)
- Arguments: -query %USERNAME%@%HOSTNAME%
- Can integrate: Unknown

`VNC Viewer <https://www.realvnc.com/download/viewer/>`_
=========================================================

- Filename: C:\\Program Files\\RealVNC\\VNC Viewer\\vncviewer.exe (example path)
- Arguments: %HostName%
- Can integrate: Unknown

Windows Computer Manager
========================

- Filename: %WINDIR%\\system32\\compmgmt.msc
- Arguments: /Computer=%HostName%
- Can integrate: Unknown

`Zenmap GUI <https://nmap.org/zenmap/>`_
========================================
Zenmap is a GUI front-end for nmap.

- Filename: C:\\Program Files\\Nmap\\zenmap.exe (example path)
- Arguments: -p "Quick scan plus" -t %Hostname%
- Can integrate: Unknown

`UltraVNC <https://nmap.org/zenmap/>`_
======================================
UltraVNC is a free and open source program for connection to remote machines using the VNC protocol.

- Filename: C:\\Program Files\\UltraVNC\\vncviewer.exe (example path)
- Arguments: %HostName%:%port% -password %PASSWORD%
- Can integrate: Unknown

COM Serial Port
===============
This will allow you to connect to a specific COM serial port using PuTTY.

- Filename: putty.exe (example path)
- Arguments: -serial com%Port%
- Can integrate: Yes

Create a new connection entry with the following information:

- Name: Serial COM**X**
- Protocol: Ext. App
- External Tool: COM Serial Port
- Port: your desired COM port # here

`Windows PowerShell <https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/powershell>`_
=========================================================================================================================================================================
Windows PowerShell is a task-based command-line shell and scripting language designed especially for system administration.

Running with suggested argument will open a PS session connected to a host. No prompt for credintials will popup.

- Filename: %WINDIR%\system32\WindowsPowerShell\v1.0\PowerShell.exe
- Arguments: -noexit $pw = \"%password%\" -replace '\^', ''; $password = ConvertTo-SecureString $pw -AsPlainText -Force; $Cred= New-Object System.Management.Automation.PSCredential (\"%username%\", $password); Enter-PSSession -ComputerName %hostname% -credential $Cred
- Can integrate: No

`Windows PowerShell (ISE) <https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/powershell_ise>`_
=========================================================================================================================================================================
Windows PowerShell Integrated Scripting Environment (ISE) is a graphical host application that enables you to read, write, run, debug, and test scripts and modules in a graphic-assisted environment.

- Filename: %WINDIR%\\system32\\WindowsPowerShell\\v1.0\\PowerShell_ISE.exe
- Arguments: args here
- Can integrate: Yes

To get all windows file systems attributes of the remote server
=========================================================================================================================================================================
It’s could be very useful for database and application servers to know all windows file systems attributes of the remote server, the size in mb, free space in mb and free % including all lettered drives ( e.g c: ) as well as mounted drives. 
Mounted drives are tricky to find and interrogate by other means.

You must run mR as a user with appropriate permission on the remote server:
shortcut = C:\Windows\System32\runas.exe /user:domain\ntuser  "C:\Program Files\mRemoteNG\mRemoteNG.exe"

- Filename: %SystemRoot%\system32\WindowsPowerShell\v1.0\powershell.exe
- Arguments: -NoExit Get-WmiObject Win32_Volume -ComputerName %hostname% | Format-Table Name, @{Name=”Size(MB)”;Expression={“{0:0,0.00}” -f($_.Capacity/1mb)}}, @{Name=”Free Space(MB)”;Expression={“{0:0,0.00}” -f($_.FreeSpace/1mb)}}, @{Name=”Free (%)”;Expression={“{0,6:P0}” -f(($_.FreeSpace/1mb) / ($_.Capacity/1mb))}}
- Can integrate: No

Result will be similar to this:

.. figure:: /images/external_tools_win-resurces.png
    
You do not need to be logged into the remote server – just highlight the name.

(suggested by: Ray Miller)

