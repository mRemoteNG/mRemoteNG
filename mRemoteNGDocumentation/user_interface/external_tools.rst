.. _external_tools:

**************
External Tools
**************

External Tools can help you get things done that can't be done in mRemoteNG.

For example you can:

- Start a command
- Launch your favorite FTP tool

This might not make much sense by itself because you can already launch your applications by using the Windows Start Menu,
Quick Launch or whatever you prefer to start your apps.

But from within mRemoteNG you can launch applications and tell them what to do with the use of arguments, parameters and variables
of the currently selected Connection. You can, for example, select your home router's SSH connection entry and do a traceroute (tracert)
on that host. This is much quicker and more powerful than opening the console and typing ``tracert yourhost``.

Main UI
=======
The below image will show the main UI of *External Tools*. You may find the interface a bit confusing in the beginning but
we will explain the various items in more details below.

:Menu:   :menuselection:`Tools --> External Tools`

.. figure:: /images/external_tools_main_ui_01.png

   External Tools open with one new entry

   Toolbar (red), Tools/Items List (blue), External Tool Properties (green)

Toolbar
-------

.. figure:: /images/external_tools_toolbar_01.png

   External Tools - Toolbar

New ``Shift-F4``
   Create a new external tool.

Delete ``Del``
   Delete selected tool item in list.

Launch
   Run the current selected tool on currently selected connection.

.. hint::

	All items can be accessed with right click menu and with a keyboard shortcut except for the Launch action.

Tools/Items List
----------------

.. figure:: /images/external_tools_tools_list_01.png

   External Tools - Tools/Items list

Basically shows the list of Tools/Items that you have created with the arguments and options.

External Tool Properties
------------------------

.. figure:: /images/external_tools_external_tool_properties_01.png

   External Tools - External Tool Properties

Is where you do most of the work to setup the Tool/Item for External Tools. We will explain each item further down this page.

Display Name
   Name of the tool, this can be any type of name.

   **For example:**

      :code:`Open in FileZilla`, :code:`FileZilla`, :code:`Traceroute`

Filename
   Application/Command to run.

   **For example:**

      :code:`cmd`, :code:`powershell`, :code:`C:\WINDOWS\system32\compmgmt.msc`,
      :code:`C:\Program Files(x86)\FileZilla FTP Client\filezilla.exe`

Arguments
   Sometimes also called switches and parameters. This is where you tell the application in the previous (filename) input what to run.
   And also which variables from mRemoteNG to use for the arguments.

   **For Example:**

      :code:`sftp://%USERNAME%:%PASSWORD%@%HOSTNAME%:%PORT%`, :code:`/K tracert %HOSTNAME%`, :code:`-NoExit tracert %HOSTNAME%`

Working directory
   From where should this Tool/Item be ran.
   
Variables
=========

Variables and arguments can be used to tell the external tool what to do.

This is the list of variables supported by mRemoteNG:

- %NAME%
- %HOSTNAME%
- %PORT%
- %USERNAME%
- %PASSWORD%
- %DOMAIN%
- %DESCRIPTION%
- %MACADDRESS%
- %USERFIELD%

mRemoteNG will also expand environment variables such as %PATH% and %USERPROFILE%. If you need to use an environment
variable with the same name as an mRemoteNG variable, use \\% instead of %. The most common use of this is for the
USERNAME environment variable. %USERNAME% will be expanded to the username set in the currently selected connection.
\\%USERNAME\\% will be expanded to the value set in the USERNAME environment variable.

If you need to send a variable name to a program without mRemoteNG expanding it, use ^% instead of %.
mRemoteNG will remove the caret (^) and leave the rest unchanged.
For example, ^%USERNAME^% will be sent to the program as %USERNAME% and will not be expanded.

Rules for variables
-------------------
- Variables always refer to the currently selected connection.
- Variable names are case-insensitive.
- Variables can be used in both the Filename and Arguments fields.


Special Character Escaping
==========================
Expanded variables will be escaped using the rules below. There are two levels of escaping that are done.

1. Is escaping for standard argument splitting (C/C++ argv, CommandLineToArgvW, etc)
2. Is escaping shell metacharacters for ShellExecute.

Argument splitting escaping
---------------------------

- Each quotation mark will be escaped by a backslash
- One or more backslashes (\\) followed by a quotation mark ("):
   - Each backslash will be escaped by another backslash
   - The quotation mark will be escaped by a backslash
      - If the connection's user field contains ``"This"`` is a ``\"test\"``
      - Then %USERFIELD% is replaced with ``\"This\"`` is a ``\\\"test\\\"``
- A variable name followed by a quotation mark (for example, %USERFIELD%") with a value ending in one or more backslashes:
   - Each backslash will be escaped by another backslash
   - Example:
      - If the connection's user field contains ``c:\Example\``
      - Then "%USERFIELD%" is replaced with ``"c:\Example\\"``

To disable argument splitting escaping for a variable, precede its name with a minus (-) sign. For example: %-USERFIELD%

Shell metacharacter escaping
----------------------------

- The shell metacharacters are ( ) % ! ^ " < > & |
- Each shell metacharacter will be escaped by a caret (^)

To disable both argument splitting and shell metacharacter escaping for a variable, precede its name with an exclamation point (!).
For example, %!USERFIELD%. This is not recommended and may cause unexpected results.

Only variables that have been expanded will be escaped. It is up to you to escape the rest of the arguments.
