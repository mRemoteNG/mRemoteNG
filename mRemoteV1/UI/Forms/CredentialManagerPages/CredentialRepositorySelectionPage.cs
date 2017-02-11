using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialRepositorySelectionPage : UserControl
    {
        private readonly PageSequence _pageSequence;
        private readonly ICredentialRepositoryList _repositoryList;

        public CredentialRepositorySelectionPage(IEnumerable<ISelectionTarget<ICredentialRepositoryConfig>> selectionTargets, ICredentialRepositoryList repositoryList, PageSequence pageSequence)
        {
            if (selectionTargets == null)
                throw new ArgumentNullException(nameof(selectionTargets));
            if (pageSequence == null)
                throw new ArgumentNullException(nameof(pageSequence));
            if (repositoryList == null)
                throw new ArgumentNullException(nameof(repositoryList));

            _pageSequence = pageSequence;
            _repositoryList = repositoryList;
            InitializeComponent();
            SetupListView(selectionTargets);
        }

        private void SetupListView(IEnumerable<ISelectionTarget<ICredentialRepositoryConfig>> selectionTargets)
        {
            olvColumnName.ImageGetter = ImageGetter;
            objectListView.MouseDoubleClick += ObjectListViewOnMouseDoubleClick;
            objectListView.SetObjects(selectionTargets);
        }

        private object ImageGetter(object rowObject)
        {
            var selection = rowObject as ISelectionTarget<ICredentialRepositoryConfig>;
            if (selection == null) return "";
            var imgHash = selection.Image.GetHashCode().ToString();
            if (!objectListView.LargeImageList.Images.ContainsKey(imgHash))
                objectListView.LargeImageList.Images.Add(imgHash, selection.Image);
            return imgHash;
        }

        private void BuildNextPage(ISelectionTarget<ICredentialRepositoryConfig> selection)
        {
            var editorPage = CredentialRepositoryPageEditorFactory.BuildXmlCredentialRepositoryEditorPage(selection.Config, _repositoryList, _pageSequence);
            editorPage.Dock = DockStyle.Fill;
            _pageSequence.ReplaceNextPage(editorPage);
        }

        private void ObjectListViewOnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Clicks < 2) return;
            OLVColumn column;
            var listItem = objectListView.GetItemAt(mouseEventArgs.X, mouseEventArgs.Y, out column);
            var clickedNode = listItem.RowObject as ISelectionTarget<ICredentialRepositoryConfig>;
            if (clickedNode == null) return;
            BuildNextPage(clickedNode);
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            var selection = objectListView.SelectedObject as ISelectionTarget<ICredentialRepositoryConfig>;
            if (selection == null) return;
            BuildNextPage(selection);
            _pageSequence.Next();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            _pageSequence.Previous();
        }
    }
}