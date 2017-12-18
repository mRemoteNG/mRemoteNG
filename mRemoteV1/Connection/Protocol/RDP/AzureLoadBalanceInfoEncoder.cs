using System.Text;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class AzureLoadBalanceInfoEncoder
    {
        public string Encode(string loadBalanceInfo)
        {
            // The ActiveX component requires a UTF-8 encoded string, but .NET uses
            // UTF-16 encoded strings by default.  The following code converts
            // the UTF-16 encoded string so that the byte-representation of the
            // LoadBalanceInfo string object will "appear" as UTF-8 to the Active component.
            // Furthermore, since the final string still has to be shoehorned into
            // a UTF-16 encoded string, I pad an extra space in case the number of
            // bytes would be odd, in order to prevent the byte conversion from
            // mangling the string at the end.  The space is ignored by the RDP
            // protocol as long as it is inserted at the end.
            // Finally, it is required that the LoadBalanceInfo setting is postfixed
            // with \r\n in order to work properly.  Note also that \r\n MUST be
            // the last two characters, so the space padding has to be inserted first.
            // The following code has been tested with Windows Azure connections
            // only - I am aware there are other types of RDP connections that
            // require the LoadBalanceInfo parameter which I have not tested
            // (e.g., Multi-Server Terminal Services Gateway), that may or may not
            // work properly.
            //
            // Sources:
            //  1. http://stackoverflow.com/questions/13536267/how-to-connect-to-azure-vm-with-remote-desktop-activex
            //  2. http://social.technet.microsoft.com/Forums/windowsserver/en-US/e68d4e9a-1c8a-4e55-83b3-e3b726ff5346/issue-with-using-advancedsettings2loadbalanceinfo
            //  3. Manual comparison of raw packets between Windows RDP client and Terminals using WireShark.
            // Copied from https://github.com/OliverKohlDSc/Terminals/blob/master/Terminals/Connections/RDPConnection.cs
            if (loadBalanceInfo.Length % 2 == 1)
                loadBalanceInfo += " ";

            loadBalanceInfo += "\r\n";
            var bytes = Encoding.UTF8.GetBytes(loadBalanceInfo);
            return Encoding.Unicode.GetString(bytes);
        }
    }
}
