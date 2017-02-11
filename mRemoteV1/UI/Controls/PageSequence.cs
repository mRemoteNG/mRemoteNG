using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    public class PageSequence
    {
        private readonly Control _pageContainer;
        private int _currentPageIndex;

        public IList<Control> Pages { get; set; }

        public PageSequence(Control pageContainer) : this(pageContainer, new List<Control>())
        {
        }

        public PageSequence(Control pageContainer, IList<Control> pages)
        {
            if (pageContainer == null)
                throw new ArgumentNullException(nameof(pageContainer));
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            _pageContainer = pageContainer;
            Pages = pages;
        }

        public void Next()
        {
            if (_currentPageIndex == Pages.Count-1) return;
            _currentPageIndex++;
            ActivatePage(_currentPageIndex);
        }

        public void Previous()
        {
            if (_currentPageIndex == 0) return;
            _currentPageIndex--;
            ActivatePage(_currentPageIndex);
        }

        public void ReplaceNextPage(Control newPage)
        {
            Pages[_currentPageIndex + 1] = newPage;
        }

        private void ActivatePage(int sequenceNumber)
        {
            _pageContainer.Controls.Clear();
            _pageContainer.Controls.Add(Pages[sequenceNumber]);
        }
    }
}