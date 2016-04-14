using System;
using System.Collections;
using System.Text;
using System.Reflection;

namespace mRemoteNG.Connection
{
    public class RDPProtocolOptionsImp : ConnectionProtocolOptions
    {
        private Protocol.RDPConnectionProtocolImp.RDGatewayUsageMethod _rdGatewayUsageMethod;
        private Protocol.RDPConnectionProtocolImp.AuthenticationLevel _rdpAuthenticationLevel;
        private Protocol.RDPConnectionProtocolImp.RDGatewayUseConnectionCredentials _rdGatewayUseConnectionCredentials;
        private Protocol.RDPConnectionProtocolImp.RDPResolutions _resolution;
        private Protocol.RDPConnectionProtocolImp.RDPSounds _redirectSound;
        private Protocol.RDPConnectionProtocolImp.RDPColors _colors;
        private string _rdGatewayHostname;
        private string _rdGatewayUsername;
        private string _rdGatewayPassword;
        private string _rdGatewayDomain;
        private bool _automaticResize;
        private bool _cacheBitmaps;
        private bool _displayWallpaper;
        private bool _displayThemes;
        private bool _enableFontSmoothing;
        private bool _enableDesktopComposition;
        private bool _redirectKeys;
        private bool _redirectDiskDrives;
        private bool _redirectPrinters;
        private bool _redirectPorts;
        private bool _redirectSmartCards;
        private bool _useConsoleSession;
        private bool _useCredSsp;
        private string _loadBalanceInfo;


        public RDPProtocolOptionsImp()
        {
            this.SetDefaults();   
        }

        public PropertyInfo[] GetSupportedOptions()
        {
            return this.GetType().GetProperties();
        }

        private void SetDefaults()
        {

        }
    }
}
