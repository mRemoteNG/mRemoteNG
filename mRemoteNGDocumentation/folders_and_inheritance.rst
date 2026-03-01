***********************
Folders and Inheritance
***********************

Folders on mRemoteNG cannot only be used to categorize connections but also to inherit
properties to underlying connections.

Example
=======
You have ten Remote Desktop enabled servers in one domain and 15 in another domain.
Normally you would spend a lot of time creating all those connections and setting
the individual properties like username, password, etc.

In mRemoteNG there is an easier way. You just create two folders
(one for domain A and one for domain B) and set all properties there.
Then create the connections itself and let it inherit every property.
The only properties left to set on the connection itself are the connection name and hostname.
Everything else will be inherited from the parent folder.

**Here is how you do this:**
Add the folder. This can be done like this:

- Right click on connections and click on **New Folder**
- :menuselection:`File --> New Folder`
- Or with keybinding: :kbd:`Ctrl+Shift+N`

.. figure:: /images/folders_and_inheritance_01.png

Then give it a name and fill all the properties you need (like you did with the test connection).

.. figure:: /images/folders_and_inheritance_02.png

When you have filled in the settings and values you can either
just drag the test Connection inside the folder or create a new one.

.. figure:: /images/folders_and_inheritance_03.png

Right now nothing has changed and nothing will be inherited.
To enable inheritance switch to the inheritance view by clicking the dedicated button.
(Marked with a red arrow below)

.. figure:: /images/folders_and_inheritance_04.png

The properties that show up now are almost the same as before,
but you only select yes or no to enable or disable a inheritance.

.. figure:: /images/folders_and_inheritance_05.png

When no is selected the property will not be inherited, yes indicates an inherited property.
For this test set Inherit Everything to Yes.
Now if you switch back to the properties view (the button left of the inheritance button)
you should see that not much is left of all those properties.

.. figure:: /images/folders_and_inheritance_06.png

Only the Name and Hostname/IP properties are left over,
everything else will be inherited from the parent folder.
Of course you can also only let some of the properties be inherited.
Just play around with this a bit and you'll get the hang of it.

Default Connection Properties & Inheritance
===========================================

In addition to inheriting properties from folders, mRemoteNG allows you to define a **Default Connection** template.
This template determines the initial property values and inheritance settings for every **new** connection or folder you create.

This is particularly useful if you want all your new connections to start with:
- A specific Protocol (e.g., SSH instead of RDP)
- Specific Inheritance settings (e.g., always inherit Username and Password from the parent folder)

Accessing Default Properties
----------------------------
You can access and modify the Default Connection settings using the buttons in the Connection Panel toolbar:

1.  **Default Connection Properties** (Red icon): Sets the default values for properties (like Protocol, Port, etc.).
2.  **Default Inheritance** (Green icon): Sets the default inheritance state (Yes/No) for each property.

.. figure:: /images/default_properties.png
   :alt: Default Connection Properties Buttons

How it works
------------
When you create a new connection:
1.  mRemoteNG copies the values from the **Default Connection Properties** to the new connection.
2.  mRemoteNG copies the inheritance settings from the **Default Inheritance** to the new connection.

.. note::
   These settings are only applied at the moment of creation. Changing the Default Connection properties later **will not** update existing connections. To update existing connections, you should use Folder Inheritance or bulk edit them.

Difference from Folder Inheritance
----------------------------------
- **Default Connection**: Acts as a "seed" or template for *new* items. Changes do not propagate to existing items.
- **Folder Inheritance**: Acts as a dynamic hierarchy. If a connection is set to inherit a property, changing that property on the parent folder *will* immediately affect the connection.

Color Property
==============
You can set a color for each connection or folder in the connections list.
This makes things clearer when you have many connections.

To set a color:

1. Select a connection or folder in the connections tree
2. In the properties panel, find the **Color** property under the Display category
3. Click on the color value and select a color from the color picker

When you set a color on a folder, all connections under that folder can inherit the same color
if their Color inheritance is enabled. This provides a visual way to group and identify
related connections in the tree view.

.. note::
   The Color property can be inherited just like other properties. Enable inheritance
   in the inheritance view to have connections automatically use their parent folder's color.
