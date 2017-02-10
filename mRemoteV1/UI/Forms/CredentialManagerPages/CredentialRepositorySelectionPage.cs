using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialRepositorySelectionPage : UserControl, ICredentialManagerPage
    {
        private Control _previousPage;

        public string PageName { get; } = "add repo";
        public Image PageIcon { get; }

        public CredentialRepositorySelectionPage(IEnumerable<ISelectionTarget<ICredentialRepositoryConfig>> selectionTargets, Control previousPage)
        {
            if (selectionTargets == null)
                throw new ArgumentNullException(nameof(selectionTargets));
            if (previousPage == null)
                throw new ArgumentNullException(nameof(previousPage));

            _previousPage = previousPage;
            InitializeComponent();
            SetupListView(selectionTargets);
        }

        private void SetupListView(IEnumerable<ISelectionTarget<ICredentialRepositoryConfig>> selectionTargets)
        {
            olvColumnName.ImageGetter = ImageGetter;
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

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            var selection = objectListView.SelectedObject as ISelectionTarget<ICredentialRepositoryConfig>;
            if (selection == null) return;
            var editorPage = CredentialRepositoryPageEditorFactory.BuildXmlCredentialRepositoryEditorPage(selection.Config, this);
            editorPage.Dock = DockStyle.Fill;
            var parent = Parent;
            parent.Controls.Clear();
            parent.Controls.Add(editorPage);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            var parent = Parent;
            parent.Controls.Clear();
            parent.Controls.Add(_previousPage);
        }
    }
}