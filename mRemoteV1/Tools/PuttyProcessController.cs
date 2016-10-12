using mRemoteNG.App.Info;

namespace mRemoteNG.Tools
{
    public class PuttyProcessController : ProcessController
    {
        public bool Start(CommandLineArguments arguments = null)
        {
            var filename = "";
            if (Settings.Default.UseCustomPuttyPath)
                filename = Settings.Default.CustomPuttyPath;
            else
                filename = GeneralAppInfo.PuttyPath;
            return Start(filename, arguments);
        }
    }
}