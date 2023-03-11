using mRemoteNG.Properties;
using mRemoteNG.Tools.Cmdline;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class PuttyProcessController : ProcessController
    {
        public bool Start(CommandLineArguments arguments = null)
        {
            var filename = Properties.OptionsAdvancedPage.Default.UseCustomPuttyPath ? Properties.OptionsAdvancedPage.Default.CustomPuttyPath : App.Info.GeneralAppInfo.PuttyPath;
            return Start(filename, arguments);
        }
    }
}