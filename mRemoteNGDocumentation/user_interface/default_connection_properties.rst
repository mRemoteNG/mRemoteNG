*****************************
Default Connection Properties
*****************************

Default connection properties are a template that is applied when creating new connections. You can set both property values (**Default Properties**) and inheritance settings (**Default Inheritance**) that will be set in new connections and folders.

.. note::
	Currently, default connection properties are saved within the user settings of mRemoteNG and not with the connection file. Sharing a connection file between multiple machines will not share default connection properties.


How to set them
===============
The defaults are edited in the **Config** panel, the same panel that shows the properties of the selected connection.
The toolbar at the top of the Config panel has four buttons, in this order:

- **Properties** - properties of the selected connection or folder
- **Inheritance** - inheritance settings of the selected connection or folder
- **Default Properties** - default connection property values
- **Default Inheritance** - default inheritance settings

.. figure:: /images/default_properties.png

- **Red** - Default connection properties
- **Green** - Default connection inheritance

Click **Default Properties** or **Default Inheritance** and change the values in the grid.
Changes are used for the next connection or folder you create and are stored in the user settings.
Click **Properties** to go back to editing the selected connection.


Default Properties
------------------
The same properties that a connection has, for example protocol, username, domain, resolution or redirection settings.
Set a value here if you want new connections to start with it instead of the built-in value.

Default Inheritance
-------------------
The same Yes/No list as the **Inheritance** view of a connection (see :doc:`/folders_and_inheritance`).
Set a property to **Yes** here if new connections and folders should inherit that property from their parent folder.
For example, if every connection you create goes into a folder that holds the username and domain,
set **Username** and **Domain** to Yes so you don't have to turn inheritance on for each new connection.


How it is used
==============

New Folders/Connections
-----------------------
When a new folder or connection is created in the connection tree (toolbar or right-click menu), all default properties and default inheritance are applied to it.
This also applies to new root groups.

.. note::
	Changing default properties does not affect existing connections. The default values are only applied when the connection is first created.

Duplicating a connection or folder copies the original item, so the defaults are not applied to the copy.

Interaction with folder inheritance
-----------------------------------
Default inheritance only decides which inheritance settings a new item starts with.
The inherited values themselves always come from the parent folder, not from the default properties.
Each inheritance setting can still be changed on the item afterwards in the **Inheritance** view.

.. note::
	Inheritance is ignored for connections and folders placed directly under the top-level **Connections** node, because there is no parent folder to inherit from.
	Default inheritance has a visible effect only once the item is inside a folder.

Applying default inheritance to an existing item
------------------------------------------------
To reset an existing connection or folder to the default inheritance settings, right click it in the connection tree and select
:menuselection:`Inheritance --> Apply default inheritance`.
This replaces all inheritance settings of that item with the current default inheritance. Locally stored property values are not overwritten,
but the effective values used by the connection can change when inheritance is enabled or disabled.

Quick Connections
-----------------
Default connection properties are also used when establishing quick connections. See :doc:`quick_connect` for more details.

Importing
---------
Connections and folders created by an import start with the default inheritance and with the default values for many
(but not all) properties, such as protocol and the RDP display and redirection settings. Username, domain, password and hostname are not taken from the defaults.
Any value that the imported file provides replaces the default.
Some import formats set their own inheritance, for example importing from an mRemoteNG connection file keeps the inheritance stored in that file,
and the Active Directory import turns on inheritance for every property except Description on imported connections.
Imported Active Directory folders retain the default inheritance settings.
