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
Specifies the number of iterations used in the encryption key derivation process.

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
  "EncryptionKeyDerivationIterations"=dword:00009c40
