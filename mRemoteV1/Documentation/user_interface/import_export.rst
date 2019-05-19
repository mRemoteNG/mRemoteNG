*************
Import/Export
*************

You can import or export your connections to mRemoteNG. Imports can be done in various
different ways. See below for more information.

Import
======

Import from File
----------------
Opens a normal file load dialog to open a exported xml or csv file for mRemoteNG.
See Export to file further down this page for information on exporting your connections.

Import from Active Directory
----------------------------
.. TODO: Needs even more information and testing (new image with a actual import of server from AD)

This option can be used to import computers from a specific OU from you Arctive Directory.

.. figure:: /images/import_from_active_directory.png

   Import from Active Directory dialog

#. Go to: :menuselection:`File --> Import --> Import from Active Directory`
#. Choose the domain to check for computers available

.. note:: Check the **Import sub OUs** checkbox if you want to import OUs recursively.

Import from Port Scan
---------------------
This option opens a dialog to import connections from a port scan.
Both network and port range can be specified.

.. important:: Port Scan uses nmap to scan the ports. Be carefull on how you scan your network, as this can be considered a brute force attack.


.. figure:: /images/import_from_port_scan.png

   Import from Port Scan dialog

- **First IP** - Start of ip to scan from
- **Last IP** - Stop of ip to scan to
- **First Port** (Optional) - Start port to scan from
- **Last Port** (Optional) - Stop port to scan to
- **Timeout [seconds]** - Seconds to wait until continuing scan

Once the scan is done you can select connections to import with some options on the lower part of the dialog:

- **Protocol to import** - Which protocol to use for the import of the connection(s)

Export to file
==============
Here you can export your settings to a file to share or backup.
The dialog shown below is the dialog of which you chose the options to export.

.. figure:: /images/import_export_dialog.png

   Export to file dialog example

Export options:
---------------
Here is a detailed explanation of the export dialog.

- **Filename** - The output filename for which to save the export
- **File Format** - Currently supports xml and comma seperated csv output file format
- **Export Items** - Options to what you want to save
   - **Export everything** - Will export all the connections
   - **Export the currently selected folder** *[nameoffolder]* - Is used to only export all connections
     in the folder selected. Note! the *[nameoffolder]* is the name to which you have selected in the connection tree.
   - **Export the currently selected connection** *[nameofconnection]* - Same as before with folder but uses the currently
     selected connection for export.
- **Export Properties** - Properties of the specific connections to export

.. note:: Options do change based on what is selected in the connection tree. You can try this out by right clicking on a folder and selecting **Export to file** on a connection to understand more
