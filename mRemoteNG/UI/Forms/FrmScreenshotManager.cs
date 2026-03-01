using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public class FrmScreenshotManager : Form
    {
        private readonly ListView _listView;
        private readonly Button _btnDelete;
        private readonly Button _btnDeleteAll;
        private readonly Button _btnOpenFolder;
        private readonly Button _btnClose;
        private readonly string _screenshotDir;

        public FrmScreenshotManager()
        {
            _screenshotDir = Path.Combine(SettingsFileInfo.SettingsPath, "Screenshots");

            Text = "Screenshot Manager";
            Size = new System.Drawing.Size(550, 400);
            MinimumSize = new System.Drawing.Size(400, 300);
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            Font = new System.Drawing.Font("Segoe UI", 8.25f);

            _listView = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = true,
                Dock = DockStyle.Fill,
                Sorting = SortOrder.Descending
            };
            _listView.Columns.Add("Name", 280);
            _listView.Columns.Add("Date Modified", 135);
            _listView.Columns.Add("Size", 70);
            _listView.SelectedIndexChanged += (s, e) => UpdateButtonStates();

            var pnlButtons = new Panel { Height = 44, Dock = DockStyle.Bottom };

            _btnDelete = new Button { Text = "Delete", Width = 80, Height = 28, Top = 8, Left = 8 };
            _btnDelete.Click += BtnDelete_Click;

            _btnDeleteAll = new Button { Text = "Delete All", Width = 80, Height = 28, Top = 8, Left = 96 };
            _btnDeleteAll.Click += BtnDeleteAll_Click;

            _btnOpenFolder = new Button { Text = "Open Folder", Width = 90, Height = 28, Top = 8, Left = 184 };
            _btnOpenFolder.Click += BtnOpenFolder_Click;

            _btnClose = new Button
            {
                Text = "Close",
                Width = 75,
                Height = 28,
                Top = 8,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _btnClose.Click += (s, e) => Close();

            pnlButtons.Controls.AddRange(new Control[] { _btnDelete, _btnDeleteAll, _btnOpenFolder, _btnClose });
            pnlButtons.Layout += (s, e) => _btnClose.Left = pnlButtons.Width - _btnClose.Width - 8;

            Controls.Add(_listView);
            Controls.Add(pnlButtons);

            LoadScreenshots();
        }

        private void LoadScreenshots()
        {
            _listView.Items.Clear();

            if (!Directory.Exists(_screenshotDir))
            {
                UpdateButtonStates();
                return;
            }

            foreach (string file in Directory.GetFiles(_screenshotDir, "*.png"))
            {
                var fi = new FileInfo(file);
                var item = new ListViewItem(fi.Name) { Tag = fi.FullName };
                item.SubItems.Add(fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                item.SubItems.Add($"{Math.Max(1, fi.Length / 1024)} KB");
                _listView.Items.Add(item);
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            _btnDelete.Enabled = _listView.SelectedItems.Count > 0;
            _btnDeleteAll.Enabled = _listView.Items.Count > 0;
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_listView.SelectedItems.Count == 0) return;

            int count = _listView.SelectedItems.Count;
            string msg = count == 1
                ? $"Delete '{_listView.SelectedItems[0].Text}'?"
                : $"Delete {count} selected screenshots?";

            if (MessageBox.Show(this, msg, "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            foreach (ListViewItem item in _listView.SelectedItems)
            {
                if (item.Tag is string path)
                {
                    try { File.Delete(path); }
                    catch { /* file may be in use */ }
                }
            }

            LoadScreenshots();
        }

        private void BtnDeleteAll_Click(object? sender, EventArgs e)
        {
            if (_listView.Items.Count == 0) return;

            if (MessageBox.Show(this, $"Delete all {_listView.Items.Count} screenshots?", "Confirm Delete All",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            foreach (ListViewItem item in _listView.Items)
            {
                if (item.Tag is string path)
                {
                    try { File.Delete(path); }
                    catch { /* file may be in use */ }
                }
            }

            LoadScreenshots();
        }

        private void BtnOpenFolder_Click(object? sender, EventArgs e)
        {
            if (!Directory.Exists(_screenshotDir))
                Directory.CreateDirectory(_screenshotDir);

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _screenshotDir,
                    UseShellExecute = true
                });
            }
            catch { /* ignore */ }
        }
    }
}
