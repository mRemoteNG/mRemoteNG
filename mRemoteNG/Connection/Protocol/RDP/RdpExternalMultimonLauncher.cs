using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    // True multi-monitor spanning ("Use all my monitors") can't be done reliably by the
    // embedded RDP ActiveX control: a docked child control only goes fullscreen on the single
    // monitor that hosts it. mstsc.exe is a standalone process that creates a real fullscreen
    // window per physical monitor, so for the multimon case we hand off to it via a temporary
    // .rdp file (use multimon:i:1), exactly like the built-in Remote Desktop client's checkbox.
    // ponytail: shell out to mstsc instead of fighting the ActiveX host - it's the only path
    // that spans physical monitors the way the user's screenshot shows.
    [SupportedOSPlatform("windows")]
    public static class RdpExternalMultimonLauncher
    {
        public static void Launch(ConnectionInfo connectionInfo)
        {
            try
            {
                string host = connectionInfo.Hostname;
                int port = connectionInfo.Port;
                string user = connectionInfo.Username ?? "";
                string domain = connectionInfo.Domain ?? "";
                string password = connectionInfo.Password ?? "";

                string rdpFile = WriteRdpFile(connectionInfo, host, port, user, domain);

                // mstsc reads the password from Windows Credential Manager (TERMSRV/<host>),
                // never from the .rdp in clear text. Stage it, then remove it after mstsc exits.
                bool stagedCredential = !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password);
                if (stagedCredential)
                    RunCmdKey("/generic:TERMSRV/" + host, "/user:" + (string.IsNullOrEmpty(domain) ? user : domain + "\\" + user), "/pass:" + password);

                Process mstsc = new()
                {
                    StartInfo = { FileName = "mstsc.exe", UseShellExecute = false }
                };
                mstsc.StartInfo.ArgumentList.Add(rdpFile);
                mstsc.Start();

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"RDP multimon: launched mstsc.exe (/multimon) for '{host}'.", true);

                // Clean up the temp file and staged credential once the external session ends.
                _ = Task.Run(() =>
                {
                    try { mstsc.WaitForExit(); } catch { /* process may already be gone */ }
                    if (stagedCredential)
                        RunCmdKey("/delete:TERMSRV/" + host);
                    try { File.Delete(rdpFile); } catch { /* best effort */ }
                });
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetPropsFailed, ex);
            }
        }

        private static string WriteRdpFile(ConnectionInfo info, string host, int port, string user, string domain)
        {
            string address = port == (int)RdpProtocol.Defaults.Port ? host : host + ":" + port;

            StringBuilder sb = new();
            sb.AppendLine("full address:s:" + address);
            sb.AppendLine("use multimon:i:1");
            sb.AppendLine("screen mode id:i:2"); // 2 = full screen
            sb.AppendLine("session bpp:i:32");
            if (!string.IsNullOrEmpty(user))
                sb.AppendLine("username:s:" + user);
            if (!string.IsNullOrEmpty(domain))
                sb.AppendLine("domain:s:" + domain);
            sb.AppendLine("redirectclipboard:i:" + (info.RedirectClipboard ? 1 : 0));
            sb.AppendLine("redirectprinters:i:" + (info.RedirectPrinters ? 1 : 0));
            sb.AppendLine("audiomode:i:" + (int)info.RedirectSound);
            if (info.RedirectDiskDrives == RDPDiskDrives.All)
                sb.AppendLine("drivestoredirect:s:*");

            string path = Path.Combine(Path.GetTempPath(), "mRemoteNG_multimon_" + Guid.NewGuid().ToString("N") + ".rdp");
            File.WriteAllText(path, sb.ToString());
            return path;
        }

        private static void RunCmdKey(params string[] args)
        {
            Process p = new()
            {
                StartInfo = { FileName = "cmdkey.exe", UseShellExecute = false, CreateNoWindow = true }
            };
            foreach (string a in args)
                p.StartInfo.ArgumentList.Add(a);
            p.Start();
            p.WaitForExit();
        }
    }
}
