*******************
Connection Settings
*******************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation <registry_settings_information>`.
    

Options
=======
Configure the options page to modify functionalities as described.

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\Connections\Options``

Single Click On Connection To Open
----------------------------------
Specifies whether to single click to connection opens/establishes connection

- **Value Name:** ``SingleClickOnConnectionOpensIt``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Single Click Switches To Open Connection
----------------------------------------
Specifies whether a single click on an open connection switches the focused tab to that connection.

- **Value Name:** ``SingleClickSwitchesToOpenConnection``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Track Active Connection In Connection Tree
------------------------------------------
This specifies whether to track open connections in the connection tree. 
When switching to an active connection, the focus in the tree view will also switch through the connection.

- **Value Name:** ``TrackActiveConnectionInConnectionTree``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Set Hostname Like Display Name
------------------------------
Specifies whether to set the hostname like the display name when creating or renaming a connection.

- **Value Name:** ``SetHostnameLikeDisplayName``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Use Filter Search
-----------------
Specifies whether filters applied in the search are reflected in the connection tree. 
This determines whether the filter hides connections that do not match (value true) or only highlights those that do match (value false).

- **Value Name:** ``UseFilterSearch``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Place Search Bar Above Connection Tree
--------------------------------------
Specifies whether the search bar is placed above the connection tree.

- **Value Name:** ``PlaceSearchBarAboveConnectionTree``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Do Not Trim Username
--------------------
Specifies whether username trimming is enabled or disabled. 
If ``true``, spaces at the beginning or end of a username will be trimmed. 
If ``false``, spaces will not be trimmed.

- **Value Name:** ``DoNotTrimUsername``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Trim: ``false``
  - Don't Trim: ``true``


RDP Reconnection Count
----------------------
Specifies the number of attempts for RDP reconnections.

- **Value Name:** ``RdpReconnectionCount``
- **Value Type:** ``REG_DWORD``
- **Values:**

  - Minimum: ``0``
  - Maximum: ``20``


RDP Overall Connection Timeout
------------------------------
Specifies the overall connection timeout for RDP connections.

- **Value Name:** ``ConRDPOverallConnectionTimeout``
- **Value Type:** ``REG_DWORD``
- **Values:**

  - Minimum: ``20``
  - Maximum: ``600``


Auto Save Intervall
-------------------
Specifies the autosave interval in minutes. 

- **Value Name:** ``AutoSaveEveryMinutes``
- **Value Type:** ``REG_DWORD``
- **Values:**

  - Minimum: ``0``
  - Maximum: ``9999``


.. note::
   To disable autosave, set *AutoSaveEveryMinutes* to ``0``.


Registry Template
=================

.. code::
  
    Windows Registry Editor Version 5.00

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Connections]

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Connections\Options]
    "SingleClickOnConnectionOpensIt"="true"
    "SingleClickSwitchesToOpenConnection"="true"
    "TrackActiveConnectionInConnectionTree"="false"
    "SetHostnameLikeDisplayName"="true"
    "UseFilterSearch"="false"
    "PlaceSearchBarAboveConnectionTree"="false"
    "DoNotTrimUsername"="true"
    "AutoSaveEveryMinutes"=dword:00000010
    "ConRDPOverallConnectionTimeout"=dword:0000012c
    "RdpReconnectionCount"=dword:0000000a

