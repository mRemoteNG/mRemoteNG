using System;

namespace mRemoteNG.UI.Controls.PageSequence
{
    public delegate void SequencedPageReplcementRequestHandler(object sender, SequencedPageReplcementRequestArgs args);

    public enum RelativePagePosition
    {
        PreviousPage,
        CurrentPage,
        NextPage
    }

    public class SequencedPageReplcementRequestArgs
    {
        public SequencedControl NewControl { get; }
        public RelativePagePosition PagePosition { get; }

        public SequencedPageReplcementRequestArgs(SequencedControl newControl, RelativePagePosition pageToReplace)
        {
            if (newControl == null)
                throw new ArgumentNullException(nameof(newControl));
            NewControl = newControl;
            PagePosition = pageToReplace;
        }
    }
}