using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Forms.CredentialManager
{
    public class PageWorkflowController
    {
        private readonly Stack<Control> _pageStack;
        private readonly Action<Control> _showNextPageAction;

        /// <summary>
        /// The starting page of this sequence of pages
        /// </summary>
        public Control StartPage { get; }

        public PageWorkflowController(Action<Control> showNextPageAction, Control startPage)
        {
            _showNextPageAction = showNextPageAction.ThrowIfNull(nameof(showNextPageAction));
            StartPage = startPage.ThrowIfNull(nameof(startPage));
            _pageStack = new Stack<Control>();
        }

        public void ShowNextPage(Control page)
        {
            _pageStack.Push(page);
            _showNextPageAction(page);
        }

        public void ShowPreviousPage()
        {
            var previousPage = _pageStack.Count > 0 
                ? _pageStack.Pop()
                : StartPage;

            _showNextPageAction(previousPage);
        }
    }
}
