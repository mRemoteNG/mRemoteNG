.. Example - External Tool - Traceroute

******************
(Basic) Traceroute
******************

This example will create a traceroute which you can call on for a connection to get the traceroute to the
connection.

Step-by-Step
============
Start with opening up external tools from: :menuselection:`Tools --> External Tools`

And create a *New* entry. See :doc:`/user_interface/external_tools/introduction`

Change *Display Name* to **Traceroute** and *Filename* to **cmd**.

See image below:

.. figure:: /images/example_et_traceroute_01.png

   Figure 1.0: Showing traceroute init settings

Now comes the interesting part where we fill in arguments that tells the console what to launch.
Here are the parts we need:

- Keep the console open - /K
- Program to run - tracert
- Variable to use - %HOSTNAME%

So lets fill these options in to the arguments like so:

.. figure:: /images/example_et_traceroute_02.png

   Figure 1.1: Showing all arguments with application tracert

This is all we really need in order to do a traceroute. Right click on a connection in the connection
list and go to :menuselection:`External Tools --> Traceroute` which will open a cmd prompt and run a
tracert against the host using hostname variable.

.. figure:: /images/example_et_traceroute_03.png

   Figure 1.2: External Tool selection for Traceroute

A console like below will appear that show the traceroute and will not exit until you close the window.

.. figure:: /images/example_et_traceroute_04.png

   Figure 1.3: CMD running tracert

If you want to use **powershell** instead. Then follow information below:

- Filename - powershell.exe
- Arguments - -NoExit tracert %HOSTNAME%

Notice that we replaced the /K with -NoExit and changed **cmd** with **powershell.exe**. See image below:

.. figure:: /images/example_et_traceroute_05.png

   Figure 1.4: Powershell and traceroute
