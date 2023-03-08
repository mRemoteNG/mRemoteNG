using System;
using System.Drawing;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;
using Message = mRemoteNG.Messages.Message;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public partial class ErrorAndInfoWindow : BaseWindow
    {
        private ControlLayout _layout = ControlLayout.Vertical;
        private readonly ThemeManager _themeManager;
        private readonly DisplayProperties _display;

        public DockContent PreviousActiveForm { get; set; }

        public ErrorAndInfoWindow() : this(new DockContent())
        {
        }

        public ErrorAndInfoWindow(DockContent panel)
        {
            WindowType = WindowType.ErrorsAndInfos;
            DockPnl = panel;
            _display = new DisplayProperties();
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.StatusInformation_16x);
            lblMsgDate.Width = _display.ScaleWidth(lblMsgDate.Width);
            _themeManager = ThemeManager.getInstance();
            ApplyTheme();
            _themeManager.ThemeChanged += ApplyTheme;
            LayoutVertical();
            FillImageList();
            ApplyLanguage();
        }

        #region Form Stuff

        private void ErrorsAndInfos_Load(object sender, EventArgs e)
        {
        }

        private void ApplyLanguage()
        {
            clmMessage.Text = Language.Message;
            cMenMCCopy.Text = Language.CopyAll;
            cMenMCDelete.Text = Language.DeleteAll;
            TabText = Language.Notifications;
            Text = Language.Notifications;
        }

        #endregion

        #region Private Methods

        private new void ApplyTheme()
        {
            if (!_themeManager.ActiveAndExtended) return;
            lvErrorCollector.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
            lvErrorCollector.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");

            pnlErrorMsg.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlErrorMsg.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            txtMsgText.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
            txtMsgText.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
            lblMsgDate.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            lblMsgDate.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void FillImageList()
        {
            imgListMC.ImageSize = _display.ScaleSize(imgListMC.ImageSize);
            imgListMC.Images.Add(_display.ScaleImage(Properties.Resources.Test_16x));
            imgListMC.Images.Add(_display.ScaleImage(Properties.Resources.StatusInformation_16x));
            imgListMC.Images.Add(_display.ScaleImage(Properties.Resources.LogWarning_16x));
            imgListMC.Images.Add(_display.ScaleImage(Properties.Resources.LogError_16x));
        }

        private void LayoutVertical()
        {
            try
            {
                pnlErrorMsg.Location = new Point(0, Height - _display.ScaleHeight(200));
                pnlErrorMsg.Size = new Size(Width, Height - pnlErrorMsg.Top);
                pnlErrorMsg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                txtMsgText.Size = new Size(
                                           pnlErrorMsg.Width - pbError.Width - _display.ScaleWidth(8),
                                           pnlErrorMsg.Height - _display.ScaleHeight(20));
                lvErrorCollector.Location = new Point(0, 0);
                lvErrorCollector.Size = new Size(Width, Height - pnlErrorMsg.Height - _display.ScaleHeight(5));
                lvErrorCollector.Anchor =
                    AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                _layout = ControlLayout.Vertical;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "LayoutVertical (UI.Window.ErrorsAndInfos) failed" +
                                                    Environment.NewLine + ex.Message, true);
            }
        }

        private void LayoutHorizontal()
        {
            try
            {
                pnlErrorMsg.Location = new Point(0, 0);
                pnlErrorMsg.Size = new Size(_display.ScaleWidth(200), Height);
                pnlErrorMsg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top;

                txtMsgText.Size = new Size(
                                           pnlErrorMsg.Width - pbError.Width - _display.ScaleWidth(8),
                                           pnlErrorMsg.Height - _display.ScaleHeight(20));
                lvErrorCollector.Location = new Point(pnlErrorMsg.Width + _display.ScaleWidth(5), 0);
                lvErrorCollector.Size = new Size(Width - pnlErrorMsg.Width - _display.ScaleWidth(5), Height);
                lvErrorCollector.Anchor =
                    AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                _layout = ControlLayout.Horizontal;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "LayoutHorizontal (UI.Window.ErrorsAndInfos) failed" +
                                                    Environment.NewLine + ex.Message, true);
            }
        }

        private void ErrorsAndInfos_Resize(object sender, EventArgs e)
        {
            try
            {
                if (Width > Height)
                {
                    if (_layout == ControlLayout.Vertical)
                        LayoutHorizontal();
                }
                else
                {
                    if (_layout == ControlLayout.Horizontal)
                        LayoutVertical();
                }

                lvErrorCollector.Columns[0].Width = lvErrorCollector.Width - 20;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "ErrorsAndInfos_Resize (UI.Window.ErrorsAndInfos) failed" +
                                                    Environment.NewLine + ex.Message, true);
            }
        }

        private void SetStyleWhenNoMessageSelected()
        {
            try
            {
                pnlErrorMsg.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                pbError.Image = null;
                txtMsgText.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
                txtMsgText.Text = "";
                lblMsgDate.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                lblMsgDate.Text = "";
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "pnlErrorMsg_ResetDefaultStyle (UI.Window.ErrorsAndInfos) failed" +
                                                    Environment.NewLine +
                                                    ex.Message, true);
            }
        }

        private void MC_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Escape) return;
                try
                {
                    if (PreviousActiveForm != null)
                        PreviousActiveForm.Show(FrmMain.Default.pnlDock);
                    else
                        Windows.TreeForm.Show(FrmMain.Default.pnlDock);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "MC_KeyDown (UI.Window.ErrorsAndInfos) failed" +
                                                    Environment.NewLine + ex.Message, true);
            }
        }

        private void lvErrorCollector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvErrorCollector.SelectedItems.Count == 0 | lvErrorCollector.SelectedItems.Count > 1)
                {
                    SetStyleWhenNoMessageSelected();
                    return;
                }

                var sItem = lvErrorCollector.SelectedItems[0];
                var eMsg = (Message)sItem.Tag;
                switch (eMsg.Class)
                {
                    case MessageClass.DebugMsg:
                        pbError.Image = _display.ScaleImage(Properties.Resources.Test_16x);
                        if (_themeManager.ThemingActive)
                        {
                            pnlErrorMsg.BackColor = Color.LightSteelBlue;
                            txtMsgText.BackColor = Color.LightSteelBlue;
                            lblMsgDate.BackColor = Color.LightSteelBlue;
                        }

                        break;
                    case MessageClass.InformationMsg:
                        pbError.Image = _display.ScaleImage(Properties.Resources.StatusInformation_16x);
                        if (_themeManager.ThemingActive)
                        {
                            pnlErrorMsg.BackColor = Color.LightSteelBlue;
                            txtMsgText.BackColor = Color.LightSteelBlue;
                            lblMsgDate.BackColor = Color.LightSteelBlue;
                        }

                        break;
                    case MessageClass.WarningMsg:
                        pbError.Image = _display.ScaleImage(Properties.Resources.LogWarning_16x);
                        if (_themeManager.ActiveAndExtended)
                        {
                            //Inverse colors for dramatic effect
                            pnlErrorMsg.BackColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("WarningText_Foreground");
                            pnlErrorMsg.ForeColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("WarningText_Background");
                            txtMsgText.BackColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("WarningText_Foreground");
                            txtMsgText.ForeColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("WarningText_Background");
                            lblMsgDate.BackColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("WarningText_Foreground");
                            lblMsgDate.ForeColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("WarningText_Background");
                        }

                        break;
                    case MessageClass.ErrorMsg:
                        pbError.Image = _display.ScaleImage(Properties.Resources.LogError_16x);
                        if (_themeManager.ActiveAndExtended)
                        {
                            pnlErrorMsg.BackColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("ErrorText_Foreground");
                            pnlErrorMsg.ForeColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("ErrorText_Background");
                            txtMsgText.BackColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("ErrorText_Foreground");
                            txtMsgText.ForeColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("ErrorText_Background");
                            lblMsgDate.BackColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("ErrorText_Foreground");
                            lblMsgDate.ForeColor =
                                _themeManager.ActiveTheme.ExtendedPalette.getColor("ErrorText_Background");
                        }

                        break;
                }

                lblMsgDate.Text = eMsg.Date.ToString(CultureInfo.InvariantCulture);
                txtMsgText.Text = eMsg.Text;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "lvErrorCollector_SelectedIndexChanged (UI.Window.ErrorsAndInfos) failed" +
                                                    Environment.NewLine +
                                                    ex.Message, true);
            }
        }

        private void cMenMC_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lvErrorCollector.Items.Count > 0)
            {
                cMenMCCopy.Enabled = true;
                cMenMCDelete.Enabled = true;
                pbError.Visible = true;
            }
            else
            {
                cMenMCCopy.Enabled = false;
                cMenMCDelete.Enabled = false;
            }

            if (lvErrorCollector.SelectedItems.Count > 0)
            {
                cMenMCCopy.Text = Language.Copy;
                cMenMCDelete.Text = Language.Delete;
            }
            else
            {
                cMenMCCopy.Text = Language.CopyAll;
                cMenMCDelete.Text = Language.DeleteAll;
            }
        }

        private void cMenMCCopy_Click(object sender, EventArgs e)
        {
            CopyMessagesToClipboard();
        }

        private void CopyMessagesToClipboard()
        {
            try
            {
                IEnumerable items;
                if (lvErrorCollector.SelectedItems.Count > 0)
                {
                    items = lvErrorCollector.SelectedItems;
                }
                else
                {
                    items = lvErrorCollector.Items;
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("----------");

                lvErrorCollector.BeginUpdate();

                foreach (ListViewItem item in items)
                {
                    if (!(item.Tag is Message message))
                    {
                        continue;
                    }

                    stringBuilder.AppendLine(message.Class.ToString());
                    stringBuilder.AppendLine(message.Date.ToString(CultureInfo.InvariantCulture));
                    stringBuilder.AppendLine(message.Text);
                    stringBuilder.AppendLine("----------");
                }

                Clipboard.SetText(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "UI.Window.ErrorsAndInfos.CopyMessagesToClipboard() failed." +
                                                    Environment.NewLine + ex.Message,
                                                    true);
            }
            finally
            {
                lvErrorCollector.EndUpdate();
            }
        }

        private void cMenMCDelete_Click(object sender, EventArgs e)
        {
            DeleteMessages();
        }

        private void DeleteMessages()
        {
            try
            {
                lvErrorCollector.BeginUpdate();

                if (lvErrorCollector.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvErrorCollector.SelectedItems)
                        item.Remove();
                }
                else
                {
                    lvErrorCollector.Items.Clear();
                }

                if (lvErrorCollector.Items.Count == 0)
                {
                    pbError.Visible = false;
                    txtMsgText.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "UI.Window.ErrorsAndInfos.DeleteMessages() failed" +
                                                    Environment.NewLine + ex.Message, true);
            }
            finally
            {
                lvErrorCollector.EndUpdate();
            }
        }

        #endregion

        private enum ControlLayout
        {
            Vertical = 0,
            Horizontal = 1
        }
    }
}