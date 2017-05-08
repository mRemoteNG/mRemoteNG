using System;
using System.Collections;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Messages;


namespace mRemoteNG.Tools
{
    public class ScanHost
    {
        #region Properties
        public static int SshPort { get; set; } = (int)ProtocolSSH1.Defaults.Port;
        public static int TelnetPort { get; set; } = (int)ProtocolTelnet.Defaults.Port;
        public static int HttpPort { get; set; } = (int)ProtocolHTTP.Defaults.Port;
        public static int HttpsPort { get; set; } = (int)ProtocolHTTPS.Defaults.Port;
        public static int RloginPort { get; set; } = (int)ProtocolRlogin.Defaults.Port;
        public static int RdpPort { get; set; } = (int)RdpProtocol.Defaults.Port;
        public static int VncPort { get; set; } = (int)ProtocolVNC.Defaults.Port;
        public ArrayList OpenPorts { get; set; }
        public ArrayList ClosedPorts { get; set; }
        public bool Rdp { get; set; }
        public bool Vnc { get; set; }
        public bool Ssh { get; set; }
        public bool Telnet { get; set; }
        public bool Rlogin { get; set; }
        public bool Http { get; set; }
        public bool Https { get; set; }
        public string HostIp { get; set; }
        public string HostName { get; set; } = "";
        public string HostNameWithoutDomain
        {
            get
            {
                if (string.IsNullOrEmpty(HostName) || HostName == HostIp)
                {
                    return HostIp;
                }
                return HostName.Split('.')[0];
            }
        }
        #endregion

        #region Methods
        public ScanHost(string host)
        {
            HostIp = host;
            OpenPorts = new ArrayList();
            ClosedPorts = new ArrayList();
        }

        public override string ToString()
        {
            try
            {
                return "SSH: " + Convert.ToString(Ssh) + " Telnet: " + Convert.ToString(Telnet) + " HTTP: " + Convert.ToString(Http) + " HTTPS: " + Convert.ToString(Https) + " Rlogin: " + Convert.ToString(Rlogin) + " RDP: " + Convert.ToString(Rdp) + " VNC: " + Convert.ToString(Vnc);
            }
            catch (Exception)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "ToString failed (Tools.PortScan)", true);
                return "";
            }
        }

        public ListViewItem ToListViewItem()
        {
            try
            {
                var listViewItem = new ListViewItem
                {
                    Tag = this,
                    Text = !string.IsNullOrEmpty(HostName) ? HostName : HostIp
                };

                listViewItem.SubItems.Add(BoolToYesNo(Ssh));
                listViewItem.SubItems.Add(BoolToYesNo(Telnet));
                listViewItem.SubItems.Add(BoolToYesNo(Http));
                listViewItem.SubItems.Add(BoolToYesNo(Https));
                listViewItem.SubItems.Add(BoolToYesNo(Rlogin));
                listViewItem.SubItems.Add(BoolToYesNo(Rdp));
                listViewItem.SubItems.Add(BoolToYesNo(Vnc));

                var strOpen = "";
                var strClosed = "";

                foreach (int p in OpenPorts)
                {
                    strOpen += p + ", ";
                }

                foreach (int p in ClosedPorts)
                {
                    strClosed += p + ", ";
                }

                listViewItem.SubItems.Add(strOpen.Substring(0, strOpen.Length > 0 ? strOpen.Length - 2 : strOpen.Length));
                listViewItem.SubItems.Add(strClosed.Substring(0, strClosed.Length > 0 ? strClosed.Length - 2 : strClosed.Length));

                return listViewItem;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Tools.PortScan.ToListViewItem() failed.", ex);
                return null;
            }
        }

        private static string BoolToYesNo(bool value)
        {
            return value ? Language.strYes : Language.strNo;
        }

        public void SetAllProtocols(bool value)
        {
            Vnc = value;
            Telnet = value;
            Ssh = value;
            Rlogin = value;
            Rdp = value;
            Https = value;
            Http = value;
        }
        #endregion
    }
}