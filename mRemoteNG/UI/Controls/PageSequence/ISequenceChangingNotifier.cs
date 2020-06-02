using System;

namespace mRemoteNG.UI.Controls.PageSequence
{
    public interface ISequenceChangingNotifier
    {
        event EventHandler Next;
        event EventHandler Previous;
        event SequencedPageReplcementRequestHandler PageReplacementRequested;
    }
}