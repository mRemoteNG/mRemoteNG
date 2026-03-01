using System;
using System.Windows.Forms;
using mRemoteNG.UI.Forms.OptionsPages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmPickDatabase : Form
    {
        private SqlServerPage _sqlServerPage;

        public FrmPickDatabase()
        {
            InitializeComponent();
            InitializeSqlServerPage();
            ApplyLanguage();
        }

        private void InitializeSqlServerPage()
        {
            _sqlServerPage = new SqlServerPage();
            _sqlServerPage.Dock = DockStyle.Top;
            _sqlServerPage.Height = this.ClientSize.Height - pnlBottom.Height;
            //_sqlServerPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            
            pnlMain.Controls.Add(_sqlServerPage);
            
            _sqlServerPage.LoadSettings();
        }

        private void ApplyLanguage()
        {
            this.Text = "Select Database";
            btnConnect.Text = "Connect"; // Language.Connect?
            btnCancel.Text = "Cancel";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _sqlServerPage.SaveSettings();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
