.. _startupExit_settings:

*************************
Startup and Exit Settings
*************************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation <registry_settings_information>`.
    

Common settings
===============
These settings are defined for global configuration.


StartupBehavior 
---------------
Specifies whether the application should start minimized or fullscreen.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\StartupExit
- **Value Name:** StartupBehavior
- **Value Type:** REG_SZ
- **Values:**
  
  - StartupBehavior (default): None
  - StartupBehavior: Minimized
  - StartupBehavior: FullScreen


Option Page Settings
====================
Configure the options page to modify functionalities as described.


SaveConnectionsOnExit
---------------------
Specifies whether connections should be saved on application exit

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\StartupExit\\Options
- **Value Name:** SaveConnectionsOnExit
- **Value Type:** REG_SZ
- **Values:**
  
  - Enable: true
  - Disable: false


OpenConnectionsFromLastSession
------------------------------
Specifies whether sessions should be automatically reconnected on application startup.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\StartupExit\\Options
- **Value Name:** OpenConnectionsFromLastSession
- **Value Type:** REG_SZ
- **Values:**
  
  - Enable: true
  - Disable: false


EnforceSingleApplicationInstance
--------------------------------
Ensures that only a single instance of the application is allowed to run.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\StartupExit\\Options
- **Value Name:** EnforceSingleApplicationInstance
- **Value Type:** REG_SZ
- **Values:**
  
  - Enable: true
  - Disable: false
