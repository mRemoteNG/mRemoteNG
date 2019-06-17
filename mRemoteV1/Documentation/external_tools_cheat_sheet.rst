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
- Can integrate: Unknown

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
Description

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

- Name: Serial COM***X***
- Protocol: Ext. App
- External Tool: COM Serial Port
- Port: your desired COM port # here

`Windows PowerShell (ISE) <https://msdn.microsoft.com/en-us/powershell/scripting/getting-started/fundamental/windows-powershell-integrated-scripting-environment--ise->`_
=========================================================================================================================================================================
Windows PowerShell is a task-based command-line shell and scripting language designed especially for system administration.

- Filename: %WINDIR%\\system32\\WindowsPowerShell\\v1.0\\PowerShell_ISE.exe
- Arguments: args here
- Can integrate: Yes

PowerShell, Enter-PSSession
===========================
This will allow you to right-click a Windows connection entry and use the hostname and user/password entry to begin a remote PowerShell session.

- Filename: %WINDIR%\\system32\\WindowsPowerShell\\v1.0\\PowerShell.exe
- Arguments: -NoExit -Command "$password = ConvertTo-SecureString '%PASSWORD%' -AsPlainText -Force; $cred = New-Object System.Management.Automation.PSCredential -ArgumentList @('%Domain%\\%Username%', $password); Enter-PSSession -ComputerName %Hostname% -Credential $cred"
- Can integrate: No
