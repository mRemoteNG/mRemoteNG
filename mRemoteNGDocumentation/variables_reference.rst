.. _variables_reference:

*******************
Variables Reference
*******************

Variables (also called parameters) can be used with External Tools to dynamically insert connection properties into commands and arguments.

Available Variables
===================

mRemoteNG supports the following variables:

%NAME%
    The display name of the connection.

%HOSTNAME%
    The hostname or IP address of the connection.

%PORT%
    The port number for the connection.

%USERNAME%
    The username configured for the connection.

%PASSWORD%
    The password configured for the connection.

%DOMAIN%
    The domain configured for the connection.

%DESCRIPTION%
    The description text of the connection.

%MACADDRESS%
    The MAC address configured for the connection.

%USERFIELD%
    The custom user field configured for the connection.

Environment Variables
======================

mRemoteNG will also expand environment variables such as %PATH% and %USERPROFILE%. If you need to use an environment
variable with the same name as an mRemoteNG variable, use \\% instead of %. The most common use of this is for the
USERNAME environment variable. %USERNAME% will be expanded to the username set in the currently selected connection.
\\%USERNAME\\% will be expanded to the value set in the USERNAME environment variable.

Preventing Variable Expansion
==============================

If you need to send a variable name to a program without mRemoteNG expanding it, use ^% instead of %.
mRemoteNG will remove the caret (^) and leave the rest unchanged.
For example, ^%USERNAME^% will be sent to the program as %USERNAME% and will not be expanded.

Rules for Variables
===================

- Variables always refer to the currently selected connection.
- Variable names are case-insensitive.
- Variables can be used in both the Filename and Arguments fields in External Tools.

Special Character Escaping
===========================

Expanded variables will be escaped using the rules below. There are two levels of escaping that are done:

1. Escaping for standard argument splitting (C/C++ argv, CommandLineToArgvW, etc)
2. Escaping shell metacharacters for ShellExecute.

Argument Splitting Escaping
----------------------------

- Each quotation mark will be escaped by a backslash
- One or more backslashes (\\) followed by a quotation mark ("):
   - Each backslash will be escaped by another backslash
   - The quotation mark will be escaped by a backslash
      - If the connection's user field contains ``"This"`` is a ``\"test\"``
      - Then %USERFIELD% is replaced with ``\"This\"`` is a ``\\\"test\\\"``
- A variable name followed by a quotation mark (for example, %USERFIELD%") with a value ending in one or more backslashes:
   - Each backslash will be escaped by another backslash
   - Example:
      - If the connection's user field contains ``c:\Example\``
      - Then "%USERFIELD%" is replaced with ``"c:\Example\\"``

To disable argument splitting escaping for a variable, precede its name with a minus (-) sign. For example: %-USERFIELD%

Shell Metacharacter Escaping
-----------------------------

- The shell metacharacters are ( ) % ! ^ " < > & |
- Each shell metacharacter will be escaped by a caret (^)

To disable both argument splitting and shell metacharacter escaping for a variable, precede its name with an exclamation point (!).
For example, %!USERFIELD%. This is not recommended and may cause unexpected results.

Only variables that have been expanded will be escaped. It is up to you to escape the rest of the arguments.

See Also
========

- :ref:`external_tools` - For information on using variables with External Tools
