using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Properties;
using mRemoteNG.Tree;
using mRemoteNG.UI.TaskDialog;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Tabs
{
    [SupportedOSPlatform("windows")]
    public partial class ConnectionTab : DockContent
    {
        /// <summary>
        ///Silent close ignores the popup asking for confirmation
        /// </summary>
        public bool silentClose { get; set; }

        /// <summary>
        /// Protocol close ignores the interface controller cleanup and the user confirmation dialog
        /// </summary>
        public bool protocolClose { get; set; }

        public ConnectionInfo? TrackedConnectionInfo { get; private set; }

        private Label? _closedStateLabel;
        private Panel? _closedStatePanel;

        public ConnectionTab()
        {
            InitializeComponent();
            Font = ConnectionTabAppearanceSettings.GetTabFont(Font);
            GotFocus += ConnectionTab_GotFocus;
        }

        private void ConnectionTab_GotFocus(object sender, EventArgs e)
        {
            TabHelper.Instance.CurrentTab = this;
        }

        public void TrackConnection(ConnectionInfo connectionInfo)
        {
            TrackedConnectionInfo = connectionInfo;
        }

        private bool _hasUnreadActivity;
        public bool HasUnreadActivity
        {
            get => _hasUnreadActivity;
            set
            {
                if (_hasUnreadActivity == value) return;
                _hasUnreadActivity = value;
                DockHandler?.Pane?.Refresh();
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            HasUnreadActivity = false;
        }

        public void ShowClosedState()
        {
            HideClosedState();

            ConnectionInfo? info = TrackedConnectionInfo;
            if (info == null)
            {
                // Fallback: simple label when no connection info is available
                _closedStateLabel ??= new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                _closedStateLabel.Text = Language.ConnenctionCloseEvent;
                Controls.Add(_closedStateLabel);
                _closedStateLabel.BringToFront();
                return;
            }

            _closedStatePanel = BuildClosedStatePanel(info);
            Controls.Add(_closedStatePanel);
            _closedStatePanel.BringToFront();
        }

        public void HideClosedState()
        {
            if (_closedStateLabel != null && Controls.Contains(_closedStateLabel))
                Controls.Remove(_closedStateLabel);

            if (_closedStatePanel != null)
            {
                if (Controls.Contains(_closedStatePanel))
                    Controls.Remove(_closedStatePanel);
                _closedStatePanel.Dispose();
                _closedStatePanel = null;
            }
        }

        private Panel BuildClosedStatePanel(ConnectionInfo info)
        {
            Panel outer = new() { Dock = DockStyle.Fill };

            Label lblName = new()
            {
                Text = info.Name,
                Font = new Font(Font.FontFamily, 14f, FontStyle.Bold),
                AutoSize = true,
            };

            string details = $"{info.Protocol}   {info.Hostname}:{info.Port}";
            if (!string.IsNullOrWhiteSpace(info.Description))
                details += $"\n{info.Description}";

            Label lblDetails = new()
            {
                Text = details,
                Font = new Font(Font.FontFamily, 9.5f),
                AutoSize = true,
                ForeColor = SystemColors.GrayText,
            };

            Button btnConnect = new()
            {
                Text = Language.Connect,
                AutoSize = true,
                Padding = new Padding(24, 4, 24, 4),
            };
            btnConnect.Click += (_, _) => Runtime.ConnectionInitiator.OpenConnection(info);

            outer.Controls.AddRange([lblName, lblDetails, btnConnect]);

            void CenterControls(object? s, EventArgs a)
            {
                const int gap = 8;
                int totalH = lblName.Height + gap + lblDetails.Height + gap * 2 + btnConnect.Height;
                int y = Math.Max(10, (outer.Height - totalH) / 2);
                int cx = outer.Width / 2;

                lblName.Location = new Point(cx - lblName.Width / 2, y);
                y += lblName.Height + gap;
                lblDetails.Location = new Point(cx - lblDetails.Width / 2, y);
                y += lblDetails.Height + gap * 2;
                btnConnect.Location = new Point(cx - btnConnect.Width / 2, y);
            }

            outer.Resize += CenterControls;
            outer.Layout += CenterControls;
            return outer;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!protocolClose)
            {
                // If the tab is showing the closed/disconnected state (no active protocol),
                // skip protocol close and confirmation — there's nothing to disconnect.
                bool hasActiveProtocol = Tag is InterfaceControl ic && ic.Protocol != null && !ic.IsDisposed;

                if (hasActiveProtocol && !silentClose)
                {
                    if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                    {
                        DialogResult result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName,
                                                            string
                                                                .Format(CultureInfo.CurrentCulture, Language.ConfirmCloseConnectionPanelMainInstruction,
                                                                        TabText), "", "", "",
                                                            Language.CheckboxDoNotShowThisMessageAgain,
                                                            ETaskDialogButtons.DisconnectCancel, ESysIcons.Question,
                                                            ESysIcons.Question);
                        if (CTaskDialog.VerificationChecked)
                        {
                            Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Multiple;
                            Settings.Default.Save();
                        }

                        if (result == DialogResult.No)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            CloseProtocolSafe();
                        }
                    }
                    else
                    {
                        CloseProtocolSafe();
                    }
                }
                else if (hasActiveProtocol)
                {
                    CloseProtocolSafe();
                }
            }

            base.OnFormClosing(e);
        }

        private void CloseProtocolSafe()
        {
            try
            {
                ((InterfaceControl?)Tag)?.Protocol?.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage("Error closing protocol", ex);
            }
        }


        #region HelperFunctions  

        public void RefreshInterfaceController()
        {
            try
            {
                InterfaceControl? interfaceControl = Tag as InterfaceControl;
                if (interfaceControl?.Info.Protocol == ProtocolType.VNC)
                    ((ProtocolVNC)interfaceControl.Protocol).RefreshScreen();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("RefreshIC (UI.Window.Connection) failed", ex);
            }
        }

        public void FireResizeEnd()
        {
            OnResizeEnd(EventArgs.Empty);
        }

        #endregion
    }
}