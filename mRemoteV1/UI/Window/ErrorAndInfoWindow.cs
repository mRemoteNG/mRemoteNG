using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.UI.Window
{
	public partial class ErrorAndInfoWindow : BaseWindow
	{
        private ControlLayout _layout = ControlLayout.Vertical;

        public DockContent PreviousActiveForm { get; set; }

	    public ErrorAndInfoWindow() : this(new DockContent())
	    {
	    }

        public ErrorAndInfoWindow(DockContent panel)
        {
            WindowType = WindowType.ErrorsAndInfos;
            DockPnl = panel;
            InitializeComponent();
            LayoutVertical();
            FillImageList();
        }

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
		    imgListMC.Images.Add(Resources.brick);
			imgListMC.Images.Add(Resources.InformationSmall);
			imgListMC.Images.Add(Resources.WarningSmall);
			imgListMC.Images.Add(Resources.ErrorSmall);
		}
				
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
				lvErrorCollector.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
						
				_layout = ControlLayout.Vertical;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "LayoutVertical (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
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
				lvErrorCollector.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
						
				_layout = ControlLayout.Horizontal;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "LayoutHorizontal (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "ErrorsAndInfos_Resize (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "pnlErrorMsg_ResetDefaultStyle (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "MC_KeyDown (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void lvErrorCollector_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (lvErrorCollector.SelectedItems.Count == 0 | lvErrorCollector.SelectedItems.Count > 1)
				{
					pnlErrorMsg_ResetDefaultStyle();
					return;
				}
						
				var sItem = lvErrorCollector.SelectedItems[0];
                var eMsg = (Messages.Message)sItem.Tag;
				switch (eMsg.Class)
				{
                    case MessageClass.DebugMsg:
				        pbError.Image = Resources.brick;
                        pnlErrorMsg.BackColor = Color.LightSteelBlue;
                        txtMsgText.BackColor = Color.LightSteelBlue;
                        break;
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
						
				lblMsgDate.Text = eMsg.Date.ToString();
				txtMsgText.Text = eMsg.Text;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "lvErrorCollector_SelectedIndexChanged (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void cMenMC_Opening(object sender, System.ComponentModel.CancelEventArgs e)
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
					var message = item.Tag as Messages.Message;
					if (message == null)
					{
						continue;
					}
							
					stringBuilder.AppendLine(message.Class.ToString());
					stringBuilder.AppendLine(message.Date.ToString());
					stringBuilder.AppendLine(message.Text);
					stringBuilder.AppendLine("----------");
				}
						
				Clipboard.SetText(stringBuilder.ToString());
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "UI.Window.ErrorsAndInfos.CopyMessagesToClipboard() failed." + Environment.NewLine + ex.Message, true);
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
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "UI.Window.ErrorsAndInfos.DeleteMessages() failed" + Environment.NewLine + ex.Message, true);
			}
			finally
			{
				lvErrorCollector.EndUpdate();
			}
		}
        #endregion
				
		public enum ControlLayout
		{
			Vertical = 0,
			Horizontal = 1
		}
	}
}
