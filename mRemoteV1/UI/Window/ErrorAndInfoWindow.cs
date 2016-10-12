using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Message = mRemoteNG.Messages.Message;
using TextBox = System.Windows.Forms.TextBox;

namespace mRemoteNG.UI.Window
{
    public class ErrorAndInfoWindow : BaseWindow
    {
        public enum ControlLayout
        {
            Vertical = 0,
            Horizontal = 1
        }

        #region Public Methods

        public ErrorAndInfoWindow(DockContent Panel)
        {
            WindowType = WindowType.ErrorsAndInfos;
            DockPnl = Panel;
            InitializeComponent();
            LayoutVertical();
            FillImageList();
        }

        #endregion

        #region Form Init

        internal PictureBox pbError;
        internal Label lblMsgDate;
        internal ListView lvErrorCollector;
        internal ColumnHeader clmMessage;
        internal TextBox txtMsgText;
        internal ImageList imgListMC;
        private System.ComponentModel.Container components;
        internal ContextMenuStrip cMenMC;
        internal ToolStripMenuItem cMenMCCopy;
        internal ToolStripMenuItem cMenMCDelete;
        internal Panel pnlErrorMsg;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Load += ErrorsAndInfos_Load;
            Resize += ErrorsAndInfos_Resize;
            pnlErrorMsg = new Panel();
            txtMsgText = new TextBox();
            txtMsgText.KeyDown += MC_KeyDown;
            lblMsgDate = new Label();
            pbError = new PictureBox();
            lvErrorCollector = new ListView();
            lvErrorCollector.KeyDown += MC_KeyDown;
            lvErrorCollector.SelectedIndexChanged += lvErrorCollector_SelectedIndexChanged;
            clmMessage = new ColumnHeader();
            cMenMC = new ContextMenuStrip(components);
            cMenMC.Opening += cMenMC_Opening;
            cMenMCCopy = new ToolStripMenuItem();
            cMenMCCopy.Click += cMenMCCopy_Click;
            cMenMCDelete = new ToolStripMenuItem();
            cMenMCDelete.Click += cMenMCDelete_Click;
            imgListMC = new ImageList(components);
            pnlErrorMsg.SuspendLayout();
            ((ISupportInitialize) pbError).BeginInit();
            cMenMC.SuspendLayout();
            SuspendLayout();
            //
            //pnlErrorMsg
            //
            pnlErrorMsg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                                 | AnchorStyles.Right;
            pnlErrorMsg.BackColor = SystemColors.Control;
            pnlErrorMsg.Controls.Add(txtMsgText);
            pnlErrorMsg.Controls.Add(lblMsgDate);
            pnlErrorMsg.Controls.Add(pbError);
            pnlErrorMsg.Location = new Point(0, 1);
            pnlErrorMsg.Name = "pnlErrorMsg";
            pnlErrorMsg.Size = new Size(198, 232);
            pnlErrorMsg.TabIndex = 20;
            //
            //txtMsgText
            //
            txtMsgText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                | AnchorStyles.Left
                                | AnchorStyles.Right;
            txtMsgText.BorderStyle = BorderStyle.None;
            txtMsgText.Location = new Point(40, 20);
            txtMsgText.Multiline = true;
            txtMsgText.Name = "txtMsgText";
            txtMsgText.ReadOnly = true;
            txtMsgText.ScrollBars = ScrollBars.Vertical;
            txtMsgText.Size = new Size(158, 211);
            txtMsgText.TabIndex = 30;
            //
            //lblMsgDate
            //
            lblMsgDate.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                | AnchorStyles.Right;
            lblMsgDate.Font = new Font("Segoe UI", 8.25F, FontStyle.Italic, GraphicsUnit.Point, Convert.ToByte(0));
            lblMsgDate.Location = new Point(40, 5);
            lblMsgDate.Name = "lblMsgDate";
            lblMsgDate.Size = new Size(155, 13);
            lblMsgDate.TabIndex = 40;
            //
            //pbError
            //
            pbError.InitialImage = null;
            pbError.Location = new Point(2, 5);
            pbError.Name = "pbError";
            pbError.Size = new Size(32, 32);
            pbError.TabIndex = 0;
            pbError.TabStop = false;
            //
            //lvErrorCollector
            //
            lvErrorCollector.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                      | AnchorStyles.Left
                                      | AnchorStyles.Right;
            lvErrorCollector.BorderStyle = BorderStyle.None;
            lvErrorCollector.Columns.AddRange(new[] {clmMessage});
            lvErrorCollector.ContextMenuStrip = cMenMC;
            lvErrorCollector.FullRowSelect = true;
            lvErrorCollector.HeaderStyle = ColumnHeaderStyle.None;
            lvErrorCollector.HideSelection = false;
            lvErrorCollector.Location = new Point(203, 1);
            lvErrorCollector.Name = "lvErrorCollector";
            lvErrorCollector.ShowGroups = false;
            lvErrorCollector.Size = new Size(413, 232);
            lvErrorCollector.SmallImageList = imgListMC;
            lvErrorCollector.TabIndex = 10;
            lvErrorCollector.UseCompatibleStateImageBehavior = false;
            lvErrorCollector.View = View.Details;
            //
            //clmMessage
            //
            clmMessage.Text = Language.strColumnMessage;
            clmMessage.Width = 184;
            //
            //cMenMC
            //
            cMenMC.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            cMenMC.Items.AddRange(new ToolStripItem[] {cMenMCCopy, cMenMCDelete});
            cMenMC.Name = "cMenMC";
            cMenMC.RenderMode = ToolStripRenderMode.Professional;
            cMenMC.Size = new Size(153, 70);
            //
            //cMenMCCopy
            //
            cMenMCCopy.Image = Resources.Copy;
            cMenMCCopy.Name = "cMenMCCopy";
            cMenMCCopy.ShortcutKeys = Keys.Control | Keys.C;
            cMenMCCopy.Size = new Size(152, 22);
            cMenMCCopy.Text = Language.strMenuCopy;
            //
            //cMenMCDelete
            //
            cMenMCDelete.Image = Resources.Delete;
            cMenMCDelete.Name = "cMenMCDelete";
            cMenMCDelete.ShortcutKeys = Keys.Delete;
            cMenMCDelete.Size = new Size(152, 22);
            cMenMCDelete.Text = Language.strMenuDelete;
            //
            //imgListMC
            //
            imgListMC.ColorDepth = ColorDepth.Depth32Bit;
            imgListMC.ImageSize = new Size(16, 16);
            imgListMC.TransparentColor = Color.Transparent;
            //
            //ErrorsAndInfos
            //
            ClientSize = new Size(617, 233);
            Controls.Add(lvErrorCollector);
            Controls.Add(pnlErrorMsg);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            HideOnClose = true;
            Icon = Resources.Info_Icon;
            Name = "ErrorsAndInfos";
            TabText = Language.strMenuNotifications;
            Text = "Notifications";
            pnlErrorMsg.ResumeLayout(false);
            pnlErrorMsg.PerformLayout();
            ((ISupportInitialize) pbError).EndInit();
            cMenMC.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        #region Public Properties

        public DockContent PreviousActiveForm { get; set; }

        #endregion

        #region Form Stuff

        private void ErrorsAndInfos_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            clmMessage.Text = Language.strColumnMessage;
            cMenMCCopy.Text = Language.strMenuNotificationsCopyAll;
            cMenMCDelete.Text = Language.strMenuNotificationsDeleteAll;
            TabText = Language.strMenuNotifications;
            Text = Language.strMenuNotifications;
        }

        #endregion

        #region Private Methods

        private void FillImageList()
        {
            imgListMC.Images.Add(Resources.InformationSmall);
            imgListMC.Images.Add(Resources.WarningSmall);
            imgListMC.Images.Add(Resources.ErrorSmall);
        }


        private ControlLayout _Layout = ControlLayout.Vertical;

        private void LayoutVertical()
        {
            try
            {
                pnlErrorMsg.Location = new Point(0, Height - 200);
                pnlErrorMsg.Size = new Size(Width, Height - pnlErrorMsg.Top);
                pnlErrorMsg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                txtMsgText.Size = new Size(pnlErrorMsg.Width - pbError.Width - 8, pnlErrorMsg.Height - 20);
                lvErrorCollector.Location = new Point(0, 0);
                lvErrorCollector.Size = new Size(Width, Height - pnlErrorMsg.Height - 5);
                lvErrorCollector.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right |
                                          AnchorStyles.Top;

                _Layout = ControlLayout.Vertical;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "LayoutVertical (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void LayoutHorizontal()
        {
            try
            {
                pnlErrorMsg.Location = new Point(0, 0);
                pnlErrorMsg.Size = new Size(200, Height);
                pnlErrorMsg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top;
                txtMsgText.Size = new Size(pnlErrorMsg.Width - pbError.Width - 8, pnlErrorMsg.Height - 20);
                lvErrorCollector.Location = new Point(pnlErrorMsg.Width + 5, 0);
                lvErrorCollector.Size = new Size(Width - pnlErrorMsg.Width - 5, Height);
                lvErrorCollector.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right |
                                          AnchorStyles.Top;

                _Layout = ControlLayout.Horizontal;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "LayoutHorizontal (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void ErrorsAndInfos_Resize(object sender, EventArgs e)
        {
            try
            {
                if (Width > Height)
                {
                    if (_Layout == ControlLayout.Vertical)
                        LayoutHorizontal();
                }
                else
                {
                    if (_Layout == ControlLayout.Horizontal)
                        LayoutVertical();
                }

                lvErrorCollector.Columns[0].Width = lvErrorCollector.Width - 20;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "ErrorsAndInfos_Resize (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void pnlErrorMsg_ResetDefaultStyle()
        {
            try
            {
                pnlErrorMsg.BackColor = Color.FromKnownColor(KnownColor.Control);
                pbError.Image = null;
                txtMsgText.Text = "";
                txtMsgText.BackColor = Color.FromKnownColor(KnownColor.Control);
                lblMsgDate.Text = "";
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "pnlErrorMsg_ResetDefaultStyle (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message,
                    true);
            }
        }

        private void MC_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                    try
                    {
                        if (PreviousActiveForm != null)
                            PreviousActiveForm.Show(frmMain.Default.pnlDock);
                        else
                            Windows.TreeForm.Show(frmMain.Default.pnlDock);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "MC_KeyDown (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void lvErrorCollector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ((lvErrorCollector.SelectedItems.Count == 0) | (lvErrorCollector.SelectedItems.Count > 1))
                {
                    pnlErrorMsg_ResetDefaultStyle();
                    return;
                }

                var sItem = lvErrorCollector.SelectedItems[0];
                var eMsg = (Message) sItem.Tag;
                switch (eMsg.MsgClass)
                {
                    case MessageClass.InformationMsg:
                        pbError.Image = Resources.Information;
                        pnlErrorMsg.BackColor = Color.LightSteelBlue;
                        txtMsgText.BackColor = Color.LightSteelBlue;
                        break;
                    case MessageClass.WarningMsg:
                        pbError.Image = Resources.Warning;
                        pnlErrorMsg.BackColor = Color.Gold;
                        txtMsgText.BackColor = Color.Gold;
                        break;
                    case MessageClass.ErrorMsg:
                        pbError.Image = Resources._Error;
                        pnlErrorMsg.BackColor = Color.IndianRed;
                        txtMsgText.BackColor = Color.IndianRed;
                        break;
                }

                lblMsgDate.Text = eMsg.MsgDate.ToString();
                txtMsgText.Text = eMsg.MsgText;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "lvErrorCollector_SelectedIndexChanged (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine +
                    ex.Message, true);
            }
        }

        private void cMenMC_Opening(object sender, CancelEventArgs e)
        {
            if (lvErrorCollector.Items.Count > 0)
            {
                cMenMCCopy.Enabled = true;
                cMenMCDelete.Enabled = true;
            }
            else
            {
                cMenMCCopy.Enabled = false;
                cMenMCDelete.Enabled = false;
            }

            if (lvErrorCollector.SelectedItems.Count > 0)
            {
                cMenMCCopy.Text = Language.strMenuCopy;
                cMenMCDelete.Text = Language.strMenuNotificationsDelete;
            }
            else
            {
                cMenMCCopy.Text = Language.strMenuNotificationsCopyAll;
                cMenMCDelete.Text = Language.strMenuNotificationsDeleteAll;
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
                var items = default(IEnumerable);
                if (lvErrorCollector.SelectedItems.Count > 0)
                    items = lvErrorCollector.SelectedItems;
                else
                    items = lvErrorCollector.Items;

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("----------");

                lvErrorCollector.BeginUpdate();

                var message = default(Message);
                foreach (ListViewItem item in items)
                {
                    message = item.Tag as Message;
                    if (message == null)
                        continue;

                    stringBuilder.AppendLine(message.MsgClass.ToString());
                    stringBuilder.AppendLine(message.MsgDate.ToString());
                    stringBuilder.AppendLine(message.MsgText);
                    stringBuilder.AppendLine("----------");
                }

                Clipboard.SetText(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "UI.Window.ErrorsAndInfos.CopyMessagesToClipboard() failed." + Environment.NewLine + ex.Message,
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
                    foreach (ListViewItem item in lvErrorCollector.SelectedItems)
                        item.Remove();
                else
                    lvErrorCollector.Items.Clear();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "UI.Window.ErrorsAndInfos.DeleteMessages() failed" + Environment.NewLine + ex.Message, true);
            }
            finally
            {
                lvErrorCollector.EndUpdate();
            }
        }

        #endregion
    }
}