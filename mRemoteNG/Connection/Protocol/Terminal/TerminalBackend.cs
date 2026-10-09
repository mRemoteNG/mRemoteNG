using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.Terminal
{
    /// <summary>
    /// Selects which terminal backend is used to open an SSH connection inside the
    /// mRemoteNG connection panel. <see cref="PuttyNg"/> keeps the historical embedded
    /// PuTTYNG behaviour, while <see cref="WindowsTerminal"/> embeds the Windows console
    /// backend (<see cref="ProtocolTerminal"/>) that drives the system OpenSSH client.
    /// </summary>
    public enum TerminalBackend
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.TerminalBackendPuttyNg))]
        PuttyNg = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.TerminalBackendWindowsTerminal))]
        WindowsTerminal = 1
    }
}
