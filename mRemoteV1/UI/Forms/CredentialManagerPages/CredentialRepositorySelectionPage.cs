using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialRepositorySelectionPage : UserControl, ICredentialManagerPage
    {
        public string PageName { get; } = "add repo";
        public Image PageIcon { get; }

        public CredentialRepositorySelectionPage(IEnumerable<ISelectionTarget<ICredentialRepository>> selectionTargets)
        {
            InitializeComponent();
            SetupListView(selectionTargets);
        }

        private void SetupListView(IEnumerable<ISelectionTarget<ICredentialRepository>> selectionTargets)
        {
            olvColumnName.ImageGetter = ImageGetter;
            objectListView.SetObjects(selectionTargets);
        }

        private object ImageGetter(object rowObject)
        {
            var selection = rowObject as ISelectionTarget<ICredentialRepository>;
            if (selection == null) return "";
            var imgHash = selection.Image.GetHashCode().ToString();
            if (!objectListView.LargeImageList.Images.ContainsKey(imgHash))
                objectListView.LargeImageList.Images.Add(imgHash, selection.Image);
            return imgHash;
        }
    }
}