*************
SSH Tunneling
*************

You can use any configured SSH connection to be used as a tunnel server for another connection.

.. figure:: /images/ssh_tunnel.png

If an SSH Tunnel is configured the connection is searched and if found a free local TCP port determined. The SSH tunnel connection
is setup with additional parameters for the tunnel. The original connection info is copied and the copy is modified to connect
to local host and the local TCP port and the target connection is opened.
You can use the SSH connection attribute for additional SSH options. It can be used by all
normal SSH connections as well to specify any additional options, e.g. to not start a shell which some SSH servers.
