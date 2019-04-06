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
