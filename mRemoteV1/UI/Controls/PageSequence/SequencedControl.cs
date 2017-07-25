using System;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.PageSequence
{
    public class SequencedControl : UserControl, ISequenceChangingNotifier
    {
        public event EventHandler Next;
        public event EventHandler Previous;
        public event SequencedPageReplcementRequestHandler PageReplacementRequested;

        protected virtual void RaiseNextPageEvent()
        {
            Next?.Invoke(this, EventArgs.Empty);
        }

        public virtual void ApplyTheme()
        {
            BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        protected virtual void RaisePreviousPageEvent()
        {
            Previous?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaisePageReplacementEvent(SequencedControl control, RelativePagePosition pagetoReplace)
        {
            PageReplacementRequested?.Invoke(this, new SequencedPageReplcementRequestArgs(control, pagetoReplace));
        }
    }
}