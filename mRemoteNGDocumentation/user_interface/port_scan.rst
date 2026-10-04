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
- The results show the resolved hostname and IP address in separate columns. Select ``Use IP address for imported connections`` to import connections using the IP instead of the hostname.
- Change the dropdown to the protocol you'd like to import and click Import. By default, only hosts with that protocol's standard port open are imported.
- Select ``Import all open ports as selected protocol`` to create one connection for each open port found in the scan range, using the selected protocol and the scanned port number.