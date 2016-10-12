
namespace mRemoteNG.Security
{
	public class SaveFilter
	{
		public SaveFilter(bool disableEverything = false)
		{
		    if (disableEverything) return;
		    Username = true;
		    Password = true;
		    Domain = true;
		    Inheritance = true;
		}

	    public bool Username { get; set; }

	    public bool Password { get; set; }

	    public bool Domain { get; set; }

	    public bool Inheritance { get; set; }
	}
}