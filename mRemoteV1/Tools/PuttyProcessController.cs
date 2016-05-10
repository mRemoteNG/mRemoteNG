

namespace mRemoteNG.Tools
{
	public class PuttyProcessController : ProcessController
	{
		public bool Start(CommandLineArguments arguments = null)
		{
			string filename = "";
			if (mRemoteNG.Settings.Default.UseCustomPuttyPath)
			{
				filename = mRemoteNG.Settings.Default.CustomPuttyPath;
			}
			else
			{
				filename = App.Info.GeneralAppInfo.PuttyPath;
			}
			return Start(filename, arguments);
		}
	}
}
