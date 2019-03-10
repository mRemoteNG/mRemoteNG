using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls.Base
{
    public class NGSearchBox : NGTextBox
    {
        public NGSearchBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // NGTextBox
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ResumeLayout(false);
        }
    }
}