using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mRemoteNG.Connection.Protocol.ICA
{
    public static class ICAClientFactory
    {

        public static IICAClient CreateClientInstance()
        {
#if (DEBUG && !DISABLE_ICA_PROTOCOL) || !DEBUG
            return new ICAClient();
#else
            return new DummyICAClient();
#endif

        }
    }
}
