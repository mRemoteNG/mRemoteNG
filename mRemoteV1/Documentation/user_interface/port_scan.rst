.. _port_scan:

*********
Port Scan
*********

The Port Scan feature (under Tools > Port Scan) is similar to a nmap port scan. 
It will scan a range of IP addresses and to determine if specific mRemoteNG supported protocols are active. Hosts can then be bulk imported into mRemoteNG.

.. tip::

    If you leave this at the default of 0 & 0, the test will be for the default protocol ports that mRemoteNG supports.

- Start the Port Scan feature by clicking Tools > Port Scan in the menu bar.
- Input your Start IP and End IP of the range you'd like to scan.
- Enter the Start Port and End Port that mRemoteNG should test for.
- Click Scan
- Wait. Possibly a long time.
- The table will populate, and eventually you'll get a notification that the scan has completed. Alternatively, you can press Stop to end the scan at any time.
- Change the dropdown to the protocol you'd like to import and click Import.