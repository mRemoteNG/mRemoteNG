**************
Menu Container
**************

Hide Menu Container
===================

You can hide the Menu Container when right-clicking the mRemoteNG title bar.

.. figure:: /images/menus_hide_menu_strip.png

Main Menu
=========

In this section we are going to explain the menus located in mRemoteNG.

- **Red** - Anchor to move menu around the interface
- **Green** - The menu items

.. figure:: /images/menus_main_menu.png

File Menu
---------
Contains standard commands for the application.

.. list-table::
   :widths: 30 70
   :header-rows: 1

   * - Item
     - Description
   * - New Connection
     - Will add a new connection to the Connections dialog after where the cursor is positioned.
   * - New Folder
     - Add a new folder in the Connections dialog tree where the cursor is positioned.
   * - New Connection File
     - Create a new connection file. Dialog will come up asking about: filename and where to place the new connection file.
   * - Open Connection File
     - Open a connection file. Dialog comes up asking about which file to open. For security reasons, this also shows a dialog to ask if you want to save the current file before continuing.
   * - Save Connection File
     - Saves the currently opened connection file. If you are using a SQL server connection instead it will send a save to the SQL server.
   * - Save Connection File As...
     - Saves the current connection file to a specific location on disk.
   * - Delete...
     - Delete currently selected item in connections dialog.
   * - Rename
     - Rename current selected item in connections dialog.
   * - Duplicate
     - Duplicate current selected item in connections dialog.
   * - Reconnect All Open Connections
     - Sends a reconnect to all the open connections in mRemoteNG.
   * - Exit
     - Exit mRemoteNG application

View Menu
---------
Menu for additional dialogs for mRemoteNG.

.. list-table::
   :widths: 30 70
   :header-rows: 1

   * - Item
     - Description
   * - Add Connection Panel
     - Create a new and empty panel.
   * - Connection Panels
     - Jump to panel.
   * - Connections
     - Show connections dialog
   * - Config
     - Show config dialog
   * - Notifications
     - Show notifications dialog
   * - Screenshots
     - Open Screenshots Manager (See: :ref:`screenshot_manager`)
   * - Jump To
     - Place focus on "Connections", "Config" or "Notifications" panel based on selection.
   * - Reset layout
     - Resets the layout of panels and dialogs. Warning will come up about the action before continuing.
   * - Lock toolbar positions
     - Locks the toolbars at the top of the application so you do not move around items by mistake.
   * - Quick Connect Toolbar
     - Show quick connect toolbar
   * - External Tools Toolbar
     - Show external tools toolbar
   * - Multi SSH Toolbar
     - Show multi ssh toolbar
   * - Fullscreen
     - Fullscreen mRemoteNG (will not fullscreen connection window but only the mRemoteNG application)

Tools Menu
----------
Additional tools that can be used and triggered in mRemoteNG.

.. list-table::
   :widths: 30 70
   :header-rows: 1

   * - Item
     - Description
   * - SSH File Transfer
     - Show SSH file transer panel (See: :ref:`ssh_file_transfer`)
   * - External Tools
     - Show external tools dialog (See: :ref:`external_tools`)
   * - Port Scan
     - Show port scan dialog (See: :ref:`port_scan`)
   * - Options
     - Opens mRemoteNG global settings and options dialog

Help Menu
---------
Get more information for the application.

.. list-table::
   :widths: 30 70
   :header-rows: 1

   * - Item
     - Description
   * - mRemoteNG Help
     - Show help panel (this panel)
   * - Website
     - Go to mRemoteNG website
   * - Donate
     - Go to mRemoteNG donation page
   * - Support Forum
     - Go to mRemoteNG suport forum
   * - Report a Bug
     - Go to github page to report a bug
   * - Check for Updates
     - Opens dialog to check for any updates of mRemoteNG
   * - About
     - Open about dialog for mRemoteNG (Shows contributors, changelog and more)
