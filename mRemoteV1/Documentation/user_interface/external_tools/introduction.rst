.. External Tools - introduction

.. red: F23C3C
.. green: 40F23C
.. blue: 3C3CF2

************
Introduction
************

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

   For more information on variables, See: :doc:`/user_interface/external_tools/variables`

Working directory
   From where should this Tool/Item be ran.
