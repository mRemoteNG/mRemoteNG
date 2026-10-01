using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Config.UserProfiles;

namespace mRemoteNG.UI.Forms
{
    internal sealed class ProfileSelectionDialog : Form
    {
        private readonly ComboBox _profiles = new() { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

        private ProfileSelectionDialog(IReadOnlyList<ConnectionProfile> profiles)
        {
            Text = "Select connection profile";
            ClientSize = new Size(360, 110);
            StartPosition = FormStartPosition.CenterParent;
            _profiles.DataSource = profiles.ToList();
            _profiles.DisplayMember = nameof(ConnectionProfile.Name);
            Controls.Add(_profiles);
            Button accept = new() { Text = "OK", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom };
            Controls.Add(accept);
            AcceptButton = accept;
        }

        public static ConnectionProfile Select(IReadOnlyList<ConnectionProfile> profiles)
        {
            using ProfileSelectionDialog dialog = new(profiles);
            return dialog.ShowDialog() == DialogResult.OK ? (ConnectionProfile)dialog._profiles.SelectedItem : null;
        }
    }
}
