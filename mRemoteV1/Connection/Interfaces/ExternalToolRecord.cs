using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection
{
    public interface ExternalToolRecord
    {
        string PreExtApp { get; set; }
        string PostExtApp { get; set; }
        string MacAddress { get; set; }
        string UserField { get; set; }
    }
}
