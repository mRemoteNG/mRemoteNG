using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.UI.Tabs
{
    /// <summary>
    /// "Log off" for a connection tab, shared by the tab's icon and its right-click menu so both
    /// ask the same question and honour the same "Ask before logging off a session" option.
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal static class SessionLogOff
    {
        /// <summary>Whether the tab's connection can be logged off right now.</summary>
        internal static bool CanLogOff(ConnectionTab tab) =>
            GetLogOff(tab)?.CanLogOff == true;

        /// <summary>Whether the tab's protocol offers log off at all, connected or not.</summary>
        internal static bool SupportsLogOff(ConnectionTab tab) => GetLogOff(tab) != null;

        /// <summary>
        /// Asks for confirmation (unless the user turned that off), then logs the session off.
        /// </summary>
        internal static void Request(ConnectionTab tab, IWin32Window owner)
        {
            ISupportsLogOff logOff = GetLogOff(tab);
            if (logOff is not { CanLogOff: true })
                return;

            string name = (tab.Tag as InterfaceControl)?.Info?.Name ?? tab.TabText;
            if (!Confirm(owner, name))
                return;

            _ = LogOffAsync(logOff, name);
        }

        /// <summary>
        /// Applies the answer to the confirmation. "Don't ask again" only takes effect together with
        /// Yes: someone who answers No has just shown they want to be asked.
        /// </summary>
        /// <returns>Whether to go ahead and log off.</returns>
        internal static bool ApplyConfirmation(Properties.OptionsAdvancedPage settings, DialogResult answer, bool dontAskAgain)
        {
            if (answer != DialogResult.Yes)
                return false;

            // Options > Advanced > "Ask before logging off a session" shows this; ticking it
            // there again brings the question, and this choice, back.
            if (dontAskAgain)
                settings.ConfirmSessionLogOff = false;

            return true;
        }

        private static bool Confirm(IWin32Window owner, string name)
        {
            Properties.OptionsAdvancedPage settings = Properties.OptionsAdvancedPage.Default;
            if (!settings.ConfirmSessionLogOff)
                return true;

            // No is the default: Enter or a stray click must not throw away work in the session.
            DialogResult answer = CTaskDialog.YesNoDefaultNo(owner, GeneralAppInfo.ProductName,
                string.Format(Language.ConfirmLogOffInstruction, name), Language.ConfirmLogOffContent,
                Language.CheckboxDoNotShowThisMessageAgain, ESysIcons.Question);
            bool proceed = ApplyConfirmation(settings, answer, CTaskDialog.VerificationChecked);
            if (!settings.ConfirmSessionLogOff)
                settings.Save();

            return proceed;
        }

        private static readonly System.Collections.Generic.HashSet<ISupportsLogOff> _logOffsInProgress = new();

        private static async Task LogOffAsync(ISupportsLogOff logOff, string name)
        {
            if (!_logOffsInProgress.Add(logOff))
                return;

            try
            {
                await logOff.LogOffAsync();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(string.Format(Language.LogOffFailed, name), ex);
            }
            finally
            {
                _logOffsInProgress.Remove(logOff);
            }
        }

        private static ISupportsLogOff GetLogOff(ConnectionTab tab) =>
            (tab?.Tag as InterfaceControl)?.Protocol as ISupportsLogOff;
    }
}
