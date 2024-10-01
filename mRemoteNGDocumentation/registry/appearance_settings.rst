*******************
Appearance Settings
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
- **Registry Path:** ``SOFTWARE\mRemoteNG\Appearance\Options``


Show Description Tooltips In Connection Tree
--------------------------------------------
Specifies whether to show tooltips with descriptions in the connection tree view.

- **Value Name:** ``ShowDescriptionTooltipsInConTree``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Show: ``true``
  - Hide: ``false``


Show Complete Connection File Path In Title
-------------------------------------------
Specifies whether to show complete connection path in the window title.

- **Value Name:** ``ShowCompleteConFilePathInTitle``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Show: ``true``
  - Hide: ``false``


Always Show System Tray Icon
----------------------------
Specifies whether to always show the system tray icon.


- **Value Name:** ``AlwaysShowSystemTrayIcon``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Show: ``true``
  - Hide: ``false``


Minimize To Tray
----------------
Specifies whether the application should minimize to the system tray.

- **Value Name:** ``MinimizeToTray``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Close To Tray
-------------
Specifies whether the application should close to the system tray.

- **Value Name:** ``CloseToTray``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Registry Template
=================

.. code::
  
    Windows Registry Editor Version 5.00

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Appearance]

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Appearance\Options]
    "AlwaysShowSystemTrayIcon"="false"
    "CloseToTray"="false"
    "MinimizeToTray"="true"
    "ShowCompleteConFilePathInTitle"="true"
    "ShowDescriptionTooltipsInConTree"="true"