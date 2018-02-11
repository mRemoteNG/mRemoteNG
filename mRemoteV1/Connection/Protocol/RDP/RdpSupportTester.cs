using System;
using System.Collections.Generic;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpSupportTester
    {
        /// <summary>
        /// Returns a list of the RDP versions that can be used on the current
        /// host.
        /// </summary>
        /// <returns></returns>
        public List<RdpVersionEnum> GetSupportedRdpVersions()
        {
            var supportedVersions = new List<RdpVersionEnum>();
            var rdpFactory = new RdpProtocolFactory();

            foreach (var version in RdpVersionEnum.Rdc6.GetAll())
            {
                var protocol = rdpFactory.CreateProtocol(version);
                if (RdpClientIsSupported(protocol))
                    supportedVersions.Add(version);
            }

            return supportedVersions;
        }

        private bool RdpClientIsSupported(ProtocolBase rdpProtocol)
        {
            try
            {
                rdpProtocol.Initialize();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                rdpProtocol.Close();
            }

            return true;
        }
    }
}
