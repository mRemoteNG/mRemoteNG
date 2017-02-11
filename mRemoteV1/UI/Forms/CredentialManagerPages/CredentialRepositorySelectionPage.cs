using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Controls.PageSequence;
using mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialRepositorySelectionPage : SequencedControl
    {
        private readonly ICredentialRepositoryList _repositoryList;

        public CredentialRepositorySelectionPage(IEnumerable<ISelectionTarget<ICredentialRepositoryConfig>> selectionTargets, ICredentialRepositoryList repositoryList)
        {
            if (selectionTargets == null)
                throw new ArgumentNullException(nameof(selectionTargets));
            if (repositoryList == null)
                throw new ArgumentNullException(nameof(repositoryList));

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

        private void ObjectListViewOnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Clicks < 2) return;
            OLVColumn column;
            var listItem = objectListView.GetItemAt(mouseEventArgs.X, mouseEventArgs.Y, out column);
            var clickedNode = listItem.RowObject as ISelectionTarget<ICredentialRepositoryConfig>;
            if (clickedNode == null) return;
            NextPage(clickedNode);
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            var selection = objectListView.SelectedObject as ISelectionTarget<ICredentialRepositoryConfig>;
            if (selection == null) return;
            NextPage(selection);
        }

        private void NextPage(ISelectionTarget<ICredentialRepositoryConfig> selection)
        {
            var editorPage = BuildEditorPage(selection);
            RaisePageReplacementEvent(editorPage, RelativePagePosition.NextPage);
            RaiseNextPageEvent();
        }

        private SequencedControl BuildEditorPage(ISelectionTarget<ICredentialRepositoryConfig> selection)
        {
            var editorPage = CredentialRepositoryPageEditorFactory.BuildXmlCredentialRepositoryEditorPage(selection.Config, _repositoryList);
            editorPage.Dock = DockStyle.Fill;
            return editorPage;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            RaisePreviousPageEvent();
        }
    }
}