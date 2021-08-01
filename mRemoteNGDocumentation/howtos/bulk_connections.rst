.. _bulk_connections:

*************************
Creating Bulk Connections
*************************

Currently, mRemoteNG does not have a feature to support editing or creating connections in bulk.
Since this is a common issue, it would be useful to have a work around while a more user-friendly feature is developed.
The best way to create bulk connections is to generate XML with a PowerShell script.

Since you likely don't want to spend your own time reading through XML files, we have provided an official script for doing this.
You can find the most recent version `here <https://github.com/mRemoteNG/mRemoteNG/blob/develop/Tools/CreateBulkConnections_ConfCons2_6.ps1>`_.

A few notes about using this script:

- While much of the setup has been done for you, you will still need to know some PowerShell in order to use this effectively. Some examples have been provided, but you will need to modify the bottom portion of the script to suite your needs.
- The script works with mRemoteNG v1.75 and will produce XML formatted for use with confCons v2.6 files. This script may need to be updated for future versions of mRemoteNG.
- As always, feel free to reach out to us in a `GitHub Issue <https://github.com/mRemoteNG/mRemoteNG/issues>`_ or on `Gitter <https://gitter.im/mRemoteNG/PublicChat>`_ if you have issues.
