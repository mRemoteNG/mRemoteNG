.. Example - External Tool - Start External Application

**********************************
(Basic) Start External Application
**********************************

This example will create a entry that will launch and login to server using filezilla and sftp.

Step-by-Step
============
Start with opening up external tools from: :menuselection:`Tools --> External Tools`

And create a *New* entry. See :doc:`/user_interface/external_tools/introduction`

Change *Display Name* to **FileZilla** and *Filename* to **C:\\Program Files\\FileZilla FTP Client\\filezilla.exe**. See image below:

.. figure:: /images/example_et_start_application_01.png

   Figure 1.0: filezilla.exe setup with external tools

We then need to arguments to use for filezilla, which we can find out either by searching for it on the great wide
internet or by called the `-h` parameter to filezilla.exe in powershell:

:code:`& 'C:\Program Files\FileZilla FTP Client\filezilla.exe' -h`

This will open a small dialog showing the various input parameters.

What we are going to use is the following for our entry:

- Application: FileZilla
- Protocol - sftp://
- Input Parameters (variables) - %HOSTNAME%, %USERNAME%,%PASSWORD% and %PORT%

All of the variables are parsed from mRemoteNG connection item to the filezilla command line.
So lets build this entry up in **External Tools** where we add all these items.

.. figure:: /images/example_et_start_application_02.png

   Figure 1.1: Complete command with filezilla and sftp

Try the launch the FileZilla based external tool now against the server you want to login too
and you will notice that the application is launched with the variables.
