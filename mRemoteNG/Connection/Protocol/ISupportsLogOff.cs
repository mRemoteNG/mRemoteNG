using System.Threading.Tasks;

namespace mRemoteNG.Connection.Protocol
{
    /// <summary>
    /// Signifies that a protocol can log the user off the remote computer: it ends the user's
    /// session there, not just this connection to it. Closing an RDP tab only disconnects, and
    /// the session keeps running on the server until someone logs it off.
    /// </summary>
    public interface ISupportsLogOff
    {
        /// <summary>
        /// Whether log off can be attempted right now, e.g. the session is connected.
        /// </summary>
        bool CanLogOff { get; }

        /// <summary>
        /// Logs the user off. Implementations type the log off command into the session the way a
        /// user would, so it only works while the session accepts keystrokes: not while it is locked.
        /// </summary>
        Task LogOffAsync();
    }
}
