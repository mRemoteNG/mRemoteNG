*******
Install
*******

Downloads are provided in three different packages, binary package, portable package and
source package. They are described further down this page.


Binary package
==============
The binary package of mRemoteNG is a compiled version of mRemoteNG which comes in an MSI installer.
This is the most common way to install mRemoteNG and get up and running. For more custom and advanced
installs of mRemoteNG then continue reading further down this page.

On the `mRemoteNG main website download page <https://mremoteng.org/download>`_ choose
(**MSI**) from the downloads to get the binary package.


Portable package
================
The portable package consists of the same files as the binary package but contains a modified version
of the executable which stores and loads all your settings from files in the application's directory.
This package can be used to run mRemoteNG from a USB stick and preserve your configuration wherever
you go.

On the `mRemoteNG main website download page <https://mremoteng.org/download>`_ choose
(**ZIP**) from the downloads to get the portable package.


Source package
==============
This package is a package provided to be launched within Visual Studio. The only way to launch
mRemoteNG using this package is by compiling it from inside Visual Studio.


Command line install
====================
There is also the possibility to install mRemoteNG through command line with the binary package.

For example:

:code:`msiexec /i C:\Path\To\mRemoteNG-Installer.exe PROPERTY1=value PROPERTY2=value`

To explain the properties that can be set during install we will go into details of them below.

Extend Installer Properties
---------------------------
The following extended properties can be provided to the installer when running it from the
command line.

+---------------------+-----------------------+------------------------------------------------------+
| Property            | Accepted Values       | Description                                          |
+=====================+=======================+======================================================+
| INSTALLDIR          | Any valid folder path | | This allows you to set the installation directory  |
|                     |                       | | from the command line. For paths that contain      |
|                     |                       | | spaces, enclose the path in double quotes ("").    |
|                     |                       | | This overrides any value found in the registry.    |
|                     |                       | | Introduced in v1.75 beta 2.                        |
+---------------------+-----------------------+------------------------------------------------------+
| IGNOREPREREQUISITES | ``0`` or ``1``        | | When the ``IGNOREPREREQUISITES`` property is set   |
|                     |                       | | to ``1``, the installer will not be halted if any  |
|                     |                       | | prerequisite check is not met. You must still run  |
|                     |                       | | the installer as Admin - this flag will not remove |
|                     |                       | | that requirement. Introduced in v1.74.             |
+---------------------+-----------------------+------------------------------------------------------+


Examples
--------
**Install to a custom folder**

   :code:`msiexec /i C:\Path\To\mRemoteNG-Installer.msi INSTALLDIR="D:\Work Apps\mRemoteNG"`

**Ignore prerequisites during a normal install**

   :code:`msiexec /i C:\Path\To\mRemoteNG-Installer.msi IGNOREPREREQUISITES=1`

**Ignore prerequisites during a silent install**

   :code:`msiexec /i C:\Path\To\mRemoteNG-Installer.msi /qn IGNOREPREREQUISITES=1`


Troubleshooting installation
============================
If you find the installer is not working as expected, there are several things you can do
to troubleshoot.

- Turn on verbose logging by using the ``/lv* <log path>`` argument at the command line.

   :code:`msiexec /i C:\Path\To\mRemoteNG-Installer.msi /l*v C:\mremoteng_install.log`
