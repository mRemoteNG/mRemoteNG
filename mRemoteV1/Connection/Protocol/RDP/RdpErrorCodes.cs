using System;
using System.Collections;
using mRemoteNG.App;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public static class RdpErrorCodes
    {
        private static Hashtable _description;

        private static void InitDescription()
        {
            _description = new Hashtable
            {
                {"0", nameof(Language.strRdpErrorUnknown)},
                {"1", nameof(Language.strRdpErrorCode1)},
                {"2", nameof(Language.strRdpErrorOutOfMemory)},
                {"3", nameof(Language.strRdpErrorWindowCreation)},
                {"4", nameof(Language.strRdpErrorCode2)},
                {"5", nameof(Language.strRdpErrorCode3)},
                {"6", nameof(Language.strRdpErrorCode4)},
                {"7", nameof(Language.strRdpErrorConnection)},
                {"100", nameof(Language.strRdpErrorWinsock)}
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
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpErrorGetFailure, ex);
                return string.Format(Language.strRdpErrorUnknown, id);
            }
        }
    }
}