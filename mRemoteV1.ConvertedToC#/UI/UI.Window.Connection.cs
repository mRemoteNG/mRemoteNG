using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows;
using mRemoteNG.Connection;
using mRemoteNG.App;
using Crownwood;
using WeifenLuo.WinFormsUI.Docking;
using PSTaskDialog;
using mRemoteNG.App.Runtime;
using mRemoteNG.Config;

namespace mRemoteNG.UI
{
	namespace Window
	{
		public class Connection : UI.Window.Base
		{

			#region "Form Init"
			internal System.Windows.Forms.ContextMenuStrip cmenTab;
			private System.ComponentModel.IContainer components;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabFullscreen;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabFullscreen {
				get { return withEventsField_cmenTabFullscreen; }
				set {
					if (withEventsField_cmenTabFullscreen != null) {
						withEventsField_cmenTabFullscreen.Click -= cmenTabFullscreen_Click;
					}
					withEventsField_cmenTabFullscreen = value;
					if (withEventsField_cmenTabFullscreen != null) {
						withEventsField_cmenTabFullscreen.Click += cmenTabFullscreen_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabScreenshot;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabScreenshot {
				get { return withEventsField_cmenTabScreenshot; }
				set {
					if (withEventsField_cmenTabScreenshot != null) {
						withEventsField_cmenTabScreenshot.Click -= cmenTabScreenshot_Click;
					}
					withEventsField_cmenTabScreenshot = value;
					if (withEventsField_cmenTabScreenshot != null) {
						withEventsField_cmenTabScreenshot.Click += cmenTabScreenshot_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabTransferFile;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabTransferFile {
				get { return withEventsField_cmenTabTransferFile; }
				set {
					if (withEventsField_cmenTabTransferFile != null) {
						withEventsField_cmenTabTransferFile.Click -= cmenTabTransferFile_Click;
					}
					withEventsField_cmenTabTransferFile = value;
					if (withEventsField_cmenTabTransferFile != null) {
						withEventsField_cmenTabTransferFile.Click += cmenTabTransferFile_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripMenuItem cmenTabSendSpecialKeys;
			internal System.Windows.Forms.ToolStripSeparator cmenTabSep1;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabRenameTab;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabRenameTab {
				get { return withEventsField_cmenTabRenameTab; }
				set {
					if (withEventsField_cmenTabRenameTab != null) {
						withEventsField_cmenTabRenameTab.Click -= cmenTabRenameTab_Click;
					}
					withEventsField_cmenTabRenameTab = value;
					if (withEventsField_cmenTabRenameTab != null) {
						withEventsField_cmenTabRenameTab.Click += cmenTabRenameTab_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabDuplicateTab;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabDuplicateTab {
				get { return withEventsField_cmenTabDuplicateTab; }
				set {
					if (withEventsField_cmenTabDuplicateTab != null) {
						withEventsField_cmenTabDuplicateTab.Click -= cmenTabDuplicateTab_Click;
					}
					withEventsField_cmenTabDuplicateTab = value;
					if (withEventsField_cmenTabDuplicateTab != null) {
						withEventsField_cmenTabDuplicateTab.Click += cmenTabDuplicateTab_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabDisconnect;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabDisconnect {
				get { return withEventsField_cmenTabDisconnect; }
				set {
					if (withEventsField_cmenTabDisconnect != null) {
						withEventsField_cmenTabDisconnect.Click -= cmenTabDisconnect_Click;
					}
					withEventsField_cmenTabDisconnect = value;
					if (withEventsField_cmenTabDisconnect != null) {
						withEventsField_cmenTabDisconnect.Click += cmenTabDisconnect_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabSmartSize;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabSmartSize {
				get { return withEventsField_cmenTabSmartSize; }
				set {
					if (withEventsField_cmenTabSmartSize != null) {
						withEventsField_cmenTabSmartSize.Click -= cmenTabSmartSize_Click;
					}
					withEventsField_cmenTabSmartSize = value;
					if (withEventsField_cmenTabSmartSize != null) {
						withEventsField_cmenTabSmartSize.Click += cmenTabSmartSize_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabSendSpecialKeysCtrlAltDel;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabSendSpecialKeysCtrlAltDel {
				get { return withEventsField_cmenTabSendSpecialKeysCtrlAltDel; }
				set {
					if (withEventsField_cmenTabSendSpecialKeysCtrlAltDel != null) {
						withEventsField_cmenTabSendSpecialKeysCtrlAltDel.Click -= cmenTabSendSpecialKeysCtrlAltDel_Click;
					}
					withEventsField_cmenTabSendSpecialKeysCtrlAltDel = value;
					if (withEventsField_cmenTabSendSpecialKeysCtrlAltDel != null) {
						withEventsField_cmenTabSendSpecialKeysCtrlAltDel.Click += cmenTabSendSpecialKeysCtrlAltDel_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabSendSpecialKeysCtrlEsc;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabSendSpecialKeysCtrlEsc {
				get { return withEventsField_cmenTabSendSpecialKeysCtrlEsc; }
				set {
					if (withEventsField_cmenTabSendSpecialKeysCtrlEsc != null) {
						withEventsField_cmenTabSendSpecialKeysCtrlEsc.Click -= cmenTabSendSpecialKeysCtrlEsc_Click;
					}
					withEventsField_cmenTabSendSpecialKeysCtrlEsc = value;
					if (withEventsField_cmenTabSendSpecialKeysCtrlEsc != null) {
						withEventsField_cmenTabSendSpecialKeysCtrlEsc.Click += cmenTabSendSpecialKeysCtrlEsc_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabViewOnly;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabViewOnly {
				get { return withEventsField_cmenTabViewOnly; }
				set {
					if (withEventsField_cmenTabViewOnly != null) {
						withEventsField_cmenTabViewOnly.Click -= cmenTabViewOnly_Click;
					}
					withEventsField_cmenTabViewOnly = value;
					if (withEventsField_cmenTabViewOnly != null) {
						withEventsField_cmenTabViewOnly.Click += cmenTabViewOnly_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabReconnect;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabReconnect {
				get { return withEventsField_cmenTabReconnect; }
				set {
					if (withEventsField_cmenTabReconnect != null) {
						withEventsField_cmenTabReconnect.Click -= cmenTabReconnect_Click;
					}
					withEventsField_cmenTabReconnect = value;
					if (withEventsField_cmenTabReconnect != null) {
						withEventsField_cmenTabReconnect.Click += cmenTabReconnect_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripMenuItem cmenTabExternalApps;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabStartChat;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabStartChat {
				get { return withEventsField_cmenTabStartChat; }
				set {
					if (withEventsField_cmenTabStartChat != null) {
						withEventsField_cmenTabStartChat.Click -= cmenTabStartChat_Click;
					}
					withEventsField_cmenTabStartChat = value;
					if (withEventsField_cmenTabStartChat != null) {
						withEventsField_cmenTabStartChat.Click += cmenTabStartChat_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabRefreshScreen;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabRefreshScreen {
				get { return withEventsField_cmenTabRefreshScreen; }
				set {
					if (withEventsField_cmenTabRefreshScreen != null) {
						withEventsField_cmenTabRefreshScreen.Click -= cmenTabRefreshScreen_Click;
					}
					withEventsField_cmenTabRefreshScreen = value;
					if (withEventsField_cmenTabRefreshScreen != null) {
						withEventsField_cmenTabRefreshScreen.Click += cmenTabRefreshScreen_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cmenTabPuttySettings;
			internal System.Windows.Forms.ToolStripMenuItem cmenTabPuttySettings {
				get { return withEventsField_cmenTabPuttySettings; }
				set {
					if (withEventsField_cmenTabPuttySettings != null) {
						withEventsField_cmenTabPuttySettings.Click -= cmenTabPuttySettings_Click;
					}
					withEventsField_cmenTabPuttySettings = value;
					if (withEventsField_cmenTabPuttySettings != null) {
						withEventsField_cmenTabPuttySettings.Click += cmenTabPuttySettings_Click;
					}
				}

			}
			private Crownwood.Magic.Controls.TabControl withEventsField_TabController;
			public Crownwood.Magic.Controls.TabControl TabController {
				get { return withEventsField_TabController; }
				set {
					if (withEventsField_TabController != null) {
						withEventsField_TabController.ClosePressed -= TabController_ClosePressed;
						withEventsField_TabController.DoubleClickTab -= TabController_DoubleClickTab;
						withEventsField_TabController.DragDrop -= TabController_DragDrop;
						withEventsField_TabController.DragEnter -= TabController_DragEnter;
						withEventsField_TabController.DragOver -= TabController_DragOver;
						withEventsField_TabController.SelectionChanged -= TabController_SelectionChanged;
						withEventsField_TabController.MouseUp -= TabController_MouseUp;
						withEventsField_TabController.PageDragEnd -= TabController_PageDragStart;
						withEventsField_TabController.PageDragStart -= TabController_PageDragStart;
						withEventsField_TabController.PageDragMove -= TabController_PageDragMove;
						withEventsField_TabController.PageDragEnd -= TabController_PageDragEnd;
						withEventsField_TabController.PageDragQuit -= TabController_PageDragEnd;
					}
					withEventsField_TabController = value;
					if (withEventsField_TabController != null) {
						withEventsField_TabController.ClosePressed += TabController_ClosePressed;
						withEventsField_TabController.DoubleClickTab += TabController_DoubleClickTab;
						withEventsField_TabController.DragDrop += TabController_DragDrop;
						withEventsField_TabController.DragEnter += TabController_DragEnter;
						withEventsField_TabController.DragOver += TabController_DragOver;
						withEventsField_TabController.SelectionChanged += TabController_SelectionChanged;
						withEventsField_TabController.MouseUp += TabController_MouseUp;
						withEventsField_TabController.PageDragEnd += TabController_PageDragStart;
						withEventsField_TabController.PageDragStart += TabController_PageDragStart;
						withEventsField_TabController.PageDragMove += TabController_PageDragMove;
						withEventsField_TabController.PageDragEnd += TabController_PageDragEnd;
						withEventsField_TabController.PageDragQuit += TabController_PageDragEnd;
					}
				}
			}
			private void InitializeComponent()
			{
				this.components = new System.ComponentModel.Container();
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Connection));
				this.TabController = new Crownwood.Magic.Controls.TabControl();
				this.cmenTab = new System.Windows.Forms.ContextMenuStrip(this.components);
				this.cmenTabFullscreen = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabSmartSize = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabViewOnly = new System.Windows.Forms.ToolStripMenuItem();
				this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
				this.cmenTabScreenshot = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabStartChat = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabTransferFile = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabRefreshScreen = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabSendSpecialKeys = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabSendSpecialKeysCtrlAltDel = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabSendSpecialKeysCtrlEsc = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabExternalApps = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabSep1 = new System.Windows.Forms.ToolStripSeparator();
				this.cmenTabRenameTab = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabDuplicateTab = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabReconnect = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabDisconnect = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTabPuttySettings = new System.Windows.Forms.ToolStripMenuItem();
				this.cmenTab.SuspendLayout();
				this.SuspendLayout();
				//
				//TabController
				//
				this.TabController.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.TabController.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
				this.TabController.Cursor = System.Windows.Forms.Cursors.Hand;
				this.TabController.DragFromControl = false;
				this.TabController.IDEPixelArea = true;
				this.TabController.IDEPixelBorder = false;
				this.TabController.Location = new System.Drawing.Point(0, -1);
				this.TabController.Name = "TabController";
				this.TabController.Size = new System.Drawing.Size(632, 454);
				this.TabController.TabIndex = 0;
				//
				//cmenTab
				//
				this.cmenTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.cmenTabFullscreen,
					this.cmenTabSmartSize,
					this.cmenTabViewOnly,
					this.ToolStripSeparator1,
					this.cmenTabScreenshot,
					this.cmenTabStartChat,
					this.cmenTabTransferFile,
					this.cmenTabRefreshScreen,
					this.cmenTabSendSpecialKeys,
					this.cmenTabPuttySettings,
					this.cmenTabExternalApps,
					this.cmenTabSep1,
					this.cmenTabRenameTab,
					this.cmenTabDuplicateTab,
					this.cmenTabReconnect,
					this.cmenTabDisconnect
				});
				this.cmenTab.Name = "cmenTab";
				this.cmenTab.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
				this.cmenTab.Size = new System.Drawing.Size(202, 346);
				//
				//cmenTabFullscreen
				//
				this.cmenTabFullscreen.Image = global::mRemoteNG.My.Resources.Resources.arrow_out;
				this.cmenTabFullscreen.Name = "cmenTabFullscreen";
				this.cmenTabFullscreen.Size = new System.Drawing.Size(201, 22);
				this.cmenTabFullscreen.Text = "Fullscreen (RDP)";
				//
				//cmenTabSmartSize
				//
				this.cmenTabSmartSize.Image = global::mRemoteNG.My.Resources.Resources.SmartSize;
				this.cmenTabSmartSize.Name = "cmenTabSmartSize";
				this.cmenTabSmartSize.Size = new System.Drawing.Size(201, 22);
				this.cmenTabSmartSize.Text = "SmartSize (RDP/VNC)";
				//
				//cmenTabViewOnly
				//
				this.cmenTabViewOnly.Name = "cmenTabViewOnly";
				this.cmenTabViewOnly.Size = new System.Drawing.Size(201, 22);
				this.cmenTabViewOnly.Text = "View Only (VNC)";
				//
				//ToolStripSeparator1
				//
				this.ToolStripSeparator1.Name = "ToolStripSeparator1";
				this.ToolStripSeparator1.Size = new System.Drawing.Size(198, 6);
				//
				//cmenTabScreenshot
				//
				this.cmenTabScreenshot.Image = global::mRemoteNG.My.Resources.Resources.Screenshot_Add;
				this.cmenTabScreenshot.Name = "cmenTabScreenshot";
				this.cmenTabScreenshot.Size = new System.Drawing.Size(201, 22);
				this.cmenTabScreenshot.Text = "Screenshot";
				//
				//cmenTabStartChat
				//
				this.cmenTabStartChat.Image = global::mRemoteNG.My.Resources.Resources.Chat;
				this.cmenTabStartChat.Name = "cmenTabStartChat";
				this.cmenTabStartChat.Size = new System.Drawing.Size(201, 22);
				this.cmenTabStartChat.Text = "Start Chat (VNC)";
				this.cmenTabStartChat.Visible = false;
				//
				//cmenTabTransferFile
				//
				this.cmenTabTransferFile.Image = global::mRemoteNG.My.Resources.Resources.SSHTransfer;
				this.cmenTabTransferFile.Name = "cmenTabTransferFile";
				this.cmenTabTransferFile.Size = new System.Drawing.Size(201, 22);
				this.cmenTabTransferFile.Text = "Transfer File (SSH)";
				//
				//cmenTabRefreshScreen
				//
				this.cmenTabRefreshScreen.Image = global::mRemoteNG.My.Resources.Resources.Refresh;
				this.cmenTabRefreshScreen.Name = "cmenTabRefreshScreen";
				this.cmenTabRefreshScreen.Size = new System.Drawing.Size(201, 22);
				this.cmenTabRefreshScreen.Text = "Refresh Screen (VNC)";
				//
				//cmenTabSendSpecialKeys
				//
				this.cmenTabSendSpecialKeys.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.cmenTabSendSpecialKeysCtrlAltDel,
					this.cmenTabSendSpecialKeysCtrlEsc
				});
				this.cmenTabSendSpecialKeys.Image = global::mRemoteNG.My.Resources.Resources.Keyboard;
				this.cmenTabSendSpecialKeys.Name = "cmenTabSendSpecialKeys";
				this.cmenTabSendSpecialKeys.Size = new System.Drawing.Size(201, 22);
				this.cmenTabSendSpecialKeys.Text = "Send special Keys (VNC)";
				//
				//cmenTabSendSpecialKeysCtrlAltDel
				//
				this.cmenTabSendSpecialKeysCtrlAltDel.Name = "cmenTabSendSpecialKeysCtrlAltDel";
				this.cmenTabSendSpecialKeysCtrlAltDel.Size = new System.Drawing.Size(141, 22);
				this.cmenTabSendSpecialKeysCtrlAltDel.Text = "Ctrl+Alt+Del";
				//
				//cmenTabSendSpecialKeysCtrlEsc
				//
				this.cmenTabSendSpecialKeysCtrlEsc.Name = "cmenTabSendSpecialKeysCtrlEsc";
				this.cmenTabSendSpecialKeysCtrlEsc.Size = new System.Drawing.Size(141, 22);
				this.cmenTabSendSpecialKeysCtrlEsc.Text = "Ctrl+Esc";
				//
				//cmenTabExternalApps
				//
				this.cmenTabExternalApps.Image = (System.Drawing.Image)resources.GetObject("cmenTabExternalApps.Image");
				this.cmenTabExternalApps.Name = "cmenTabExternalApps";
				this.cmenTabExternalApps.Size = new System.Drawing.Size(201, 22);
				this.cmenTabExternalApps.Text = "External Applications";
				//
				//cmenTabSep1
				//
				this.cmenTabSep1.Name = "cmenTabSep1";
				this.cmenTabSep1.Size = new System.Drawing.Size(198, 6);
				//
				//cmenTabRenameTab
				//
				this.cmenTabRenameTab.Image = global::mRemoteNG.My.Resources.Resources.Rename;
				this.cmenTabRenameTab.Name = "cmenTabRenameTab";
				this.cmenTabRenameTab.Size = new System.Drawing.Size(201, 22);
				this.cmenTabRenameTab.Text = "Rename Tab";
				//
				//cmenTabDuplicateTab
				//
				this.cmenTabDuplicateTab.Name = "cmenTabDuplicateTab";
				this.cmenTabDuplicateTab.Size = new System.Drawing.Size(201, 22);
				this.cmenTabDuplicateTab.Text = "Duplicate Tab";
				//
				//cmenTabReconnect
				//
				this.cmenTabReconnect.Image = (System.Drawing.Image)resources.GetObject("cmenTabReconnect.Image");
				this.cmenTabReconnect.Name = "cmenTabReconnect";
				this.cmenTabReconnect.Size = new System.Drawing.Size(201, 22);
				this.cmenTabReconnect.Text = "Reconnect";
				//
				//cmenTabDisconnect
				//
				this.cmenTabDisconnect.Image = global::mRemoteNG.My.Resources.Resources.Pause;
				this.cmenTabDisconnect.Name = "cmenTabDisconnect";
				this.cmenTabDisconnect.Size = new System.Drawing.Size(201, 22);
				this.cmenTabDisconnect.Text = "Disconnect";
				//
				//cmenTabPuttySettings
				//
				this.cmenTabPuttySettings.Name = "cmenTabPuttySettings";
				this.cmenTabPuttySettings.Size = new System.Drawing.Size(201, 22);
				this.cmenTabPuttySettings.Text = "PuTTY Settings";
				//
				//Connection
				//
				this.ClientSize = new System.Drawing.Size(632, 453);
				this.Controls.Add(this.TabController);
				this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
				this.Icon = global::mRemoteNG.My.Resources.Resources.mRemote_Icon;
				this.Name = "Connection";
				this.TabText = "UI.Window.Connection";
				this.Text = "UI.Window.Connection";
				this.cmenTab.ResumeLayout(false);
				this.ResumeLayout(false);

			}
			#endregion

			#region "Public Methods"

			public Connection(DockContent Panel, string FormText = "")
			{
				FormClosing += Connection_FormClosing;
				DockStateChanged += Connection_DockStateChanged;
				Load += Connection_Load;
				if (string.IsNullOrEmpty(FormText)) {
					FormText = mRemoteNG.My.Language.strNewPanel;
				}

				this.WindowType = Type.Connection;
				this.DockPnl = Panel;
				this.InitializeComponent();
				this.Text = FormText;
				this.TabText = FormText;
			}

			public Magic.Controls.TabPage AddConnectionTab(mRemoteNG.Connection.Info conI)
			{
				try {
					Crownwood.Magic.Controls.TabPage nTab = new Crownwood.Magic.Controls.TabPage();
					nTab.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

					if (mRemoteNG.My.Settings.ShowProtocolOnTabs) {
						nTab.Title = conI.Protocol.ToString() + ": ";
					} else {
						nTab.Title = "";
					}

					nTab.Title += conI.Name;

					if (mRemoteNG.My.Settings.ShowLogonInfoOnTabs) {
						nTab.Title += " (";

						if (!string.IsNullOrEmpty(conI.Domain)) {
							nTab.Title += conI.Domain;
						}

						if (!string.IsNullOrEmpty(conI.Username)) {
							if (!string.IsNullOrEmpty(conI.Domain)) {
								nTab.Title += "\\";
							}

							nTab.Title += conI.Username;
						}

						nTab.Title += ")";
					}

					nTab.Title = nTab.Title.Replace("&", "&&");

					System.Drawing.Icon conIcon = mRemoteNG.Connection.Icon.FromString(conI.Icon);
					if (conIcon != null) {
						nTab.Icon = conIcon;
					}

					if (mRemoteNG.My.Settings.OpenTabsRightOfSelected) {
						this.TabController.TabPages.Insert(this.TabController.SelectedIndex + 1, nTab);
					} else {
						this.TabController.TabPages.Add(nTab);
					}

					nTab.Selected = true;
					_ignoreChangeSelectedTabClick = false;

					return nTab;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "AddConnectionTab (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}

				return null;
			}
			#endregion

			#region "Private Methods"
			public void UpdateSelectedConnection()
			{
				if (TabController.SelectedTab == null) {
					My.MyProject.Forms.frmMain.SelectedConnection = null;
				} else {
					InterfaceControl interfaceControl = TabController.SelectedTab.Tag as InterfaceControl;
					if (interfaceControl == null) {
						My.MyProject.Forms.frmMain.SelectedConnection = null;
					} else {
						My.MyProject.Forms.frmMain.SelectedConnection = interfaceControl.Info;
					}
				}
			}
			#endregion

			#region "Form"
			private void Connection_Load(object sender, System.EventArgs e)
			{
				ApplyLanguage();
			}

			private bool _documentHandlersAdded = false;
			private bool _floatHandlersAdded = false;
			private void Connection_DockStateChanged(System.Object sender, EventArgs e)
			{
				if (DockState == DockState.Float) {
					if (_documentHandlersAdded) {
						My.MyProject.Forms.frmMain.ResizeBegin -= Connection_ResizeBegin;
						My.MyProject.Forms.frmMain.ResizeEnd -= Connection_ResizeEnd;
						_documentHandlersAdded = false;
					}
					DockHandler.FloatPane.FloatWindow.ResizeBegin += Connection_ResizeBegin;
					DockHandler.FloatPane.FloatWindow.ResizeEnd += Connection_ResizeEnd;
					_floatHandlersAdded = true;
				} else if (DockState == DockState.Document) {
					if (_floatHandlersAdded) {
						DockHandler.FloatPane.FloatWindow.ResizeBegin -= Connection_ResizeBegin;
						DockHandler.FloatPane.FloatWindow.ResizeEnd -= Connection_ResizeEnd;
						_floatHandlersAdded = false;
					}
					My.MyProject.Forms.frmMain.ResizeBegin += Connection_ResizeBegin;
					My.MyProject.Forms.frmMain.ResizeEnd += Connection_ResizeEnd;
					_documentHandlersAdded = true;
				}
			}

			private void ApplyLanguage()
			{
				cmenTabFullscreen.Text = mRemoteNG.My.Language.strMenuFullScreenRDP;
				cmenTabSmartSize.Text = mRemoteNG.My.Language.strMenuSmartSize;
				cmenTabViewOnly.Text = mRemoteNG.My.Language.strMenuViewOnly;
				cmenTabScreenshot.Text = mRemoteNG.My.Language.strMenuScreenshot;
				cmenTabStartChat.Text = mRemoteNG.My.Language.strMenuStartChat;
				cmenTabTransferFile.Text = mRemoteNG.My.Language.strMenuTransferFile;
				cmenTabRefreshScreen.Text = mRemoteNG.My.Language.strMenuRefreshScreen;
				cmenTabSendSpecialKeys.Text = mRemoteNG.My.Language.strMenuSendSpecialKeys;
				cmenTabSendSpecialKeysCtrlAltDel.Text = mRemoteNG.My.Language.strMenuCtrlAltDel;
				cmenTabSendSpecialKeysCtrlEsc.Text = mRemoteNG.My.Language.strMenuCtrlEsc;
				cmenTabExternalApps.Text = mRemoteNG.My.Language.strMenuExternalTools;
				cmenTabRenameTab.Text = mRemoteNG.My.Language.strMenuRenameTab;
				cmenTabDuplicateTab.Text = mRemoteNG.My.Language.strMenuDuplicateTab;
				cmenTabReconnect.Text = mRemoteNG.My.Language.strMenuReconnect;
				cmenTabDisconnect.Text = mRemoteNG.My.Language.strMenuDisconnect;
				cmenTabPuttySettings.Text = mRemoteNG.My.Language.strPuttySettings;
			}

			private void Connection_FormClosing(object sender, FormClosingEventArgs e)
			{
				if (!My.MyProject.Forms.frmMain.IsClosing & ((mRemoteNG.My.Settings.ConfirmCloseConnection == ConfirmClose.All & TabController.TabPages.Count > 0) | (mRemoteNG.My.Settings.ConfirmCloseConnection == ConfirmClose.Multiple & TabController.TabPages.Count > 1))) {
					DialogResult result = cTaskDialog.MessageBox(this, mRemoteNG.My.MyProject.Application.Info.ProductName, string.Format(mRemoteNG.My.Language.strConfirmCloseConnectionPanelMainInstruction, this.Text), "", "", "", mRemoteNG.My.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, null);
					if (cTaskDialog.VerificationChecked) {
						mRemoteNG.My.Settings.ConfirmCloseConnection = mRemoteNG.My.Settings.ConfirmCloseConnection - 1;
					}
					if (result == DialogResult.No) {
						e.Cancel = true;
						return;
					}
				}

				try {
					foreach (Crownwood.Magic.Controls.TabPage tabP in this.TabController.TabPages) {
						if (tabP.Tag != null) {
							mRemoteNG.Connection.InterfaceControl interfaceControl = tabP.Tag;
							interfaceControl.Protocol.Close();
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "UI.Window.Connection.Connection_FormClosing() failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			public new event EventHandler ResizeBegin;
			private void Connection_ResizeBegin(System.Object sender, EventArgs e)
			{
				if (ResizeBegin != null) {
					ResizeBegin(this, e);
				}
			}

			public new event EventHandler ResizeEnd;
			public void Connection_ResizeEnd(System.Object sender, EventArgs e)
			{
				if (ResizeEnd != null) {
					ResizeEnd(sender, e);
				}
			}
			#endregion

			#region "TabController"
			private void TabController_ClosePressed(object sender, System.EventArgs e)
			{
				if (this.TabController.SelectedTab == null) {
					return;
				}

				this.CloseConnectionTab();
			}

			private void CloseConnectionTab()
			{
				Crownwood.Magic.Controls.TabPage selectedTab = TabController.SelectedTab;
				if (mRemoteNG.My.Settings.ConfirmCloseConnection == ConfirmClose.All) {
					DialogResult result = cTaskDialog.MessageBox(this, mRemoteNG.My.MyProject.Application.Info.ProductName, string.Format(mRemoteNG.My.Language.strConfirmCloseConnectionMainInstruction, selectedTab.Title), "", "", "", mRemoteNG.My.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, null);
					if (cTaskDialog.VerificationChecked) {
						mRemoteNG.My.Settings.ConfirmCloseConnection = mRemoteNG.My.Settings.ConfirmCloseConnection - 1;
					}
					if (result == DialogResult.No) {
						return;
					}
				}

				try {
					if (selectedTab.Tag != null) {
						mRemoteNG.Connection.InterfaceControl interfaceControl = selectedTab.Tag;
						interfaceControl.Protocol.Close();
					} else {
						CloseTab(selectedTab);
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "UI.Window.Connection.CloseConnectionTab() failed" + Constants.vbNewLine + ex.Message, true);
				}

				UpdateSelectedConnection();
			}

			private void TabController_DoubleClickTab(Crownwood.Magic.Controls.TabControl sender, Crownwood.Magic.Controls.TabPage page)
			{
				_firstClickTicks = 0;
				if (mRemoteNG.My.Settings.DoubleClickOnTabClosesIt) {
					this.CloseConnectionTab();
				}
			}

			#region "Drag and Drop"
			private void TabController_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
			{
				if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true)) {
					mRemoteNG.App.Runtime.OpenConnection(e.Data.GetData("System.Windows.Forms.TreeNode", true).Tag, this, mRemoteNG.Connection.Info.Force.DoNotJump);
				}
			}

			private void TabController_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
			{
				if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true)) {
					e.Effect = DragDropEffects.Move;
				}
			}

			private void TabController_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
			{
				e.Effect = DragDropEffects.Move;
			}
			#endregion
			#endregion

			#region "Tab Menu"
			private void ShowHideMenuButtons()
			{
				try {
					if (this.TabController.SelectedTab == null) {
						return;
					}

					mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

					if (IC == null) {
						return;
					}

					if (IC.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP) {
						mRemoteNG.Connection.Protocol.RDP rdp = IC.Protocol;

						cmenTabFullscreen.Enabled = true;
						cmenTabFullscreen.Checked = rdp.Fullscreen;

						cmenTabSmartSize.Enabled = true;
						cmenTabSmartSize.Checked = rdp.SmartSize;
					} else {
						cmenTabFullscreen.Enabled = false;
						cmenTabSmartSize.Enabled = false;
					}

					if (IC.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.VNC) {
						this.cmenTabSendSpecialKeys.Enabled = true;
						this.cmenTabViewOnly.Enabled = true;

						this.cmenTabSmartSize.Enabled = true;
						this.cmenTabStartChat.Enabled = true;
						this.cmenTabRefreshScreen.Enabled = true;
						this.cmenTabTransferFile.Enabled = false;

						mRemoteNG.Connection.Protocol.VNC vnc = IC.Protocol;
						this.cmenTabSmartSize.Checked = vnc.SmartSize;
						this.cmenTabViewOnly.Checked = vnc.ViewOnly;
					} else {
						this.cmenTabSendSpecialKeys.Enabled = false;
						this.cmenTabViewOnly.Enabled = false;
						this.cmenTabStartChat.Enabled = false;
						this.cmenTabRefreshScreen.Enabled = false;
						this.cmenTabTransferFile.Enabled = false;
					}

					if (IC.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.SSH1 | IC.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.SSH2) {
						this.cmenTabTransferFile.Enabled = true;
					}

					if (IC.Protocol is mRemoteNG.Connection.Protocol.PuttyBase) {
						this.cmenTabPuttySettings.Enabled = true;
					} else {
						this.cmenTabPuttySettings.Enabled = false;
					}

					AddExternalApps();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "ShowHideMenuButtons (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void cmenTabScreenshot_Click(System.Object sender, System.EventArgs e)
			{
				cmenTab.Close();
				Application.DoEvents();
				System.Windows.screenshotForm.AddScreenshot(mRemoteNG.Tools.Misc.TakeScreenshot(this));
			}

			private void cmenTabSmartSize_Click(System.Object sender, System.EventArgs e)
			{
				this.ToggleSmartSize();
			}

			private void cmenTabReconnect_Click(System.Object sender, System.EventArgs e)
			{
				this.Reconnect();
			}

			private void cmenTabTransferFile_Click(System.Object sender, System.EventArgs e)
			{
				this.TransferFile();
			}

			private void cmenTabViewOnly_Click(System.Object sender, System.EventArgs e)
			{
				this.ToggleViewOnly();
			}

			private void cmenTabStartChat_Click(object sender, System.EventArgs e)
			{
				this.StartChat();
			}

			private void cmenTabRefreshScreen_Click(object sender, System.EventArgs e)
			{
				this.RefreshScreen();
			}

			private void cmenTabSendSpecialKeysCtrlAltDel_Click(System.Object sender, System.EventArgs e)
			{
				this.SendSpecialKeys(mRemoteNG.Connection.Protocol.VNC.SpecialKeys.CtrlAltDel);
			}

			private void cmenTabSendSpecialKeysCtrlEsc_Click(System.Object sender, System.EventArgs e)
			{
				this.SendSpecialKeys(mRemoteNG.Connection.Protocol.VNC.SpecialKeys.CtrlEsc);
			}

			private void cmenTabFullscreen_Click(System.Object sender, System.EventArgs e)
			{
				this.ToggleFullscreen();
			}

			private void cmenTabPuttySettings_Click(System.Object sender, System.EventArgs e)
			{
				this.ShowPuttySettingsDialog();
			}

			private void cmenTabExternalAppsEntry_Click(object sender, System.EventArgs e)
			{
				StartExternalApp(sender.Tag);
			}

			private void cmenTabDisconnect_Click(System.Object sender, System.EventArgs e)
			{
				this.CloseTabMenu();
			}

			private void cmenTabDuplicateTab_Click(System.Object sender, System.EventArgs e)
			{
				this.DuplicateTab();
			}

			private void cmenTabRenameTab_Click(System.Object sender, System.EventArgs e)
			{
				this.RenameTab();
			}
			#endregion

			#region "Tab Actions"
			private void ToggleSmartSize()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							if (IC.Protocol is mRemoteNG.Connection.Protocol.RDP) {
								mRemoteNG.Connection.Protocol.RDP rdp = IC.Protocol;
								rdp.ToggleSmartSize();
							} else if (IC.Protocol is mRemoteNG.Connection.Protocol.VNC) {
								mRemoteNG.Connection.Protocol.VNC vnc = IC.Protocol;
								vnc.ToggleSmartSize();
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "ToggleSmartSize (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void TransferFile()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							if (IC.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.SSH1 | IC.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.SSH2) {
								SSHTransferFile();
							} else if (IC.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.VNC) {
								VNCTransferFile();
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "TransferFile (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SSHTransferFile()
			{

				try {
					mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

					System.Windows.Show(Type.SSHTransfer);

					mRemoteNG.Connection.Info conI = IC.Info;

					System.Windows.sshtransferForm.Hostname = conI.Hostname;
					System.Windows.sshtransferForm.Username = conI.Username;
					System.Windows.sshtransferForm.Password = conI.Password;
					System.Windows.sshtransferForm.Port = conI.Port;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "SSHTransferFile (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void VNCTransferFile()
			{
				try {
					mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;
					mRemoteNG.Connection.Protocol.VNC vnc = IC.Protocol;
					vnc.StartFileTransfer();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "VNCTransferFile (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void ToggleViewOnly()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							if (IC.Protocol is mRemoteNG.Connection.Protocol.VNC) {
								cmenTabViewOnly.Checked = !cmenTabViewOnly.Checked;

								mRemoteNG.Connection.Protocol.VNC vnc = IC.Protocol;
								vnc.ToggleViewOnly();
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "ToggleViewOnly (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void StartChat()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							if (IC.Protocol is mRemoteNG.Connection.Protocol.VNC) {
								mRemoteNG.Connection.Protocol.VNC vnc = IC.Protocol;
								vnc.StartChat();
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "StartChat (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void RefreshScreen()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							if (IC.Protocol is mRemoteNG.Connection.Protocol.VNC) {
								mRemoteNG.Connection.Protocol.VNC vnc = IC.Protocol;
								vnc.RefreshScreen();
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "RefreshScreen (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SendSpecialKeys(mRemoteNG.Connection.Protocol.VNC.SpecialKeys Keys)
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							if (IC.Protocol is mRemoteNG.Connection.Protocol.VNC) {
								mRemoteNG.Connection.Protocol.VNC vnc = IC.Protocol;
								vnc.SendSpecialKeys(Keys);
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "SendSpecialKeys (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void ToggleFullscreen()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							if (IC.Protocol is mRemoteNG.Connection.Protocol.RDP) {
								mRemoteNG.Connection.Protocol.RDP rdp = IC.Protocol;
								rdp.ToggleFullscreen();
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "ToggleFullscreen (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void ShowPuttySettingsDialog()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl objInterfaceControl = this.TabController.SelectedTab.Tag;

							if (objInterfaceControl.Protocol is mRemoteNG.Connection.Protocol.PuttyBase) {
								mRemoteNG.Connection.Protocol.PuttyBase objPuttyBase = objInterfaceControl.Protocol;

								objPuttyBase.ShowSettingsDialog();
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "ShowPuttySettingsDialog (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void AddExternalApps()
			{
				try {
					//clean up
					cmenTabExternalApps.DropDownItems.Clear();

					//add ext apps
					foreach (Tools.ExternalTool extA in Runtime.ExternalTools) {
						ToolStripMenuItem nItem = new ToolStripMenuItem();
						nItem.Text = extA.DisplayName;
						nItem.Tag = extA;

						nItem.Image = extA.Image;

						nItem.Click += cmenTabExternalAppsEntry_Click;

						cmenTabExternalApps.DropDownItems.Add(nItem);
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "cMenTreeTools_DropDownOpening failed (UI.Window.Tree)" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void StartExternalApp(Tools.ExternalTool ExtA)
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							ExtA.Start(IC.Info);
						}
					}

				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "cmenTabExternalAppsEntry_Click failed (UI.Window.Tree)" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void CloseTabMenu()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							IC.Protocol.Close();
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "CloseTabMenu (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void DuplicateTab()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							mRemoteNG.App.Runtime.OpenConnection(IC.Info, mRemoteNG.Connection.Info.Force.DoNotJump);
							_ignoreChangeSelectedTabClick = false;
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "DuplicateTab (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void Reconnect()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;
							mRemoteNG.Connection.Info conI = IC.Info;

							IC.Protocol.Close();

							mRemoteNG.App.Runtime.OpenConnection(conI, mRemoteNG.Connection.Info.Force.DoNotJump);
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Reconnect (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void RenameTab()
			{
				try {
					string nTitle = Interaction.InputBox(mRemoteNG.My.Language.strNewTitle + ":", , this.TabController.SelectedTab.Title.Replace("&&", "&"));

					if (!string.IsNullOrEmpty(nTitle)) {
						this.TabController.SelectedTab.Title = nTitle.Replace("&", "&&");
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "RenameTab (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Protocols"
			public void Prot_Event_Closed(object sender)
			{
				mRemoteNG.Connection.Protocol.Base Prot = sender;
				CloseTab(Prot.InterfaceControl.Parent);
			}
			#endregion

			#region "Tabs"
			private delegate void CloseTabCB(Crownwood.Magic.Controls.TabPage TabToBeClosed);
			private void CloseTab(Crownwood.Magic.Controls.TabPage TabToBeClosed)
			{
				if (this.TabController.InvokeRequired) {
					CloseTabCB s = new CloseTabCB(CloseTab);

					try {
						this.TabController.Invoke(s, TabToBeClosed);
					} catch (System.Runtime.InteropServices.COMException comEx) {
						this.TabController.Invoke(s, TabToBeClosed);
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't close tab" + Constants.vbNewLine + ex.Message, true);
					}
				} else {
					try {
						this.TabController.TabPages.Remove(TabToBeClosed);
						_ignoreChangeSelectedTabClick = false;
					} catch (System.Runtime.InteropServices.COMException comEx) {
						CloseTab(TabToBeClosed);
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't close tab" + Constants.vbNewLine + ex.Message, true);
					}

					if (this.TabController.TabPages.Count == 0) {
						this.Close();
					}
				}
			}

			private bool _ignoreChangeSelectedTabClick = false;
			private void TabController_SelectionChanged(object sender, EventArgs e)
			{
				_ignoreChangeSelectedTabClick = true;
				UpdateSelectedConnection();
				FocusIC();
				RefreshIC();
			}

			private int _firstClickTicks = 0;
			private Rectangle _doubleClickRectangle;
			private void TabController_MouseUp(object sender, MouseEventArgs e)
			{
				try {
					if (!(Native.GetForegroundWindow() == My.MyProject.Forms.frmMain.Handle) & !_ignoreChangeSelectedTabClick) {
						Crownwood.Magic.Controls.TabPage clickedTab = TabController.TabPageFromPoint(e.Location);
						if (clickedTab != null & !object.ReferenceEquals(TabController.SelectedTab, clickedTab)) {
							Native.SetForegroundWindow(Handle);
							TabController.SelectedTab = clickedTab;
						}
					}
					_ignoreChangeSelectedTabClick = false;

					switch (e.Button) {
						case MouseButtons.Left:
							int currentTicks = Environment.TickCount;
							int elapsedTicks = currentTicks - _firstClickTicks;
							if (elapsedTicks > SystemInformation.DoubleClickTime | !_doubleClickRectangle.Contains(MousePosition)) {
								_firstClickTicks = currentTicks;
								_doubleClickRectangle = new Rectangle(MousePosition.X - (SystemInformation.DoubleClickSize.Width / 2), MousePosition.Y - (SystemInformation.DoubleClickSize.Height / 2), SystemInformation.DoubleClickSize.Width, SystemInformation.DoubleClickSize.Height);
								FocusIC();
							} else {
								TabController.OnDoubleClickTab(TabController.SelectedTab);
							}
							break;
						case MouseButtons.Middle:
							CloseConnectionTab();
							break;
						case MouseButtons.Right:
							ShowHideMenuButtons();
							Native.SetForegroundWindow(Handle);
							cmenTab.Show(TabController, e.Location);
							break;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "TabController_MouseUp (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void FocusIC()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;
							IC.Protocol.Focus();
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "FocusIC (UI.Window.Connections) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			public void RefreshIC()
			{
				try {
					if (this.TabController.SelectedTab != null) {
						if (this.TabController.SelectedTab.Tag is mRemoteNG.Connection.InterfaceControl) {
							mRemoteNG.Connection.InterfaceControl IC = this.TabController.SelectedTab.Tag;

							if (IC.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.VNC) {
								(IC.Protocol as mRemoteNG.Connection.Protocol.VNC).RefreshScreen();
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "RefreshIC (UI.Window.Connection) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Window Overrides"
			protected override void WndProc(ref Message m)
			{
				try {
					if (m.Msg == Native.WM_MOUSEACTIVATE) {
						Crownwood.Magic.Controls.TabPage selectedTab = TabController.SelectedTab;
						if (selectedTab != null) {
							Rectangle tabClientRectangle = selectedTab.RectangleToScreen(selectedTab.ClientRectangle);
							if (tabClientRectangle.Contains(MousePosition)) {
								InterfaceControl interfaceControl = TabController.SelectedTab.Tag as InterfaceControl;
								if (interfaceControl != null && interfaceControl.Info != null) {
									if (interfaceControl.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP) {
										interfaceControl.Protocol.Focus();
										return;
										// Do not pass to base class
									}
								}
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("UI.Window.Connection.WndProc() failed.", ex, , true);
				}

				base.WndProc(ref m);
			}
			#endregion

			#region "Tab drag and drop"
			public bool InTabDrag { get; set; }
			private void TabController_PageDragStart(object sender, MouseEventArgs e)
			{
				Cursor = Cursors.SizeWE;
			}

			private void TabController_PageDragMove(object sender, MouseEventArgs e)
			{
				InTabDrag = true;
				// For some reason PageDragStart gets raised again after PageDragEnd so set this here instead

				Crownwood.Magic.Controls.TabPage sourceTab = TabController.SelectedTab;
				Crownwood.Magic.Controls.TabPage destinationTab = TabController.TabPageFromPoint(e.Location);

				if (!TabController.TabPages.Contains(destinationTab) | object.ReferenceEquals(sourceTab, destinationTab))
					return;

				int targetIndex = TabController.TabPages.IndexOf(destinationTab);

				TabController.TabPages.SuspendEvents();
				TabController.TabPages.Remove(sourceTab);
				TabController.TabPages.Insert(targetIndex, sourceTab);
				TabController.SelectedTab = sourceTab;
				TabController.TabPages.ResumeEvents();
			}

			private void TabController_PageDragEnd(object sender, MouseEventArgs e)
			{
				Cursor = Cursors.Default;
				InTabDrag = false;
				mRemoteNG.Connection.InterfaceControl interfaceControl = TabController.SelectedTab.Tag as mRemoteNG.Connection.InterfaceControl;
				if (interfaceControl != null)
					interfaceControl.Protocol.Focus();
			}
			#endregion
		}
	}
}
