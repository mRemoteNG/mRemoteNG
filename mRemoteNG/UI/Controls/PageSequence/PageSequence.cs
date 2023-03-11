using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.PageSequence
{
    [SupportedOSPlatform("windows")]
    public class PageSequence : IDisposable
    {
        private readonly Control _pageContainer;
        private readonly IList<SequencedControl> _pages = new List<SequencedControl>();

        public IEnumerable<SequencedControl> Pages => _pages;
        public int CurrentPageIndex { get; private set; }

        public PageSequence(Control pageContainer, IEnumerable<SequencedControl> pages) : this(pageContainer, pages.ToArray())
        {
        }

        public PageSequence(Control pageContainer, params SequencedControl[] pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            _pageContainer = pageContainer ?? throw new ArgumentNullException(nameof(pageContainer));
            foreach (var page in pages)
            {
                SubscribeToPageEvents(page);
                _pages.Add(page);
            }
        }

        public virtual void NextPage()
        {
            CurrentPageIndex++;
            ActivatePage(CurrentPageIndex);
            if (CurrentPageIndex == _pages.Count - 1)
                Dispose();
        }

        public virtual void PreviousPage()
        {
            CurrentPageIndex--;
            ActivatePage(CurrentPageIndex);
            if (CurrentPageIndex == 0)
                Dispose();
        }

        public virtual void ReplacePage(SequencedControl newPage, RelativePagePosition pageToReplace)
        {
            var indexModifier = 0;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (pageToReplace)
            {
                case RelativePagePosition.PreviousPage:
                    indexModifier--;
                    break;
                case RelativePagePosition.NextPage:
                    indexModifier++;
                    break;
            }

            var pageIndexToReplace = CurrentPageIndex + indexModifier;
            UnsubscribeFromPageEvents(_pages[pageIndexToReplace]);
            SubscribeToPageEvents(newPage);
            _pages[pageIndexToReplace] = newPage;
        }

        private void ActivatePage(int sequenceNumber)
        {
            _pageContainer.Controls.Clear();
            _pageContainer.Controls.Add(_pages[sequenceNumber]);
        }

        private void SubscribeToPageEvents(ISequenceChangingNotifier page)
        {
            if (_pages.Contains(page)) return;
            page.Next += PageOnNext;
            page.Previous += PageOnPrevious;
            page.PageReplacementRequested += PageOnPageReplacementRequested;
        }

        private void UnsubscribeFromPageEvents(ISequenceChangingNotifier page)
        {
            if (!_pages.Contains(page)) return;
            page.Next -= PageOnNext;
            page.Previous -= PageOnPrevious;
            page.PageReplacementRequested -= PageOnPageReplacementRequested;
        }

        private void PageOnNext(object sender, EventArgs eventArgs)
        {
            NextPage();
        }

        private void PageOnPrevious(object sender, EventArgs eventArgs)
        {
            PreviousPage();
        }

        private void PageOnPageReplacementRequested(object sender, SequencedPageReplcementRequestArgs args)
        {
            ReplacePage(args.NewControl, args.PagePosition);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            foreach (var page in _pages)
            {
                UnsubscribeFromPageEvents(page);
            }
        }
    }
}