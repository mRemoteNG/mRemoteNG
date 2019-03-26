############
Known Issues
############

CredSSP - CVE-2018-0886 - Authentication error
==============================================

mRemoteNG uses the Microsoft Terminal Services Client (MSTSC) libraries in order to make Remote Desktop connections.

.. note::

    mRemoteNG has no control over the functionality changes implemented by Microsoft. 

Please refer to `Microsoft's Documentation <https://support.microsoft.com/en-us/help/4093492/credssp-updates-for-cve-2018-0886-march-13-2018>`_ for full details regarding this problem.
Patched clients attempting to connect to Unpatched servers will fail with the following error:

.. figure:: /images/credssp-error.png

The same error will occur with MSTSC directly on a patched client attempting to connect to an unpatched server.

Per the MS documentation, the only way around this is to do the following:

- Patch the servers
- set the "Encryption Oracle Remediation" policy to "Vulnerable" - refer to the MS documentation above for details:

 .. figure:: /images/oracle_remediation_setting.png

- Uninstall `KB4103727 <https://support.microsoft.com/en-us/help/4103727/windows-10-update-kb4103727>`_