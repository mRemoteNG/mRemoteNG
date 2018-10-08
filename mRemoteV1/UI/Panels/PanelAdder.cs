using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Forms.Input;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Panels
{
    public class PanelAdder
    {
        public Form AddPanel(string title = "", bool noTabber = false)
        {
            try
            {
                var connectionForm = new ConnectionWindow(new DockContent());
                BuildConnectionWindowContextMenu(connectionForm);
                SetConnectionWindowTitle(title, connectionForm);
                ShowConnectionWindow(connectionForm);
                PrepareTabControllerSupport(noTabber, connectionForm);
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

        private static void PrepareTabControllerSupport(bool noTabber, ConnectionWindow connectionForm)
        {
            if (noTabber)
                connectionForm.TabController.Dispose();
            else
                Runtime.WindowList.Add(connectionForm);
        }

        private static void SetConnectionWindowTitle(string title, ConnectionWindow connectionForm)
        {
            if (title == "")
                title = Language.strNewPanel;
            connectionForm.SetFormText(title.Replace("&", "&&"));
        }

        private static void BuildConnectionWindowContextMenu(DockContent pnlcForm)
        {
            var cMen = new ContextMenuStrip();
            var cMenRen = CreateRenameMenuItem(pnlcForm);
            var cMenScreens = CreateScreensMenuItem(pnlcForm);
            cMen.Items.AddRange(new ToolStripItem[] { cMenRen, cMenScreens });
            pnlcForm.TabPageContextMenuStrip = cMen;
        }

        private static ToolStripMenuItem CreateScreensMenuItem(DockContent pnlcForm)
        {
            var cMenScreens = new ToolStripMenuItem
            {
                Text = Language.strSendTo,
                Image = Resources.Monitor,
                Tag = pnlcForm
            };
            cMenScreens.DropDownItems.Add("Dummy");
            cMenScreens.DropDownOpening += cMenConnectionPanelScreens_DropDownOpening;
            return cMenScreens;
        }

        private static ToolStripMenuItem CreateRenameMenuItem(DockContent pnlcForm)
        {
            var cMenRen = new ToolStripMenuItem
            {
                Text = Language.strRename,
                Image = Resources.Rename,
                Tag = pnlcForm
            };
            cMenRen.Click += cMenConnectionPanelRename_Click;
            return cMenRen;
        }

        private static void cMenConnectionPanelRename_Click(object sender, EventArgs e)
        {
            try
            {
                var conW = (ConnectionWindow)((ToolStripMenuItem)sender).Tag;

                var nTitle = "";
                input.InputBox(Language.strNewTitle, Language.strNewTitle + ":", ref nTitle);

                if (!string.IsNullOrEmpty(nTitle))
                {
                    conW.SetFormText(nTitle.Replace("&", "&&"));
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelRename_Click: Caught Exception: ", ex);
            }
        }

        private static void cMenConnectionPanelScreens_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                var cMenScreens = (ToolStripMenuItem)sender;
                cMenScreens.DropDownItems.Clear();

                for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
                {
                    var cMenScreen = new ToolStripMenuItem(Language.strScreen + " " + Convert.ToString(i + 1))
                    {
                        Tag = new ArrayList(),
                        Image = Resources.Monitor_GoTo
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

        private static void cMenConnectionPanelScreen_Click(object sender, EventArgs e)
        {
            Screen screen = null;
            DockContent panel = null;
            try
            {
                var tagEnumeration = (IEnumerable)((ToolStripMenuItem)sender).Tag;
                if (tagEnumeration == null) return;
                foreach (var obj in tagEnumeration)
                {
                    var screen1 = obj as Screen;
                    if (screen1 != null)
                    {
                        screen = screen1;
                    }
                    else if (obj is DockContent)
                    {
                        panel = (DockContent)obj;
                    }
                }
                Screens.SendPanelToScreen(panel, screen);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelScreen_Click: Caught Exception: ", ex);
            }
        }
    }
}