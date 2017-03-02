
namespace mRemoteNG.Tree
{
    public class AlwaysConfirmYes : IConfirm
    {
        public bool Confirm()
        {
            return true;
        }
    }
}