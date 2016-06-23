using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmCredentialEditor : Form
    {
        public FrmCredentialEditor()
        {
            InitializeComponent();
            HideTabControllerHeader();
        }

        private void HideTabControllerHeader()
        {
            tabControlCredentialEditor.Appearance = TabAppearance.FlatButtons;
            tabControlCredentialEditor.ItemSize = new Size(0, 1);
            tabControlCredentialEditor.SizeMode = TabSizeMode.Fixed;
        }
    }
}