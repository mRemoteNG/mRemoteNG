using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManager
{
    public sealed partial class CredentialRepositoryTypeSelectionPage : SequencedControl
    {
        private readonly ICredentialRepositoryList _repositoryList;
        private readonly PageWorkflowController _pageWorkflowController;

        public CredentialRepositoryTypeSelectionPage(
            IEnumerable<ISelectionTarget<ICredentialRepositoryConfig>> selectionTargets, 
            ICredentialRepositoryList repositoryList,
            PageWorkflowController pageWorkflowController)
        {
            if (selectionTargets == null)
                throw new ArgumentNullException(nameof(selectionTargets));

            _repositoryList = repositoryList.ThrowIfNull(nameof(repositoryList));
            _pageWorkflowController = pageWorkflowController.ThrowIfNull(nameof(pageWorkflowController));
            InitializeComponent();
            ApplyTheme();
            ApplyLanguage();
            SetupListView(selectionTargets);
        }

        private void ApplyLanguage()
        {
            buttonBack.Text = Language.strBack;
            buttonContinue.Text = Language.strContinue;
        }

        private void SetupListView(IEnumerable<ISelectionTarget<ICredentialRepositoryConfig>> selectionTargets)
        {
            olvColumnName.ImageGetter = ImageGetter;
            objectListView.MouseDoubleClick += ObjectListViewOnMouseDoubleClick;
            objectListView.SetObjects(selectionTargets);
        }

        private object ImageGetter(object rowObject)
        {
            if (!(rowObject is ISelectionTarget<ICredentialRepositoryConfig> selection))
                return "";

            var imgHash = selection.Image.GetHashCode().ToString();
            if (!objectListView.LargeImageList.Images.ContainsKey(imgHash))
                objectListView.LargeImageList.Images.Add(imgHash, selection.Image);
            return imgHash;
        }

        private void ObjectListViewOnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Clicks < 2)
                return;

            var listItem = objectListView.GetItemAt(mouseEventArgs.X, mouseEventArgs.Y, out var column);
            if (!(listItem.RowObject is ISelectionTarget<ICredentialRepositoryConfig> clickedNode))
                return;

            NextPage(clickedNode);
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            if (!(objectListView.SelectedObject is ISelectionTarget<ICredentialRepositoryConfig> selection))
                return;

            NextPage(selection);
        }

        private void NextPage(ISelectionTarget<ICredentialRepositoryConfig> selection)
        {
            var editorPage = BuildEditorPage(selection);
            //RaisePageReplacementEvent(editorPage, RelativePagePosition.NextPage);
            //RaiseNextPageEvent();
            _pageWorkflowController.ShowNextPage(editorPage);
        }

        private SequencedControl BuildEditorPage(ISelectionTarget<ICredentialRepositoryConfig> selection)
        {
            var editorPage = selection.BuildEditorPage(Optional<ICredentialRepositoryConfig>.Empty, _repositoryList, _pageWorkflowController);
            editorPage.Dock = DockStyle.Fill;
            return editorPage;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            //RaisePreviousPageEvent();
            _pageWorkflowController.ShowPreviousPage();
        }
    }
}