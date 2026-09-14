namespace mRemoteNG.Plugins.PortScan;

internal sealed class PortScanHostResult
{
    public required string HostIp { get; init; }

    public string HostName { get; set; } = string.Empty;

    public bool Http { get; set; }

    public bool Https { get; set; }

    public List<int> ClosedPorts { get; } = [];

    public List<int> OpenPorts { get; } = [];

    public bool Rdp { get; set; }

    public bool Rlogin { get; set; }

    public bool Ssh { get; set; }

    public bool Telnet { get; set; }

    public bool Vnc { get; set; }

    public string ClosedPortsDisplay => string.Join(", ", ClosedPorts);

    public string HostDisplayName => string.IsNullOrWhiteSpace(HostName) ? HostIp : HostName;

    public string HostNameWithoutDomain
    {
        get
        {
            if (string.IsNullOrWhiteSpace(HostName) || string.Equals(HostName, HostIp, StringComparison.OrdinalIgnoreCase))
            {
                return HostIp;
            }

            return HostName.Split('.')[0];
        }
    }

    public string OpenPortsDisplay => string.Join(", ", OpenPorts);

    public string HttpDisplay => ToDisplay(Http);

    public string HttpsDisplay => ToDisplay(Https);

    public string RdpDisplay => ToDisplay(Rdp);

    public string RloginDisplay => ToDisplay(Rlogin);

    public string SshDisplay => ToDisplay(Ssh);

    public string TelnetDisplay => ToDisplay(Telnet);

    public string VncDisplay => ToDisplay(Vnc);

    private static string ToDisplay(bool value)
    {
        return value ? "Yes" : "No";
    }
}
