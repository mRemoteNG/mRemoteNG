using System;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.PageSequence
{
    public class SequencedControl : UserControl, ISequenceChangingNotifier
    {
        public event EventHandler Next;
        public event EventHandler Previous;
        public event SequencedPageReplcementRequestHandler PageReplacementRequested;

        public SequencedControl()
        {
            Themes.ThemeManager.getInstance().ThemeChanged += ApplyTheme;
        }

        protected virtual void RaiseNextPageEvent()
        {
            Next?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void ApplyTheme()
        {
            if (!Themes.ThemeManager.getInstance().ThemingActive) return;
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