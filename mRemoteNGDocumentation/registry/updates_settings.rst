.. _updates_settings:

******************
Updates Settings
******************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation </registry_settings_information>`.
    

Common settings
===============
These settings are defined for global configuration.


AllowCheckForUpdates 
--------------------
Allows or disallows checking for updates.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates
- **Value Name:** AllowCheckForUpdates
- **Value Type:** REG_SZ
- **Values:**
  
  - **Enable (default):** true
  - **Disable:** false


AllowCheckForUpdatesAutomatical
-------------------------------
Allows or disallows automatic search for updates.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates
- **Value Name:** AllowCheckForUpdatesAutomatical
- **Value Type:** REG_SZ
- **Values:**
  
  - **Enable (default):** true
  - **Disable:** false

.. note::
   If "AllowCheckForUpdates" is set to false, the automatic update check is already disabled.


AllowCheckForUpdatesManual
--------------------------
Allows or disallows manual search for updates.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates
- **Value Name:** AllowCheckForUpdatesManual
- **Value Type:** REG_SZ
- **Values:**
  
  - **Enable (default):** true
  - **Disable:** false

.. note::
   If "AllowCheckForUpdates" is set to false, the automatic update check is already disabled.


AllowPromptForUpdatesPreference
-------------------------------
Controls whether a prompt for checking the updates preferences is displayed at startup.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates
- **Value Name:** AllowPromptForUpdatesPreference
- **Value Type:** REG_SZ
- **Values:**
  
  - **Enable:** true
  - **Disable:** false


Option Page Settings
====================
Configure the options page to modify functionalities as described.


CheckForUpdatesFrequencyDays
----------------------------
Specifies the number of days between automatic update checks.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates\\Options
- **Value Name:** CheckForUpdatesFrequencyDays
- **Value Type:** REG_DWORD

.. note::
   If 'AllowCheckForUpdates' is set to false, the automatic update check is already disabled, and 'CheckForUpdatesFrequencyDays' does not take effect.


UpdateChannel
-------------
Specifies the preferred update channel. Important note: Values are case-sensitive!

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates\\Options
- **Value Name:** UpdateChannel
- **Value Type:** REG_SZ
- **Values:**
  
  - Channel: Stable
  - Channel: Nightly
  - Channel: Preview


UseProxyForUpdates
------------------
Indicates whether proxy usage for updates is enabled.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates\\Options
- **Value Name:** UseProxyForUpdates
- **Value Type:** REG_SZ
- **Values:**
  
  - Enable: true
  - Disable: false


ProxyAddress
------------
Specifies the address of the proxy for updates.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates\\Options
- **Value Name:** ProxyAddress
- **Value Type:** REG_SZ

.. note::
    If 'UseProxyForUpdates' is disabled, these settings do not take effect.


ProxyPort
---------
Specifies the port used for proxy connections during updates.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates\\Options
- **Value Name:** ProxyPort
- **Value Type:** REG_DWORD

.. note::
    If 'UseProxyForUpdates' is disabled, these settings do not take effect.


UseProxyAuthentication
----------------------
Indicates whether proxy authentication is enabled.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates\\Options
- **Value Name:** UseProxyAuthentication
- **Value Type:** REG_SZ
- **Values:**
  - Enable Value: true
  - Disable Value: false

.. note::
    If 'UseProxyForUpdates' is disabled, these settings do not take effect.


ProxyAuthUser
-------------
Specifies the authentication username for the proxy.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates\\Options
- **Value Name:** ProxyAuthUser
- **Value Type:** REG_SZ

.. note::
    If 'UseProxyForUpdates' is disabled, these settings do not take effect.

.. note::
    If 'ProxyAuthUser' is disabled, these settings do not take effect.

ProxyAuthPass 
-------------
(currently not supported)
Represents the authentication password for the proxy.

.. warning::

    Do not store decrypted passwords in the registry!

    Storing decrypted passwords in the registry poses a significant security risk and is strongly discouraged. It can expose sensitive information, compromise user credentials, and lead to unauthorized access. Always follow best security practices and avoid storing plaintext passwords in any form, including the registry.


- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Updates\\Options
- **Value Name:** ProxyAuthPass
- **Value Type:** REG_DWORD

.. note::
    If 'UseProxyForUpdates' is disabled, these settings do not take effect.

.. note::
    If 'ProxyAuthUser' is disabled, these settings do not take effect.