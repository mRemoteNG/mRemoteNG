*************
Quick Connect
*************

The Quick Connect functionality of mRemoteNG allows you to quickly connect to a remote host using a variety of network protocols.

Prerequisites
=============
- Knowledge of a DNS host name or IP address
- Knowledge of an appropriate protocol to communicate with remote host
- A predefined mRemoteNG connection

Using QuickConnect
==================

.. tip::

    You can input username and hostname simultaneously when using user@domain as connection string.

To use Quick Connect, ensure the Quick Connect toolbar is enabled by selecting View and then Quick Connect Toolbar.
Next, input a DNS host name or IP address into the box labeled "Connect". This box will also save previous entries during your session.

.. figure:: /images/quick_connect_01.png

.. figure:: /images/quick_connect_02.png

Select the appropriate network protocol by clicking the arrow next to the Connect box.

.. figure:: /images/quick_connect_03.png

If you wish to use an existing connection, select the globe icon next to the protocol button and select the appropriate connection.

Configuration
=============

Quick connections take most of their configuration from the :doc:`default_connection_properties`.
All default properties are used `except` for:

- Hostname
- Protocol
- Port (the default port for the selected protocol is used)
