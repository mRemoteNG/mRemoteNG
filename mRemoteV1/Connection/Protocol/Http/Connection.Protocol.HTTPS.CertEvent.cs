using Gecko;
using Gecko.Events;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.UI.TaskDialog;
// ReSharper disable RedundantAssignment

namespace mRemoteNG.Connection.Protocol.Http
{
    internal abstract class CertEvent
    {
        //Refernce: https://bitbucket.org/geckofx/geckofx-33.0/issues/90/invalid-security-certificate-error-on
        internal static void GeckoBrowser_NSSError(object sender, GeckoNSSErrorEventArgs e)
        {
            /* some messages say "Certificate", some say "certificate"
             * I'm guessing that this is going to be a localization issue... 
             * Log a message so we can try to find a better solution if problems are reported in the future...
             */
            if (!e.Message.ToLower().Contains("certificate"))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"Unhandled NSSError: {e.Message}");
                e.Handled = false;
                return;
            }

            string[] commandButtons =
            {
                Language.strHttpsInsecureAllowOnce,     // 0
                Language.strHttpsInsecureAllowAlways,   // 1
                Language.strHttpsInsecureDontAllow      // 2
            };

            CTaskDialog.ShowTaskDialogBox(null, GeneralAppInfo.ProductName, Language.strHttpsInsecurePromptTitle,
                string.Format(Language.strHttpsInsecurePrompt, e.Uri.AbsoluteUri), "", "", "", "",
                string.Join(" | ", commandButtons), ETaskDialogButtons.None, ESysIcons.Question, ESysIcons.Question);
           
            var allow = false;
            var temporary = true;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (CTaskDialog.CommandButtonResult)
            {
                case 0:
                    allow = true;
                    temporary = true;
                    break;
                case 1:
                    allow = true;
                    temporary = false;
                    break;
                case 2:
                    allow = false;
                    temporary = true; // just to be safe
                    break;
            }
           
            if (!allow)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"User did not allow navigation to {e.Uri.AbsoluteUri} with an insecure certificate: {e.Message}");
                return;
            }

            /* "temporary == false" (aka always) might not work: 
            * https://bitbucket.org/geckofx/geckofx-45.0/issues/152/remembervalidityoverride-doesnt-save-in
            * However, my testing was successful in Gecko 45.0.22
            */
            CertOverrideService.GetService().RememberValidityOverride(e.Uri, e.Certificate,
                CertOverride.Mismatch | CertOverride.Time | CertOverride.Untrusted, temporary);

            e.Handled = true;
            ((GeckoWebBrowser)sender).Navigate(e.Uri.AbsoluteUri);
        }
    }
}
