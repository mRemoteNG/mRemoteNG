namespace mRemoteNG.Connection
{
    /// <summary>
    /// Determines which address field is used as the primary connection target.
    /// </summary>
    public enum ConnectionAddressPrimary
    {
        /// <summary>Use the Hostname field (default, backward-compatible).</summary>
        Hostname = 0,

        /// <summary>Use the dedicated IP Address field instead of Hostname.</summary>
        IPAddress = 1
    }
}
