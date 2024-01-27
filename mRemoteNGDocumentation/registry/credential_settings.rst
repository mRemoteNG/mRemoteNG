.. _credential_settings:

*********************
Credential Settings
*********************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation </registry_settings_information>`.
    

Common settings
===============
These settings are defined for global configuration.


AllowExportUsernames
--------------------
Determines whether exporting usernames is allowed.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials
- **Value Name:** AllowExportUsernames
- **Value Type:** REG_SZ
- **Values:**
  
  - **Enable (default):** true
  - **Disable:** false


AllowExportPasswords
--------------------
Determines whether exporting passwords is allowed.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials
- **Value Name:** AllowExportPasswords
- **Value Type:** REG_SZ
- **Values:**
  
  - **Enable (default):** true
  - **Disable:** false


AllowSaveUsernames
------------------
Determines whether saving usernames is allowed.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials
- **Value Name:** AllowSaveUsernames
- **Value Type:** REG_SZ
- **Values:**
  
  - **Enable (default):** true
  - **Disable:** false

.. note::
   If 'AllowSaveUsernames' is set to false, stored user names in the connection persist until the connection goes through modification or usage. 
   Subsequently, stored user names are removed. 
   Additionally, new connections will not be able to store usernames.


AllowSavePasswords
------------------
Determines whether saving passwords is allowed.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials
- **Value Name:** AllowSavePasswords
- **Value Type:** REG_SZ
- **Values:**
  
  - **Enable (default):** true
  - **Disable:** false

.. note::
   If 'AllowSavePasswords' is set to false, stored passwords in the connection persist until the connection goes through modification or usage. 
   Subsequently, stored passwords are removed.
   Additionally, new connections will not be able to store passwords.


AllowModifyCredentialSettings
-----------------------------
Specifies if the 'Credentials' option page is configurable.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials
- **Value Name:** AllowModifyCredentialSettings
- **Value Type:** REG_SZ
- **Values:**
  
  - **Enable (default):** true
  - **Disable:** false

.. note::
   If 'AllowModifyCredentialSettings' is set to false, 'UseCredentials' is automatically set to 'None' (noinfo).


Option Page Settings
====================
Configure the options page to modify functionalities as described.

UseCredentials
--------------
Specifies the radio button state on the credentials page to prefill empty credential fields:
- "None" is selected to leave the fields unfilled.
- "Windows Logon Information" is chosen to autofill with Single Sign-On (SSO) data.
- "Custom" is opted for utilizing the defined information.

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials\\Options
- **Value Name:** UseCredentials
- **Value Type:** REG_SZ
- **Values:**
  
  - Radio (1) None: `noinfo`
  - Radio (2) Windows Logon: `windows`
  - Radio (3) Custom: `custom`


UserViaAPIDefault
-----------------
Specifies the user set via API as the default username.
Important: only used when "UseCredentials" is set to "Custom".

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials\\Options
- **Value Name:** UserViaAPIDefault
- **Value Type:** REG_SZ

.. note::
  Only takes effect if 'UseCredentials' is set to custom.


DefaultUsername
---------------
Specifies the default username.
Important: only used when "UseCredentials" is set to "Custom".

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials\\Options
- **Value Name:** DefaultUsername
- **Value Type:** REG_SZ

.. note::
  Only takes effect if 'UseCredentials' is set to custom.


DefaultPassword
---------------
(currently not supported)

Specifies the default password.

.. warning::

    Do not store decrypted passwords in the registry!

    Storing decrypted passwords in the registry poses a significant security risk and is strongly discouraged. It can expose sensitive information, compromise user credentials, and lead to unauthorized access. Always follow best security practices and avoid storing plaintext passwords in any form, including the registry.


- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials\\Options
- **Value Name:** DefaultPassword
- **Value Type:** REG_SZ

.. note::
  Only takes effect if 'UseCredentials' is set to custom.


DefaultDomain
-------------
Specifies the default domain.
Important: only used when "UseCredentials" is set to "Custom".

- **Registry Hive:** HKEY_LOCAL_MACHINE
- **Registry Path:** SOFTWARE\\mRemoteNG\\Credentials\\Options
- **Value Name:** DefaultDomain
- **Value Type:** REG_SZ

.. note::
  Only takes effect if 'UseCredentials' is set to custom.