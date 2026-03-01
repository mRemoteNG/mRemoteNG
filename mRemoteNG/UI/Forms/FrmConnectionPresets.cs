using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Config;
using mRemoteNG.Connection;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public class FrmConnectionPresets : Form
    {
        private readonly ConnectionPresetService _presetService;
        private readonly IReadOnlyList<ConnectionInfo> _selectedConnections;

        private readonly ListBox _presetListBox = new();
        private readonly TextBox _presetNameTextBox = new();
        private readonly Button _savePresetButton = new();
        private readonly Button _applyPresetButton = new();
        private readonly Button _deletePresetButton = new();
        private readonly Button _closeButton = new();
        private readonly Label _statusLabel = new();

        public bool PresetApplied { get; private set; }

        public FrmConnectionPresets(
            ConnectionPresetService presetService,
            IEnumerable<ConnectionInfo> selectedConnections)
        {
            _presetService = presetService ?? throw new ArgumentNullException(nameof(presetService));
            _selectedConnections = selectedConnections?.Where(connection => connection != null).ToList() ?? [];

            InitializeComponent();
            LoadPresetNames();
            UpdateButtonState();

            if (_selectedConnections.Count == 0)
            {
                SetStatus("No connections selected.");
            }
        }

        private void InitializeComponent()
        {
            Text = "Connection Presets";
            ClientSize = new Size(460, 360);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;

            Label presetsLabel = new()
            {
                AutoSize = true,
                Location = new Point(12, 12),
                Text = "Available presets:"
            };

            _presetListBox.Location = new Point(12, 32);
            _presetListBox.Size = new Size(200, 250);
            _presetListBox.SelectedIndexChanged += PresetListBox_SelectedIndexChanged;

            Label presetNameLabel = new()
            {
                AutoSize = true,
                Location = new Point(225, 12),
                Text = "Preset name:"
            };

            _presetNameTextBox.Location = new Point(225, 32);
            _presetNameTextBox.Size = new Size(220, 23);
            _presetNameTextBox.TextChanged += PresetNameTextBox_TextChanged;

            _savePresetButton.Location = new Point(225, 67);
            _savePresetButton.Size = new Size(220, 30);
            _savePresetButton.Text = "Save / Update from Selection";
            _savePresetButton.Click += SavePresetButton_Click;

            _applyPresetButton.Location = new Point(225, 107);
            _applyPresetButton.Size = new Size(220, 30);
            _applyPresetButton.Text = "Apply to Selection";
            _applyPresetButton.Click += ApplyPresetButton_Click;

            _deletePresetButton.Location = new Point(225, 147);
            _deletePresetButton.Size = new Size(220, 30);
            _deletePresetButton.Text = "Delete Preset";
            _deletePresetButton.Click += DeletePresetButton_Click;

            _statusLabel.Location = new Point(12, 292);
            _statusLabel.Size = new Size(433, 52);
            _statusLabel.AutoEllipsis = true;
            _statusLabel.TextAlign = ContentAlignment.MiddleLeft;

            _closeButton.Location = new Point(370, 324);
            _closeButton.Size = new Size(75, 25);
            _closeButton.Text = "Close";
            _closeButton.Click += CloseButton_Click;

            AcceptButton = _applyPresetButton;
            CancelButton = _closeButton;

            Controls.Add(presetsLabel);
            Controls.Add(_presetListBox);
            Controls.Add(presetNameLabel);
            Controls.Add(_presetNameTextBox);
            Controls.Add(_savePresetButton);
            Controls.Add(_applyPresetButton);
            Controls.Add(_deletePresetButton);
            Controls.Add(_statusLabel);
            Controls.Add(_closeButton);
        }

        private void LoadPresetNames(string? selectedPresetName = null)
        {
            IReadOnlyList<string> presetNames = _presetService.GetPresetNames();

            _presetListBox.BeginUpdate();
            _presetListBox.Items.Clear();
            foreach (string presetName in presetNames)
            {
                _presetListBox.Items.Add(presetName);
            }
            _presetListBox.EndUpdate();

            if (!string.IsNullOrWhiteSpace(selectedPresetName))
            {
                int selectedIndex = _presetListBox.FindStringExact(selectedPresetName);
                if (selectedIndex >= 0)
                {
                    _presetListBox.SelectedIndex = selectedIndex;
                }
            }

            UpdateButtonState();
        }

        private void PresetListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_presetListBox.SelectedItem is string presetName)
            {
                _presetNameTextBox.Text = presetName;
            }

            UpdateButtonState();
        }

        private void PresetNameTextBox_TextChanged(object? sender, EventArgs e)
        {
            UpdateButtonState();
        }

        private void SavePresetButton_Click(object? sender, EventArgs e)
        {
            if (_selectedConnections.Count == 0)
            {
                SetStatus("No connections selected.");
                return;
            }

            string presetName = _presetNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(presetName))
            {
                SetStatus("Preset name is required.");
                return;
            }

            bool saveSucceeded = _presetService.SavePreset(presetName, _selectedConnections[0]);
            if (!saveSucceeded)
            {
                SetStatus("Preset could not be saved.");
                return;
            }

            LoadPresetNames(presetName);
            SetStatus($"Preset '{presetName}' saved.");
        }

        private void ApplyPresetButton_Click(object? sender, EventArgs e)
        {
            if (_presetListBox.SelectedItem is not string presetName)
            {
                SetStatus("Select a preset to apply.");
                return;
            }

            bool applySucceeded = _presetService.ApplyPreset(presetName, _selectedConnections);
            if (!applySucceeded)
            {
                SetStatus($"Preset '{presetName}' could not be applied.");
                return;
            }

            PresetApplied = true;
            SetStatus($"Preset '{presetName}' applied to {_selectedConnections.Count} connection(s).");
        }

        private void DeletePresetButton_Click(object? sender, EventArgs e)
        {
            if (_presetListBox.SelectedItem is not string presetName)
            {
                SetStatus("Select a preset to delete.");
                return;
            }

            bool deleted = _presetService.DeletePreset(presetName);
            if (!deleted)
            {
                SetStatus($"Preset '{presetName}' could not be deleted.");
                return;
            }

            _presetNameTextBox.Clear();
            LoadPresetNames();
            SetStatus($"Preset '{presetName}' deleted.");
        }

        private void CloseButton_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void SetStatus(string message)
        {
            _statusLabel.Text = message;
        }

        private void UpdateButtonState()
        {
            bool hasSelectedConnections = _selectedConnections.Count > 0;
            bool hasPresetName = !string.IsNullOrWhiteSpace(_presetNameTextBox.Text);
            bool hasSelectedPreset = _presetListBox.SelectedItem is string;

            _savePresetButton.Enabled = hasSelectedConnections && hasPresetName;
            _applyPresetButton.Enabled = hasSelectedConnections && hasSelectedPreset;
            _deletePresetButton.Enabled = hasSelectedPreset;
        }
    }
}
