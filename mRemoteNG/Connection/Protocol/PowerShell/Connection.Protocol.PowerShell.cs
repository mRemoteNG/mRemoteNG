using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.PowerShell
{
    [SupportedOSPlatform("windows")]
    public class ProtocolPowerShell : ProtocolBase
    {
        #region Private Fields

        private IntPtr _handle;
        private readonly ConnectionInfo _connectionInfo;
        private ConsoleControl.ConsoleControl _consoleControl;

        public ProtocolPowerShell(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        #endregion

        #region Public Methods

        public override bool Connect()
        {
            try
            {
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, "Attempting to start remote PowerShell session.", true);

                _consoleControl = new ConsoleControl.ConsoleControl
                {
                    Dock = DockStyle.Fill,
                    BackColor = ColorTranslator.FromHtml("#012456"),
                    ForeColor = Color.White,
                    IsInputEnabled = true,
                    Padding = new Padding(0, 20, 0, 0)
                };

                /*
                 * Prepair powershell script parameter and create script
                 */
                // Path to the Windows PowerShell executable; can be configured through options.
                string psExe = @"C:\Windows\system32\WindowsPowerShell\v1.0\PowerShell.exe";

                // Maximum number of login attempts; can be configured through options.
                int psLoginAttempts = 3;

                string psUsername;
                if (string.IsNullOrEmpty(_connectionInfo.Domain))
                    // Set the username without domain 
                    psUsername = _connectionInfo.Username;
                else
                    // Set the username to Domain\Username if Domain is not empty
                    psUsername = $"{_connectionInfo.Domain}\\{_connectionInfo.Username}";

                /* 
                 * The PowerShell script is designed to facilitate multiple login attempts to a remote host using user-provided credentials,
                 * with an option to specify the maximum number of attempts.
                 * It handles username and password entry, attempts to establish a PSSession, and reports on login outcomes, ensuring a graceful exit in case of repeated failures.
                 */
                string psScriptBlock = $@"
                    [CmdletBinding()]
                    param (
                        [Parameter(Mandatory=$true)]
                        [String] $Hostname,         # The hostname you want to connect to (mandatory parameter)
                        [String] $Username,         # The username, if provided
                        [String] $Password,         # The password for authentication
                        [int]    $LoginAttempts = 3 # The number of login attempts, default set to 3
                    )

                    # Dynamically parameters
                    DynamicParam {{
                        $RuntimeParameterDictionary = New-Object System.Management.Automation.RuntimeDefinedParameterDictionary;

                        # SecurePassword
                        $ParameterName = 'SecurePassword';
                        $AttributeCollection = New-Object System.Collections.ObjectModel.Collection[System.Attribute];
                        $ParameterAttribute = New-Object System.Management.Automation.ParameterAttribute;
                        $ParameterAttribute.Mandatory = $False;
                        $AttributeCollection.Add($ParameterAttribute);
                        try {{
                            # Try converting the stored password to a secure string
                            $PSBoundParameters.$($ParameterName) = ConvertTo-SecureString $Password -AsPlainText -Force -ErrorAction Stop;
                        }}
                        catch{{
                            # Create an empty SecureString if the password cannot be converted (if the password is empty)
                            $PSBoundParameters.$($ParameterName) = [SecureString]::new();
                        }}
                        $PSBoundParameters.Password = $null;
                        $RuntimeParameter = New-Object System.Management.Automation.RuntimeDefinedParameter($ParameterName, [SecureString], $AttributeCollection);
                        $RuntimeParameterDictionary.Add($ParameterName, $RuntimeParameter);

                        return $RuntimeParameterDictionary;
                    }}

                    process {{
                        # Initialize the $cred variable
                        $cred = $null;

                        # Check if a username is provided. 
                        #   Please note that some logins may not require a password. Therefore, the first attempt can fail if a username is set and a password is not.
                        if (-not [string]::IsNullOrEmpty($PSBoundParameters.Username)) {{
                            # Create a PSCredential object with the provided username and password
                            $cred = New-Object System.Management.Automation.PSCredential ($PSBoundParameters.Username, $PSBoundParameters.SecurePassword);

                            # It will be needed to determine whether the login credentials were provided or not.
                            $providedCred = $true;
                        }}

                        # At least one login attempt is required to ensure functionality
                        if ($LoginAttempts -lt 0) {{$LoginAttempts = 1;}}

                        # Loop for connection attempts for $LoginAttempts
                        for ($i = 0; $i -lt $LoginAttempts; $i++) {{
                            <#
                                The cases for when 'Get-Credential' is needed:
                                1. `$i -gt 0`: Indicates the first login attempt has failed.
                                2. `-not $cred`: Implies that no credentials have been sent to the function.
                                3. `$cred -and $cred.UserName -match ""^([^\\]+\\)$""`: Implies that only the regular Windows domain name is parameterized.

                                NOTE:
                                If the regular expression is used in an if statement such as if (.... -match ""^[^\\]+\\$"")...,
                                there will be conversion problems with the string. This can then lead to errors when executing PowerShell.

                                To work around this problem, create the $regex variable and enclose the expression in single quotes.
                                Due to the use of variables, double quotes are no longer required in the if statement, and it can be written as follows: if (.... -match $Regex)....
                                This approach avoids possible string conversion problems caused by double quotes.
                            #>
                            [string] $regex = '^[^\\]+\\$'
                            if ($i -gt 0 -or (-not $cred) -or ($cred -and $cred.Username -match $regex)){{
                                # Prompt for credentials with a message and pre-fill username if available
                                try {{
                                    if (-not [string]::IsNullOrEmpty($cred.UserName)) {{
                                        $cred = Get-Credential -Message $Hostname -UserName $cred.UserName -ErrorAction Stop;
                                    }}
                                    else {{
                                        $cred = Get-Credential -Message $Hostname -ErrorAction Stop;
                                    }}

                                    $providedCred = $false; # provided creds are overwritten
                                }}
                                catch {{
                                    # If something is wrong for $cred
                                    $cred = $null
                                }}
                            }}

                            # Try PSSession
                            try {{
                                # If credentials are not provided, abort the loop (mean Get-Credential is canceled)
                                if ( $cred ) {{
                                    Enter-PSSession -ComputerName $Hostname -Credential $cred -ErrorAction Stop;
                                    break;  # Successfully entered PSSession, exit the loop
                                }}
                                else {{
                                    write-Host '{Language.PsCanceled}';
                                    exit;
                                }}
                            }}
                            # Handle the case when PSSession entry fails
                            catch [System.Management.Automation.Remoting.PSRemotingTransportException]{{
                                If (-not $providedCred) {{
                                    Write-Host '{Language.PsConnectionFailed}';
                                    Write-Host;
                                }}
                                else {{
                                    $LoginAttempts++;
                                }}
                            }}
                            catch {{
                                # Handle other exceptions
                                Write-Host $_.Exception.Message;
                                Write-Host;
                                Write-Host '{Language.PsFailed}';
                                exit;
                            }}
                        }}

                        # Maximum login attempts reached
                        if ($i -ge $LoginAttempts) {{
                            Write-Host '{Language.PsLoginAttempts}';
                            exit;
                        }}
                    }}
                ";

                // Setup process for script with arguments
                //* The -NoProfile parameter would be a valuable addition but should be able to be deactivated.
                _consoleControl.StartProcess(psExe, $@"-NoExit -Command ""& {{ {psScriptBlock} }}"" -Hostname ""'{_connectionInfo.Hostname}'"" -Username ""'{psUsername}'"" -Password ""'{_connectionInfo.Password}'"" -LoginAttempts {psLoginAttempts}");
                
                while (!_consoleControl.IsHandleCreated) break;
                _handle = _consoleControl.Handle;
                NativeMethods.SetParent(_handle, InterfaceControl.Handle);
                
                Resize(this, new EventArgs());
                base.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(Language.ConnectionFailed, ex);
                return false;
            }
        }

        public override void Focus()
        {
            try
            {
                NativeMethods.SetForegroundWindow(_handle);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.IntAppFocusFailed, ex);
            }
        }

        protected override void Resize(object sender, EventArgs e)
        {
            try
            {
                if (InterfaceControl.Size == Size.Empty) return;
                NativeMethods.MoveWindow(_handle, -SystemInformation.FrameBorderSize.Width,
                                         -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height),
                                         InterfaceControl.Width + SystemInformation.FrameBorderSize.Width * 2,
                                         InterfaceControl.Height + SystemInformation.CaptionHeight +
                                         SystemInformation.FrameBorderSize.Height * 2, true);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.IntAppResizeFailed, ex);
            }
        }

        #endregion

        #region Enumerations

        public enum Defaults
        {
            Port = 5985
        }

        #endregion
    }
}
