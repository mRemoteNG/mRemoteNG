namespace mRemoteNG.Connection.Protocol
{
    /// <summary>
    /// Signifies that a protocol supports View Only mode. When in View Only mode,
    /// the control will not capture and send input events to the remote host.
    /// </summary>
    public interface ISupportsViewOnly
    {
        /// <summary>
        /// Whether this control is in view only mode.
        /// </summary>
        bool ViewOnly { get; set; }

        /// <summary>
        /// Toggles whether the control will capture and send input events to the remote host.
        /// The local host will continue to receive data from the remote host regardless of this setting.
        /// </summary>
        void ToggleViewOnly();
    }
}