*************
Credential Vault Connector
*************

.. warning::

This feature is currently only developed for Thycotic Secret Server (on-premise installations). It is implemented for RDP and SSH connections.

mRemote supports fetching credentials from external credential vaults. This allows providing credentials to the connection without storing these to disk, which has numerous benefits (security, auditing, rotating passwords, etc).

Instead of specifying username/password/domain directly in mRemote, leave these fields empty and just set the secret id: 

.. figure:: /images/credvault01.png

The secret id is the unique identifier of your secret, you can find it in the URL in your thycotic interface.
e.g. https://cred.domain.local/SecretServer/app/#/secret/3318/general  -> the secret id is 3318

Before initiating the connection mRemote will access your Secret Server API URL and fetch the data. For this to work the API endpoint URL and access credentials need to be specified. A popup will show up if this information has not yet been set.

.. figure:: /images/credvault02.png

