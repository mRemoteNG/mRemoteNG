.. External Tools - variables

*********
Variables
*********

Variables and arguments can be used to tell the external tool what to do.

This is the list of variables supported by mRemoteNG:

- %NAME%
- %HOSTNAME%
- %PORT%
- %USERNAME%
- %PASSWORD%
- %DOMAIN%
- %DESCRIPTION%
- %MACADDRESS%
- %USERFIELD%

mRemoteNG will also expand environment variables such as %PATH% and %USERPROFILE%. If you need to use an environment
variable with the same name as an mRemoteNG variable, use \\% instead of %. The most common use of this is for the
USERNAME environment variable. %USERNAME% will be expanded to the username set in the currently selected connection.
\\%USERNAME\\% will be expanded to the value set in the USERNAME environment variable.

If you need to send a variable name to a program without mRemoteNG expanding it, use ^% instead of %.
mRemoteNG will remove the caret (^) and leave the rest unchanged.
For example, ^%USERNAME^% will be sent to the program as %USERNAME% and will not be expanded.

Rules for variables
-------------------
- Variables always refer to the currently selected connection.
- Variable names are case-insensitive.
- Variables can be used in both the Filename and Arguments fields.


Special Character Escaping
==========================
Expanded variables will be escaped using the rules below. There are two levels of escaping that are done.

1. Is escaping for standard argument splitting (C/C++ argv, CommandLineToArgvW, etc)
2. Is escaping shell metacharacters for ShellExecute.

Argument splitting escaping
---------------------------

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

Shell metacharacter escaping
----------------------------

- The shell metacharacters are ( ) % ! ^ " < > & |
- Each shell metacharacter will be escaped by a caret (^)

To disable both argument splitting and shell metacharacter escaping for a variable, precede its name with an exclamation point (!).
For example, %!USERFIELD%. This is not recommended and may cause unexpected results.

Only variables that have been expanded will be escaped. It is up to you to escape the rest of the arguments.


Variable Examples
=================

+-------------------+----------------+------------------------+
| Arguments         | User Field     | Result                 |
+===================+================+========================+
| %USERFIELD%       | "Example" Text |                        |
+-------------------+----------------+------------------------+
| %-USERFIELD%      | "Example" Text |                        |
+-------------------+----------------+------------------------+
| %!USERFIELD%      | "Example" Text |                        |
+-------------------+----------------+------------------------+
| ^%USERFIELD^%     | "Example" Text |                        |
+-------------------+----------------+------------------------+
| ^^%USERFIELD^^%   | "Example" Text |                        |
+-------------------+----------------+------------------------+
| -d "%USERFIELD%"  | c:\\Example\\  |                        |
+-------------------+----------------+------------------------+
| -d "%-USERFIELD%" | c:\\Example\\  |                        |
+-------------------+----------------+------------------------+
| -d "%USERFIELD%"  | Left & Right   |                        |
+-------------------+----------------+------------------------+
| -d "%!USERFIELD%" | Left & Right   |                        |
+-------------------+----------------+------------------------+
| %WINDIR%          | N/A            |                        |
+-------------------+----------------+------------------------+
| \\%WINDIR\\%      | N/A            |                        |
+-------------------+----------------+------------------------+
| \\^%WINDIR\\^%    | N/A            |                        |
+-------------------+----------------+------------------------+
| \\%WINDIR\\%      | N/A            |                        |
+-------------------+----------------+------------------------+
