using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using mRemoteNG.Credential;

namespace mRemoteNG.Connection
{
    public interface ConnectionRecord : Record, Connectable
    {
        string Name { get; set; }
        string Description { get; set; }
        string ConstantID { get; set; }
        ConnectionRecord Parent { get; set; }
        ConnectionRecordMetaData MetaData { get; }
        ConnectionRecordInheritanceController Inherit { get; }
        ConnectionProtocol Protocol { get; set; }
        CredentialRecord Credential { get; set; }

        //ExternalToolRecord ExternalTool { get; set; }
        string PreExtApp { get; set; }
        string PostExtApp { get; set; }
        string MacAddress { get; set; }
        string UserField { get; set; }

        
    }
}