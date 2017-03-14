using Gecko;
using Gecko.Events;
using mRemoteNG.App.Info;
using mRemoteNG.UI.TaskDialog;
// ReSharper disable RedundantAssignment

namespace mRemoteNG.Connection.Protocol.Http
{
    internal abstract class CertEvent
    {

        //Refernce: https://bitbucket.org/geckofx/geckofx-33.0/issues/90/invalid-security-certificate-error-on
        internal static void GeckoBrowser_NSSError(object sender, GeckoNSSErrorEventArgs e)
        {
            if (!e.Message.Contains("Certificate"))
            {
                e.Handled = false;
                return;
            }

            string[] commandButtons =
            {
                $"Allow Once",      // 0
                $"Allow Always",    // 1
                $"Don't Allow"      // 2
            };

            CTaskDialog.ShowTaskDialogBox(null, GeneralAppInfo.ProductName, $"Allow Insecure Certificate?",
                string.Format($"Allow Insecure Certificate for URL: {0}?", e.Uri.AbsoluteUri),
                "", "", "", "", string.Join(" | ", commandButtons), ETaskDialogButtons.None, ESysIcons.Question,
                ESysIcons.Question);

           
            var allow = false;
            var always = false;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (CTaskDialog.CommandButtonResult)
            {
                case 0:
                    allow = true;
                    always = false;
                    break;
                case 1:
                    allow = true;
                    always = true;
                    break;
                case 2:
                    allow = false;
                    always = false;
                    break;
            }

            // "always" might not work: https://bitbucket.org/geckofx/geckofx-45.0/issues/152/remembervalidityoverride-doesnt-save-in
            if(allow)
                CertOverrideService.GetService().RememberValidityOverride(e.Uri, e.Certificate, CertOverride.Mismatch | CertOverride.Time | CertOverride.Untrusted, always);
            e.Handled = true;
            ((GeckoWebBrowser)sender).Navigate(e.Uri.AbsoluteUri);
        }
    }
}
