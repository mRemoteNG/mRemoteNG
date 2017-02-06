using System;
using System.Windows.Forms;
using mRemoteNG.Tree;


namespace mRemoteNG.Credential
{
    public class CredentialDeletionMsgBoxConfirmer : IConfirm<ICredentialRecord>
    {
        private readonly Func<string, string, MessageBoxButtons, MessageBoxIcon, DialogResult> _confirmationFunc;

        public CredentialDeletionMsgBoxConfirmer(Func<string, string, MessageBoxButtons, MessageBoxIcon, DialogResult> confirmationFunc)
        {
            if (confirmationFunc == null)
                throw new ArgumentNullException(nameof(confirmationFunc));

            _confirmationFunc = confirmationFunc;
        }

        public bool Confirm(ICredentialRecord confirmationTarget)
        {
            var promptText = string.Format(Language.strConfirmDeleteCredentialRecord, confirmationTarget.Title);
            return PromptUser(promptText);
        }

        private bool PromptUser(string promptMessage)
        {
            var msgBoxResponse = _confirmationFunc.Invoke(promptMessage, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return msgBoxResponse == DialogResult.Yes;
        }
    }
}