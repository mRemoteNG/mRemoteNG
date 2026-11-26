*****************
Security Settings
*****************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation <registry_settings_information>`.
    

Options
====================
Configure the options page to modify functionalities as described.

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\Security\Options``

Encrypt Complete Connections File
---------------------------------
Specifies the encryption engine used for encryption.

- **Value Name:** ``EncryptCompleteConnectionsFile``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Use DPAPI Protection
--------------------
.. versionadded:: v1.78.2

Specifies whether the connections file is protected with Windows DPAPI (tied to the current
Windows user account and machine) instead of a password-derived key. When enabled, the
Encryption Engine, Block Cipher Mode and Key Derivation Iterations settings below have no effect,
since DPAPI uses none of them. See :doc:`../howtos/dpapi_protection` for details.

- **Value Name:** ``UseDpapiProtection``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Encryption Engine
-----------------
Specifies the encryption engine used for encryption.

- **Value Name:** ``EncryptionEngine``
- **Value Type:** ``REG_SZ``
- **Values:**

  - ``AES``
  - ``Serpent``
  - ``Twofish``


Encryption Block Cipher Mode
----------------------------
Specifies the block cipher mode used for encryption.

- **Value Name:** ``EncryptionBlockCipherMode``
- **Value Type:** ``REG_SZ``
- **Values:**

  - ``GCM``
  - ``CCM``
  - ``EAX``


Encryption Key Derivation Iterations
------------------------------------
Specifies the number of iterations used to derive the key of each encrypted value in the credential
repository list and the user profile store. It does not apply to the connections file: from file
format 3.0 on, the key of a connections file is derived once for the whole file, with 600,000
iterations and a salt written in the file.

- **Value Name:** ``EncryptionKeyDerivationIterations``
- **Value Type:** ``REG_DWORD``
- **Values:**

  - Minimum: ``1000``
  - Maximum: ``50000``


Registry Template
=================

.. code::
  
  Windows Registry Editor Version 5.00

  [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Security]

  [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Security\Options]
  "EncryptionEngine"="AES"
  "EncryptionBlockCipherMode"="GCM"
  "EncryptCompleteConnectionsFile"="false"
  "UseDpapiProtection"="false"
  "EncryptionKeyDerivationIterations"=dword:00009c40
