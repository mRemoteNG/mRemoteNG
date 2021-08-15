*************************************
Connect to virtual machine on Hyper-V
*************************************

Introduction
============
When set up properly, you can use mRemoteNG to connect to virtual machines running on Hyper-V.
This how to provides you with all the information you need to get things running.

To be able to connect to the virtual machine we need its' id.
You can find it by executing the following powershell command on the Hyper-V server:

.. code-block:: 

   Get-VM | select Name, ID

Create a new connection, set the protocol to RDP and set the "Use VM ID" property to true.
Enter the id in the new property field that just appeared in the connection section and set the port to 2179.

Enter the id of the virtual machine you found out earlier and you are able to connect to the virtual machine.

Prerequisites
=============
For the scenario above to work there is some configuration that may be necessary for you to set up, depending on your environment.

You must be a member of the *Administrators* **and** *Hyper-V Administrators* groups on the Hyper-V Server to be able to remotely connect to any virtual machine running on the host via VMRDP.
If this is not the case your user has to be granted access to remotely access the machine.
The following Powershell command achieves this:

.. code-block:: 

   Grant-VMConnectAccess -VMName <VMNAME> -UserName <DOMAIN>\\<USERNAME>

Port 2179 must be open on Hyper-V server and on the machine you are connecting from. Use the following command to open the ports on the firewall if needed:

.. code-block:: 

   netsh advfirewall firewall add rule name="VMRDP" dir=in action=allow protocol=TCP localport=2179 (incoming)
	netsh advfirewall firewall add rule name="VMRDP" dir=out action=allow protocol=TCP localport=2179 (outgoing)

In case you are facing "Unknown disconnection reason 3848" error when connecting, you need to configure a number of registry settings on your client and the Hyper-V Server to make the connection work.
This problem occurs because of the CredSSP (Credential Security Service Provider) policy on the client and/or Hyper-V Server not allowing to authentication of remote users by default.

.. note::

   See Microsoft support file `954357 <https://support.microsoft.com/en-us/help/954357/when-i-use-the-virtual-machine-connection-tool-to-connect-to-a-virtual>`_ for more information on this topic.
    
.. note::

   For more information on RDP error codes see `this Microsoft article <https://social.technet.microsoft.com/wiki/contents/articles/37870.rds-remote-desktop-client-disconnect-codes-and-reasons.aspx>`_.

Start the PowerShell console with administrative privileges and run the following commands:

.. code-block:: 

   New-ItemProperty -Path HKLM\:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowDefaultCredentialsDomain -Name Hyper-V -PropertyType String -Value "*" -Force
   New-ItemProperty -Path HKLM\:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowSavedCredentialsDomain -Name Hyper-V -PropertyType String -Value "*" -Force
   New-ItemProperty -Path HKLM\:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowDefaultCredentials -Name Hyper-V -PropertyType String -Value "*" -Force
   New-ItemProperty -Path HKLM\:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowFreshCredentialsDomain -Name Hyper-V -PropertyType String -Value "*" -Force
   New-ItemProperty -Path HKLM\:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowFreshCredentials -Name Hyper-V -PropertyType String -Value "*" -Force
   New-ItemProperty -Path HKLM\:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowFreshCredentialsWhenNTLMOnly -Name Hyper-V -PropertyType String -Value "*" -Force
   New-ItemProperty -Path HKLM\:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowFreshCredentialsWhenNTLMOnlyDomain -Name Hyper-V -PropertyType String -Value "*" -Force
   New-ItemProperty -Path HKLM\:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowSavedCredentials -Name Hyper-V -PropertyType String -Value "*" -Force
   New-ItemProperty -Path HKLM\:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowSavedCredentialsWhenNTLMOnly -Name Hyper-V -PropertyType String -Value "*" -Force
