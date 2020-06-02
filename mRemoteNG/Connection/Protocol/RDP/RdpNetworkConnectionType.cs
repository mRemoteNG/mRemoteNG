namespace mRemoteNG.Connection.Protocol.RDP
{
    /// <summary>
    /// Indicates the speed of the connection between the
    /// client and remote machine.
    /// </summary>
    public enum RdpNetworkConnectionType
    {
        /// <summary>
        /// Modem (56 Kbps)
        /// </summary>
        Modem = 1,

        /// <summary>
        /// Low-speed broadband (256 Kbps to 2 Mbps)
        /// </summary>
        BroadbandLow = 2,

        /// <summary>
        /// Satellite (2 Mbps to 16 Mbps, with high latency)
        /// </summary>
        Satellite = 3,

        /// <summary>
        /// High-speed broadband (2 Mbps to 10 Mbps)
        /// </summary>
        BroadbandHigh = 4,

        /// <summary>
        /// Wide area network (WAN) (10 Mbps or higher, with high latency)
        /// </summary>
        Wan = 5,

        /// <summary>
        /// Local area network (LAN) (10 Mbps or higher)
        /// </summary>
        Lan = 6,

        /// <summary>
        /// Automatically detect the connection type. Warning: setting
        /// this will prevent the client from setting several performance
        /// options such as displaying wallpaper and remote cursors.
        /// </summary>
        AutoDetect = 7
    }
}
