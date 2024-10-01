*********************
Notification Settings
*********************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation <registry_settings_information>`.
    

Common
======

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\Notifications``

Allow Logging
-------------
Specifies whether logging to a file is allowed or not.

- **Value Name:** ``AllowLogging``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


Allow Notifications
-------------------
Specifies whether notifications are allowed or not.

- **Value Name:** ``AllowNotifications``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


Allow Popups
------------
Specifies whether pop-up notifications are allowed or not.

- **Value Name:** ``AllowPopups``
- **Value Type:** ``REG_SZ``
- **Default value:** ``true``
- **Values:**

  - Disallow: ``false``


Options: Notification Panel
===========================
Configure the options page to modify functionalities as described. These settings are scoped under the "Notification Panel".

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\Notifications\Options``

.. note::
   In some configurations, an initial application restart may be required after the initial launch.


Write Debug Messages
--------------------
Specifies whether debug messages are written to the notification panel. 
Show this message types checkbox: Debug

- **Value Name:** ``NfpWriteDebugMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Write Info Messages
-------------------
Specifies whether information messages are written to the notification panel.
Show this message types checkbox: Information

- **Value Name:** ``NfpWriteInfoMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Write Warning Messages
----------------------
Specifies whether warning messages are written to the notification panel.
Show this message types checkbox: Warning

- **Value Name:** ``NfpWriteWarningMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Write Error Messages
--------------------
Specifies whether error messages are written to the notification panel.
Show this message types checkbox: Error

- **Value Name:** ``NfpWriteErrorMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Switch To Notification Panel On Information
-------------------------------------------
Specifies whether to switch to notification panel when information messages are received.
Switch to notifications panel checkbox: Information

- **Value Name:** ``SwitchToMCOnInformation``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Switch To Notification Panel On Warning
---------------------------------------
Specifies whether to switch to notification panel when warning messages are received.
Switch to notifications panel checkbox: Warning

- **Value Name:** ``SwitchToMCOnWarning``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Switch To Notification Panel On Error
-------------------------------------
Specifies whether to switch to notification panel when error messages are received.
Switch to notifications panel checkbox: Error

- **Value Name:** ``SwitchToMCOnError``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Options: Logging Panel
======================
Configure the options page to modify functionalities as described. These settings are scoped under the "Logging Panel".

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\Notifications\Options``

.. note::
   In some configurations, an initial application restart may be required after the initial launch.


Log To Application Directory
----------------------------
Specifies whether logs should be written to the application directory.

- **Value Name:** ``LogToApplicationDirectory``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``

.. warning::
  Users must have permissions to the application dir!
  
.. note::
  If set to true, LogFilePath cannot be configured.


Log File Path
-------------
Specifies the file path for logging.

- **Value Name:** ``LogFilePath``
- **Value Type:** ``REG_SZ``

.. warning::
  Users must have permissions to write to the file!

.. note::
  If LogToApplicationDirectory is set to true, this setting has no effect; LogToApplicationDirectory must be set to false. If only LogFilePath is present, LogToApplicationDirectory will be automatically set to false.


Write Debug Messages
--------------------
Specifies whether debug messages should be written to the text log.

- **Value Name:** ``LfWriteDebugMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``

  
Write Info Messages
-------------------
Specifies whether information messages should be written to the text log.

- **Value Name:** ``LfWriteInfoMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Write Warning Messages
----------------------
Specifies whether warning messages should be written to the text log.

- **Value Name:** ``LfWriteWarningMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Write Error Messages
--------------------
Specifies whether error messages should be written to the text log.

- **Value Name:** ``LfWriteErrorMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Options: Popup Panel
====================
Configure the options page to modify functionalities as described. These settings are scoped under the "Popup Panel".

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\Notifications\Options``

.. warning::
  Settings could affect the user experience.

.. note::
   In some configurations, an initial application restart may be required after the initial launch.

  
Write Debug Messages
--------------------
Specifies whether debug messages should be displayed as popups.

- **Value Name:** ``PuWriteDebugMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``

.. warning::
  Activating could lead to the program becoming unusable. Suitable only for debugging purposes.

  
Write Info Messages
-------------------
Specifies whether information messages should be displayed as popups.

- **Value Name:** ``PuWriteInfoMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Write Warning Messages
----------------------
Specifies whether warning messages should be displayed as popups.

- **Value Name:** ``PuWriteWarningMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Write Error Messages
--------------------
Specifies whether error messages should be displayed as popups.

- **Value Name:** ``PuWriteErrorMsgs``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Registry Template
=================

.. code::

    Windows Registry Editor Version 5.00

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Notifications]
    "AllowLogging"="false"
    "AllowNotifications"="false"
    "AllowPopups"="false"

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\Notifications\Options]
    "NfpWriteDebugMsgs"="true"
    "NfpWriteInfoMsgs"="true"
    "NfpWriteWarningMsgs"="true"
    "NfpWriteErrorMsgs"="true"
    "SwitchToMCOnInformation"="false"
    "SwitchToMCOnWarning"="false"
    "SwitchToMCOnError"="false"
    "LfWriteDebugMsgs"="true"
    "LfWriteErrorMsgs"="true"
    "LfWriteInfoMsgs"="true"
    "LfWriteWarningMsgs"="true"
    "LogFilePath"="c:\...."
    "LogToApplicationDirectory"="false"
    "PuWriteDebugMsgs"="false"
    "PuWriteErrorMsgs"="false"
    "PuWriteInfoMsgs"="false"
    "PuWriteWarningMsgs"="false"




