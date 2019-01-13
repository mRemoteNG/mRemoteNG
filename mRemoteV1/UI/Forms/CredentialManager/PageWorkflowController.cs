using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Forms.CredentialManager
{
    public class PageWorkflowController
    {
        private readonly Stack<Control> _pageStack;
        private readonly Action<Control> _nextPage;

        /// <summary>
        /// The starting page of this sequence of pages
        /// </summary>
        public Control StartPage { get; }

        public PageWorkflowController(Action<Control> nextPage, Control startPage)
        {
            _nextPage = nextPage.ThrowIfNull(nameof(nextPage));
            StartPage = startPage.ThrowIfNull(nameof(startPage));
            _pageStack = new Stack<Control>();
        }

        public void NextPage(Control page)
        {
            _pageStack.Push(page);
            _nextPage(page);
        }

        public void PreviousPage()
        {
            var previousPage = _pageStack.Count > 0 
                ? _pageStack.Pop()
                : StartPage;

            _nextPage(previousPage);
        }
    }
}
