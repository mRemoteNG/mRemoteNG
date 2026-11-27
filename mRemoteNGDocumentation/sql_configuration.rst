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

Concurrent edits
================

Shared trees use schema 3.4 and a revision recorded when the tree is loaded.
If another writer saves that revision first, your save is rejected and your
draft stays in memory. Automatic synchronization will not replace that draft.
Preserve the edits you need before explicitly reloading the shared tree;
reloading discards the draft and resumes automatic synchronization.

Root metadata, connection rows, the update marker and the next revision are
saved in one transaction. Loading captures the root metadata, encrypted rows
and revision together, then prompts for the password for that captured version.
Existing shared databases require a coordinated schema cutover before use;
the disposable integration test profile does not perform that cutover.
