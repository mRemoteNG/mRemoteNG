using System;
using mRemoteNG.App;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;


namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public class UltraVNCWindow : BaseWindow
    {
        #region Form Init

        internal System.Windows.Forms.ToolStrip tsMain;
        internal System.Windows.Forms.Panel pnlContainer;
        internal System.Windows.Forms.ToolStripButton btnDisconnect;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(UltraVNCWindow));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.btnDisconnect = new System.Windows.Forms.ToolStripButton();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.btnDisconnect
            });
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(446, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "ToolStrip1";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("btnDisconnect.Image")));
            this.btnDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(70, 22);
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // pnlContainer
            // 
            this.pnlContainer.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom)
                                                     | System.Windows.Forms.AnchorStyles.Left)
                                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContainer.Location = new System.Drawing.Point(0, 27);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(446, 335);
            this.pnlContainer.TabIndex = 1;
            // 
            // UltraVNCWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(446, 362);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.tsMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UltraVNCWindow";
            this.TabText = "UltraVNC SC";
            this.Text = "UltraVNC SC";
            this.Load += new System.EventHandler(this.UltraVNCSC_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Declarations

        //Private WithEvents vnc As AxCSC_ViewerXControl

        #endregion

        #region Public Methods

        public UltraVNCWindow()
        {
            this.WindowType = WindowType.UltraVNCSC;
            this.DockPnl = new DockContent();
            this.InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void UltraVNCSC_Load(object sender, System.EventArgs e)
        {
            ApplyLanguage();

            StartListening();
        }

        private void ApplyLanguage()
        {
            btnDisconnect.Text = Language.Disconnect;
        }

        private void StartListening()
        {
            try
            {
                //If vnc IsNot Nothing Then
                //    vnc.Dispose()
                //    vnc = Nothing
                //End If

                //vnc = New AxCSC_ViewerXControl()
                //SetupLicense()

                //vnc.Parent = pnlContainer
                //vnc.Dock = DockStyle.Fill
                //vnc.Show()

                //vnc.StretchMode = ViewerX.ScreenStretchMode.SSM_ASPECT
                //vnc.ListeningText = Language.InheritListeningForIncomingVNCConnections & " " & Settings.UVNCSCPort

                //vnc.ListenEx(Settings.UVNCSCPort)
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    "StartListening (UI.Window.UltraVNCSC) failed" +
                                                    Environment.NewLine + ex.Message);
                Close();
            }
        }

#if false
        private void SetupLicense()
		{
			try
			{
				//Dim f As System.Reflection.FieldInfo
				//f = GetType(AxHost).GetField("licenseKey", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
				//f.SetValue(vnc, "{072169039103041044176252035252117103057101225235137221179204110241121074}")
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "VNC SetupLicense failed (UI.Window.UltraVNCSC)" + Environment.NewLine + ex.Message, true);
			}
		}
#endif

        //Private Sub vnc_ConnectionAccepted(ByVal sender As Object, ByVal e As AxViewerX._ISmartCodeVNCViewerEvents_ConnectionAcceptedEvent) Handles vnc.ConnectionAccepted
        //    mC.AddMessage(Messages.MessageClass.InformationMsg, e.bstrServerAddress & " is now connected to your UltraVNC SingleClick panel!")
        //End Sub

        //Private Sub vnc_Disconnected(ByVal sender As Object, ByVal e As System.EventArgs) Handles vnc.Disconnected
        //    StartListening()
        //End Sub

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            //vnc.Dispose()
            Dispose();
            Windows.Show(WindowType.UltraVNCSC);
        }

        #endregion
    }
}