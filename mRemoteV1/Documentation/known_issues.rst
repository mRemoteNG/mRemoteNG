############
Known Issues
############

CredSSP - CVE-2018-0886 - Authentication error
==============================================

mRemoteNG uses the Microsoft Terminal Services Client (MSTSC) libraries
in order to make Remote Desktop connections.

.. note::

    mRemoteNG has no control over the functionality changes implemented by Microsoft.

Please refer to `Microsoft's Documentation <https://support.microsoft.com/en-us/help/4093492/credssp-updates-for-cve-2018-0886-march-13-2018>`_ for full details regarding this problem.
Patched clients attempting to connect to Unpatched servers will fail with the following error:

.. figure:: /images/credssp-error.png

The same error will occur with MSTSC directly on a patched
client attempting to connect to an unpatched server.

Per the MS documentation, the only way around this is to do the following:

- Patch the servers
- set the "Encryption Oracle Remediation" policy to "Vulnerable" - refer to the MS documentation above for details:

 .. figure:: /images/oracle_remediation_setting.png

- Uninstall `KB4103727 <https://support.microsoft.com/en-us/help/4103727/windows-10-update-kb4103727>`_

I can't open more than X number of RDP sessions. New sessions fail with error code 3334
=======================================================================================
The issue here is likely the amount of resources available to the RDP component to open the connection. This was alleviated in `MR-714 <https://mremoteng.atlassian.net/browse/MR-714>`_ and `MR-864 <https://mremoteng.atlassian.net/browse/MR-864>`_

Other things you can do to help reduce the issue:

- On your RDP connections, set CacheBitmaps to False (this reduces the memory usage of each connection)
- Consider removing KB2830477 if you have it installed. This seems to increase the likelyhood of getting 3334 error codes.

RDP connections fail with error code 264
========================================
This issue is often caused by trying to retrieve session information.

Try doing the following:

- Disable "Automatically get session information" (Tools -> Options -> Advanced)

ATI Tray Tools
==============
mRemoteNG is not compatible with ATI Tray Tools. We are aware of the issue and
hope to have it fixed in a future version. We recommend that you disable or
uninstall ATI Tray Tools while using mRemoteNG.

mRemoteNG crashes with the error "Class not registered" when trying to connect using RDP
========================================================================================
You may also see a message like "System.Runtime.InteropServices.COMException (0x80040154)"

If you are running mRemoteNG on Windows 7 or Server 2008:

- You may be missing one or more required windows updates (See: :ref:`requirements`.).
- A common issue is that `KB2574819 <https://support.microsoft.com/en-us/kb/2574819>`_ is either missing or has been installed after `KB2592687 <https://support.microsoft.com/en-us/kb/2592687>`_. They must be installed in the correct order. If you do not have KB2574819, follow these instructions:
  - Uninstall `KB2592687 <https://support.microsoft.com/en-us/kb/2592687>`_
  - Install `KB2574819 <https://support.microsoft.com/en-us/kb/2574819>`_
  - (Re)Install `KB2592687 <https://support.microsoft.com/en-us/kb/2592687>`_
  - Reboot your machine

If you are running mRemoteNG on Windows 8/10 or Server 2012+:

- Try to repair the mRemoteNG installation using the installer or uninstall/reinstall. Receiving this error on these OS's is just an install fluke (or you've fiddled with your registry).

VNC connections fail with the error "The server is using an unsupported version of the RFB protocol. The server is using version 4.1 but only version 3.x is supported."
========================================================================================================================================================================
RFB version 4.0 and higher is a proprietary version owned by `RealVNC Limited <https://www.realvnc.com/>`_. Building support for newer versions will likely result in licensing fees. Therefore, it is unlikely that mRemoteNG will have support for version 4.0+ anytime soon.

Unfortunately, the only way around this limitation is to use an open source
implementation of VNC server such as `TightVNC <http://tightvnc.com/>`_
or `UltraVNC <http://www.uvnc.com/>`_

Cannot click some UI elements in an RDP connection window.
==========================================================
It may seem like some elements are not clickable along the top
and left sides of your RDP connection window. More information can be found in issue #210

This is likely due to non-standard (>100%) DPI scaling on your local machine.

To turn this off:

On Windows 7 / 8

- Start menu -> Control Panel -> Display
- Ensure the option **Smaller - 100% (default)** is selected

On Windows 10

- Start menu -> Settings -> Display
- Ensure the slider under **Change the size of text, apps, and other items** is all the way to the left (at 100%)

SSH login fails when password contains extended ASCII characters
================================================================
Initial login to SSH (or WinSCP) fails when the password contains
extended ASCII characters (such as: €šœ£ÁØë).
Typing the password into the SSH session directly works.

Investigation suggests that there is an issue in character encoding
when mRemoteNG passes the value to the cmd line, which then invokes PuTTY.
This was investigated in issue `#186 <https://github.com/mRemoteNG/mRemoteNG/issues/186>`_

The only resolution for this issue is to not use extended ASCII characters
in passwords that will be sent to PuTTY or similar tools.

RDP tries to reconnect whenever I resize the window
===================================================
Your RDP connection reconnects after resizing mRemoteNG or the connection panel.

This will occur anytime the connection window changes size and
the following connection options are set:

- Resolution: **Fit to Panel**
- Automatic Resize: **Yes**

To prevent reconnecting, you can do one of several things:

- Change the resolution to Smart Size. This will scale the original connection area when the view window size changes. This does not preserve aspect ratio.
- Turn off Automatic Resize. When the view window size changes, you will see scroll bars or dead space.

There is no way to update the view window size without a reconnect.
This is an RDP protocol limitation.

AltGr key combinations stop working in other apps when connected to RDP
=======================================================================
When connected to an RDP session AltGr, keyboard combinations sometimes stop working.

This is a known issue with The Microsoft RDP library that cannot be solved by mRemoteNG.
There are three known work arounds for this issue:

- Disconnect the RDP session which caused the issue. Since it can be difficult to determine which connection is to blame, you may need to disconnect all RDP sessions. Once you have confirmed AltGr combinations are working again, you may reconnect your RDP session(s).
- When the issue occurs, hold/press the Ctrl key. This is known to release the AltGr key from the RDP session.
- Use :kbd:`Ctrl` + :kbd:`Alt` instead of :kbd:`AltGr`.
