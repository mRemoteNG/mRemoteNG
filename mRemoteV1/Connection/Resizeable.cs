using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection
{
    public interface Resizeable
    {
        void Resize(System.Object sender, EventArgs e);
        void ResizeBegin(System.Object sender, EventArgs e);
        void ResizeEnd(System.Object sender, EventArgs e)
    }
}
