using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Panels
{
    [SupportedOSPlatform("windows")]
    public class PanelAdder
    {
        public const string DefaultPanelName = "General";

        public ConnectionWindow AddPanel(string title = "", bool showImmediately = true)
        {
            try
            {
                ConnectionWindow connectionForm = new(new DockContent());
                BuildConnectionWindowContextMenu(connectionForm);
                SetConnectionWindowTitle(title, connectionForm);
                // Only show immediately if requested (for user-created empty panels)
                // When opening connections, we defer showing until first tab is added
                if (showImmediately)
                    ShowConnectionWindow(connectionForm);
                FrmMain.Default?.ShowHidePanelTabs();
                PrepareTabSupport(connectionForm);
                return connectionForm;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Couldn\'t add panel" + Environment.NewLine + ex.Message);
                return null;
            }
        }

        public bool DoesPanelExist(string panelName)
        {
            return Runtime.WindowList?.OfType<ConnectionWindow>().Any(w => w.TabText == panelName)
                ?? false;
        }

        private static void ShowConnectionWindow(ConnectionWindow connectionForm)
        {
            connectionForm.Show(FrmMain.Default.pnlDock, DockState.Document);
        }

        private static void PrepareTabSupport(ConnectionWindow connectionForm)
        {
            Runtime.WindowList.Add(connectionForm);
        }

        private static void SetConnectionWindowTitle(string title, ConnectionWindow connectionForm)
        {
            if (string.IsNullOrEmpty(title))
                title = DefaultPanelName;
            connectionForm.SetFormText(title.Replace("&", "&&"));
        }

        private static void BuildConnectionWindowContextMenu(DockContent pnlcForm)
        {
            ContextMenuStrip cMen = new();
            ToolStripMenuItem cMenRen = CreateRenameMenuItem(pnlcForm);
            ToolStripMenuItem cMenScreens = CreateScreensMenuItem(pnlcForm);
            ToolStripMenuItem cMenClose = CreateCloseMenuItem(pnlcForm);
            cMen.Items.AddRange(new ToolStripItem[] {cMenRen, cMenScreens, cMenClose});

            if (pnlcForm is ConnectionWindow connectionWindow && connectionWindow.IsGeneralPanel)
            {
                cMenClose.Visible = false;
            }

            pnlcForm.TabPageContextMenuStrip = cMen;
        }

        private static ToolStripMenuItem CreateScreensMenuItem(DockContent pnlcForm)
        {
            ToolStripMenuItem cMenScreens = new()
            {
                Text = Language.SendTo,
                Image = Properties.Resources.Monitor_16x,
                Tag = pnlcForm
            };
            cMenScreens.DropDownItems.Add("Dummy");
            cMenScreens.DropDown.ShowItemToolTips = true;
            cMenScreens.DropDownOpening += (sender, e) => PopulateScreensMenu(cMenScreens, pnlcForm);
            cMenScreens.DropDownClosed += (sender, e) => ScreenIdentifier.Hide();
            return cMenScreens;
        }

        private static ToolStripMenuItem CreateRenameMenuItem(DockContent pnlcForm)
        {
            ToolStripMenuItem cMenRen = new()
            {
                Text = Language.Rename,
                Image = Properties.Resources.Rename_16x,
                Tag = pnlcForm
            };
            cMenRen.Click += cMenConnectionPanelRename_Click;
            return cMenRen;
        }

        private static ToolStripMenuItem CreateCloseMenuItem(DockContent pnlcForm)
        {
            ToolStripMenuItem cMenClose = new()
            {
                Text = Language._Close,
                Image = Properties.Resources.Close_16x,
                Tag = pnlcForm
            };
            cMenClose.Click += cMenConnectionPanelClose_Click;
            return cMenClose;
        }

        private static void cMenConnectionPanelRename_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectionWindow conW = (ConnectionWindow)((ToolStripMenuItem)sender).Tag;

                using (FrmInputBox newTitle = new(Language.NewTitle, Language.NewTitle + ":", ""))
                    if (newTitle.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(newTitle.returnValue))
                        conW.SetFormText(newTitle.returnValue.Replace("&", "&&"));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelRename_Click: Caught Exception: ", ex);
            }
        }

        private static void cMenConnectionPanelClose_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectionWindow conW = (ConnectionWindow)((ToolStripMenuItem)sender).Tag;
                if (conW.IsGeneralPanel)
                {
                    return;
                }

                conW.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelClose_Click: Caught Exception: ", ex);
            }
        }

        private static void PopulateScreensMenu(ToolStripMenuItem cMenScreens, DockContent panel)
        {
            try
            {
                cMenScreens.DropDownItems.Clear();

                bool rdpFullscreenByDefault = Properties.OptionsTabsPanelsPage.Default.SendToScreenRdpFullscreen;
                Screen[] screens = Screen.AllScreens;
                for (int i = 0; i <= screens.Length - 1; i++)
                {
                    Screen screen = screens[i];
                    int screenNumber = i + 1;
                    ToolStripMenuItem cMenScreen = new(Language.Screen + " " + Convert.ToString(screenNumber))
                    {
                        Image = Properties.Resources.Monitor_16x,
                        ToolTipText = rdpFullscreenByDefault
                            ? Language.SendToRightClickWindowedHint
                            : Language.SendToRightClickFullscreenHint
                    };
                    // Right-click does the opposite of the configured default. Click fires for right-clicks too but
                    // doesn't carry the button, so it is captured on MouseDown (keyboard activation has no MouseDown).
                    bool rightClicked = false;
                    cMenScreen.MouseDown += (sender, e) => rightClicked = e.Button == MouseButtons.Right;
                    cMenScreen.MouseEnter += (sender, e) => ScreenIdentifier.Show(screen, screenNumber);
                    cMenScreen.MouseLeave += (sender, e) => ScreenIdentifier.Hide();
                    cMenScreen.Click += (sender, e) =>
                    {
                        bool invert = rightClicked;
                        rightClicked = false;
                        cMenConnectionPanelScreen_Click(panel, screen, rdpFullscreenByDefault != invert);
                    };
                    cMenScreens.DropDownItems.Add(cMenScreen);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelScreens_DropDownOpening: Caught Exception: ", ex);
            }
        }

        private static void cMenConnectionPanelScreen_Click(DockContent panel, Screen screen, bool rdpFullscreen)
        {
            try
            {
                ScreenIdentifier.Hide();
                Screens.SendPanelToScreen(panel, screen, rdpFullscreen);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelScreen_Click: Caught Exception: ", ex);
            }
        }
    }
}