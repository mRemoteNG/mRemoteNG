*************
Connecting to CyberArk - Privileged Access Manager
*************
CyberArk's Privileged Access Manager is a full life-cycle solution for managing the most privileged accounts and SSH Keys in the enterprise. It enables organizations to secure, provision, manage, control and monitor all activities associated with all types of privileged identities.

If yours company use it, you may easy setup mRemoteNG to connect through Privileged Session Manager to your target system.

You should provide in host field yours CyberArk Digital Vault server FQDN or IP address, and in alternate shell field (under Miscellaneous section):

:code:`psm /u target-user /a target-address /c connection-component`

.. figure:: /images/cyberark_pam_connection_setup.png

:code:`/c connection-component` should be :code:`/c PSM-RDP` in this case

.. note::

Please check CyberArk `**documentation** <https://docs.cyberark.com/Product-Doc/OnlineHelp/PAS/Latest/en/Content/PASIMP/PSSO-ConfigureRDPStart.htm?TocPath=End%20user%7CConnect%20to%20Accounts%7CPrivileged%20Single%20Sign-On%7CConnect%20through%20Privileged%20Session%20Manager%20for%20Windows%7C_____2>`_ for more detail information about parameters
