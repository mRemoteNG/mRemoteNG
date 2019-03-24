.. External Tools - cheat sheet

.. TODO: Items to add:
   - Script run (ps1, bat etc)
   - Microsoft tools and sysinternals


***********
Cheat Sheet
***********

The list below of various examples is by no means a full list of ways to use
**External Tools** but gives you a idea of how it can be used in different ways.

Applications
============

WinSCP
------
Start WinSCP and login to specific server

| *Filename:* C:\\Program Files\\WinSCP\\WinSCP.exe
| *Arguments:* scp://%USERNAME%:%PASSWORD%@%HOSTNAME%

Ubuntu shell (WSL) - Open SSH
-----------------------------
Start ubuntu shell on windows and then SSH to the connection

| *Filename:*
| *Arguments:*

Powershell - PSSession to host
------------------------------
Open a powershell and enter psession to a host

| *Filename:* powershell
| *Arguments:* -NoExit Enter-PSSession -ComputerName %HOSTNAME% -Credential (Get-Credential)


Management
==========

Dell OpenManage (iDRAC)
-----------------------
Opens up internet explorer and goes to iDRAC interface on port 1311

| *Filename:* iexplore
| *Arguments:* https://%HOSTNAME%:1311

HP System Management HomePage
-----------------------------
Opens up internet explorer and goes to HP homepage

| *Filename:* iexplore
| *Arguments:* https://%HOSTNAME%:81
