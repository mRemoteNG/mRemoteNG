**************
External Tools
**************

Start External Application
==========================

This example will create a entry that will launch and login to server using filezilla and sftp.
Start with opening up external tools from: :menuselection:`Tools --> External Tools` And create a *New* entry.
Change *Display Name* to **FileZilla** and *Filename* to **C:\\Program Files\\FileZilla FTP Client\\filezilla.exe**. See image below:

.. figure:: /images/example_et_start_application_01.png

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

Try the launch the FileZilla based external tool now against the server you want to login too
and you will notice that the application is launched with the variables.

Traceroute
==========

This example will create a traceroute which you can call on for a connection to get the traceroute to the
connection. Start with opening up external tools from: :menuselection:`Tools --> External Tools`
And create a *New* entry. See :doc:`/user_interface/external_tools`
Change *Display Name* to **Traceroute** and *Filename* to **cmd**.

See image below:

.. figure:: /images/example_et_traceroute_01.png

   Figure 1.0: Showing traceroute init settings

Now comes the interesting part where we fill in arguments that tells the console what to launch.
Here are the parts we need:

- Keep the console open - /K
- Program to run - tracert
- Variable to use - %HOSTNAME%

So lets fill these options in to the arguments like so:

.. figure:: /images/example_et_traceroute_02.png

This is all we really need in order to do a traceroute. Right click on a connection in the connection
list and go to :menuselection:`External Tools --> Traceroute` which will open a cmd prompt and run a
tracert against the host using hostname variable.

.. figure:: /images/example_et_traceroute_03.png

A console like below will appear that show the traceroute and will not exit until you close the window.

.. figure:: /images/example_et_traceroute_04.png

If you want to use **powershell** instead. Then follow information below:

- Filename - powershell.exe
- Arguments - -NoExit tracert %HOSTNAME%

Notice that we replaced the /K with -NoExit and changed **cmd** with **powershell.exe**. See image below:

.. figure:: /images/example_et_traceroute_05.png
