*******************
SQL Server Settings
*******************

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    **Modifying the Registry**, **Restricted Registry Settings** and **Disclaimer** 
    on :doc:`Registry Settings Infromation <registry_settings_information>`.


Options
=======
Configure the options page to modify functionalities as described.

- **Registry Hive:** ``HKEY_LOCAL_MACHINE``
- **Registry Path:** ``SOFTWARE\mRemoteNG\SQLServer\Options``

Use SQL Server
--------------
Specifies whether SQL Server is being used.

- **Value Name:** ``UseSQLServer``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


SQL Server Type 
---------------
Specifies the type of SQL Server being used.

- **Value Name:** ``SQLServerType``
- **Value Type:** ``REG_SZ``
- **Values:**

  - ``mssql``
  - ``mysql``


SQL Host
--------
Specifies the hostname/IP/FQDN of the SQL Server.

- **Value Name:** ``SQLHost``
- **Value Type:** ``REG_SZ``


SQL Database Name
-----------------
Specifies the name/instance of the SQL database.

- **Value Name:** ``SQLDatabaseName``
- **Value Type:** ``REG_SZ``


SQL User
--------
Specifies the username for accessing the SQL Server.

- **Value Name:** ``SQLUser``
- **Value Type:** ``REG_SZ``


SQL User Password 
-----------------
Specifies the password for accessing the SQL Server.

- **Value Name:** ``SQLPassword``
- **Value Type:** ``REG_SZ``


.. warning::
  Plain-text passwords are not supported.


SQL Read Only
-------------
Specifies whether the SQL connection is read-only.

- **Value Name:** ``SQLReadOnly``
- **Value Type:** ``REG_SZ``
- **Values:**

  - Enable: ``true``
  - Disable: ``false``


Registry Template
=================

.. code::

    Windows Registry Editor Version 5.00

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\SQLServer]

    [HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\SQLServer\Options]
    "UseSQLServer"="false"
    "SQLDatabaseName"=""
    "SQLReadOnly"="true"
    "SQLUser_"=""
    "SQLServerType_"="MSSQL"
    "SQLHost_"=""

