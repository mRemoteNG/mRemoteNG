using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms.Input;
using mRemoteNG.UI.Window;
using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Panels
{
    public class PanelAdder
    {
        private readonly WindowList _windowList;
        private readonly Screens _screens;
        private readonly Func<ConnectionWindow> _connectionWindowBuilder;
        private readonly DockPanel _dockPanel;

        public PanelAdder(WindowList windowList, Func<ConnectionWindow> connectionWindowBuilder, Screens screens, DockPanel dockPanel)
        {
            _connectionWindowBuilder = connectionWindowBuilder;
            _screens = screens.ThrowIfNull(nameof(screens));
            _windowList = windowList.ThrowIfNull(nameof(windowList));
            _dockPanel = dockPanel.ThrowIfNull(nameof(dockPanel));
        }

        public Form AddPanel(string title = "", bool noTabber = false)
        {
            try
            {
                var connectionForm = _connectionWindowBuilder();
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
            return _windowList
                .OfType<ConnectionWindow>()
                .Any(w => w.TabText == panelName);
        }

        private void ShowConnectionWindow(ConnectionWindow connectionForm)
        {
            connectionForm.Show(_dockPanel, DockState.Document);
        }

        private void PrepareTabControllerSupport(bool noTabber, ConnectionWindow connectionForm)
        {
            if (noTabber)
                connectionForm.TabController.Dispose();
            else
                _windowList.Add(connectionForm);
        }

        private static void SetConnectionWindowTitle(string title, ConnectionWindow connectionForm)
        {
            if (title == "")
                title = Language.strNewPanel;
            connectionForm.SetFormText(title.Replace("&", "&&"));
        }

        private void BuildConnectionWindowContextMenu(DockContent pnlcForm)
        {
            var cMen = new ContextMenuStrip();
            var cMenRen = CreateRenameMenuItem(pnlcForm);
            var cMenScreens = CreateScreensMenuItem(pnlcForm);
            cMen.Items.AddRange(new ToolStripItem[] { cMenRen, cMenScreens });
            pnlcForm.TabPageContextMenuStrip = cMen;
        }

        private ToolStripMenuItem CreateScreensMenuItem(DockContent pnlcForm)
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

        private void cMenConnectionPanelScreens_DropDownOpening(object sender, EventArgs e)
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

        private void cMenConnectionPanelScreen_Click(object sender, EventArgs e)
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
                _screens.SendPanelToScreen(panel, screen);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenConnectionPanelScreen_Click: Caught Exception: ", ex);
            }
        }
    }
}