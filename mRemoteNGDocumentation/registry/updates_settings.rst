******************
Updates Settings
******************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation <registry_settings_information>`.
    

Common
======

- Registry Hive: ``HKEY_LOCAL_MACHINE``
- Registry Path: ``SOFTWARE\mRemoteNG\Updates``


Allow Check For Updates 
-----------------------
Allows or disallows checking for updates.

- **Value Name:** ``AllowCheckForUpdates``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


Allow Check For Updates Automatical
-----------------------------------
Allows or disallows automatic search for updates.

- **Value Name:** ``AllowCheckForUpdatesAutomatical``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


.. note::
   If **AllowCheckForUpdates** is set to ``false``, the automatic update check is already disabled.


Allow Check For Updates Manual
------------------------------
Allows or disallows manual search for updates.

- **Value Name:** ``AllowCheckForUpdatesManual``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


.. note::
   If **AllowCheckForUpdates** is set to ``false``, the automatic update check is already disabled.


Options
=======
Configure the options page to modify functionalities as described.

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\Updates\Options``


Disallow Prompt For Updates Preference
--------------------------------------
Specifies whether a popup is shown to configure update preferences at startup.

- **Value Name:** ``DisallowPromptForUpdatesPreference``
- **Value Type:** ``REG_SZ``
- **Values:**

  - to disable promt: ``true``


Check For Updates Frequency Days
--------------------------------
Specifies the number of days between automatic update checks.

- **Value Name:** ``CheckForUpdatesFrequencyDays``
- **Value Type:** ``REG_DWORD``

.. note::
   If **AllowCheckForUpdates** is set to ``false``, the automatic update check is already disabled, and **CheckForUpdatesFrequencyDays** does not take effect.


Update Channel
--------------
Specifies the preferred update channel.

- **Value Name:** ``UpdateChannel``
- **Value Type:** ``REG_SZ``
- **Values:**
  
  - Channel: ``Stable``
  - Channel: ``Nightly``
  - Channel: ``Preview``


Use Proxy For Updates
---------------------
Indicates whether proxy usage for updates is enabled.

- **Value Name:** ``UseProxyForUpdates``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Proxy Address
-------------
Specifies the address of the proxy for updates.

- **Value Name:** ``ProxyAddress``
- **Value Type:** ``REG_SZ``

.. note::
    If **UseProxyForUpdates** is ``false``, these settings do not take effect.


Proxy Port
----------
Specifies the port used for proxy connections during updates.

- **Value Name:** ``ProxyPort``
- **Value Type:** ``REG_DWORD``

.. note::
    If **UseProxyForUpdates** is ``false``, these settings do not take effect.


Use Proxy Authentication
------------------------
Indicates whether proxy authentication is enabled.

- **Value Name:** ``UseProxyAuthentication``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``

.. note::
    If **UseProxyForUpdates** is ``false``, these settings do not take effect.


Proxy Auth User
---------------
Specifies the authentication username for the proxy.

- **Value Name:** ``ProxyAuthUser``
- **Value Type:** ``REG_SZ``

.. note::
    If **UseProxyForUpdates** and **ProxyAuthUser** is ``false``, these settings do not take effect.


Proxy Auth Pass 
---------------
**(currently not supported)**

Represents the authentication password for the proxy.

- **Value Name:** ``ProxyAuthPass``
- **Value Type:** ``REG_DWORD``

.. note::
    If **UseProxyForUpdates** and **ProxyAuthUser** is ``false``, these settings do not take effect.


.. warning::
  Plain-text passwords are not supported.


Registry Template
=================

.. code::

    Windows Registry Editor Version 5.00

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Updates]
    "AllowCheckForUpdates"="false"
    "AllowCheckForUpdatesAutomatical"="false"
    "AllowCheckForUpdatesManual"="false"
    

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Updates\Options]
    "DisallowPromptForUpdatesPreference"="true"
    "CheckForUpdatesFrequencyDays"=dword:00000014
    "UpdateChannel"="Stable"
    
    "UseProxyForUpdates"="false"
    "ProxyAddress"=""
    "ProxyPort"=dword:00000050

    "UseProxyAuthentication"="false"
    "ProxyAuthUser"=""
    "ProxyAuthPass"=""
