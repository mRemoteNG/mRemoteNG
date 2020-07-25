***************
Troubleshooting
***************

Logfile
=======

The mRemoteNG.log is located in the following location:

MSI/Installed version
---------------------

::

   %APPDATA%\mRemoteNG\mRemoteNG.log
   (example: `C:\Users\[username]\AppData\Roaming\mRemoteNG\mRemoteNG.log`)

Portable version
----------------

::

   [location of mRemoteNG.exe]\mRemoteNG.log

Crash at Startup
================

Try deleting the :code:`user.config` file. It contains all the user-specific program settings. This file is automatically upgraded between version when new user settings are added.

Installed Version
-----------------

::

   %LOCALAPPDATA%\mRemoteNG\<most recently updated folder>\<mRemoteNG version>\user.config

Portable Version
----------------

::

   %APPDATA%\mRemoteNG\<most recently updated folder>\<mRemoteNG version>\user.config

Crash Information
=================

- Provide the Stack Trace from the crash prompt or from the Windows Application Event Log `example <https://blogs.msdn.microsoft.com/cobold/2010/03/01/collecting-crash-dumps/>`_)
- Check `C:\Users\All Users\Microsoft\Windows\WER\Report*` folders for any reports related to mRemoteNG
- Check `%LOCALAPPDATA%\CrashDumps <https://msdn.microsoft.com/en-us/library/windows/desktop/bb787181(v=vs.85).aspx>`_ for any mRemoteNG.exe.*.dmp files
- Attach the Error Reports, Dumps and mRemoteNG.log to a new `Issue <https://github.com/mRemoteNG/mRemoteNG/issues>`_

Backup and Recovery
===================

By default, your connections file is backed up every time it is saved.
These backup files are normal/valid connections file - they have only been renamed to avoid being overwritten.
mRemoteNG will save the 10 most recent backups.

Files and Locations
-------------------

Your backup files are located in the same place as your normal connections file.
This could be one of three places:

- Normal version: `%AppData%\\mRemoteNG`
- Portable version: In the same location as mRemoteNG.exe
- If you have saved your confCons.xml to a custom location, go there.

There are 2 different backup naming schemes:

- `confCons.xml.backup` is the most recent backup that was taken.
- `confCons.xml.YYYYMMDD-HHmmssxxxx.backup` is a rolling backup that was moved to a rolling backup file on the date specified in the file name.

Recovering corrupted connections file
-------------------------------------

If you find that your confCons.xml file has corrupted or has lost its data,
you will need to revert to a previous version.

- Locate your confCons.xml file
- Find the most recent backup file that appears to have data (>1KB in size).
- Rename or delete the corrupted `confCons.xml` file.
- Rename the chosen backup file to remove the date stamp and .backup suffix. Unless you set a custom path, your backup file should now be named `confCons.xml`.
