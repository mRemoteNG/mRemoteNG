*************
Dynamic Host Addresses
*************

.. warning::

This feature is in beta and currently supports Amazon EC2 only

Some hosts may have dynamic ip addresses or hostnames. For example, Amazon EC2 instances without elastic ips change their public address upon stop and restart events.
mRemote can automatically update the host property when ec2 region and instance-id properties are set:

.. figure:: /images/ec2instance.png

Before initiating the connection, mRemote will contact the EC2 API, fetch the current public ipv4 address and write it to the host field. Afterwards the connection is initiated as usual.

For this to work, a valid EC2 access key needs to be specified. mRemote will ask for it via a popup window if not yet specified. This token is then stored to registry. We strongly recommend to generate **readonly** access keys in the EC2 AMI interface.
