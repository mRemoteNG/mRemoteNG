.. _ssh_file_transfer:

*****************
SSH File Transfer
*****************

SSH File Transfer functionality allows you to securely transfer files to a
remote host over an encrypted tunnel using either **SFTP** or **SCP**.

Prerequisites
=============
- SSH File Transfer requires an SSH service to listen on an available network port (default 22) on a remote host.
- A username and password must be supplied to connect with the remote host.
- The remote host must have a writeable folder on its filesystem to place the transferred files.

Configuration Options
=====================
- **Host** - The remote host you connect to. Can be DNS name or IP address.
- **Port** - Remote network port listening for SSH/SFTP/SCP traffic.
- **User** - Username for account to log on to remote host.
- **Password** - Password for account to log on to remote host.
- **Protocol** - Choice of SCP or SFTP protocol used for communication.
- **Local File** - Path of file to transfer from local host.
- **Remote File** - Path where file will be transferred on remote host. (e.g. "/home/John/Documents")

Using SSH File Transfer
=======================
Begin by going to :menuselection:`Tools --> SSH File Transfer`.

The tool will open a new panel inside mRemoteNG which allows you to configure
some options for the SSH File Transfer. Each option is needed in order to
complete a file transfer over SSH.

.. figure:: /images/ssh_file_transfer_01.png

   Main SSH File Transfer panel

- To populate the **Local File** option, select the **Browse** button and navigate to
  the desired file on the local filesystem.

- To populate the **Remote File** option, manually type desired filesystem path,
  including the desired file name.

Once all options are populated, select **Transfer** and the progress bar at
the bottom of the window will show the progress of the transfer.

Troubleshooting SSH File Transfer
=================================
To troubleshoot issues with SSH File Transfer, consult the log under
:code:`%AppData%\mRemoteNG\mRemoteNG.log`.
This log provides verbose information about successful and failed connections.

Common Issues
-------------
ERROR - Please fill all fields
 This issue was likely encountered because you did not provide all
 information needed to establish the connection.

ERROR- SSH background transfer failed!
 This issue was likely encountered due to a permissions issue.
 Ensure you have appropriate access to write to the specified Remote File.

System.Net.Sockets.SocketException (0x80004005): No connection could be made because the target machine actively refused it
 This issue was likely encountered because the local host could not contact the remote
 host specified on the remote port specified.
 The issue may be caused by improperly configured firewall rules or a
 SSH service not listening properly on the remote host.
