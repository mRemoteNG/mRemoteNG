*********
Uninstall
*********

Standard Uninstall
==================
mRemoteNG basic binary package can be uninstalled with Windows Control Panel. If for some reason it does not work please
follow information provided below for Manual Uninstall.

Manual Uninstall
================
If for some reason you cannot uninstall mRemoteNG from the Windows Control Panel,
you can manually uninstall the program using the following steps:

.. note::

	If you are using the Portable version, simply deleting the folder that contains mRemoteNG should be sufficient. These uninstall instructions are only necessary for the normal binary .MSI installed version of mRemoteNG

#. Delete the folder where mRemoteNG was installed. By default, this is:
	``C:\Program Files(x86)\mRemoteNG``

#. Delete all mRemoteNG custom registry entries (See above table for locations)

#. Delete the mRemoteNG install entry from one of the following locations. Search for "mRemoteNG" in the DisplayName field:
		- x86: ``HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\``
		- x64: ``HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\``

#. (Optional) If you would also like to delete user data, delete the folders mentioned here:
