*********************
Credential Settings
*********************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation <registry_settings_information>`.
    

Common
======

- Registry Hive: ``HKEY_LOCAL_MACHINE``
- Registry Path: ``SOFTWARE\mRemoteNG\Credentials``


Allow Export Usernames
----------------------
Specifies whether the export of usernames for saved connections is allowed.

- **Value Name:** ``AllowExportUsernames``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


Allow Export Passwords
----------------------
Specifies whether the export of passwords for saved connections is allowed.

- **Value Name:** ``AllowExportPasswords``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


Allow Save Usernames
--------------------
Specifies whether the saving of usernames for saved connections is allowed.

- **Value Name:** ``AllowSaveUsernames``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


.. note::
   If **AllowSaveUsernames** is set to ``false``, 
   stored user names in the connection persist until the connection goes through modification or usage. 
   Subsequently, stored user names are removed. 
   Additionally, new connections will not be able to store usernames.


Allow Save Passwords
--------------------
Specifies whether the saving of passwords for saved connections is allowed.

- **Value Name:** ``AllowSavePasswords``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


.. note::
   If **AllowSavePasswords** is set to ``false``, 
   stored passwords in the connection persist until the connection goes through modification or usage. 
   Subsequently, stored passwords are removed.
   Additionally, new connections will not be able to store passwords.


Options
=======
Configure the options page to modify functionalities as described.

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\Credentials\Options``

Use Credentials
---------------
Specifies the radio button state on the credentials page to prefill empty credential fields:

(1) "None" is selected to leave the fields unfilled.
(2) "Windows Logon Information" is chosen to autofill with Single Sign-On (SSO) data.
(3) "Custom" is opted for utilizing the defined information.


- **Value Name:** ``UseCredentials``
- **Value Type:** ``REG_SZ``
- **Values:**

  - (1): ``noinfo``
  - (2): ``windows``
  - (3): ``custom``


User Via API Default
--------------------
Specifies the user set via API as the default username.

- **Value Name:** ``UserViaAPIDefault``
- **Value Type:** ``REG_SZ``

.. note::
  Only takes effect if *UseCredentials* is set to ``custom`` or *DefaultUserViaAPIEnabled* is set to ``false``.


Default Username
----------------
Specifies the default username.

- **Value Name:** ``DefaultUsername``
- **Value Type:** ``REG_SZ``

.. note::
  Only takes effect if *UseCredentials* is set to ``custom`` or *DefaultUsernameEnabled* is set to ``false``.


Default Password
----------------
Specifies the default password.

- **Value Name:** ``DefaultPassword``
- **Value Type:** ``REG_SZ``


.. warning::
  Plain-text passwords are not supported.


.. note::
  Only takes effect if *UseCredentials* is set to ``custom`` or *DefaultPasswordEnabled* is set to ``false``.


Default Domain
--------------
Specifies the default domain.

- **Value Name:** ``DefaultDomain``
- **Value Type:** ``REG_SZ``

.. note::
  Only takes effect if *UseCredentials* is set to ``custom``.


Default Username Enabled
------------------------
Controls whether the default username field is enabled or disabled. 
Locking the field may make more sense than disabling the entire settings option.

- **Value Name:** ``DefaultUsernameEnabled``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - to disable: ``false``


Default Password Enabled
------------------------
Controls whether the default password field is enabled or disabled. 
Locking the field may make more sense than disabling the entire settings option.

- **Value Name:** ``DefaultPasswordEnabled``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - to disable: ``false``


Default User Via API Enabled
----------------------------
Controls whether the default user via API field is enabled or disabled. 
Locking the field may make more sense than disabling the entire settings option.

- **Value Name:** ``DefaultUserViaAPIEnabled``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - to disable: ``false``


Registry Template
=================

.. code::

  Windows Registry Editor Version 5.00

  [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Credentials]
  "AllowExportPasswords"="false"
  "AllowExportUsernames"="false"
  "AllowSavePasswords"="false"
  "AllowSaveUsernames"="false"

  [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Credentials\Options]
  "UseCredentials"="custom"
  "UserViaAPIDefault"=""
  "DefaultUsername"=""
  "DefaultPassword"=""
  "DefaultDomain"=""

  "DefaultUsernameEnabled"="false"
  "DefaultPasswordEnabled"="false"
  "DefaultUserViaAPIEnabled"="false"

