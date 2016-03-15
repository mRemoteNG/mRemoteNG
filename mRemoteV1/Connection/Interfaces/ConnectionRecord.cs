using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using mRemoteNG.Credential;

namespace mRemoteNG.Connection
{
    public interface ConnectionRecord : Record
    {
        string Name { get; set; }
        string Description { get; set; }
        string ConstantID { get; set; }
        ConnectionRecord Parent { get; set; }
        ConnectionRecordMetaData MetaData { get; }
        ConnectionRecordInheritanceController Inherit { get; }
        ConnectionProtocol Protocol { get; set; }
        CredentialRecord Credential { get; set; }
        ExternalToolRecord ExternalTool { get; set; }

        void Connect();
        void Disconnect();
        void Reconnect();
    }
}