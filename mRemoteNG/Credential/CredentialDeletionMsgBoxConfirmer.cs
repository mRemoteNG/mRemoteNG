using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Tree;
using mRemoteNG.Resources.Language;


namespace mRemoteNG.Credential
{
    public class CredentialDeletionMsgBoxConfirmer : IConfirm<IEnumerable<ICredentialRecord>>
    {
        private readonly Func<string, string, MessageBoxButtons, MessageBoxIcon, DialogResult> _confirmationFunc;

        public CredentialDeletionMsgBoxConfirmer(
            Func<string, string, MessageBoxButtons, MessageBoxIcon, DialogResult> confirmationFunc)
        {
            if (confirmationFunc == null)
                throw new ArgumentNullException(nameof(confirmationFunc));

            _confirmationFunc = confirmationFunc;
        }

        public bool Confirm(IEnumerable<ICredentialRecord> confirmationTargets)
        {
            var targetsArray = confirmationTargets.ToArray();
            if (targetsArray.Length == 0) return false;
            if (targetsArray.Length > 1)
                return PromptUser(string.Format("Are you sure you want to delete these {0} selected credentials?", targetsArray.Length));
            return PromptUser(string.Format(Language.ConfirmDeleteCredentialRecord,targetsArray.First().Title));
        }

        private bool PromptUser(string promptMessage)
        {
            var msgBoxResponse = _confirmationFunc.Invoke(promptMessage, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return msgBoxResponse == DialogResult.Yes;
        }
    }
}