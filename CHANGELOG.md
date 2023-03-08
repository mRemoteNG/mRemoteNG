# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
## [1.77.3.1784]
### Fixed
- #2362: Fix use of sql database
- #2356: Improve speed for the display of the options page
- #2352: SSH.NET Update
- #2346: Modify "auto reconnect" to have the ability to really auto-reconnect
- #2340: Set the default theme setting
- #2339: Add 2 missing settings
- #2261: Implement Show/Hide file menu in view menu
- #2244: Save RCG and RestrictedAdmin fields correctly in connections file

### Added
- #2285: Support extraction of SSH private keys from external cred prov
- #2268: Postregsql database support

### Updated
- #2295: Updates hyperlink style to make links more visible to end users
- #2337: Set language.resx to auto generate the designer class

## [1.77.3]
### Added
- #1736: Update of SSH.NET to 2020.0.2 to allow File Transfer again
- #2138: Improve compatibility with Remote Desktop Connection Manager v2.83
- #2123: Thycotic Secret Server - Added 2FA OTP support
### Changed
- #1546: Enable resize without reconnect for RDP Version Rdc9 or higher
 
## [1.77.2]
### Added
- #2086: Replace WebClient with async HttpClient for updater.
- #1850: Minify config xml
- #1770: Added missing RDP performance settings
- #1516: added API to access credential vault (Thycotic Secret Server) by specifying SSAPI:ID as username
- #1476: Configurable backups. Can now edit/set backup frequency, backup path, and max number of backup files.
- #1427: Fix RDP local desktop scale not taking effect on remote
- #1332: Added option to hide menu strip container
- #870: Added option to push inheritance settings to child nodes recursively
- #545: Option to minimize to system tray on closing
- #503: SSH Execute a single command after login
- #420: SSH tunneling implemented
- #327: Added Alternative Shell for RDP settings
- #319: Override quick connect username when using user@domain
- #283: Support for native PowerShell remoting as new protocol
- #xxx: Add external connector to retrieve ip address from Amazon EC2 Instance IDs
### Changed
- #2102: Extended the field RenderingEngine from 10 chars to 16
- #2022: Replaced CefSharp with WebView2
- #2014: Revised icons
- #2013: Removed components check
- #2011: Removed screenshot manager
- #2010: Redesigned menus
- #2005: Removed in-app documentation
- #1777: Cleaned up VisualStudio project structure
- #1767: Turned about window into a simple popup form
- #1690: Replaced GeckoFX (Firefox) with CefSharp (Chromium)
- #1325: Language resource files cleanup
- #xxxx: Secret Server connector via new field "API User ID" instead of SSAPI: prefix
### Fixed
- #2125: Fixed string parsing logic for Quick Connect toolbar.
- #2122: Fix to avoid throwing exception incase if not able decrypt connections and ask to open another one or create a new. 
- #2117: Fix of broken Links due migration to .NET 6 and branch renaming
- #2098: Fix failed BinaryFileTest
- #2097: Fix failed tests related to mRemoteNGTests.UI.Window.ConfigWindowTests
- #2096: Corrected encryption code of LegacyRijndaelCryptographyProvider
- #2089: Fixed the exception thrown by menu buttons "Documentation" and "Website"
- #2087: Fixed application crash, when the update file is launched from the application
- #2079: Fixed theme files not being copied to output directory
- #2012: Updated PuTTYNG to v0.76
- #1884: Allow setting Port when using MSSQL
- #1783: Added missing inheritance properties to SQL scripts
- #1773: Connection issue with MySql - Missing fields in
- #1756: Cannot type any character on MultiSSH toolbar 
- #1720: Show configuration file name in title of password prompt form
- #1713: Sound redirection does not work if Clipboard redirection is set to No
- #1632: 1.77.1 breaks RDP drive and sound redirection
- #1610: Menu bar changes to English when canceling options form
- #1595: Unhandled exception when trying to browse through non existent multi ssh history with keyboard key strokes
- #1589: Update SQL tables instead of rewriting them
- #1465: REGRESSION: Smart Cards redirection to Remote Desktop not working
- #1363: Don't show "Disk Usage" button in installer
- #1337: Unhandled exception after closing mRemoteNG
- #359: Making a VNC connection to an unreachable host causes the application to not respond for 20-30 seconds
- #618: Do not break the Windows Clipboard Chain when exiting.

## [1.77.1] - 2019-09-02
### Added
- #1512: Added option to close panel from right click menu
- #1434: Revised sort button in connection tree to be able to sort in both orders
- #1400: Added file download handling to HTTP(S) connections using Gecko
- #1385: Added option to start mRemoteNG minimized
- #826: Allow selecting RDP version to use when connecting
### Changed
- #1544: Improved Polish translations
- #1518: Inheritance is no longer automatically enabled when importing nodes from Active Directory
- #1468: Improved mRemoteNG startup time
- #1443: Chinese (simplified) translation improvements
- #1437: Norwegian translation improvements
- #1378: Hyperlinks embedded within mRemoteNG now open in the system default browser
- #1239: Increased default key derivation function (KDF) iterations from 1000 to 10000
- #718: Moved port property from 'protocol' to 'connection' section
- Moved most RDP enums outside of the RDP protocol class. Scripts which reference these enums will need to be updated.
- Removed the "Automatically get session info" from the advanced options screen since it is no longer used.
### Fixed
- #1505: About screen now better follows theme colors
- #1493: Updated database setup scripts for MSSQL and MySQL
- #1470: The "Favorite" setting is now properly saved in the local connection settings file (not saved in database)
- #1447: Exception occurs when resetting layout
- #1439: Searching in hosts tree loses first keystroke
- #1428: Fixed a rare error when checking for FIPS
- #1426: Tabbing is reversed in config window
- #1425: Connections didn't always respect the panel property
- #841: Allow for sorting in port scan results
- #617: Added missing description for password protect field in root node
- #553: Browser language not set when using Gecko rendering engine
- #323: Wallpaper always shows in RDP connections, even when turned off


## [1.77.0] - 2019-04-29
### Added
- #1422: Added possibility to connect to virtual machines running on Hyper-V
- #1414: Add "Remote Audio Capture" option for RDP
- #1336: Added ability to run External tools on folders
- #1320: Added ability to favorite items in the connection tree
- #1318: Added support for saving connections in MySQL
- #1293: Importing .rdp files now imports gateway settings
- #1246: Improved connections loading to tolerate missing attributes in the confCons xml file
- #1230: Added option to track the currently focused connection in the connection tree
- #1220: Added an Apple/Mac connection icon
- #1218: A splashscreen has been added when mRemoteNG starts
- #1216: Connection tree search bar can be placed at the top or bottom of connection tree
- #1201: The help files packaged with mRemoteNG have been rewritten
- #1186: Certain dialogs are not correctly using localized text for buttons
- #1170: The Options window no longer displays in the Windows taskbar when open
- #1141: 'Copy Hostname' option added to connection tree context menu
- #1123: Added a dialog that will display when unhandled exceptions occur
- #1102: Added a button to clear connections searchbox
- #1042: Added a connection icon for OSX/MacOS
- #951: Added property to Enable/Disable Clipboard Sharing for RDP connections
- #929: Added the hostname to certain RDP error/disconnect messages where it was missing
- #928: Add context menu items to 'Close all but this' and 'Close all tabs to the right'
- #907: Added option to disable trimming whitespace from username field
- #896: Added a "view only" mode for RDP connections
- #416: Added ability to Enable/Disable Clipboard Sharing for RDP connections
- #321: Added support for displaying on HiDPI screens
### Changed
- #1430: Revised options dialog to prevent components from overlapping in some translations (e.g. russian)
- #1389: Connection config window refactoring. Default connection info buttons now always available.
- #1384: Revised help files and switched to sphinx as a documentation system
- #1223: Open External Links in Default Web Browser
- #1129: Spanish translation improvements
- #1072: Russian translation improvements
- #1016: Chinese (simplified) translation improvements
- #765: Port Scan Issues (single port scan option now available)
- #155: Replace MagicLibrary with DockPanelSuite
- #154: MR-139: Close Button on Each Tab - new default theme has a close button on each tab
<!-- ### Deprecated -->
<!-- ### Removed -->
### Fixed
- #1383: Fixed issue where default Computer OU was not showing up when importing from Active Directory
- #1248: RemoveMagicLib Bugs - various bugs that cropped up as a result of removing magiclib
- #1245: Options form takes nearly 3 seconds to appear when Theming is active
- #1240: Theming problem with NGNumericUpDown
- #1238: Connection panel not translated until opened for the first time
- #1186: Fixed several dialog boxes to use localized button text
- #1170: Prevent Options window from showing up in taskbar
- #1064: "Esc" button does does not close some dialogs
- #1044: Dragging (grabbing) the program window requires 2 clicks
<!-- ### Security -->

## [1.76.20] - 2019-04-12
### Fixed
- #1401: Connections corrupted when importing RDC Manager files that are missing certain fields

## [1.76.19] - 2019-04-04
### Fixed
- #1374: Vertical Scroll Bar missing in PuTTYNG after 0.70.0.1 & 0.71 updates

## [1.76.18] - 2019-03-20
### Fixeed
- #1365: PuTTY window not centered after 0.71 update

## [1.76.16] - 2019-03-14
### Fixed
- #1362: Updated PuTTYNG to 0.71

## [1.76.15] - 2019-03-09
### Added
- Importing multiple files now only causes 1 save event, rather than 1 per file imported
### Fixed
- #1303: Exception on first connection with new SQL server database
- #1304: Resolved several issues with importing multiple RDP Manager v2.7 files

## [1.76.14] - 2019-02-08
### Changed
- #222: Pre-Release Test build for running on systems with FIPS Enabled

## [1.76.12] - 2018-11-08
### Added
- #1180: Allow saving certain connection properties locally when using database
### Fixed
- #1181: Connections sometimes dont immediately load when switching to sql feature
- #1173: Fixed memory leak when loading connections multiple times
- #1168: Autohide Connection and Config tab won't open when ssh connection active
- #1134: Fixed issue where opening a connection opens same connection on other clients when using database feature
- #449: Encrypt passwords saved to database

## [1.76.11] - 2018-10-18
### Fixed
- #1139: Feature "Reconnect to previously opened sessions" not working
- #1136: Putty window not maximized

## [1.76.10] - 2018-10-07
### Fixed
- #1124: Enabling themes causes an exception

## [1.76.9] - 2018-10-07
### Fixed
- #1117: Duplicate panel created when "Reconnect on Startup" and "Create Empty Panel" settings enabled
- #1115: Exception when changing from xml data storage to SQL
- #1110: Pressing Delete button during connection rename attempts to delete the connection instead of the text
- #1106: Inheritance does not work when parent has C# default type set
- #1092: Invalid Cast Exceptions loading default connectioninfo 
- #1091: Minor themeing issues
- #853: Added some additional safety checks and logging to help address RDP crashes

## [1.76.8] - 2018-08-25
### Fixed
- #1088: Delete and Launch buttons are not disabled when last external tool deleted
- ${1}'Save connections after every edit' setting not honored
- #1082: Connections not given GUID if Id is empty in connection xml

## [1.76.7] - 2018-08-22
### Fixed
- #1076: Wrong object selected when duplicating connection then switching between properties and inheritance in config window
- #1068: Fixed some toolbar positioning bugs

## [1.76.6] - 2018-08-03)
### Fixed
- #1062: Entering correct password when starting app does not load connections file

## [1.76.5] - 2018-08-02)
### Removed
- #893: Removed unneeded files from build/package
### Fixed
- #1057: Hitting F2 with no connection node selected caused unhandled exception
- #1052: 'Switch to notification panel' feature does not always switch
- #1051: Tooltips always displayed regardless of 'Show description tooltips in connection tree' setting
- #1050: Config window retains access to previously selected node after loading new connections file
- #1045: Config window shows several incorrect properties for HTTPS connections
- #1040: Canceling "select panel" form does not cancel
- #1039: Set default theme when themes disabled
- #1038: Unable to add connection with active filter
- #1036: Exception when themes are active and options page closed on Connections then reopened
- #1034: Connection context menu not being translated
- #1030: Exception thrown if importing from port scan and no tree node is selected
- #1020: BackupFileKeepCount setting not limiting backup file count
- #1004: Duplicating root or PuTTy node through hotkey causes unhandled exception
- #1002: Disabling filtering without clearing keyword leaves filtered state
- #1001: Connection tree context menu hotkeys stop working and disappear in some cases
- #999: Some hotkeys stop working if File menu was called when PuTTy Saved Sessions was selected
- #998: Can sometimes add connection under PuTTY Sessions node
- #991: Error when deleting host in filtered view
- #971: Portable Settings now apply to any machine they are used on
- #961: Connections file overwritten if correct decryption password not provided
- #868: if statement returned the same value
- #762: Increased button size to fit locaized text

## [1.76.4 Alpha 6] - 2018-06-03
### Added
- #924: Notification for "No Host Specified" when clicking folders in quick-connect menu
- Added option for creating an empty panel on startup
### Changed
- #942: Improved Russian translation of several items
- #902: Menu bar can once again be moved. View -> "Lock toolbar positions" now also locks the menu position
### Fixed
- #948: Fixed issue where many menu item translations were not being used
- #938: Minor layout improvements on the Port Scan screen
- #916: Default properties were not being saved

## [1.76.3 Alpha 5] - 2018-03-14
### Fixed
- #911: Csv exports sometimes do not include all fields
- #807: Inheritance is sometimes turned on for nodes under root Connections node

## [1.76.2 Alpha 4] - 2018-03-03
### Fixed
- #899: DoNotPlay is Case Sensitive in XML Serialization

## [1.76.1 Alpha 3] - 2018-02-24
### Added
- #625: Added ability to import mRemoteNG formatted CSV files
- #648: The port scan ping timeout is now configurable
### Fixed
- Fixed a few Xml serialization bugs that would occur if boolean values weren't capitalized

## [1.76.0 Alpha 2] - 2018-02-01
### Added
- #838: Added an option to lock toolbars
- #836: Added a Read Only option for SQL connections
- #829: Add option that fixes connecting to Azure instances with LoadBalanceInfo
### Fixed
- #840: Fix theme loading issue in installer version
- #800: Fixed issue with PuTTY sessions not showing some extended characters
- Fixed a few toolbar layout issues

## [1.76.0 Alpha 1] - 2017-12-08
### Added
- #799: Added option to save connections on every edit
- #798: Added button to test SQL database connections on SQL options page
- #611: Added multi-ssh toolbar for sending commands to many SSH clients at once
- #519: You can now import normal mRemoteNG files - they do not have to be exports
- #504: Added Korean translation
- #485: The Domain field is now visible/editable for connection with the IntApp protocol
- #468: Default connection info Panel property is now saved
- #429: Added Czech translation
- #421: When a connection file cannot be loaded, we will now prompt for how to proceed rather than always exiting.
- #338: Added option to filter connection tree when searching
- #225: Added support for importing Remote Desktop Connection Manager v2.7 RDG files
- #207: Can now specify a working directory for external tools
- #197: Selecting a quick connect protocol will start a connection with that host
- #184: Improve search to include description and hostname fields
- #152: Added option "Show on Toolbar" to external tools
- Added more logging/notifications options
### Changed
- #784: Rearranged some settings in the Options pages to prevent overlap with some translations
- #704: Portable version now saves settings in application directory
- #671: Revamped UI theme system
- #608: The Help -> Support Forum menu item now directs users to our Reddit community
- #558: Connection tree now shows customizable icons instead of play/pause icon
- #493: Changed backup file name time stamp to use local system time rather than UTC
- #357: Updated GeckoFX to v45.45.0.32
- Improved compatability between environments when building mRemoteNG from source
### Removed
- #797: Removed duplicate translation strings
### Fixed
- #747: Fixed unnecessary "PuttySessions.Watcher.StartWatching" error message
- #650: Fixed German translation typo
- #639: Fixed Italian translation typo
- #479: New connection tree nodes not starting in edit mode
- #233: Fixed crash that can occur when disconnecting from VNC server
- #195: Access to https with self-signed certificates not working

## [1.75.7012] - 2017-12-01
### Changed
- #810: Official mRemoteNG builds will now be signed with a DigiCert certificate
### Fixed
- #814: Fixed bug that prevented reordering connections by dragging
- #803: File path command line argument not working with network path

## [1.75.7011] - 2017-11-07
### Fixed
- #778: Custom connection file path command line argument (/c) not working
- #763: Sometimes minimizing folder causes connection tree to disappear
- #761: Connections using external tools do not start (introduced in 1.75.7009)
- #758: "Decryption failed" message when loading from SQL server
- Fixed issues with /resetpanels and /resetpos command line arguments
- Resolved bug where connection tree hotkeys would sometimes be disabled

## [1.75.7010] - 2017-10-29
### Fixed
- #756: CustomConsPath always null

## [1.75.7009] - 2017-10-28
### Changed
- #635: Updated PuTTYNG to 0.70
- Minor error message correction
- Minor code refactoring
### Fixed
- #676: Portable version ignores /cons param on first run
- #675: Attempting to add new connection/folder does not work in some situations
- #665: Can not add new connection or new folder in some situations
- #658: Keep Port Scan tab open after import
- #646: Exception after click on import port scan
- #610: mRemoteNG cannot start /crashes for some users on Windows server 2012 R2 server
- #600: Missing horizontal scrollbar on Connections Panel
- #596: Exception when launching external tool without a connection selected
- #550: Sometimes double-clicking connection tree node began rename instead of connecting
- #536: Prevented log file creation when writeLogFile option is not set
- #529: Erratic Tree Selection when using SQL Database
- #482: Default connection password not decrypted when loaded
- #335: The Quick Connect Toolbar > Connection view does not show open connections with the play icon
- #176: Unable to enter text in Quick Connect when SSH connection active


## NO RELEASE - 2017-06-15
### Fixed
- #466: Installer still failing on Win7 for updates - 1.75.7005 (Hotifx 5)
- #462: Remove no longer used files from portable version - 1.75.7003 (Hotfix 4)

## [1.75.7008] - 2017-06-15
### Changed
- Minor updates to the installer build
### Fixed
- #589: MSI doesn't update with newer PuTTYNG version that fixes #583 (Again, Sorry!)

## [1.75.7007] - 2017-06-14
### Fixed
- #583: SSH (PuTTYNG) Sessions are not properly integrated into the main mRemoteNG window (Sorry!)

## [1.75.7006] - 2017-06-13
### Changed
- #531: Update PuTTYNG to 0.69
### Fixed
- #377: Use all space on About page
- #527: Additional protections to avoid problems on update check in heavily firewalled environments
- #530: Fixed issue where using External Tool on existing connection causes creation of 'New Connection' entry
- #546: Quick Connect from notification area icon displays warning when clicking on a folder (see #334)

## [1.75.7005] - 2017-04-27
### Changed
- #410: Update PuTTYNG to 0.68
- Minor code cleanup/optimizations/null checks
### Fixed
- #434: Fix complier warnings CA1049 & CA2111
- #442: Fixed issue loading PuTTY sessions that have spaces in the name
- #502: Problems with ParentID for Duplicated Containers/Connections with SQL Connection Storage
- #514: Expanded property not saved/loaded properly from SQL
- #518: Exception when Importing File

## [1.75.7003] - 2017-03-24
### Fixed
- #464: Resolved issue when importing a connections file while using SQL server feature

## [1.75.7002] - 2017-03-10
### Fixed
- #448: Resolved issue with SQL saving

## [1.75.7001] - 2017-03-10
### Changed
- #408: Update SQL scripts

## [1.75 hotfix 1] - 2017-03-06
### Changed
- #437: Modify version numbering scheme
### Fixed
- #422: Uncaught exception when clicking in connection tree whitespace
- #312: Resolved KeePass auto-type issue
- #427: Export does not respect filtering user/password/domain

## [1.75] - 2017-03-01
### Known Issues
- File hash check will fail when updating from 1.75 Beta 1 to newer versions.
  - Exception will be: "MD5 Hashes didn't match!" for 1.75 Beta 1 - 1.75 RC1
### Added
- #344: Use SHA512 File Hashes to validate downloads (in the update mechanism & posted to the Downloads page)
- Added Release Channels to the update check functionality allowing users to select one of 3 release channels for updates: Stable, Beta, Dev
- #360: Help -> About, Version # is now selectable/copyable
- #221: RDP: Optional disconnect after X number of minutes of inactivity
### Changed
- The usual general code clean up and refactoring
- #325: Code clean up and additional logging for External Tools based connections
- #298: Code clean up and additional logging around application startup
- #291, #236: External Tools code clean up and additional logging
### Fixed
- #369: Reset Layout Option Does Not Reset Notification Pane
- #362: Invalid cast exception when using the Notification Area Icon minimize/restore
- #334: Quick Connect displays warning when clicking on a folder
- #325: When using a connection with an external app, results in opening the same external app continuously
- #311: Import from Active Directory does not use machine's domain by default
- #258: Rename Tab dialog - populate original name in dialog (1.72 functionality)
- #211, #267: Recursive AD Import wasn't fully functional 

## [1.75 Beta 3] - 2016-12-01
### Known Issues
- Portable build MD5 check will fail when updating from 1.75 Beta 1 to newer versions.
### Fixed
- #289: Install fails during update process (only affects 1.75 beta 1 - 1.75 beta 2)

## [1.75 Beta 2] - 2016-12-01
### Added
- #273: Added Turkish translation provided by forum user "rizaemet"
- #217: Create user manual documentation for the SSH File Transfer feature
### Fixed
- #254: Component check window position issues and uncaught exception
- #260: Crash when attempting to load fully encrypted confCons v2.5
- #261: Double clicking folder in treeview doesn't expand it in 1.75 beta1
- #271: Install package is not using the last installation path
- #278: Silent installs not detecting prerequisites

## [1.75 Beta 1] - 2016-11-15
### Added
- MR-971: Added Right Click method to Port Scan to import discovered hosts
- MR-1000, - #211: Sub OU AD Import
- #172: Implement "audioqualitymode" for RDP sessions
- #160: Allow portable version to perform an update check (and download the latest .zip)
- #157: Implement new cryptography providers (See Tools -> Options -> Security)
### Changed
- Lots of code clean up and refactoring
- Limit log file size to 1 main log + 5 backups * 10MB.
- Minor UI Tweak: Default Menubar and QuickConnect Bar to same line
- MR-364: Removed "Announcement" functionality 
- MR-366: Show PuTTY type and version on components check screen
- MR-586: Reduce HTTP/HTTPS document title length that is appended to the connection tab title
- MR-938: Adjust RDP Resolution list (ensure most common resolutions were available)
- MR-975: Replaced TreeView with TreeListView for displaying connection tree. This was a large change which separated the GUI from the domain model.
- #144: Removed export option for "VisionApp Remote Desktop 2008"
- MR-220: Don't close the AD importer after import
- #167: fix/implement the update check (was disabled in 1.74 as part of the C# conversion)
### Fixed
- MR-967: File transfer doesn't work when specifying full path and file name (as prompted)
- MR-979: switched to notifications panel incorrectly (when configured NOT to do so)

## [1.75 Alpha 3] - 2016-08-12
### Added
- MR-896: Added prerequisite installer check for KB2574819. Prevents "Class not registered" errors when opening RDP connections.
### Changed
- PR-130: Fix Scan button width to fit Russian translation
### Removed
- MR-946: Remove old/insecure SharpSSH and related components. Replace with SSH.NET for File Transfer Functionality
### Fixed
- MR-965, MR-871, MR-629: Error 264 on RDP Connect attempt - Added timeout value to Tools -> Options -> Connections

## [1.75 Alpha 2] - 2016-08-03
### Added
- MR-961, PR-133: Option to reconnect all currently opened connection tabs 
- MR-917: Improved cryptographic support
### Changed
- Updated GeckoFx package
- Updated DockPanelSuite library to 2.10 Final
- Japanese translation updated
- MR-942: Refactored code relating to loading the connections file 
### Fixed
- MR-910: Fixes to support Remote Desktop Gateways
- MR-874: Incorrect RDP prerequisite check in installer

## [1.75 Alpha 1] - 2016-07-08
### Changed
- Additional code cleanup
### Fixed
- MR-905: mRemoteNG crashes at startup (in FIPS policy check)
- MR-902: mRemoteNG crashes after locking/unlocking the local system after loading a VNC connection

## [1.74] - 2016-06-28
### Added
- MR-821: Initial Japanese translation included
### Changed
- New operating system requirements
  - Windows 7 SP1 (with RDP Client v8: KB2592687/KB2923545) or later
  - .NET Framework 4 or later
- XULRunner is no longer required for Gecko support (see below)
- Converted source from Visual Basic to C Sharp
- Lots of code refactoring/clean up/general stability changes
- Updated to latest DockPanelSuite and enabled a slick new theme!
- MR-145: Installer is now MSI based
- MR-255: Updated RDP Client to version 8
- MR-389: Updates to IE rendering engine
  - Support for latest version of IE (9-11)
  - Dropped support for IE 7 & IE 8
- MR-850: Replaced XULRunner with GeckoFx
  - No need to manually configure to have the Gecko rendering engine available now
  - Install image is now significantly larger due to the inclusion of the Gecko Engine
- Port Scan is now Asynchronous (and is significantly faster)
### Removed
- MR-714: Removed Keyboard shortcut functionality
  - Introduced in 1.73 beta that was never officially released
  - This caused stability issues
  - May be re-added in a future release
  - Removal does *NOT* impact the ability to send keyboard shortcuts to RDP sessions (when redirect key combinations is set to "yes")
- MR-559: Removed RDP Sessions panel functionality. This required a library for which no trusted origin/source could be located
- Removed a bunch of old code/libraries and replaced them accordingly
### Security
- MR-775, MR-745: Updated PuTTY to 0.67
### Fixed
- MR-874: Added work-around to installer to ignore installation prerequisites
- MR-884: Slow startup in some scenarios checking authenticode certificate
- MR-872: Crash in External Tools when arguments aren't quoted
- MR-854: crashes when right clicking on connection tab
- MR-852: Option "Allow only a single instance of the application" non-functional
- MR-836: Trying to delete a folder of connections only deletes 2 connections at a time
- MR-824, MR-706: Suppress Script Errors when using the IE rendering engine
- MR-822: Improve RDP error code messages
- MR-640: Fixed Inheritance not working
- MR-639: RDP: Connect to console session
- MR-610, MR-582, MR-451: RDP: Protocol Error 3334 or exceptions with large number of connections open
  - This problem appears largely resolved by most reports and testing
  - Further workarounds/problem avoidance: Disable Bitmap Caching on all RDP session configuration
- MR-429: Display issue on the Options -> Advanced panel
- MR-385: Inheritance settings lost when moving item to the root of the tree

## New Dev Team

## [1.73 Beta 2] - [NEVER RELEASED]
### Added
- Added support for importing files from PuTTY Connection Manager.
### Fixed
- Fixed issue MR-619 - Keyboard shortcuts stop working after locking the screen with Win+L
- Improved the import and export functionality.

## [1.73 Beta 1] - 2013-11-19
### Added
- Added feature MR-16 - Add keyboard shortcuts to switch between tabs
- Added feature MR-141 - Add a default protocol option
- Added feature MR-212 - Add option to connect without credentials
- Added feature MR-512 - Add support for importing files from Remote Desktop Connection Manager
- Added feature MR-547 - Add support for Xming Portable PuTTY
- MR-590: Added "Reset" to config panel context menu to allow resetting some config settings to their default value.
- Added and improved menu icons.
### Changed
- Made improvement MR-250 - Show the name of the selected connection tab in the title of the window
- Made improvement MR-367 - Make the 'Connect' button on the 'Quick Connect' toolbar a forced dropdown
- Made improvement MR-419 - Password prompt dialog should have a meaningful window title
- Made improvement MR-486 - Allow escaping of variable names for external tools
- Made improvement MR-590 - Make panels docked to the edge of the window keep their size
- Improved handling of variables in external tool arguments.
### Removed
- Removed misleading log messages about RD Gateway support.
- Removed invalid "Site" configuration option from PuTTY Saved Sessions.
- Fixed issue MR-187 - F7 keyboard shortcut for New Folder conflicts with remote connections
- Fixed issue MR-523 - Changes to external tools are not saved until exiting the program
- Fixed issue MR-556 - Export fails when overwriting an existing file
- Fixed issue MR-594 - Crash on startup if write access is denied to the IE browser emulation registry key
- Fixed issue MR-603 - Some configuration options are still shown even when inheritance is enabled
- Fixed PuTTY Saved Sessions still showing if all saved sessions are removed.
- Fixed config panel showing settings from previously loaded connection file after loading a new one.

## [1.72] - 2013-11-13
### Fixed
- Fixed issue MR-592 - Unable to run VBS script as an external tool
- Fixed issue MR-596 - Incorrect escaping of quotation marks in external tool arguments

## [1.71] - 2013-10-29
### Removed
- Removed warning message when mRemoteNG is started for the first time about new connections file being created.
### Fixed
- Fixed issue MR-574 - Crash when retrieving RDP session list if eolwtscom.dll is not registered
- Fixed issue MR-578 - Connections file is reset
- Fixed log file not showing operating system version on Windows XP and Windows Server 2003.
- Fixed the wrong connections file opening on startup under certain conditions.
- Fixed checking for updates even when disabled.
- Improved error reporting when loading connections files.

## [1.71 Release Candidate 2] - 2013-10-16
### Fixed
- Fixed issue MR-560 - Cannot Auto-Update With Open Connections: Unable to find an entry point named 'TaskDialogIndirect' in DLL 'ComCtl32'
- Fixed issue MR-565 - Double Folder keep heritage on the initial Folder
- Fixed issue MR-566 - Typo in German UI Automatic Update Settings
- Fixed duplicated folders possibly being named "New Connection" instead of the original folder's name.

## [1.71 Release Candidate 1] - 2013-10-01
### Added
- Added Chinese (Traditional) translation.
- Added partial Greek and Hungarian translations.
### Changed
- Updated PuTTY to version 0.63.
- Updated translations.
### Fixed
- Fixed issue MR-495 - Having a negative range in port scan creates memory exhaustion.
- Fixed issue MR-514 - Window Proxy test failed without close button
- Fixed issue MR-521 - Right-Clicking in "Sessions" panel crashes mRemoteNG
- Fixed issue MR-525 - Could not start on windows 7 64bit
- Fixed issue MR-535 - SQL error saving Connections
- Fixed issue MR-538 - RDP loses connection when hiding config or connections pane
- Fixed issue MR-542 - Wrapped putty has security flaw
- Made minor improvements to the port scan functionality.
- Fixed possible cross-thread operation exception when loading connections from SQL.
- Fixed PuTTY Saved Sessions not showing after loading a new connections file.

## [1.71 Beta 5] - 2013-06-09
### Fixed
- Fixed issue MR-491 - Could not start RDP Connection
- Fixed issue MR-499 - TS Gateway is not working in latest release 1.71
- Fixed typo in SQL queries.

## [1.71 Beta 4] - 2013-05-28
### Added
- Added feature MR-435 - Add digital signature check to updater
- Added Norwegian (Bokmal) and Portuguese (Brazil) translations.
- Added Spanish translation to the installer.
- Added PuTTY Session Settings command to the Config panel for PuTTY Saved Sessions.
### Changed
- Updated translations.
- Changed Internet Explorer to no longer force IE7 compatibility mode.
- Changed the "Launch PuTTY" button in the "Options" dialog to open PuTTY from the path the user has currently set, instead of what was previously saved.
- Updated VncSharpNG to 1.3.4896.25007
- Lowered required version of RDC from 6.1 to 6.0.
### Fixed
- Fixed issue MR-255 - The version of the RDP AX client should be updated to 7
- Fixed issue MR-392 - Sessions Panel - context menu entries need to be context aware
- Fixed issue MR-422 - Gives error Object reference not set to an instance of an object.
- Fixed issue MR-424 - Import of a few Linux SSH2 hosts discovered via the port scan tool results in a UE
- Fixed issue MR-439 - MRemoteNG 1.70 does not start
- Fixed issue MR-440 - RDP import with non-standard port
- Fixed issue MR-443 - Instructions for eolwtscom.dll registration for Portable version are inaccurate
- Fixed issue MR-446 - Putty saved sessions show in connection panel
- Fixed issue MR-459 - Maximized -> Minimized -> Restored results in mangled active display
- Fixed issue MR-463 - Add support for LoadBalanceInfo to RDP
- Fixed issue MR-470 - Quick Connect to Linux server uses invalid credentials
- Fixed issue MR-471 - PuTTY Saved Sessions disappears from connection list
- Fixed issue MR-487 - Initiate connections on MouseUp event
- Fixed an exception or crash when choosing unnamed colors for themes.
- Fixed possible error "Control does not support transparent background colors" when modifying themes.
- Fixed changes to the active theme not being saved reliably.
- Fixed handling of the plus (+) character in PuTTY session names.
- Improved update and announcement checking.
- Improved the PuTTY Saved Sessions list to update automatically when any changes are made.
- Improved loading time of large connection files.

## [1.71 Beta 3] - 2013-03-20
### Fixed
- Fixed issue MR-397 - Putty disappears from the screen
- Fixed issue MR-398 - Full Screen mode doesn't correctly make use of available space
- Fixed issue MR-402 - scrollbar touch moves putty window
- Fixed issue MR-406 - Items disappear from External Tools toolbar when accessing External Tools panel
- Fixed issue MR-410 - Unhandled exception when clicking New button under Theme
- Fixed issue MR-413 - Can't use application
- Fixed new connections having a globe icon.
- Fixed the category names in the themes tab of the options dialog on Windows XP not showing correctly.
- Fixed PuTTY saved sessions with spaces or special characters not being listed.

## [1.71 Beta 2] - 2013-03-19
### Added
- Added feature MR-336 - Customizable background color for the windows/panels
- Added feature MR-345 - Two separate options for confirming closure of Tabs and Connection Panels
- Added feature MR-346 - Option to show/hide the description box at the bottom of the Config panel
- Added feature MR-351 - Import connections from PuTTY
### Changed
- The username and domain settings are now hidden for VNC connections since they are not supported.
- Changed "Automatically get session information" to be disabled by default.
- RDP connections can now be switched to full screen mode when redirect key combinations is enabled.
### Fixed
- Fixed issue MR-354 - Re-ordering tabs doesn't give good, reliable visual feedback
- Fixed issue MR-375 - Changing a connection's icon using the picture button should immediately update Icon field
- Fixed issue MR-377 - Several redundant panels can be opened
- Fixed issue MR-379 - Connection variables not working with external tools
- Fixed issue MR-381 - Notifications panel - whitespace context menu allows Copy and Delete on nothing
- Fixed issue MR-401 - Checkbox misaligned

## [1.71 Beta 1] - 2013-03-04
### Added
- Added feature MR-329 - Create Option to disable the "Quick: " prefix
- Added detection of newer versions of connection files and database schemata. mRemoteNG will now refuse to open them to avoid data loss.
### Fixed
- Fixed issue MR-67 - Sort does not recursively sort
- Fixed issue MR-117 - Remote Session Info Window / Tab does not populate
- Fixed issue MR-121 - Config pane not sorting properties correctly when switching between alphabetical and categorized view
- Fixed issue MR-130 - Issues duplicating folders
- Fixed issue MR-142 - Start of mRemoteNG takes about one minute and consumes excessive CPU
- Fixed issue MR-158 - Password field not accepting Pipe
- Fixed issue MR-330 - Portable version saves log to user's profile folder
- Fixed issue MR-333 - Unnecessary prompt for 'close all open connections?'
- Fixed issue MR-342 - Incorrect view in config pane of new connection after viewing default inheritance
- Fixed issue MR-352 - Passwords with " (quotation mark) and # (hash key) characters make mRemoteNG to open PuttyNG dialog
- Fixed issue MR-362 - Rename 'Screenshot Manager' to 'Screenshots' on the View menu to match Panel name
- Improved appearance and discoverability of the connection search box.
- If RDC 7.0 or higher is installed, the connection bar is no longer briefly shown when connecting to an RDP connection with redirect key combinations enabled.
- If RDC 8.0 or higher is installed, RDP connections automatically adjust their size when the window is resized or when toggling full screen mode.

## [1.70] - 2013-03-07
### Fixed
- Fixed issue MR-339 - Connection group collapses with just one click
- Fixed issue MR-340 - Object reference not set to an instance of an object.
- Fixed issue MR-344 - Move "Always show panel tabs" option
- Fixed issue MR-350 - VerifyDatabaseVersion (Config.Connections.Save) failed. Version string portion was too short or too long.
- Fixed issue MR-355 - Moving sub folders to top level causes property loss
- Fixed tabs not closing on double-click when the active tab is a PuTTY connection.

## [1.70 Release Candidate 2] - 2013-02-25
### Changed
- Re-enabled PuTTYNG integration enhancements on Windows 8
### Fixed
- Fixed issue MR-332 - Can't select different tab with one click after disconnecting existing tab
- Fixed issue MR-338 - PuTTYNG crashing on fresh install of mRemoteNG
Re-enabled PuTTYNG integration enhancements on Windows 8

## [1.70 Release Candidate 1] - 2013-02-22
- Fixed issue MR-183 - Error trying to save connections when using SQL - Invalid column name _parentConstantId
- Fixed issue MR-225 - Tabs do not open in a panel until multiple panels are displayed.
- Fixed issue MR-229 - Integrated PuTTY doesn't work in Windows 8 RP
- Fixed issue MR-264 - Windows 8 support
- Fixed issue MR-317 - Difficulty right-clicking on Tab
- Fixed issue MR-318 - Wrong tab gets selected when tab names overflow on the tab bar
- Fixed issue MR-321 - New connection panel doesn't get panel header if its the only one or is moved
- Fixed issue MR-322 - Connection Button not listing servers
- Added option to always show panel tabs.
- Fixed "Decryption failed. Padding is invalid and cannot be removed." notification.
- Fixed KiTTY opening in a separate window when using a saved session.

## [1.70 Beta 2] - 2013-02-18
### Added
- Added translations for Spanish (Argentina), Italian, Polish, Portuguese, Chinese (Simplified).
### Changed
- mRemoteNG now requires .NET Framework 3.0 instead of 2.0.
- Updated translations.
- Improved the use of Tab and Shift-Tab to cycle through entries in the Config grid.
- Improved loading of XML files from older versions of mRemote/mRemoteNG.
### Fixed
- Fixed issue MR-47 - Silent Installation Prompts for Language
- Fixed issue MR-54 - Error When disconnecting from SSL channel RDP
- Fixed issue MR-58 - Bug when duplicating connection in connection view 
- Fixed issue MR-68 - Config Window Loses Options
- Fixed issue MR-71 - Minimizing mRemoteNG causes temporary re-size of Putty sessions (windows)
- Fixed issue MR-80 - Reconnect previous sessions
- Fixed issue MR-81 - Problem Duplicating Folder w/ Sub-Folders
- Fixed issue MR-85 - Microsoft .NET Framework warning
- Fixed issue MR-86 - Citrix GDI+ Error when screen is locked
- Fixed issue MR-96 - When pressing SHIFT+F4 to create a new connection inside a folder, the new connections doesn't inherit any properties from its parent
- Fixed issue MR-101 - Collapse all folders causes a NullReferenceException
- Fixed issue MR-165 - Can't close About window if it is the last tab
- Fixed issue MR-166 - Inheritance button is disabled on some connections
- Fixed issue MR-167 - Name and description of properties not show in inheritance list
- Fixed issue MR-171 - Inherit configuration not showing friendly names for each inherit component
- Fixed issue MR-172 - RDGatewayPassword is unencrypted in confCons.xml file
- Fixed issue MR-174 - Trailing Space on a Hostname/IP will cause the connection not to happen.
- Fixed issue MR-175 - Problem with focus when 2 or more PuTTY sessions opened
- Fixed issue MR-176 - Del key while editing connection name triggers 'Delete Connection'
- Fixed issue MR-178 - 3 different panels crashes all connections
- Fixed issue MR-181 - Sessions on startup
- Fixed issue MR-190 - Can't click on tab/session
- Fixed issue MR-196 - Cannot export list without usernames and passwords
- Fixed issue MR-199 - when using screen inside putty, screen becomes dead when reduce mremoteNG
- Fixed issue MR-202 - The Connection "Tab" show Ampersands as underscores.
- Fixed issue MR-214 - Hostname/IP reset
- Fixed issue MR-224 - Session tabs become un-clickable after duplicating a tab or opening a new one in the same panel
- Fixed issue MR-233 - Backslash at end of password prevents success of putty invocation and corresponding auto-logon
- Fixed issue MR-235 - Config file gets corrupted when leaving the password entry box with ESC
- Fixed issue MR-264 - Windows 8 support
- Fixed issue MR-277 - Inheritance configuration button not appear in configuration tab
- Fixed issue MR-284 - SSH: Text not showing properly
- Fixed issue MR-299 - mRemoteNG crashes while using remotely (Windows XP remote desktop)
- Fixed issue MR-306 - Fatal .NET exception on program start
- Fixed issue MR-313 - PuTTY window not maximized when loading from saved session

## [1.70 Beta 1] - 2012-02-27
### Added
- Added compatibility check for "Use FIPS compliant algorithms" security setting.
Improved reporting of errors when encrypting and decrypting connection files.
- Added partial Polish translation.
- Added the option to use a smart card for RD Gateway credentials.
- Added debugging symbols for VncSharpNG.
- A backup of the connection file is saved when it is loaded. The most recent ten backup copies are kept.
### Changed
- The panel tabs are now hidden if only one panel is open.
- Show changes live as connection tabs are being dragged around to change their order.
- Updated PuTTY to version 0.62.
- Made the use of CredSSP for RDP connections optional.
- Updated VncSharpNG to version 1.2.4440.36644.
### Fixed
- Fixed issue MR-77 - VerifyDatabaseVersion (Config.Connections.Save) failed. Version string portion was too short or too long.
- Fixed issue MR-78 - Renaming Connections
- Fixed issue MR-79 - MoveUp/Down item doesn't work + Sort button broken
- Fixed issue MR-93 - Regional settings problem when using SQL connection in mRemoteNG
- Fixed issue MR-97 - Integrate Dutch translation
- Fixed issue MR-98 - Integrate Russian and Ukranian translations
- Fixed issue MR-99 - Integrate Spanish translation
- Fixed issue MR-131 - RD Gateway does not respect setting for use different credentials
- Fix focus issue with RDP connections when changing tabs.
- Improved error handling when loading connection files.
- Fixed bugs with creating a new connection file.

## [1.69] - 2011-12-09
### Added
- Added Credits, License, and Version History items to the Start Menu and made Start Menu item names localizable.
### Changed
- Updated PuTTY to version 0.61
- Binaries are now digitally signed
### Deprecated
- Disabled automatic updates in the portable edition
### Fixed
- Fixed issue #66 - Fresh Install Fails to Create Config
- Fixed issue #69 - Connection file gets erased
- Fixed issue #72 - scrollbars added to RDP window after minimize/restore of mRemoteNG
- Fixed file name in window title changing when exporting an XML file.
- Fixed Use only Notifications panel checkbox.

## [1.68] - 2011-07-07
### Fixed
- Fixed issue #48 - VerifyDatabaseVersion fails with new (empty) database tables.
- Fixed issue #60 - Can't save connections file
- Fixed issue #62 - Connection file error upon launch.

## [1.67] - 2011-06-05
### Added
- Added a language selection option so users can override the language if they don't want it automatically detected.
- Added partial French translation to the application.
- Added Thai translation to the installer.
- Added buttons for Add Connection, Add Folder, and Sort Ascending (A-Z) to the Connections panel toolbar.
- Added 15-bit Color RDP setting.
- Added Font Smoothing and Desktop Composition RDP settings.
- Added the mRemoteNG icon to the list of selectable icons for connection entries.
- Added confirmation before closing connection tabs.
### Changed
- Updated graphics in the installer to mRemoteNG logo.
- Moved the items under Tools in the Connections panel context menu up to the top level.
- Changed sorting to sort all subfolders below the selected folder.
- Allow sorting of connections if a connection entry is selected.
- Changed the Options page into a normal dialog.
- Changed to use full four part version numbers with major, minor, build, and revision.
- Changed hard coded SQL database name into a user configurable setting.
### Fixed
- Fixed migration of external tools configuration and panel layout from Local to Roaming folder.
- Disable ICA Hotkeys for Citrix connections.Fixes issue with international users.
- Fixed RD Gateway default properties and RDP reconnection count setting not being saved.
- Fixed bug 33 - IPv6 doesn't work in quick Connect box.
- Fixed rename edit control staying open when collapsing all folders.
- Fixed adding a connection entry if nothing is selected in the tree.
- Fixed loading of RDP Colors setting from SQL.
- Improved error handling when loading XML connection files.
- Fixed bug 42 - Maximized location not remembered with multiple monitors.
- Improved loading and saving of window location.
- Removed flickering on start up.
- Improved Reset Layout function.
- Fixed tab order of controls in Options dialog.
- Fixed bug 45 - Changing some settings in the config file may not save.

## [1.66] - 2011-05-02
### Fixed
- Fixed connections not working

## [1.65] - 2011-05-02
### Added
- Added code to the installer to check that the user is in the 'Power Users' or 'Administrators' group
### Changed
- Ctrl-Tab and Ctrl-Shift-Tab no longer work to switch tabs within mRemoteNG
### Fixed
- Fixed Ctrl-Tab and Ctrl-Shift-Tab not working in any other applications while mRemoteNG is running
- Fixed bug 36 - Install creates shortcuts only for the installing user
- Fixed bug 38 - Application uses the wrong Application Data settings folder (in Local Settings)

## [1.64] - 2011-04-27
### Added
- Added multilanguage support and German translation to the application
- Added Czech, Dutch, French, German, Polish, and Spanish translations to the installer
- Added Ctrl-Tab hotkey to switch to the next tab and Ctrl-Shift-Tab to switch to the previous tab
- Added Tab key to cycle through entries in the Config grid and Shift-Tab to cycle in reverse
- Added ability to configure external tools to run before or after a connection is established
- Added credit for the DockPanel Suite to the About page
### Changed
- Changed how new connection files are created
- Changed the internal namespace of the application to mRemoteNG instead of mRemote
- Updated DockPanel Suite to version 2.5 RC1
- Updated VNCSharpNG to correct Ctrl and Alt key pass-through behavior
### Fixed
- Fixed bug 6 - VNC CTRL+key & keyboard combo mappings are broken
- Fixed bug 12 - Tab switch is not working in config panel
- Fixed bug 14 - RDP Connection authentication problem
- Fixed bug 22 - External App parameter macro expansion doesn't work with "try to integrate"
- Fixed bug 25 - Unhandled exception when mRemoteNG opens
- Fixed missing parameters in macro expansion for external tools
- Fixed RD Gateway and other inheritance bugs

## [1.63] - 2010-02-02
### Added
- Added View->Reset Layout menu item
- Added F11 shortcut key to View->Full Screen
- Added support for Credential Security Support Provider (CredSSP) which is required for Network Level Authentication (NLA)
- Added support for connecting through Remote Desktop Gateway servers
- Added PuTTY Settings item to tab context menu
### Changed
- New icon and logo
- Updated DockPanel Suite from 2.2.0 to 2.3.1
- Popups can now be allowed in Internet Explorer by holding Ctrl+Alt when clicking a link
### Fixed
- Fixed problems moving or resizing the main window while PuTTY (SSH/telnet/rlogin/raw) connections are open
- Fixed PuTTY processes not closing on Vista and 7 with UAC enabled
- Fixed error if the mouse is clicked outside of the remote screen area of a VNC connection
- Fixed flashing and red lines at bottom of the window on first run
- Improved RDP error reporting

## [1.62] - 2010-01-19
### Added
- Switched to VncSharp, an open source VNC component
- VNC is supported again except for the following features:
  - Windows authentication
  - Setting the compression, encoding and color settings
  - Connecting through a proxy server
  - Free SmartSize mode (it does the same thing as Aspect SmartSize mode now)
- Added option to change how often updates are checked
- Added RDP, VNC and ICA version numbers to Components Check page
### Changed
- Rearranged the Options page and added an Updates tab
- Open Updates options tab before connecting for the first time
- No longer show About page on first run
- Renamed Quicky toolbar to Quick Connect toolbar
- Changed back to allowing toolbars to dock to the left or right of the menu bar and added gripper to move it around
### Fixed
- Fixed a bug with the inheritance buttons on the Config panel disappearing after awhile

## [1.61] - 2010-01-14
### Changed
- This version of mRemoteNG does not support VNC
### Removed
- Removed unlicensed SmartCode Solutions ViewerX VNC Viewer ActiveX

## [1.60] - 2010-01-09
### Added
- Added Report a Bug and Support Forum links to the Help menu
### Changed
- Changed name to mRemoteNG
- Changed filename delimiter in title bar from pipe to dash
- Changed default format for saving screenshot images to PNG
- Changed website addresses
- Changed website links in Help menu and About page to load within mRemoteNG instead of launching an external browser
- Moved Check for Updates to the Help menu
### Removed
- Removed snakes game Easter egg
- Removed references to visionapp Remote Desktop
### Fixed
- Fixed menu bar not staying docked to left side

## [1.50] - 2010-01-06
### Added
- Added the following formats to the "Save Connections As" function:
  - mRemote CSV (standard CSV file with all properties)
  - vRD 2008 CSV (standard CSV file with properties relevant for importing connections in vRD 2008)
### Fixed
- Fixed bug in inheritance code (SmartSize Mode and View Only properies were always shown when using VNC)

## 1.49
### Added
- Added features to the update function
- Added Announcement feature
### Changed
- mRemote and visionapp Remote Desktop 2008 merge!
- Read more here: ~~http://www.mremote.org/wiki/visionappMerge.ashx~~
- or in the Announcement panel.
- Changed copyright notice in about screen and text when connecting via VNC
### Fixed
- Fixed some SQL-related problems

## 1.48
### Added
- Added startup components check with directions to fix component installation (also available in Tools - Components Check)
- Added "Try to integrate" option to Ext. Apps. If enabled mRemote will try to integrate the app into a tab container like any other connection protocol.
- Added Ext. App as protocol. Any Ext. App can be launched just like a normal connection.
- Added option to completely encrypt connection files (tools - options - advancecd)
- Added Rendering Engine option for HTTP/S protocols
- You can now use the Gecko (Firefox) rendering engine
```
For this to work you need to download xulrunner (get it here: ftp://ftp.mozilla.org/pub/xulrunner/releases/1.8.1.3/contrib/win32/)
It must be the 1.8.1.3 release, 1.9.0.0 does NOT work!
Extract the contents to a path of your choice and set the correct path in Tools - Options - Advanced - XULrunner path
The interface is tab enabled and usage is generally very firefox-like. So you can open new tabs with Ctrl+T, jump to the location bar with Ctrl+L and so on...
```
- Added "MAC Address", "User Field" fields and %MacAddress%, %UserField% variables to use in Ext. Apps
- Added descriptions for all fields in the config editor### Changed
### Changed
```
ATTENTION! There is a bug in the automatic update code in 1.45 so you will have to download the new version manually from ~~http://www.mremote.org/wiki/Downloads.ashx~~

Example (DameWare Mini Remote Control):
Create a new Ext. App with the following properties:
Display Name: DameWare
Filename: c:\PathToYourDameWareInstallDir\DWRCC.exe
Arguments: -c: -h: -m:%hostname% -u:%username% -p:"%password%" -d:%domain%
Options: Try to integrate
Create a new connection and select Ext. App as protocol
Then choose DameWare in the Ext. App field
If you have problems with a particular app that takes a long time to start up consider setting a higher PuTTY/Ext. Apps wait time in Tools - Options - Advanced### Fixed
```
```
WARNING! There have been changes to the connections file/SQL tables
Please always backup your whole config before updating to a new mRemote beta release, especially when there have been changes to the config files/SQL tables
To get SQL working with the new version please update your tables like in the provided script (Info - Help - SQL Configuration)
These are the added lines:
[RenderingEngine] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
[MacAddress] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
[UserField] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
[ExtApp] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
[InheritRenderingEngine] [bit] NOT NULL ,
[InheritMacAddress] [bit] NOT NULL ,
[InheritUserField] [bit] NOT NULL ,
[InheritExtApp] [bit] NOT NULL ,
```
### Fixed
- Fixed bug in connections loading code when using SQL storage
- Fixed bug in reconnect code
- Fixed VNC sessions not refreshing screen automatically when switching between tabs or panels

## 1.45
### Added
- New german language build available
- Added support for RDP 6.1 (XP SP3/Vista SP1) features (Server Authentication, Console Session, TS Gateway not yet...)
- Added basic support for UltraVNC SingleClick (Tools - UltraVNC SingleClick); the listening port is configurable in the options
### Changed
```
WARNING! There have been changes to the connections file/SQL tables
Please always backup your whole config before updating to a new mRemote beta release, especially when there have been changes to the config files/SQL tables
To get SQL working with the new version please update your tables like in the provided script (Info - Help - SQL Configuration)
These are the added lines:
[RDPAuthenticationLevel] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
[InheritRDPAuthenticationLevel] [bit] NOT NULL ,
```
### Fixed
- Fixed VNC connections not working on x64
- Fixed screenshots save all feature overwriting files with the same name (not actually a bug, but rather a new feature ;)
- Fixed ICA Encryption Strength not inheriting properly

## 1.43
### Fixed
- Added function to send either the main form or a connection panel to another screen
  - To send the form to another screen, right click the titlebar or the taskbar button and select "Send to..."
  - If you want to send a panel to another screen, right click the panel's tab and do the same
### Fixed
- Fixed PuTTY connections not supporting credentials with spaces
- Fixed form not opening on last position when using multiple screens
- Fixed kiosk mode not working on secondary screen

## 1.42
### Added
- Added minimize to tray option
- Added option to enable switching to open connections with a single click on the corresponding connection in the tree
- Added button to test proxy settings
### Changed
 - IMPORTANT NEWS: Please read the page that opens when you first start this release or go to: ~~http://www.mRemote.org/wiki/MainPage.ashx#Commercial~~
### Fixed
- Fixed: If the active connection tab is a PuTTY connection, Alt+Tab to mRemote now focuses the PuTTY window
- Fixed encoding problem with PuTTY sessions that included spaces
- Fixed problem that made mRemote inaccesible when closing it on a second monitor and then disabling this monitor
- Fixed: Inheritance defaults of some new VNC properties were not saved in the portable package

## 1.41
### Added
- Added complete support for SmartCode's ViewerX and removed VncSharp
```
Many thx to everyone who donated to make this happen!!! I didn't think that it wouldn't even take a week! =)
I hope everyone will be satisfied by the functions and possibilities this new control provides
If you use one of the non-setup packages you must register the control yourself
Open a cmd and change to the directory you installed mRemote to
Type regsvr32 scvncctrl.dll and click ok
```
### Changed
- Changed shortcuts and added buttons for them to the view menu under "Jump To" because they were causing several problems
```
WARNING! There have been changes to the connections file/SQL tables and the Ext. Apps XML file
Please always backup your whole config before updating to a new mRemote beta release, especially when there have been changes to the config files/SQL tables
To get SQL working with the new version please update your tables like in the provided script (Info - Help - SQL Configuration)
```

## 1.40
### Added
- Added (limited) support for the trial version of SmartCode's VNC ActiveX
```
To enable it go to Options - Advanced and check "Try SmartCode VNC ActiveX"
When connecting a pop up will open, wait about 10 seconds, then click on "Trial" to continue
I will integrate this control fully into mRemote if I get enough Donations to buy the single developer license ($375,-)
So if you want to see better VNC support (All UltraVNC, TightVNC and RealVNC functions) in mRemote, please help me and donate some bucks
For donations either go to the mRemote Wiki (Info - Website) or click on Info - Donate to directly go to PayPal
I will announce the current donation amount every day (or as often as I can) on the Wiki main page
If you want to know more about the control go here: http://www.s-code.com/products/viewerx/
```
- Added feature to choose the panel a connection will open in when...
  1. no panel name was assigned in the properties
  2. you opened a connection with the option to choose the panel before connecting
  3. you checked "Always show panel selection dialog when opening connectins" in Options - Tabs & Panels
- Added Shortcuts to focus the standard panels
  - Alt+C: Switch between Connections & Config panel
  - Alt+S: Switch between Sessions & Screenshots panel
  - Alt+E: Switch to Errors & Infos panel
- Added some new icons

## 1.39
### Added
- Added MagicLibrary.dll to the release again (forgot it in the 1.38 packages, sorry)
- Added auto-reconnect for ICA
- Added feature that automatically clears whitespaces in the Quicky Textfield
- Added special feature: Go to the set password dialog and type "ijustwannaplay" (without the quotes) in the password field... ;)

## 1.38
### Added
- Added automatic reconnect feature for RDP (Options - Advanced)
- Added connections drop-down to the quicky toolbar (same as the tray icon menu)
- Added setting in the options to enable/disable that double clicking on a connection tab closes it
- Added option to automatically set the hostname like the display name when creating new connections
### Fixed
- Fixed bug that caused the properties of a folder to be filled with "Object reference not set to an instance of an object." when adding a folder to the root with Default Inheritance enabled
- Fixed bug that made the properties of a newly added Connection to the root unavailable when Default Inheritance was enabled
- Fixed bug that the default settings for Pre/Post Ext. App, and their inheritance settings were not being saved
- Fixed bug in settings loading methods that caused the application to hang when an error occured
- Fixed bug in Ext. Apps panel that copied the properties of the previously selected Ext. App when "Wait for exit" was checked
- Fixed bug in the SQL Query that creates the tables needed by mRemote
- Attempt to fix the "Drop-Down on Screenshot" bug on some machines

## 1.35
### Added
- Added single instance mode (look in Options - Startup/Exit) - No cmd arguments supported yet!
- Added possibilty to start a Ext. App before connecting and on disconnect (e.g. for VPN/RAS)
- Added option to the Ext. Apps to tell mRemote to wait for the exit of the Ext. App
- Added encryption setting for ICA
### Changed
```
WARNING! There have been changes to the connections file/SQL tables and the Ext. Apps XML file
Please always backup your whole config before updating to a new mRemote beta release, especially when there have been changes to the config files/SQL tables
Here's a list of new columns that need to be created before saving connections to an SQL server:
Name: ICAEncryptionStrength, Data-Type: varchar, Length: 64, Allow Nulls: No
Name: InheritICAEncryptionStrength, Data-Type: bit, Length: 1, Allow Nulls: No
Name: PreExtApp, Date-Type: varchar, Length: 512, Allow Nulls: Yes
Name: PostExtApp, Date-Type: varchar, Length: 512, Allow Nulls: Yes
Name: InheritPreExtApp, Date-Type: bit, Length: 1, Allow Nulls: No
Name: InheritPostExtApp, Date-Type: bit, Length: 1, Allow Nulls: No
```

## 1.33
### Fixed
- Fixed problem that caused RDP connections not to initialize properly when using XP SP3
- Fixed bug in Port Scan that prevented hosts with no hostname from being imported

## 1.32
### Added
- Added: Inheritance defaults can now be customized (look in the root properties of your connections tree)
### Changed
- Changed Target CPU to AnyCPU again as I think the x86 setting caused problems on x64 machines (although it shouldn't)
### Fixed
- Fixed bug that made password-secured connection files not load properly because the return value from the password screen was always null
- Fixed a lot of outdated code in the import functions (Import from XML, Import from AD, Import from RDP files)
- Fixed bug that caused properties with a ' character not to be saved properly when using SQL Server

## 1.31
### Fixed
- Small speed improvement to the port scanner
- Fixed bug that caused SQL live-update to not work when not using AD Authentication
- Fixed bug that caused Save As not to work

## 1.30
### Added
- Added experimental SQL Server with live-update (multi-user) support (see Help - Getting started - SQL Configuration)
- Added bunch of new icons to the UI, most of them by famfamfam.com
- Added dropdown button to Quicky Toolbar to choose protocol
- Many smaller changes and additions
### Fixed
- Fixed: Wrong default PuTTY session name
- Fixed bug in Port Scanner that caused an error when no DNS name could be resolved

## 1.25
### Added
- Added inheritance for folders
- Added port scan feature and possibility to import from a scan
- Added toolbar for Ext. Apps (see View - External Applications Toolbar)
- Added quick connect as toolbar
- Added code that creates a backup of the current connections file every time it is loaded (It's named YourConsFile.xml_BAK)
- Added description variable to Ext. Apps
### Fixed
- Fixed bug that allowed inheriting from root node
- Fixed bug that caused Ext. Apps launched from a connection tab to use the selected tree node instead of the current tab
- Fixed bug that caused mRemote not to save panel layout and Ext. Apps on exit

## 1.24
### Fixed
- Fixed a bug in connections loading mechanism that caused a corrupted connections file when upgrading from a previous version

## 1.23
### Added
- Added feature to remember which connections were opened on last runtime and reconnect to them on the next start (see Tools - Options - Startup/Exit)
- A command line switch is also available to cancel reconnecting (/noreconnect or /norc)
- Added Auto Save feature (Tools - Options - Connections - Auto Save every...)
- Added Ext. Apps to connection tab context menu
- Added better error handling for RDP connection creation
### Fixed
- Fixed problem with Sessions feature on 64bit systems
- Fixed Sessions feature not working when using global credentials
- Fixed several problems with the Active Directory OU picker control
- Fixed bug in Connection duplicate code that caused duplicated connection to still have previous tree node assigned

## 1.20
### Added
- Added External Applications feature (check the help section for more info)
- Added duplicate feature to Connections tree
### Fixed
- Fixed: MagicLibrary.dll was not included in the setup package

## 1.16
### Added
- New Domain: www.mRemote.org
- There's a new setting in the options to fine tune the time to wait until the window has been created
- Added reconnect feature in tab menu
### Fixed
- Fixed PuTTY connections appearing in a new window
- Fixed export not working

## 1.15
### Added
- Added: New portable package
- Added: Defaults for new connections can now be customized
  - Click the root item and then the new Properties-like button with a small yellow star to get to the settings
### Fixed
- Fixed Import from Active Directory not working
- Fixed problem with single click connect not focusing correctly
- Fixed root node not being renamed after changing name in property grid

## 1.10
### Added
- Added support for setting a password to protect the connections file with (look in the root of your connections tree)
- Added RDP file import feature
- Added new command-line switch to reset panel's positions
- Added HTTPS as protocol
- Added HTTP/S basic authentication
- Added support for setting a Proxy server for automatic updates
### Changed
- Some changes in help section
### Fixed
- Fixed the bug that passwords stored in the options weren't decrypted when a connection was opened
- Fixed "bug" that prevented "Connect to console session" from working in RDC6.1 (Vista SP1 RC1/XP SP3 RC1)

## 1.00
Merry Christmas! =)
```
1.00 is a (almost) complete rewrite of the whole application
The code base is now much cleaner and more (easily) extendable
New features include (but are not limited to):
Every part of the application is now integrated into panels which can be moved, docked and undocked, hidden, moved to another monitor, etc.
This makes many new and exciting ways to manage connection and application windows possible
You can for example open up 4 PuTTY sessions in 4 different panels and align them in the main application so you can use all 4 side by side - 2 on the upper side and 2 on the bottom for example
This can be done for EVERY part of the application, it's completely modular and customizable
Connection and folder (previously called containers) properties have moved to a new property grid control
Every setting (with the exclusion of the hostname, which wouldn't make any sense) can now be inherited from the parent folder
Connection file saving/loading is now handled a bit different (more in the help section)
Application restart is no longer nececary after changing options, they are active with a click of the OK button
Smart size can now be activated also if a connection is already open (in the right click menu of the active tab)
A panel name can be stored with every connection (or folder, if inherting) to always open the connection in the specified panel
And last but not least, many bugs have been fixed, though there are probably many new bugs aswell - Did I already mention this is a rewrite? ;)
I hope you like my work and if you do please consider donating on the mRemote website to support me a little. Any amount will do! Thx!
```

## 0.50
### Added
- Added possibility to change resolution or display mode (Fit to window, Fullscreen, Smart size)
- Added new setting in options to show logon info on tab titles
- Added new feature that catches popup dialogs and puts them in a managed interface. This is another step to make mRemote a single window application.
- Added QuickConnect history and auto-complete functions
- Added a few new Icons (Linux, Windows, ESX, Log, Finance)
- Pressing escape switches back to the connection list
- There is a context menu that allows you to copy selected errors/warnings/infos to the clipboard (text only) or to delete them
- There also are settings in the option to change when to switch to the tab and to switch back to the normal behaviour of displaying message popups

### Changed
- Connections file version is now 1.2
### Removed
- Removed old Terminal (SSH, Telnet) control and embedded PuTTY instead
```
This decision brings mostly good but also some bad news
The good news is that now everything that works in putty also works in mRemote
This means X11 forwarding, SSH port forwarding, session logging, appearance customization, etc. should be working fine now
It also brings some new protocols (Rlogin, RAW)
The bad news is that I cannot fully integrate Putty into mRemote because it is a standalone application and thus has it's own window handle
This means that you won't be able to use Ctrl+Tab to switch between tabs, catching errors or infos through the new Errors and Infos tab isn't possible, etc.
```
### Fixed
- Improved options tab
- Fixed some form drawing bugs

## 0.35
### Added
- Added tab switching/closing hotkeys
  - Switch to next tab: Ctrl+Tab
  - Switch to previous tab: Ctrl+Shift+Tab
  - Close active tab: Ctrl+W
  - This does not and will probably never work with RDP connections!
### Changed
- Changed shortcuts to menu items in main menu as they interfered with some terminal key bindings
### Fixed
- Fixed bug in updating code that still displayed the current version in the old format (x.x.x.x instead of x.xx)
- Fixed bug where the colors setting was not correctly read after saving and reloading a connections file (only with 256 colors setting)
- Fixed bug that made connect to console session and fullscreen options not work
- Fixed bug that when opening options, update or about tab caused weird paddings next to the tab or other strange behaviour

## 0.30
### Added
- Added HTTP as protocol to allow for basic web-based administration
- Added new connections menu to the toolbar
  - Left click on a connection connects
  - Right click on a container or connection opens the config tab for the selected item
- Added two new connection context menu entries for quickly connecting to console session or connecting in fullscreen
- The connections tree can now be hidden
  - To hide it right click on the splitter (the divider between the connections tree and the tabbing interface)
### Changed
- Changed "Redirect Key combinations (like in fullscreen)" to be disabled when in kiosk mode as it has no effect then anyway
### Fixed
Improved tray icon menu (just like the main connections menu)
- Several small bugfixes and code improvements
### Removed
- Removed overlay (RDP locking) feature in favor of simply grabbing input when clicking inside the control area
  - I hope nobody is too sad that the nice looking overlay feature had to go, but..., well, it had to! ;-)

## 0.20
### Added
- Added Drag and Drop support for tabs
- Added tab context menu
  - Switch to/from fullscreen
  - Take a screenshot
  - Transfer files via SCP/SFTP (SSH)
  - Send special keys (VNC)
  - Rename tabs
  - Duplicate tabs (Create another instance of the connection)
  - Show config
  - Close tab (disconnect)
- Added middle click support for tabs (close/disconnect)
- Added SSH file transfer (SCP/SFTP) support
- Added Tools menu to the tree context menu
- Transfer files via SCP/SFTP (SSH)
- Import/Export features
- Sorting
### Changed
- Changed version format
### Removed
- Removed Fullscreen and Send special keys buttons from the main toolbar as they are now in the tab context menu
### Fixed
- Fixed the problem that caused mRemote to crash when dragging a parent node of the connections tree onto one of it's child nodes
- Fixed problem in importing mechanism that allowed importing connections including the root which resulted in multiple root items that couldn't be deleted
- Fixed problem with quick connect

## 0.0.9.0
### Added
- Added support for redirecting key combinations (Alt+Tab, Winkey, ...)
- Added Import/Export features
- Added Quick Connect Port support, just type the host you want to connect to followed by a ":" and then the port
- Added Connect/Disconnect buttons to connections context menu
- Added two new icons (Test Server | TST; Build Server | BS)
### Changed
- Many changes to the connections loading/saving mechanisms
- confCons version is now 1.0
### Fixed
- Some code cleanup
- Fixed auto session info to only try to get session information when a RDP connection is selected
- Fixed AD Import feature (didn't care if imported items were computers, groups, users, ... ;)
- Fixed settings and connections not saving when installing updates from the auto-updater
- Fixed form size and location not saving properly when closing the application in minimized state or in maximized state on a secondary monitor

## 0.0.8.2
### Added
- Added SSH1 to Quick Connect GUI
### Changed
- Changed buffer size of terminal control, it's now 500 lines
### Fixed
- Fixed terminal connections not getting focus when changing tabs
- Fixed bug in terminal code that caused hitting "home" to show "~" instead of jumping to the start of the line
- Fixed bug that caused that hitting enter in mRemote wouldn't do anything when options was opened before

## 0.0.8.0
### Added
- Added code to check if the msrdp com control is registered
### Changed
- Many Improvements to the terminal control (ssh1(!), ssh2, telnet)
### Fixed
- Fixed bug that caused mRemote to crash when moving connection into root node (only with inheritance enabled)
- Fixed bug: Pressing delete when editing a node's name caused delete messagebox to show

## 0.0.7.5
### Added
- Added inheritance feature to inherit connection settings from parent container
### Changed
- Expanded/Collapsed state of tree nodes will now be saved
- Reduced auto session info delay to 700ms
- Some code maintainance
- Some corrections to connections tree and quick search behaviour
- Changed connections file version to 0.9
### Fixed
- Fixed bug in TerminalControl that caused the error message "error loading string"
- Fixed: Settings saving on exit was broken in 0.0.7.0, this is fixed now
- Fixed connections context menu bug that made import from ad option inaccessible
- Fixed session info filling up with infos about hosts previously selected

## 0.0.7.0
### Changed
- Massive GUI redesign and changes, hope you like it! =)
### Fixed
- Fixed bug that made session info to query immediately after selecting a connection (when enabled), there is now a one second delay to prevent collecting session info for more than one host

## 0.0.6.8
### Added
- Added connection import feature for Active Directory
### Changed
- Tidied up project references
- Multiple changes to setup routine
### Fixed
- Improved error handling for auto-update
- Improved download handling for auto-update
- Fixed bug that made download finished/failed message box appear multiple times when update was canceled and re-downloaded
- Fixed bug where double-clicking a container opened all connections inside this container

## 0.0.6.6
### Changed
- Changed port textbox control to only allow digits
- Small changes to connection code for SSH
### Fixed
- Fixed port setting not saving (or always displaying default port for selected protocol)

## 0.0.6.5
### Added
- Added auto update feature
### Changed
- Changed: Multiple UI Changes (added shortcuts, rearranged menu items, ...)
### Fixed
- Fixed the problem where the connections file version was saved either with a dot or a comma, depending on system language
- Fixed not being able to connect to SSH2 hosts without specifying username and password
- Fixed several problems with Quick Connect
- Improved saving of config changes
- Fixed connections tab not closing when using SSH

## 0.0.6.0
### Added
- Added new protocols: SSH2 and Telnet
- Added first command line switch/parameter "/consfile"
  - Ex.: mRemote.exe /consfile "%PathToYourConnectionsFile%"
- Added button to screenshots to delete a screenshot
- Added Host Status (Ping) feature
### Changed
- Many code rewrites and changes in almost every area
- Changed the way connections get loaded
- The default path for the connection file is no longer in the application directory but in the local application data folder. 
  - Ex.: c:\Documents and Settings\felix\Local Settings\Application Data\Felix_Deimel\mRemote\
  - If opening a connection file from a custom location (click on open link) saving will also occur in this file and not like in previous versions to the default connections file
  - To import your old connection file please use the following procedure: start mRemote, click on "Open" and find your old connection file. Then click on "Save As" and save it with the default file name to the default location
- Changed the font and style of context menus
- Changed Quick Connect UI
### Fixed
- Fixed connection settings in config tab not saving when clicking another connection before jumping to another config field
- Fixed a bug where renaming a container caused the first connection in the same container to be renamed too

## 0.0.5.0 R2
### Fixed
- Fixed a bug that prevented connections from opening when icon files were assigned in a previous version of mRemote

## 0.0.5.0
### Added
- Added (Global) fullscreen / kiosk feature
- Added redirection settings for disk drives, printers, ports, smart cards and sound
- Added option to write a log file
- Added option to open new tabs on the right side of the currently selected tab
- Added possibility to connect to all nodes in a container
### Changed
- Changed session functions to work in background
- Changed icon choosing mechanism and added a bunch of default icons
- Changed: Containers with connection can now be deleted just like empty containers
- Changed screenshot functions to now collect all screenshots in one tab
- Changed: More settings can now be changed on container basis
- Changed config file version to 0.6
- Changed: Small internal changes to the connection saving/creating and opening mechanisms
### Fixed
- Fixed "Display Wallpapers" and "Display Themes" settings, they are working now

## 0.0.3.6
### Added
- Added Feature to display an overlay when RDP connection tab has lost the focus, clicking on this gives the focus back to the control
- Added standard handlers for F2 (rename) and DEL (delete) keys in the treeview
- Added icon preview for connections in config tab
### Changed
- Changed the way new connections and containers are being created in the treeview. The pop up window will not be displayed any longer, instead everything is handled inplace by the treeview.
- Changed some minor UI related stuff
### Fixed
- Fixed bug in tab closing mechanism that caused icons (play/pause) to not be set on the correct tree nodes

## 0.0.3.5
### Added
- Added Feature to query and log off sessions on a remote machine and option to do this automatically
- Added Option to show icon in system tray with connection menu
### Changed
- Changed controls to flat style as I think this fits the whole application more than the old 3D look
- Multiple UI changes to eliminate annoying behaviour

## 0.0.3.3
### Added
- Added Feature to specify which login information to use when no info is provided in the config of a remote machine
### Fixed
- Fixed bug in Quick Find where trying to open a connection when no node was found caused an error
- Fixed bug where the main form was not rendered correctly when hiding top bar and using XP Themes
- Fixed bug in drag-drop routine that caused application to hang when trying to drop a node on one of it's child nodes
- Fixed bug where taskbar buttons for fullscreen rdp windows did not disappear after disconnecting

## 0.0.3.2
### Added
- Added new Save As Dialog with feature to only save specific connection settings
- Added Option to display Tooltips when hovering over host entries in the connection tree
- Added Option to ask at exit when there are open connections
### Fixed
- Fixed bug where saving connections file with spaces in the root node caused an error -> updated Connection File Version to 0.5
- Fixed bug in options tab where the browse button for a custom connection file didn't do anything

## 0.0.3.0
### Added
- Added Options Tab
  - Load connections file from different location
  - Save/Don't Save connections file on exit
  - Show current tab name in window title
- Added drag and drop functionality to the connections tree
- Added feature to hide top bar
- Added feature to send special keys (VNC)
### Changed
- Updated VncSharp Library to 0.88 (still pretty buggy)

## 0.0.2.7
### Added
- Added feature to save connection settings to all connections in the selected container
### Deprecated
- Disabled "Display Wallpaper" and "Display Themes" checkboxes as these features are not implemented
### Fixed
- Icon choosing bug fixed
- Taskbar button had no text when in fullscreen - fixed
- Fixed bug in Quick Connect GUI

## 0.0.2.5
### Added
- Added new connections toolstrip (same functions as context menu)
### Changed
- Splitter position is now saved on exit
### Fixed
- Quick connect button bug fixed
- Search field resize bug fixed

## 0.0.2.4
### Added
- Added Keep Alive Interval (1 Minute)
- Added Options to choose between RDP & VNC
- Added Port Setting for RDP
- Added Option to connect to console
- Added Menu Entries to move Connections & Containers up & down
### Changed
- Changed default color depth to 16bit
- Some small code improvements

[Unreleased]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.20..HEAD
[1.76.20]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.19..v1.76.20
[1.76.19]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.18..v1.76.19
[1.76.18]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.17..v1.76.18
[1.76.17]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.16..v1.76.17
[1.76.16]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.15..v1.76.16
[1.76.15]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.14..v1.76.15
[1.76.14]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.13..v1.76.14
[1.76.13]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.12...v1.76.13
[1.76.12]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.11..v1.76.12
[1.76.11]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.10..v1.76.11
[1.76.10]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.9..v1.76.10
[1.76.9]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.8..v1.76.9
[1.76.8]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.7..v1.76.8
[1.76.7]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.6..v1.76.7
[1.76.6]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76.5..v1.76.6
[1.76.5]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76Alpha6..v1.76.5
[1.76.4 Alpha 6]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76Alpha5..v1.76Alpha6
[1.76.3 Alpha 5]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76Alpha4..v1.76Alpha5
[1.76.2 Alpha 4]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76Alpha3..v1.76Alpha4
[1.76.1 Alpha 3]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76Alpha2..v1.76Alpha3
[1.76.0 Alpha 2]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.76Alpha1..v1.76Alpha2
[1.76.0 Alpha 1]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75.7012..v1.76Alpha1
[1.75.7012]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75.7011..v1.75.7012
[1.75.7011]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75.7010..v1.75.7011
[1.75.7010]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75.7009..v1.75.7010
[1.75.7009]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Hotfix8..v1.75.7009
[1.75.7008]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Hotfix7..v1.75Hotfix8
[1.75.7007]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Hotfix6..v1.75.Hotfix7
[1.75.7006]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Hotfix5..v1.75Hotfix6
[1.75.7005]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Hotfix4..v1.74Hotfix5
[1.75.7004]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Hotfix3..v1.75Hotifx4
[1.75.7003]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Hotifx2..v1.75Hotifx3
[1.75.7002]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Hotifx1..v1.75Hotfix2
[1.75.7001]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75..v1.75Hotfix1
[1.75]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Beta3..v1.75
[1.75 Beta 3]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Beta2..v1.75Beta3
[1.75 Beta 2]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Beta1..v1.75Beta2
[1.75 Beta 1]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Alpha3..v1.75Beta1
[1.75 Alpha 3]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Alpha2..v1.75Alpha3
[1.75 Alpha 2]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.75Alpha1..v1.75Alpha2
[1.75 Alpha 1]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.74..v1.75Alpha1
[1.74]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.74Beta2..v1.74
[1.73 Beta 2]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.73Beta1..v1.73Beta2
[1.73 Beta 1]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.72..v1.73Beta1
[1.72]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.71..v1.72
[1.71]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.71RC2..v1.71
[1.71 Release Candidate 2]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.71RC1..v1.71RC2
[1.71 Release Candidate 1]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.71Beta5..v1.71RC1
[1.71 Beta 5]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.71Beta4..1.71Beta5
[1.71 Beta 4]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.71Beta3..1.71Beta4
[1.71 Beta 3]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.71Beta2..v1.71Beta3
[1.71 Beta 2]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.71Beta1..v1.71Beta2
[1.71 Beta 1]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.70..v1.71Beta1
[1.70]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.70RC2..v1.70
[1.70 Release Candidate 2]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.70RC1..v1.70RC2
[1.70 Release Candidate 1]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.70Beta2..v1.70RC1
[1.70 Beta 2]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.70Beta1..v1.70Beta2
[1.70 Beta 1]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.69..v1.70Beta1
[1.69]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.68..v1.69
[1.68]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.67..v1.68
[1.67]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.66..v1.67
[1.66]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.65..v1.66
[1.65]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.64..v1.65
[1.64]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.63..v1.64
[1.63]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.62..v1.63
[1.62]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.61..v1.62
[1.61]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.60..v1.61
[1.60]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.50..v1.60
[1.50]: https://github.com/mRemoteNG/mRemoteNG/compare/v1.49..v1.50
