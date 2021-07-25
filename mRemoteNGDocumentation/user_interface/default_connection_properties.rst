*****************************
Default Connection Properties
*****************************

Default connection properties are a template that is applied when creating new connections. You can set both property values and inheritance settings that will be set in new connections.

.. note::
	Currently, default connection properties are saved within the user settings of mRemoteNG and not with the connection file. Sharing a connection file between multiple machines will not share default connection properties.


How it is used
==============

New Folders/Connections
-----------------------
When a new folder or connection is created, all default properties and default inheritance are applied to it.

.. note::
	Changing default properties does not affect existing connections. The default values are only applied when the connection is first created.


Quick Connections
-----------------
Default connection properties are also used when establishing quick connections. See :doc:`quick_connect` for more details.


How to set them
===============
You can set default properties and inheritance by going to the Connection Tree and clicking the appropriate button for the default settings you would like to change.

.. figure:: /images/default_properties.png

- **Red** - Default connection properties
- **Green** - Default connection inheritance
