using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public static class RdpErrorCodes
    {
        private static Hashtable? _description;

        private static void InitDescription()
        {
            _description = new Hashtable
            {
                {1, Language.RdpErrorCode1},
                {2, Language.RdpErrorOutOfMemory},
                {3, Language.RdpErrorWindowCreation},
                {4, Language.RdpErrorCode2},
                {5, Language.RdpErrorCode3},
                {6, Language.RdpErrorCode4},
                {7, Language.RdpErrorConnection},
                {100, Language.RdpErrorWinsock},
                {3334, Language.RdpError3334}
            };
        }

        public static string GetError(int id, string hostname = "")
        {
            try
            {
                if (_description == null)
                    InitDescription();

                return (string?)_description?[id] ?? string.Format(CultureInfo.InvariantCulture, Language.RdpErrorUnknown, hostname, id);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpErrorGetFailure, ex);
                return string.Format(CultureInfo.InvariantCulture, Language.RdpErrorUnknown, hostname, id);
            }
        }
    }
}