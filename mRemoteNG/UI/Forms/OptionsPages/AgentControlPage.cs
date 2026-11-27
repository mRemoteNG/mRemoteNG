using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.App.AgentControl;
using mRemoteNG.Resources.Language;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    internal sealed class AgentControlPage : OptionsPage
    {
        private readonly CheckBox _enable = new() { AutoSize = true };
        private readonly Label _description = new() { AutoSize = true, MaximumSize = new Size(620, 0) };
        private readonly Button _grant = new() { AutoSize = true };
        private readonly ComboBox _targets = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 420 };
        private readonly Button _scopedGrant = new() { AutoSize = true };
        private readonly Button _revoke = new() { AutoSize = true };
        private readonly Label _grantValue = new() { AutoSize = true, MaximumSize = new Size(620, 0) };
        private readonly AgentControlRuntime _runtime;

        public AgentControlPage(AgentControlRuntime runtime)
        {
            _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
            PageName = Text("AgentControlOptions", "AI control");
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);
            AutoScroll = true;
            Padding = new Padding(12);
            FlowLayoutPanel layout = new()
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            _description.Text = DescriptionText();
            _enable.Text = Text("AgentControlEnable", "Enable local control for this session");
            _grant.Text = Text("AgentControlGrant", "Grant 5-minute access to list allowed actions");
            _scopedGrant.Text = Text("AgentControlScopedGrant", "Grant 5-minute read and preview access to selected connection");
            foreach (AgentControlTargetOption target in AgentControlRuntime.GetAvailableTargets())
                _targets.Items.Add(target);
            _revoke.Text = Text("AgentControlRevoke", "Revoke all access and stop");
            _grantValue.Text = Text("AgentControlNoGrant", "No active grant issued by this page.");
            _enable.Checked = _runtime.IsEnabled;
            _grant.Enabled = _runtime.IsEnabled;
            _scopedGrant.Enabled = _runtime.IsEnabled && _targets.Items.Count > 0;
            _revoke.Enabled = _runtime.IsEnabled;
            _enable.CheckedChanged += EnableChanged;
            _grant.Click += GrantClicked;
            _scopedGrant.Click += ScopedGrantClicked;
            _revoke.Click += RevokeClicked;
            layout.Controls.AddRange([_description, _enable, _grant, _targets, _scopedGrant, _grantValue, _revoke]);
            Controls.Add(layout);
            ApplyTheme();
        }

        public override string PageName { get; set; }

        public override void ApplyLanguage()
        {
            PageName = Text("AgentControlOptions", "AI control");
            _description.Text = DescriptionText();
            _enable.Text = Text("AgentControlEnable", "Enable local control for this session");
            _grant.Text = Text("AgentControlGrant", "Grant 5-minute access to list allowed actions");
            _scopedGrant.Text = Text("AgentControlScopedGrant", "Grant 5-minute read and preview access to selected connection");
            _revoke.Text = Text("AgentControlRevoke", "Revoke all access and stop");
        }

        private void EnableChanged(object sender, EventArgs e)
        {
            if (_enable.Checked)
            {
                DialogResult answer = MessageBox.Show(this,
                    Text("AgentControlEnableConfirm", "This starts a local read-only control pipe for this session. Requests still require a temporary grant. Continue?"),
                    Text("AgentControlOptions", "AI control"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (answer != DialogResult.Yes)
                {
                    _enable.CheckedChanged -= EnableChanged;
                    _enable.Checked = false;
                    _enable.CheckedChanged += EnableChanged;
                    return;
                }
                try
                {
                    _runtime.EnableForCurrentSession();
                    _description.Text = DescriptionText();
                }
                catch
                {
                    _enable.Checked = false;
                    _grantValue.Text = Text("AgentControlStartFailed", "Could not start local control.");
                }
            }
            else
            {
                RevokeAndStop();
            }
            _grant.Enabled = _runtime.IsEnabled;
            _revoke.Enabled = _runtime.IsEnabled;
            _scopedGrant.Enabled = _runtime.IsEnabled && _targets.Items.Count > 0;
        }

        private void GrantClicked(object sender, EventArgs e)
        {
            try
            {
                CapabilityGrant grant = _runtime.IssueActionsDiscoveryGrant();
                _grantValue.Text = string.Format(Text("AgentControlGrantCreated", "Temporary grant ID: {0}. Share it only with the intended local client; it expires in five minutes."), grant.GrantId);
            }
            catch
            {
                _grantValue.Text = Text("AgentControlGrantFailed", "Could not issue the temporary grant.");
            }
        }

        private void ScopedGrantClicked(object sender, EventArgs e)
        {
            if (_targets.SelectedItem is not AgentControlTargetOption target)
            {
                _grantValue.Text = Text("AgentControlChooseTarget", "Choose a connection before granting access.");
                return;
            }
            try
            {
                CapabilityGrant grant = _runtime.IssueScopedReadGrant(target.TargetId);
                _grantValue.Text = string.Format(Text("AgentControlScopedGrantCreated", "Temporary grant ID: {0}. It can read and preview safe information for the selected connection for five minutes."), grant.GrantId);
            }
            catch
            {
                _grantValue.Text = Text("AgentControlGrantFailed", "Could not issue the temporary grant.");
            }
        }

        private void RevokeClicked(object sender, EventArgs e)
        {
            RevokeAndStop();
            _description.Text = DescriptionText();
            _enable.Checked = false;
            _grant.Enabled = false;
            _scopedGrant.Enabled = false;
            _revoke.Enabled = false;
            _grantValue.Text = Text("AgentControlNoGrant", "No active grant issued by this page.");
        }

        private void RevokeAndStop()
        {
            Task task = _runtime.DisableAndRevokeAsync();
            task.GetAwaiter().GetResult();
        }

        private string DescriptionText() => _runtime.IsEnabled
            ? string.Format(Text("AgentControlEnabledDescription", "Local read-only control is active for this session at pipe {0}. Access is temporary and must be granted explicitly."), _runtime.PipeName)
            : Text("AgentControlDescription", "Local read-only AI access is disabled by default. Enabling it starts a current-user-only named pipe for this app session. No access grants are saved.");

        private static string Text(string name, string fallback) => Language.ResourceManager.GetString(name) ?? fallback;
    }
}
