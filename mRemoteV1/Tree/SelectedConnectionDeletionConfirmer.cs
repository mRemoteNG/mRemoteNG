using System;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.UI.Controls;


namespace mRemoteNG.Tree
{
    public class SelectedConnectionDeletionConfirmer : IConfirm
    {
        private readonly IConnectionTree _connectionTree;
        private readonly Func<string, string, MessageBoxButtons, MessageBoxIcon, DialogResult> _confirmationFunc;

        public SelectedConnectionDeletionConfirmer(IConnectionTree connectionTree, Func<string, string, MessageBoxButtons, MessageBoxIcon, DialogResult> confirmationFunc)
        {
            _connectionTree = connectionTree;
            _confirmationFunc = confirmationFunc;
        }

        public bool Confirm()
        {
            var deletionTarget = _connectionTree.SelectedNode;
            var deletionTargetAsContainer = deletionTarget as ContainerInfo;
            if (deletionTargetAsContainer != null)
                return deletionTargetAsContainer.HasChildren()
                    ? UserConfirmsNonEmptyFolderDeletion(deletionTargetAsContainer)
                    : UserConfirmsEmptyFolderDeletion(deletionTargetAsContainer);
            return UserConfirmsConnectionDeletion(deletionTarget);
        }

        private bool UserConfirmsEmptyFolderDeletion(AbstractConnectionInfoData deletionTarget)
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeFolder, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private bool UserConfirmsNonEmptyFolderDeletion(AbstractConnectionInfoData deletionTarget)
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeFolderNotEmpty, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private bool UserConfirmsConnectionDeletion(AbstractConnectionInfoData deletionTarget)
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeConnection, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private bool PromptUser(string promptMessage)
        {
            var msgBoxResponse = _confirmationFunc.Invoke(promptMessage, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return msgBoxResponse == DialogResult.Yes;
        }
    }
}