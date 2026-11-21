
.. _registry_settings_information:

*****************************
Registry Settings Information
*****************************

.. warning::
    Before proceeding with any changes to the Windows Registry, it is imperative that you carefully read and comprehend the 
    :ref:`Modifying the Registry <warning_modifying_registry>`, :ref:`Restricted Registry Settings <warning_restricted_registry_setting>`
    and :ref:`Disclaimer <disclaimer>`.
    

.. _warning_modifying_registry:

Modifying the Registry
======================

Making changes to the Windows Registry is a sensitive and advanced operation. Incorrect modifications can lead to system instability, 
data loss, or even render your operating system unusable. Proceed with caution and follow these guidelines:

1. **Backup Registry:** Before making any changes, create a backup of the Registry. This ensures that you can restore it to a working state if issues arise.

2. **Know What You're Doing:** Only modify the Registry if you understand the implications of your changes. Incorrect values or deletions can affect system functionality and applications.

3. **Documentation:** Document the changes you make. Include details such as the date, purpose, and the specific keys/values altered. This documentation can be crucial for troubleshooting later.

4. **Create System Restore Point:** Create a System Restore Point before proceeding. This provides an additional layer of recovery in case problems occur.

5. **Registry Editor:** Use the built-in Registry Editor (`regedit`) to make changes. Avoid third-party tools unless you are confident in their reliability.

6. **One Change at a Time:** Make one change at a time and test its impact before proceeding to the next. This helps identify the cause if issues arise.

7. **Verify Information:** Double-check the information you find online before applying it to your Registry. Incorrect instructions can lead to problems.

8. **Permissions:** Be cautious with changing permissions. Modifying permissions incorrectly can result in restricted access to important system components.

9. **Seek Professional Help:** If you are uncertain or uncomfortable with Registry modifications, seek assistance from knowledgeable professionals.

10. **Emergency Plan:** Have a plan in case something goes wrong. Know how to restore your Registry from the backup or the System Restore Point.

By acknowledging these warnings and following best practices, you can minimize the risks associated with making changes to the Windows Registry.


.. _warning_restricted_registry_setting:

Restricted Registry Settings
============================

This specific registry setting is intentionally restricted, and users do not have default permissions to modify it. 
Even if you possess administrative rights on your system, any changes to this registry entry are prohibited by default.

Attempting to alter this registry entry without proper authorization or without following specific instructions from your system 
administrator may result in adverse effects on system stability, security, or functionality. 
Any unauthorized changes can lead to unintended consequences, system errors, or even render your operating system inoperable.

If you require adjustments to this registry setting, please consult with your system administrator or IT support. 
They can provide guidance, assess the implications of any changes, and implement the necessary modifications following established procedures.

Exercise caution, adhere to your organization's policies, and avoid making unauthorized alterations to the Windows Registry, 
as it can impact the overall integrity and performance of your system.


.. _How_to_open_the_Windows_Registry:

How to open the Windows Registry
================================

The Windows Registry is a crucial component for system configuration. Here's a guide on how to open the Registry on Windows:

**Using Run Dialog:**
   - Press `Win + R` to open the Run dialog.
   - Type `regedit` and press Enter.
   - The Registry Editor will open.

**Create Desktop Shortcut:**
   - Right-click on the desktop.
   - Choose "New" -> "Shortcut."
   - Enter `regedit` as the location and click "Next."
   - Provide a name for the shortcut and click "Finish."


.. _registry_encrypted_passwords:

Encrypted Passwords
===================

The password values, ``SQLPassword``, ``DefaultPassword`` and ``ProxyAuthPass``, take a password encrypted for the
computer they are set on, with Windows DPAPI for the machine: any user of that computer can decrypt it, but a copy of
the value does not open on another computer, so a registry export taken elsewhere does not give the password away.
For the same reason a value made on one computer does not work on another, and a policy that sets one value on every
computer does not work: make the value on each computer, with a script run there.

On the computer itself, the Secure Key Generator in Options > Security makes the value and copies it to the clipboard.
With Windows PowerShell 5.1 a script makes it this way, and creates the key on a computer that does not have it yet:

.. code:: powershell

    Add-Type -AssemblyName System.Security
    $password = Read-Host -AsSecureString 'Password'
    $bytes = [Text.Encoding]::UTF8.GetBytes([Net.NetworkCredential]::new('', $password).Password)
    $entropy = [Text.Encoding]::UTF8.GetBytes('mRemoteNG registry password')
    $protected = [Security.Cryptography.ProtectedData]::Protect($bytes, $entropy, 'LocalMachine')
    $value = 'dpapi-machine:' + [Convert]::ToBase64String($protected)
    $key = 'HKLM:\SOFTWARE\mRemoteNG\SQLServer\Options'
    if (-not (Test-Path $key)) { New-Item -Path $key -Force | Out-Null }
    Set-ItemProperty -Path $key -Name 'SQLPassword' -Value $value

A value made by an earlier version, with the generator encrypting it under the default key, still works, with a
warning in the Notifications panel: the default key is public, so anyone with a copy of the value can decrypt it, on
any computer. Make it again to protect it.


.. _disclaimer:

Disclaimer
==========
Any changes made by yourself to the system settings, including but not limited to the Windows Registry, are undertaken at your own risk. 
mRemoteNG shall not be held liable for any consequences arising from user-initiated modifications.

Please be advised of the following:

1. **User Responsibility:**
   - You, as the user, are solely responsible for the changes made to system configurations.

2. **No Liability Assumed:**
   - mRemoteNG do not assume any liability for potential issues, damages, or data loss resulting from user-initiated modifications.

3. **Recommended Best Practices:**
   - Users are encouraged to adhere to recommended best practices, such as creating backups and seeking professional assistance when necessary.

4. **System Stability:**
   - Modifications to system settings, including the Windows Registry, can impact system stability. It is essential to exercise caution and fully understand the implications of changes.

5. **Professional Assistance:**
   - If you are uncertain about specific configurations or require assistance, consider consulting with professional support services.

By proceeding with user-initiated changes, you acknowledge and accept the responsibility for any potential consequences that may arise. This disclaimer applies to all adjustments made independently by users.

Thank you for your understanding and cooperation.
