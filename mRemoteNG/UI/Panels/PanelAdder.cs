using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Panels
{
    [SupportedOSPlatform("windows")]
    public static class PanelAdder
    {
        public static ConnectionWindow? AddPanel(string title = "", bool showImmediately = true)
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
                PrepareTabSupport(connectionForm);
                return connectionForm;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Couldn\'t add panel" + Environment.NewLine + ex.Message);
                return null;
            }
        }

        public static bool DoesPanelExist(string panelName)
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
                title = "New Panel";
            connectionForm.SetFormText(title.Replace("&", "&&", StringComparison.Ordinal));
        }

        private static void BuildConnectionWindowContextMenu(DockContent pnlcForm)
        {
            ContextMenuStrip cMen = new();
            ToolStripMenuItem cMenRen = CreateRenameMenuItem(pnlcForm);
            ToolStripMenuItem cMenScreens = CreateScreensMenuItem(pnlcForm);
            ToolStripMenuItem cMenClose = CreateCloseMenuItem(pnlcForm);
            cMen.Items.AddRange(new ToolStripItem[] {cMenRen, cMenScreens, cMenClose});
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
            cMenScreens.DropDownOpening += cMenConnectionPanelScreens_DropDownOpening;
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

        private static void cMenConnectionPanelRename_Click(object? sender, EventArgs e)
        {
            try
            {
                if (sender is not ToolStripMenuItem menuItem || menuItem.Tag is not ConnectionWindow conW)
                    return;

                using (FrmInputBox newTitle = new(Language.NewTitle, Language.NewTitle + ":", ""))
                    if (newTitle.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(newTitle.returnValue))
                        conW.SetFormText(newTitle.returnValue.Replace("&", "&&", StringComparison.Ordinal));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelRename_Click: Caught Exception: ", ex);
            }
        }

        private static void cMenConnectionPanelClose_Click(object? sender, EventArgs e)
        {
            try
            {
                if (sender is not ToolStripMenuItem menuItem || menuItem.Tag is not ConnectionWindow conW)
                    return;
                conW.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelClose_Click: Caught Exception: ", ex);
            }
        }

        private static void cMenConnectionPanelScreens_DropDownOpening(object? sender, EventArgs e)
        {
            try
            {
                if (sender is not ToolStripMenuItem cMenScreens) return;
                cMenScreens.DropDownItems.Clear();

                for (int i = 0; i <= Screen.AllScreens.Length - 1; i++)
                {
                    ToolStripMenuItem cMenScreen = new(Language.Screen + " " + Convert.ToString(i + 1, CultureInfo.InvariantCulture))
                    {
                        Tag = new ArrayList(),
                        Image = Properties.Resources.Monitor_16x
                    };
                    ((ArrayList)cMenScreen.Tag).Add(Screen.AllScreens[i]);
                    ((ArrayList)cMenScreen.Tag).Add(cMenScreens.Tag);
                    cMenScreen.Click += cMenConnectionPanelScreen_Click;
                    cMenScreens.DropDownItems.Add(cMenScreen);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelScreens_DropDownOpening: Caught Exception: ", ex);
            }
        }

        private static void cMenConnectionPanelScreen_Click(object? sender, EventArgs e)
        {
            Screen? screen = null;
            DockContent? panel = null;
            try
            {
                if (sender is not ToolStripMenuItem menuItem || menuItem.Tag is not IEnumerable tagEnumeration)
                    return;
                foreach (object obj in tagEnumeration)
                {
                    if (obj is Screen screen1)
                    {
                        screen = screen1;
                    }
                    else if (obj is DockContent dockContent)
                    {
                        panel = dockContent;
                    }
                }

                if (panel != null && screen != null)
                    Screens.SendPanelToScreen(panel, screen);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelScreen_Click: Caught Exception: ", ex);
            }
        }
    }
}