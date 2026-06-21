using System;
using System.Windows.Forms;
using mRemoteNG.UI.Window;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class SessionsMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenSessionsNextSession;
        private ToolStripMenuItem _mMenSessionsPreviousSession;
        private ToolStripSeparator _mMenSessionsSep1;
        private readonly ToolStripMenuItem[] _sessionNumberItems = new ToolStripMenuItem[9];

        public SessionsMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenSessionsNextSession = new ToolStripMenuItem();
            _mMenSessionsPreviousSession = new ToolStripMenuItem();
            _mMenSessionsSep1 = new ToolStripSeparator();

            // Initialize session number menu items (Ctrl+1 through Ctrl+9)
            for (int i = 0; i < 9; i++)
            {
                _sessionNumberItems[i] = new ToolStripMenuItem();
            }

            // 
            // mMenSessions
            // 
            DropDownItems.Add(_mMenSessionsNextSession);
            DropDownItems.Add(_mMenSessionsPreviousSession);
            DropDownItems.Add(_mMenSessionsSep1);

            for (int i = 0; i < 9; i++)
            {
                DropDownItems.Add(_sessionNumberItems[i]);
            }

            Name = "mMenSessions";
            Size = new System.Drawing.Size(61, 20);
            Text = Language._Sessions;

            // 
            // mMenSessionsNextSession
            // 
            _mMenSessionsNextSession.Name = "mMenSessionsNextSession";
            _mMenSessionsNextSession.ShortcutKeys = Keys.Control | Keys.Right;
            _mMenSessionsNextSession.Size = new System.Drawing.Size(230, 22);
            _mMenSessionsNextSession.Text = Language.NextSession;
            _mMenSessionsNextSession.Click += mMenSessionsNextSession_Click;

            // 
            // mMenSessionsPreviousSession
            // 
            _mMenSessionsPreviousSession.Name = "mMenSessionsPreviousSession";
            _mMenSessionsPreviousSession.ShortcutKeys = Keys.Control | Keys.Left;
            _mMenSessionsPreviousSession.Size = new System.Drawing.Size(230, 22);
            _mMenSessionsPreviousSession.Text = Language.PreviousSession;
            _mMenSessionsPreviousSession.Click += mMenSessionsPreviousSession_Click;

            // 
            // mMenSessionsSep1
            // 
            _mMenSessionsSep1.Name = "mMenSessionsSep1";
            _mMenSessionsSep1.Size = new System.Drawing.Size(227, 6);

            // Initialize session number items (Ctrl+1 through Ctrl+9)
            for (int i = 0; i < 9; i++)
            {
                int sessionNumber = i + 1;
                _sessionNumberItems[i].Name = $"mMenSessionsSession{sessionNumber}";
                _sessionNumberItems[i].ShortcutKeys = Keys.Control | (Keys)((int)Keys.D1 + i);
                _sessionNumberItems[i].Size = new System.Drawing.Size(230, 22);
                _sessionNumberItems[i].Text = string.Format(Language.JumpToSession.ToString(), sessionNumber);
                _sessionNumberItems[i].Enabled = false; // Initialize as disabled
                int capturedIndex = i; // Capture the index for the lambda
                _sessionNumberItems[i].Click += (s, e) => JumpToSessionNumber(capturedIndex);
            }

            // Initialize navigation items as disabled
            _mMenSessionsNextSession.Enabled = false;
            _mMenSessionsPreviousSession.Enabled = false;

            // Hook up the dropdown opening event to update enabled state
            DropDownOpening += SessionsMenu_DropDownOpening;
        }

        public void ApplyLanguage()
        {
            Text = Language._Sessions;
            _mMenSessionsNextSession.Text = Language.NextSession;
            _mMenSessionsPreviousSession.Text = Language.PreviousSession;

            for (int i = 0; i < 9; i++)
            {
                _sessionNumberItems[i].Text = string.Format(Language.JumpToSession.ToString(), i + 1);
            }
        }

        public void UpdateMenuState()
        {
            // Update enabled state of menu items based on active sessions
            var connectionWindow = GetActiveConnectionWindow();
            bool hasMultipleSessions = false;
            int sessionCount = 0;

            if (connectionWindow != null)
            {
                var documents = connectionWindow.GetDocuments();
                sessionCount = documents.Length;
                hasMultipleSessions = sessionCount > 1;
            }

            _mMenSessionsNextSession.Enabled = hasMultipleSessions;
            _mMenSessionsPreviousSession.Enabled = hasMultipleSessions;

            // Enable/disable session number items based on session count
            for (int i = 0; i < 9; i++)
            {
                _sessionNumberItems[i].Enabled = (i < sessionCount);
            }
        }

        private void SessionsMenu_DropDownOpening(object sender, EventArgs e)
        {
            // Update state when menu is opened (for visual feedback)
            UpdateMenuState();
        }

        private void mMenSessionsNextSession_Click(object sender, EventArgs e)
        {
            var connectionWindow = GetActiveConnectionWindow();
            connectionWindow?.NavigateToNextTab();
        }

        private void mMenSessionsPreviousSession_Click(object sender, EventArgs e)
        {
            var connectionWindow = GetActiveConnectionWindow();
            connectionWindow?.NavigateToPreviousTab();
        }

        private void JumpToSessionNumber(int index)
        {
            var connectionWindow = GetActiveConnectionWindow();
            connectionWindow?.NavigateToTab(index);
        }

        private ConnectionWindow GetActiveConnectionWindow()
        {
            return FrmMain.Default.pnlDock?.ActiveDocument as ConnectionWindow;
        }
    }
}
