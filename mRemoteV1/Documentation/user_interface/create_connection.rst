***********
Connections
***********

Creating a connection
=====================

.. tip::

    You can see an indicator in the properties window that is glowing green:

    .. figure:: /images/connections_status.png

    This icon does a ICMP ping on to check response from the server. If it glows green it indicates a connection response can be made using ping to the host. However this is turned off on windows by default. You have to enable ICMP and allow the firewall access for it.


Right click on the root item (the little blue globe named **Connections**) in the Connections panel and select **New Connection**.

.. figure:: /images/connections_rightclick_menu.png

A new item shows up under the root item. You can give it a name now (or rename it later). We'll just call this connection "Test" for the moment.

.. figure:: /images/connections_test_item.png

Now lets look at the config panel in the bottom left, just under the connections panel. As you may notice this is where you configure all the properties of connections and folders.

.. figure:: /images/connections_config.png

Fill in the necessary properties and you have just created your first connection!
You can now connect to the server with a simple double-click on the "Test"-connection!

Opening and Closing Connections
===============================

.. note::

    If the connecting fails, the notifications panel will pop up and show an error message describing the problem.

There are multiple ways to open a connection in mRemoteNG, but the easiest way is to double click the connection in the Connections panel. 
If you double click the connection you will notice that the connection is going to try and open in a new panel called "General" and under a tab called "Test". 
If all goes well you should see the remote desktop without any problems.

.. figure:: /images/connections_open.png

To close the connection you can do any of the following:
- Log off in the start menu - Closes the connection and logs you out completely from RDP
- Close the panel with the - Which leaves your session active on server but closes connection in mRemoteNG
- Close the connection tab with - Also keeps your login active on server but closes RDP connection in mRemoteNG
- Double click the connection tab - Same as above where the connection is active on server but closes RDP connection in mRemoteNG
