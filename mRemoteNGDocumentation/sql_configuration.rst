.. _sql_configuration:

*****************
SQL Configuration
*****************

.. warning::

    The SQL feature is in an early beta stage and not intended for use in a production environment! I recommend you to do a full backup of your connections and settings before switching to SQL Server.

Supported Databases
===================

The list below includes databases that are officially supported. Others may already work and this list may expand with future updates.

- MSSQL
- MySQL

Steps to configure your SQL Server
==================================
- Create a new Database called "mRemoteNG" on your SQL Server.
- Give the users that you want to grant access to the mRemoteNG Connections Database Read/Write permissions on the Database.

Steps to configure mRemoteNG for SQL
====================================
- Start mRemoteNG if it's not already running.
- Go to Tools - Options - SQL Server
- Check the box that says "Use SQL Server to load & save connections".
- Fill in your SQL Server hostname or ip address.
- If you do not use your Windows logon info to authenticate against the SQL Server fill in the correct Username and Password.
- Click OK to apply the changes. The main window title should now change to "mRemoteNG | SQL Server".
- Now click on File - Save to update the tables on your SQL Server with the data from the loaded connections xml file. (Do not click File - New, this doesn't work yet)
- You should now be able to do everything you were able to do with the XML storage plus see the changes live on another mRemoteNG instance that is connected to the same Database.

Database version 3.4 and the stored passwords
=============================================
From database version 3.4 on, the passwords stored in the database are encrypted with AES-GCM under a key derived once for the database from the connections password, with PBKDF2-HMAC-SHA256, 600,000 iterations and a random salt kept in ``tblRoot`` (columns ``KdfSalt`` and ``KdfIterations``). Earlier versions used the MD5 of the password as the key, with no salt.

The first time this version of mRemoteNG opens a database in version 3.3 or earlier, after the password is entered, it upgrades the database to version 3.4: it adds the two columns to ``tblRoot`` and encrypts every stored password again under the new key, in one transaction, so if anything fails the passwords stay as they were.

- Back up the database before the first start of this version.
- Update every mRemoteNG that uses the database at the same time: earlier versions cannot read the passwords of a database in version 3.4.
- The account mRemoteNG connects with needs permission to alter ``tblRoot`` for the upgrade.
- A database in a version newer than the one this version of mRemoteNG reads is not opened; the message names both versions.
