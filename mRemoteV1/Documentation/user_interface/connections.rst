***********
Connections
***********

The connections dialog is the main collection of all connections that inside mRemoteNG.
This document will explain the details of the connections dialog.

Connection Tree
===============

Menu Items
----------

.. figure:: /images/connections_top_bar.png

- **Red** - New Connection
- **Green** - New Folder
- **Blue** - View (Expand/Collapse all folders)
- **Yellow** - Ascending sort

New Connection
--------------

.. tip::

    You can also duplicate an existing connection. Just right click on folder or connection to duplicate the item. The information is then carried over for editing. This can save a lot of time when the connection list is large.

.. tip::

    When inside an SSH session you can open the PuTTY menu by holding down the CTRL key while right-clicking into the session window.

Creates a new connection item in the connections dialog after where cursor is present.

New Folder
----------

.. tip::

    Folders can help to make adding connections easier. By setting a folder with some values that can be inheritaded down to the connections.

Creates a new folder in connections dialog after where cursor is present.

View
----
Collapses or expands all directories in the connection dialog.
Useful when working with a lot of connections sorted in different directories.

Ascending
---------
Works like a sort or a refresh to get connection in ascending order.
(Descending order is note supported yet) When you have been moving around in the tree of connections,
just click this item to refresh the list and get everything in ascending ordering.

Configuration
=============

.. figure:: /images/connections_main.png

Config dialog to setup the connection specific properties.
This includes inheritance from other items before the item and more.
Details below is about how to work with this dialog to get the most out of connections and configuration.

Menu Items
----------

.. figure:: /images/config_top_bar.png

- **Red** - Sort values Categories or Alphabetical
- **Green** - Show Properties, Inheritance values
- **Blue** - Connection icon
- **Yellow** - Host status (based on ICMP ping)

Sort Values
-----------
Sorts the values in properties either by Categories or Alphabetically.

- Categories sort - Shows values in categories with expanding options.
- Alphabetical sort - Expands everything and shows values in alphabetical order instead

Icon
----

.. note::

    Don't forget that mRemoteNG will save the change on exit auto unless you have unchecked this setting in options.

The icon indicates the visual identifier for the connection.
Clicking the icon will let you set a different icon for the connection.

Status
------

.. note::

    In order for this to work you have to open up ICMP. On windows servers this is also disabled in windows firewall.

Is a indicator that will glow red or green depending on the status of the host.
The status is based on ICMP ping to the host.


Creating a connection
=====================

.. tip::

    You can see an indicator in the properties window that is glowing green:

    .. figure:: /images/connections_status.png

    This icon does a ICMP ping on to check response from the server. If it glows green it indicates a connection response can be made using ping to the host. However this is turned off on windows by default. You have to enable ICMP and allow the firewall access for it.


Right click on the root item (the little blue globe named **Connections**)
in the Connections panel and select **New Connection**.

.. figure:: /images/connections_rightclick_menu.png

A new item shows up under the root item. You can give it a name now (or rename it later).
We'll just call this connection "Test" for the moment.

.. figure:: /images/connections_test_item.png

Now lets look at the config panel in the bottom left, just under the connections panel.
As you may notice this is where you configure all the properties of connections and folders.

.. figure:: /images/connections_config.png

Fill in the necessary properties and you have just created your first connection!
You can now connect to the server with a simple double-click on the "Test"-connection!

Opening and Closing Connections
===============================

.. note::

    If the connecting fails, the notifications panel will pop up and show an error message describing the problem.

There are multiple ways to open a connection in mRemoteNG,
but the easiest way is to double click the connection in the Connections panel.
If you double click the connection you will notice that the connection is going
to try and open in a new panel called "General" and under a tab called "Test".
If all goes well you should see the remote desktop without any problems.

.. figure:: /images/connections_open.png

To close the connection you can do any of the following:

- Log off in the start menu (Closes the connection and logs you out completely from RDP)
- Close the panel with the (Which leaves your session active on server but closes connection in mRemoteNG)
- Close the connection tab with (Also keeps your login active on server but closes RDP connection in mRemoteNG)
- Double click the connection tab (Same as above where the connection is active on server but closes RDP connection in mRemoteNG)
