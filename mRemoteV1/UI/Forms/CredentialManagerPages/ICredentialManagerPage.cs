using System.Drawing;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public interface ICredentialManagerPage
    {
        string PageName { get; }

        Image PageIcon { get; }
    }
}