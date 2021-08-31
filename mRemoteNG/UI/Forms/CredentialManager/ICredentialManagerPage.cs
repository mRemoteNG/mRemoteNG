using System.Drawing;

namespace mRemoteNG.UI.Forms.CredentialManager
{
    public interface ICredentialManagerPage
    {
        string PageName { get; }

        Image PageIcon { get; }
    }
}