using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Forms
{
    public partial class SplashPage : Form
    {
        BackgroundWorker splashPageWorker;

        public SplashPage()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }

        public void InitializeBackgroundWorker()
        {
            splashPageWorker = new BackgroundWorker();
        }
    }
}
