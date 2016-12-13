using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Container;


namespace mRemoteNG.Tree
{
    public class ConnectionDeletionConfirmer
    {
        public static bool UserConfirmsDeletion(ConnectionInfo deletionTarget)
        {
            var deletionTargetAsContainer = deletionTarget as ContainerInfo;
            if (deletionTargetAsContainer != null)
                return deletionTargetAsContainer.HasChildren()
                    ? UserConfirmsNonEmptyFolderDeletion(deletionTargetAsContainer)
                    : UserConfirmsEmptyFolderDeletion(deletionTargetAsContainer);
            return UserConfirmsConnectionDeletion(deletionTarget);
        }

        private static bool UserConfirmsEmptyFolderDeletion(AbstractConnectionInfoData deletionTarget)
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeFolder, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private static bool UserConfirmsNonEmptyFolderDeletion(AbstractConnectionInfoData deletionTarget)
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeFolderNotEmpty, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private static bool UserConfirmsConnectionDeletion(AbstractConnectionInfoData deletionTarget)
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeConnection, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private static bool PromptUser(string promptMessage)
        {
            var msgBoxResponse = MessageBox.Show(promptMessage, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (msgBoxResponse == DialogResult.Yes);
        }
    }
}