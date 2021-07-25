*************
Notifications
*************

The notifications panel contains any errors or informational messages that mRemoteNG triggers.
Some example errors can be if there is a problem to connect, information on lost connection and so much more.
Notification settings can be found in (Tools > Options > Notifications)
below we will explain what can be set and how they do affect for various troubleshooting.

Notifications general settings
==============================

.. tip::

    If you dont want the panel to show at all. Then unmark all options inSwitch to Notification panel on. Then the panel will not come up automatically.

.. figure:: /images/notifications_panel.png

This will tell mRemoteNG what type of messages and the level of messages to send to the panel. It does not the level for the log that mRemoteNG has but only for panel output.

There is also 2 different options mentioned below:

- Show these message types - Level of messages to show in panel. (default: Warnings and Errors)
- Switch to Notifications panel on - If interface should switch to the panel when a level of message occurs (default: all enabled)

Logging settings
================
Here you define the logging of messages.
That is a continues log which can be used to backtrack any error that has occurred.
Good when for example reporting issues about mRemoteNG or to check more details about problems.

Log path - Choose where the log should recide (default: Log to application directory)
Log these message types - Level of logging to logfile (default: Informations, Warnings, Errors)

Popups settings
===============

.. figure:: /images/notifications_popup.png

When items are selected here you will recieve a popup on the error that occurrs
based on level chosen in settings here.
This can be useful if you do not want to use the notification area
and only get a popup if error occurs. (**default**: all off)
