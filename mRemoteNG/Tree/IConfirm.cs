namespace mRemoteNG.Tree
{
    public interface IConfirm<in TConfirmationTarget>
    {
        bool Confirm(TConfirmationTarget confirmationTarget);
    }
}