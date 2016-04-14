using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection
{
    public interface ConnectionRecordInheritanceController : Record
    {
        object Parent { get; set; }
        bool IsDefault { get; set; }

        void TurnOnInheritanceCompletely();
        void TurnOffInheritanceCompletely();
    }
}