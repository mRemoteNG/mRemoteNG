using mRemoteNG.App;
using System;
using System.Collections;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public static class RdpErrorCodes
    {
        private static Hashtable _description;

        private static void InitDescription()
        {
            _description = new Hashtable
            {
                {0, "Language.strRdpErrorUnknown"},
                {1, "Language.strRdpErrorCode1"},
                {2, "Language.strRdpErrorOutOfMemory"},
                {3, "Language.strRdpErrorWindowCreation"},
                {4, "Language.strRdpErrorCode2"},
                {5, "Language.strRdpErrorCode3"},
                {6, "Language.strRdpErrorCode4"},
                {7, "Language.strRdpErrorConnection"},
                {100, "Language.strRdpErrorWinsock"}
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