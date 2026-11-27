using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{
    internal sealed class FileRecoveryDialog : Form
    {
        private readonly ListBox _candidates = new() { Dock = DockStyle.Fill, IntegralHeight = false };
        private readonly Button _restore = new() { Text = TextResource("FileRecoveryRestore", "Restore selected"), Enabled = false, DialogResult = DialogResult.OK, AutoSize = true };
        private readonly Button _cancel = new() { Text = TextResource("FileRecoveryCancel", "Cancel"), DialogResult = DialogResult.Cancel, AutoSize = true };

        public FileRecoveryCandidate SelectedCandidate => _candidates.SelectedItem is CandidateItem item ? item.Candidate : null;

        public FileRecoveryDialog(IReadOnlyList<FileRecoveryCandidate> candidates)
        {
            Text = TextResource("FileRecoveryTitle", "Recover connections file");
            MinimumSize = new Size(560, 340);
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            AccessibleName = Text;

            Label explanation = new()
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 58,
                Padding = new Padding(8),
                Text = TextResource("FileRecoveryPrompt", "The connections file could not be loaded. Choose a validated backup to restore. The current file will be preserved before replacement.")
            };

            FlowLayoutPanel buttons = new()
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(8)
            };
            buttons.Controls.Add(_restore);
            buttons.Controls.Add(_cancel);

            Controls.Add(_candidates);
            Controls.Add(explanation);
            Controls.Add(buttons);

            foreach (FileRecoveryCandidate candidate in candidates)
                _candidates.Items.Add(new CandidateItem(candidate));

            _candidates.SelectedIndexChanged += (_, _) => _restore.Enabled = _candidates.SelectedIndex >= 0;
            AcceptButton = _restore;
            CancelButton = _cancel;
        }

        private static string TextResource(string name, string fallback) => Language.ResourceManager.GetString(name) ?? fallback;

        private sealed class CandidateItem(FileRecoveryCandidate candidate)
        {
            public FileRecoveryCandidate Candidate { get; } = candidate;
            public override string ToString() => string.Format(TextResource("FileRecoveryCandidateFormat", "{0} — {1:g} — {2:N0} bytes"),
                Path.GetFileName(Candidate.Path), Candidate.LastWriteTimeUtc.ToLocalTime(), Candidate.Length);
        }
    }
}
