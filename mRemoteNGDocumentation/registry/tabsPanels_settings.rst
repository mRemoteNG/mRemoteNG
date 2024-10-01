***********************
Tabs and Panel Settings
***********************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation <registry_settings_information>`.
    

Options
=======
Configure the options page to modify functionalities as described.

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\TabsAndPanels\Options``

Always Show Panel Tabs
----------------------
Specifies whether panel tabs are always shown or not. This is useful when the panel attribute in a connection is set to group open connections.

- **Value Name:** ``AlwaysShowPanelTabs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Show Logon Info On Tabs
-----------------------
Specifies whether logon information (username) is shown on tabs.

- **Value Name:** ``ShowLogonInfoOnTabs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Show Protocol On Tabs
---------------------
Specifies whether protocol information is displayed on tabs (e.g., RDP:).

- **Value Name:** ``ShowProtocolOnTabs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Marking Quick Connect Tabs
--------------------------
Specifies whether quick connect tabs are marked by the prefix "Quick:".

- **Value Name:** ``IdentifyQuickConnectTabs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Double Click To Close Tab
-------------------------
Specifies whether double-clicking on a tab closes it.

- **Value Name:** ``DoubleClickOnTabClosesIt``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Always Show Panel Selection Dialog
----------------------------------
Specifies whether the panel selection dialog is always shown. 
When set to true, initiating a connection will prompt a dialog to appear, allowing the user to choose the panel to which the connection will be added or create a new one.


- **Value Name:** ``AlwaysShowPanelSelectionDlg``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Create Empty Panel On Start Up
------------------------------
Specifies whether an empty panel is created on startup.

- **Value Name:** ``CreateEmptyPanelOnStartUp``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Start Up Panel Name
-------------------
Specifies the name of the startup panel.

- **Value Name:** ``StartUpPanelName``
- **Value Type:** ``REG_SZ``
- **Values:**


.. note::
  It doesn't take effect if 'CreateEmptyPanelOnStartUp' is unchecked.


Registry Template
=================

.. code::

  Windows Registry Editor Version 5.00

  [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\TabsAndPanels]

  [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\TabsAndPanels\Options]
  "AlwaysShowPanelTabs"="true"
  "ShowLogonInfoOnTabs"="true"
  "ShowProtocolOnTabs"="true"
  "IdentifyQuickConnectTabs"="true"
  "DoubleClickOnTabClosesIt"="true"
  "AlwaysShowPanelSelectionDlg"="true"
  "CreateEmptyPanelOnStartUp"="true"
  "StartUpPanelName"=""

