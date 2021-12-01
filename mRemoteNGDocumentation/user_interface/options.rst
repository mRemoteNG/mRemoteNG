*******
Options
*******

Options window which can also be named settings is the window
where you can personalize your options for all of mRemoteNG.
This includes how to set logging, credentials and so on.
Continue reading for the details of the different options here.

Startup/Exit
============
Options below are for the various settings for Startup/Exit of mRemoteNG.

.. list-table::
   :widths: 30 70 70
   :header-rows: 1

   * - Option
     - Default
     - Description
   * - Reconnect to previously opened sessions on startup
     - Off
     - This option will allow you to open the connection from which you where connected to after last exit of application
   * - Allow only a single instance of the application (mRemoteNG restart required)
     - Off
     - Enforces and makes sure only a single instance of mRemoteNG is running on the computer

Appearance
==========
Various options for mRemoteNG appearance.

.. list-table::
   :widths: 30 70 70
   :header-rows: 1

   * - Option
     - Default
     - Description
   * - Language
     - (Automatically Detect)
     - Which language to use for the interface of mRemoteNG
   * - Show description tooltips in connection tree
     - Off
     - Holding mouse over a item in connection tree will show a popout from mouse with information
   * - Show full connections file path in window title
     - Off
     - Adds the complete path to the title of mRemoteNG to where the connection file is located
   * - Always show notification area icon
     - Off
     - Adds mRemoteNG to the taskbar in the OS
   * - Minimize to notification area
     - Off
     - Will place mRemoteNG in taskbar on minimize

Tabs & Panels
=============
Various settings for how tabs & panels should work in mRemoteNG.

.. list-table::
   :widths: 30 70 70
   :header-rows: 1

   * - Option
     - Default
     - Description
   * - Always show panel tabs
     - Off
     - Will always show the tabs & panels in mRemoteNG
   * - Open new tab to the right of the currently selected tab
     - On
     - When active then open next tab on the right of the active selection in mRemoteNG. Turn this off and next tab will open the next connection at the end of all tabs
   * - Show logon information on tab names
     - Off
     - Show your login in the connection tab
   * - Show protocols on tab names
     - Off
     - When active then in the tab show what protocol is used for the connection
   * - Identify quick connect tabs by adding the prefix "Quick:"
     - Off
     - When active shows Quick: before the connection name in the tab connection to easier identify what is a quick connection and what is a non quick connection
   * - Double click on tab closes it
     - On
     - When double clicking a tab it will close the connection but does not log you out from the server. The connection in this case is active on the destination server
   * - Always show panel selection dialog when opening connections
     - Off
     - 	Option to allow you to always select what panel to place the connection on. If this is off it will create a General panel where the connection is placed or use the connections set panel from the connection options
   * - Create a empty panel when mRemoteNG starts
     - Off
     - On startup if this is active mRemoteNG will create a panel mentioned under Panel Name

Connections
===========
.. list-table::
   :widths: 30 70 70
   :header-rows: 1

   * - Option
     - Default
     - Description
   * - Single click on connections opens it
     - Off
     - In connection tree when this is active will try to connect on single click. By default this is turned off to use double click to open connection.
   * - Single click on opened connection in Connection Tree switches to the opened Connection Tab
     - Off
     - Allows you to single click on a active connection in the connection tree to go to that open connection in the tabs faster.
   * - Set hostname like display name when creating or renaming connections
     - Off
     - Will make mRemoteNG try to use the remote host hostname to set the title of the tab in mRemoteNG.
   * - Filter search matches in connection tree
     - Off
     - Allows you to filter out the connections to which does not match your filter search in the connection tree. If not active the search will only select the filter to which you do search.
   * - RDP Reconnect count
     - 5
     - Value in seconds
   * - RDP Connection Timeout
     - 20
     - Value in seconds
   * - Auto save time in minutes (0 means disabled)
     - 0
     - Value in minutes
   * - When closing connections Warn me...
     - ... when any connection closes
     - Various options of how mRemoteNG should act when you close connections. The different options are listed below:
       ::

           - ... when any connection closes
           - ... when closing multiple connections
           - ... only when exiting mRemoteNG
           - ... never
           By default a warning will come up on closing a connection. Change this value based on your prefered settings.
   * - Connection Backup Frequency
     - On Edit
     - Various options of when mRemoteNG should create a backup of the connections file. The different options are listed below:
       ::

           - Never backup connections
           - On Edit
           - On Exit
           - Daily
           - Weekly
           By default a backup will be saved every time the connections are edited. Change this value based on your prefered settings.
   * - Maximum number of backups
     - 10
     - Number of backup copies of the connection file to keep.
   * - Location of backup files
     - (blank)
     - Full path of backup copies of the connection files.
	
Credentials
===========
Options for credentials in mRemoteNG. The main purpose here is that when you have empty username, password or domain field then use below information.

.. list-table::
   :widths: 30 70 70
   :header-rows: 1

   * - Option
     - Default
     - Description
   * - None
     - On
     - Use no specific settings on login
   * - My Current credentials (Windows logon information)
     - Off
     - This option will use the logon information for the OS. This is useful if you are in a domain that uses specific credentials and want to login to servers with those credentials
   * - The following
     - Off
     - Use one or two of the options below for the empty login or all of them. For example if you have a different domain that you login to the servers with

SQL Server
==========

.. note::

    To understand more about SQL Server connection please see here: :ref:`sql_configuration`

.. list-table::
   :widths: 30 70 70
   :header-rows: 1

   * - Option
     - Default
     - Description
   * - Use SQL Server to load & save connections
     - Off
     - Enable to fetch connections from a database.

Updates
=======
Options for how mRemoteNG should check for updates from the website.

.. list-table::
   :widths: 30 70 70
   :header-rows: 1

   * - Option
     - Default
     - Description
   * - Check for updates at startup
     - On (Every 14 days)
     - Here you can choose how often mRemoteNG checks for updates. Standard is every 14 days
   * - Release Channel
     - Stable
     - The main channel to use for mRemoteNG. Note that the channels are described under the selection. Stable is suggested for normal usage but its always good to get feedback on upcoming releases
   * - Use a proxy server to connect
     - Off
     - Proxy to connect through to check for updates. This is not a proxy connection for when you connect to a server but more to check for updates

Theme
=====
This is not enabled by default but can be used inside mRemoteNG.
To enable themes you have to first enable it in the checkbox at the bottom of the options.
Then restart mRemoteNG in order for it to work.

.. note::

    Default theme is: vs2015light

.. note::

    To know more about themes and how to create your own See Here

Advanced
========

.. list-table::
   :widths: 30 70 70
   :header-rows: 1

   * - Option
     - Default
     - Description
   * - Automatically get session information
     - Off
     -
   * - Automatically try to reconnect when disconnected from server (RDP & ICA only)
     - Off
     -
   * - Use UTF8 encoding for RDP "Load Balance info" property
     - Off
     -
   * - Use custom PuTTY path
     - Off
     -
   * - To configure PuTTY sessions click this button
     - Launch PuTTY
     - Will launch the putty agent so you can edit the sessions
   * - Maximum PuTTY and integrated external tools wait time
     - 2 seconds
     -
