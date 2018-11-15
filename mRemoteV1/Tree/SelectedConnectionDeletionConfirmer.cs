using mRemoteNG.Connection;
using mRemoteNG.Container;
using System;
using System.Windows.Forms;


namespace mRemoteNG.Tree
{
    public class SelectedConnectionDeletionConfirmer : IConfirm<ConnectionInfo>
    {
        private readonly Func<string, DialogResult> _confirmationFunc;

        public SelectedConnectionDeletionConfirmer(Func<string, DialogResult> confirmationFunc)
        {
            _confirmationFunc = confirmationFunc;
        }

        public bool Confirm(ConnectionInfo deletionTarget)
        {
            if (deletionTarget == null)
                return false;

            var deletionTargetAsContainer = deletionTarget as ContainerInfo;
            if (deletionTargetAsContainer != null)
                return deletionTargetAsContainer.HasChildren()
                    ? UserConfirmsNonEmptyFolderDeletion(deletionTargetAsContainer)
                    : UserConfirmsEmptyFolderDeletion(deletionTargetAsContainer);
            return UserConfirmsConnectionDeletion(deletionTarget);
        }

        private bool UserConfirmsEmptyFolderDeletion(AbstractConnectionRecord deletionTarget)
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeFolder, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private bool UserConfirmsNonEmptyFolderDeletion(AbstractConnectionRecord deletionTarget)
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeFolderNotEmpty, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private bool UserConfirmsConnectionDeletion(AbstractConnectionRecord deletionTarget)
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeConnection, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private bool PromptUser(string promptMessage)
        {
            var msgBoxResponse = _confirmationFunc(promptMessage);
            return msgBoxResponse == DialogResult.Yes;
        }
    }
}