using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection
{
    public interface ConnectionRecordMetaData : ICloneable
    {
        bool IsContainer { get; set; }
        int PositionID { get; set; }
        bool IsDefault { get; set; }
        bool IsQuickConnect { get; set; }
        bool PleaseConnect { get; set; }
    }
}