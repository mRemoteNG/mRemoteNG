

namespace mRemoteNG.Tools
{
	public class PuttyProcessController : ProcessController
	{
		public bool Start(CommandLineArguments arguments = null)
		{
			string filename = "";
			if (My.Settings.Default.UseCustomPuttyPath)
			{
				filename = My.Settings.Default.CustomPuttyPath;
			}
			else
			{
				filename = App.Info.General.PuttyPath;
			}
			return Start(filename, arguments);
		}
	}
}
