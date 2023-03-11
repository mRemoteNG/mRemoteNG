using System;
using System.Collections;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public static class RdpErrorCodes
    {
        private static Hashtable _description;

        private static void InitDescription()
        {
            _description = new Hashtable
            {
                {"0", nameof(Language.RdpErrorUnknown)},
                {"1", nameof(Language.RdpErrorCode1)},
                {"2", nameof(Language.RdpErrorOutOfMemory)},
                {"3", nameof(Language.RdpErrorWindowCreation)},
                {"4", nameof(Language.RdpErrorCode2)},
                {"5", nameof(Language.RdpErrorCode3)},
                {"6", nameof(Language.RdpErrorCode4)},
                {"7", nameof(Language.RdpErrorConnection)},
                {"100", nameof(Language.RdpErrorWinsock)}
            };
        }

        public static string GetError(int id)
        {
            try
            {
                if (_description == null)
                    InitDescription();

                return (string)_description?[id];
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpErrorGetFailure, ex);
                return string.Format(Language.RdpErrorUnknown, id);
            }
        }
    }
}