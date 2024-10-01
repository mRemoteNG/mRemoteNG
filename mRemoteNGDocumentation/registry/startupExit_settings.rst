*************************
Startup and Exit Settings
*************************
.. versionadded:: v1.77.3

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation <registry_settings_information>`.
    

Options
=======
Configure the options page to modify functionalities as described.

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\StartupExit\Options``


Startup Behavior
----------------
Specifies the startup behavior of the application:

- None: The application starts without any specific behavior, using the last closed resolution or position.
- Minimized: The application starts minimized.
- FullScreen: The application starts in fullscreen mode.

- **Value Name:** ``StartupBehavior``
- **Value Type:** ``REG_SZ``
- **Values:**

  - ``None``
  - ``Minimized``
  - ``FullScreen``


Open Connections From Last Session
----------------------------------
Specifies whether sessions should be automatically reconnected on application startup.

- **Value Name:** ``OpenConnectionsFromLastSession``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Enforce Single Application Instance
-----------------------------------
Ensures that only a single instance of the application is allowed to run.

- **Value Name:** ``EnforceSingleApplicationInstance``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``
   


Registry Template
=================

.. code::

    Windows Registry Editor Version 5.00

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\StartupExit]

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\StartupExit\Options]
    "EnforceSingleApplicationInstance"="true"
    "OpenConnectionsFromLastSession"="true"
    "StartupBehavior"="FullScreen"


