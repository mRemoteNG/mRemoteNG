************
RDP Protocol
************

Microsoft's Remote Desktop Protocol is a highly configurable remoting protocol.


RDP Protocol Versions
=====================

There are several versions of the RDP protocol supported in mRemoteNG. Each newer protocol adds support for additional connection properties. By default, RDP connections will attempt to use the highest RDP protocol version supported by both mRemoteNG and your machine. You can manually select a protocol version using the ``RDP Protocol Version`` connection property.

.. note::
	The ability to select the RDP protocol version was added in mRemoteNG v1.77.1. The RDP 8 protocol was used from mRemoteNG v1.74.0 to v1.77.0. RDP 5 was used in v1.73 and earlier.

RDP 6
-----

.. list-table::
	:widths: 30 70
	:header-rows: 1

	* - Property
	  - Description
	* - Use Console Session
	  - Connect to the console session of the remote host
	* - Server Authentication
	  - When connecting, RDP clients attempt to authenticate the remote server to ensure that it is who you expect. This option allows you to select how to handle authentication failures. The possible values are ``Always connect, even if authentication fails``, ``Don't connect if authentication fails``, ``Warn me if authentication fails``.
	* - Minute to Idle
	  - Allows you to specify the number of minutes an RDP connection can sit idle before automatically being disconnected. A value of ``0`` means the connection will never disconnect due to inactivity.
	* - Load Balance Info
	  - Load balancing information can be provided to load balancing routers to route RDP connection requests.
	* - Use CredSSP
	  - Allows you to user Credential Security Support Provider (CredSSP) for authentication if it is available.
	* - Use Gateway
	  - Specify whether you would like to use an RD Gateway server. Possible values are ``Never``, ``Always``, ``Detect``
	* - Gateway Hostname
	  - The hostname of the RD Gateway server.
	* - Gateway Credentials
	  - Allows you to select the authentication to use when connecting to the RD Gateway.

	  	- ``Use the same username and password`` uses the same authentication information that was specified in the ``Username``, ``Domain``, and ``Password`` fields of the same connection. 
	  	- ``Use a different username and password`` allows you to specify alternate credentials using the ``Gateway Username``, ``Gateway Password``, and ``Gateway Domain`` fields. 
	  	- ``Use a smart card`` uses your current smart card for authenticaton to the RD Gateway.
	* - Gateway Username
	  - The username to use when authenticating to the RD Gateway.
	* - Gateway Password
	  - The password to use when authenticating to the RD Gateway.
	* - Gateway Domain
	  - The domain to use when authenticating to the RD Gateway.
	* - Resolution
	  - Allows you to select the resolution to use when connecting. 

  		- ``Fit to panel`` will dynamically select a resolution based on the size of the connection window. When the connection window is resized, scroll bars or grey space will be shown. The ``Automatic Resize`` connection property can be used to automatically resize the connection when the window size changes.
  		- ``Fullscreen`` will place the RDP connection into fullscreen mode immediately after connecting. When the connection window is resized, scroll bars or grey space will be shown. The ``Automatic Resize`` connection property can be used to automatically resize the connection when the window size changes.
  		- ``Smart size`` will select an appropriate resolution when initially connecting. When the connection window is resized, the connection image will be stretched or squished to prevent scroll bars or grey space from being shown.
  		- Specific resolutions are also provided which will guarantee the resolution of the connection. Scroll bars or grey space will be shown if the connection size does not match the connection window size.
	* - Colors
	  - Allows you the specify the color quality to use for the connection. Higher values look better but increases network utilization.
	* - Cache Bitmaps
	  - Uses additional system memory to reduce network bandwidth usage.
	* - Display Wallpaper
	  - Specifies whether the wallpaper of the remote machine should be shown.
	* - Display Themes
	  - Specifies whether the theme of the remote machine should be shown.
	* - Font Smoothing
	  - Enabling this option turns on ClearType, making text clearer and easier to read. Enabling this option increases bandwidth usage of the connection.
	* - Desktop Composition
	  - Enables visual effects on the remote desktop and features like glass window frames, 3D window transition animations, and Windows Flip. Uses more network bandwidth.
	* - Redirect Key Combinations
	  - Select whether key combinations (e.g. Alt-Tab) should be redirected to the remote host.
	* - Redirect Disk Drives
	  - Select whether local disk drives should be shown on the remote host.
	* - Redirect Printers
	  - Select whether local printers should be shown on the remote host.
	* - Redirect Clipboard
	  - Select whether the clipboard should be shared.
	* - Redirect Ports
	  - Select whether local ports (ie. com, parallel) should be shown on the remote host.
	* - Redirect Smart Cards
	  - Select whether local smart cards should be available on the remote host.
	* - Redirect Sounds
	  - Determine how remote sound should be redirected. Possible values are ``Bring to this computer``, ``Leave at remote computer``, ``Do not play``
	

RDP 7
-----

In RDP 7, support was added for specifying the network connection type. This property is used to determine whether certain performance options will be honored by the remote server. Some performance settings (such as Display Wallpaper or Display Themes) will not work when using protocols older than RDP 7 to connect to remote hosts that are Windows 8 or higher.

.. list-table::
	:widths: 30 70
	:header-rows: 1

	* - Property
	  - Description
	* - Sound Quality 
	  - Specify the quality of sound redirection. Only valid when ``Redirect Sounds`` is enabled. Possible options are ``Dynamic``, ``Medium``, ``High``
	* - Redirect Audio Capture
	  - Enabled you to redirect the default audio input device on the remote machine to your local computer.


RDP 8
-----

In RDP 8, support was improved for reconnecting RDP connections for resizing operations.

.. list-table::
	:widths: 30 70
	:header-rows: 1

	* - Property
	  - Description
	* - Automatic Resize
	  - When this property is enabled and the connection window is resized, the RDP connection will automatically reconnect with the new window resolution. This prevent scroll bars from being shown and preserves the aspect ratio of the connection (prevents image stretching). This property is only available when ``Resolution`` is set to either ``Fullscreen`` or ``Fit to Panel``.


RDP 9
-----

In RDP 9, support was further improved for reconnecting RDP connections for resizing operations.

.. list-table::
	:widths: 30 70
	:header-rows: 1

	* - Property
	  - Description
	* - Automatic Resize
	  - When this property is enabled and ``Resolution`` is set to ``Fit to Panel`` and the connection window is resized, the RDP connection will automatically change the resolution without reconnecting.


RDP 10
------

We support this protocol version, but are not yet using any of its features.
