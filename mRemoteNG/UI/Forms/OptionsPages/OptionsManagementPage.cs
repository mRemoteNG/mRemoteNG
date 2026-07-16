using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Settings;
using mRemoteNG.Config.Settings.Store;
using mRemoteNG.Resources.Language;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptionsManagementPage : OptionsPage
    {
        private IOptionsRepository _optionsRepository;
        private List<OptionInfo> _currentOptions;
        private TableLayoutPanel tableLayoutPanel;
        private TableLayoutPanel formLayoutPanel;
        private FlowLayoutPanel buttonPanel;
        private bool _isLoading;

        public OptionsManagementPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);
            _currentOptions = [];
            EnsureOptionsRepositoryInitialized();
        }

        #region Public Properties

        public override string PageName
        {
            get => "Options Management (Dev)";
            set { }
        }

        #endregion

        #region Public Methods

        public void SetOptionsRepository(IOptionsRepository optionsRepository)
        {
            _optionsRepository = optionsRepository;
        }

        private void EnsureOptionsRepositoryInitialized()
        {
            if (_optionsRepository is not null)
                return;

            try
            {
                if (!Runtime.OptionsRepositoryManager.IsInitialized)
                {
                    Runtime.OptionsRepositoryManager.Initialize(SettingsFileInfo.SettingsPath);
                }

                if (Runtime.OptionsRepositoryManager.IsInitialized)
                {
                    _optionsRepository = Runtime.OptionsRepositoryManager.Repository;
                }
            }
            catch (Exception ex)
            {
                string details = ex.InnerException is null ? ex.Message : $"{ex.Message}\n{ex.InnerException.Message}";
                MessageBox.Show(
                    $"Failed to initialize options database: {details}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            Text = "Options Management";
            groupBoxOptions.Text = "Manage Options";
            btnAdd.Text = "Add";
            btnEdit.Text = "Edit";
            btnDelete.Text = "Delete";
            btnRefresh.Text = "Refresh";
            labelKey.Text = "Key:";
            labelValue.Text = "Value:";
            labelCategory.Text = "Category:";
            labelDescription.Text = "Description:";
            labelType.Text = "Type:";

            // Setup DataGridView columns
            SetupDataGridViewColumns();
        }

        public override async void LoadSettings()
        {
            EnsureOptionsRepositoryInitialized();
            if (_optionsRepository is null)
                return;

            _isLoading = true;
            try
            {
                await RefreshOptionsAsync();
            }
            finally
            {
                _isLoading = false;
            }
        }

        public override void SaveSettings()
        {
            // Options are saved immediately, not on form close
        }

        #endregion

        #region Private Methods

        private void InitializeComponent()
        {
            tableLayoutPanel = new TableLayoutPanel();
            dataGridViewOptions = new DataGridView();
            groupBoxOptions = new GroupBox();
            formLayoutPanel = new TableLayoutPanel();
            labelKey = new Label();
            textBoxKey = new TextBox();
            labelValue = new Label();
            textBoxValue = new TextBox();
            labelCategory = new Label();
            textBoxCategory = new TextBox();
            labelType = new Label();
            comboBoxType = new ComboBox();
            labelDescription = new Label();
            textBoxDescription = new TextBox();
            buttonPanel = new FlowLayoutPanel();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewOptions).BeginInit();
            groupBoxOptions.SuspendLayout();
            formLayoutPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            SuspendLayout();

            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(dataGridViewOptions, 0, 0);
            tableLayoutPanel.Controls.Add(groupBoxOptions, 0, 1);
            tableLayoutPanel.Controls.Add(buttonPanel, 0, 2);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.Padding = new Padding(10);
            tableLayoutPanel.RowCount = 3;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel.TabIndex = 0;

            dataGridViewOptions.AllowUserToAddRows = false;
            dataGridViewOptions.AllowUserToDeleteRows = false;
            dataGridViewOptions.AllowUserToResizeRows = false;
            dataGridViewOptions.AutoGenerateColumns = false;
            dataGridViewOptions.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewOptions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewOptions.Dock = DockStyle.Fill;
            dataGridViewOptions.MultiSelect = false;
            dataGridViewOptions.Name = "dataGridViewOptions";
            dataGridViewOptions.ReadOnly = true;
            dataGridViewOptions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewOptions.TabIndex = 0;
            dataGridViewOptions.SelectionChanged += DataGridViewOptions_SelectionChanged;

            groupBoxOptions.Controls.Add(formLayoutPanel);
            groupBoxOptions.Dock = DockStyle.Fill;
            groupBoxOptions.Name = "groupBoxOptions";
            groupBoxOptions.TabIndex = 1;
            groupBoxOptions.TabStop = false;

            formLayoutPanel.ColumnCount = 2;
            formLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            formLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            formLayoutPanel.Controls.Add(labelKey, 0, 0);
            formLayoutPanel.Controls.Add(textBoxKey, 1, 0);
            formLayoutPanel.Controls.Add(labelValue, 0, 1);
            formLayoutPanel.Controls.Add(textBoxValue, 1, 1);
            formLayoutPanel.Controls.Add(labelCategory, 0, 2);
            formLayoutPanel.Controls.Add(textBoxCategory, 1, 2);
            formLayoutPanel.Controls.Add(labelType, 0, 3);
            formLayoutPanel.Controls.Add(comboBoxType, 1, 3);
            formLayoutPanel.Controls.Add(labelDescription, 0, 4);
            formLayoutPanel.Controls.Add(textBoxDescription, 1, 4);
            formLayoutPanel.Dock = DockStyle.Fill;
            formLayoutPanel.Name = "formLayoutPanel";
            formLayoutPanel.Padding = new Padding(5);
            formLayoutPanel.RowCount = 5;
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            formLayoutPanel.TabIndex = 0;

            labelKey.Anchor = AnchorStyles.Left;
            labelKey.AutoSize = true;
            labelKey.Name = "labelKey";
            labelKey.TabIndex = 0;

            textBoxKey.Dock = DockStyle.Fill;
            textBoxKey.Margin = new Padding(5);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.TabIndex = 1;

            labelValue.Anchor = AnchorStyles.Left;
            labelValue.AutoSize = true;
            labelValue.Name = "labelValue";
            labelValue.TabIndex = 2;

            textBoxValue.Dock = DockStyle.Fill;
            textBoxValue.Margin = new Padding(5);
            textBoxValue.Multiline = true;
            textBoxValue.Name = "textBoxValue";
            textBoxValue.TabIndex = 3;

            labelCategory.Anchor = AnchorStyles.Left;
            labelCategory.AutoSize = true;
            labelCategory.Name = "labelCategory";
            labelCategory.TabIndex = 4;

            textBoxCategory.Dock = DockStyle.Fill;
            textBoxCategory.Margin = new Padding(5);
            textBoxCategory.Name = "textBoxCategory";
            textBoxCategory.TabIndex = 5;

            labelType.Anchor = AnchorStyles.Left;
            labelType.AutoSize = true;
            labelType.Name = "labelType";
            labelType.TabIndex = 6;

            comboBoxType.Dock = DockStyle.Fill;
            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.Items.AddRange(new object[] { "string", "int", "bool", "double" });
            comboBoxType.Margin = new Padding(5);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.SelectedIndex = 0;
            comboBoxType.TabIndex = 7;

            labelDescription.Anchor = AnchorStyles.Left;
            labelDescription.AutoSize = true;
            labelDescription.Name = "labelDescription";
            labelDescription.TabIndex = 8;

            textBoxDescription.Dock = DockStyle.Fill;
            textBoxDescription.Margin = new Padding(5);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.TabIndex = 9;

            buttonPanel.AutoSize = true;
            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;
            buttonPanel.Margin = new Padding(5, 10, 5, 0);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.TabIndex = 2;
            buttonPanel.WrapContents = false;

            btnAdd.AutoSize = true;
            btnAdd.MinimumSize = new System.Drawing.Size(80, 30);
            btnAdd.Name = "btnAdd";
            btnAdd.TabIndex = 0;
            btnAdd.Click += BtnAdd_Click;

            btnEdit.AutoSize = true;
            btnEdit.MinimumSize = new System.Drawing.Size(80, 30);
            btnEdit.Name = "btnEdit";
            btnEdit.TabIndex = 1;
            btnEdit.Click += BtnEdit_Click;

            btnDelete.AutoSize = true;
            btnDelete.MinimumSize = new System.Drawing.Size(80, 30);
            btnDelete.Name = "btnDelete";
            btnDelete.TabIndex = 2;
            btnDelete.Click += BtnDelete_Click;

            btnRefresh.AutoSize = true;
            btnRefresh.MinimumSize = new System.Drawing.Size(80, 30);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.TabIndex = 3;
            btnRefresh.Click += BtnRefresh_Click;

            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(tableLayoutPanel);
            Name = "OptionsManagementPage";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewOptions).EndInit();
            groupBoxOptions.ResumeLayout(false);
            formLayoutPanel.ResumeLayout(false);
            formLayoutPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
            buttonPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void SetupDataGridViewColumns()
        {
            dataGridViewOptions.Columns.Clear();
            dataGridViewOptions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Visible = false
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Key",
                HeaderText = "Key",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Value",
                HeaderText = "Value",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 160F
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Category",
                HeaderText = "Category",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OptionType",
                HeaderText = "Type",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 220F
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ModifiedDate",
                HeaderText = "Modified",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
        }

        private async Task RefreshOptionsAsync()
        {
            if (_optionsRepository is null)
                return;

            try
            {
                var options = await _optionsRepository.GetAllOptionsAsync();
                _currentOptions = options.ToList();

                dataGridViewOptions.DataSource = null;
                dataGridViewOptions.DataSource = _currentOptions;

                ClearFormFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading options: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewOptions_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewOptions.SelectedRows.Count == 0)
            {
                ClearFormFields();
                return;
            }

            var selectedRow = dataGridViewOptions.SelectedRows[0];
            var option = (OptionInfo)selectedRow.DataBoundItem;

            if (option is not null)
            {
                textBoxKey.Text = option.Key;
                textBoxValue.Text = option.Value ?? "";
                textBoxCategory.Text = option.Category ?? "";
                textBoxDescription.Text = option.Description ?? "";
                comboBoxType.SelectedItem = option.OptionType ?? "string";
            }
        }

        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxKey.Text))
            {
                MessageBox.Show("Key is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnsureOptionsRepositoryInitialized();
            if (_optionsRepository is null)
            {
                MessageBox.Show("Options database is not available.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var newOption = new OptionInfo
            {
                Key = textBoxKey.Text.Trim(),
                Value = textBoxValue.Text,
                Category = string.IsNullOrWhiteSpace(textBoxCategory.Text) ? null : textBoxCategory.Text,
                Description = string.IsNullOrWhiteSpace(textBoxDescription.Text) ? null : textBoxDescription.Text,
                OptionType = comboBoxType.SelectedItem?.ToString() ?? "string"
            };

            try
            {
                await _optionsRepository.AddOptionAsync(newOption);
                await RefreshOptionsAsync();
                MessageBox.Show("Option added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding option: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewOptions.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an option to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewOptions.SelectedRows[0];
            var option = (OptionInfo)selectedRow.DataBoundItem;

            if (option is null)
                return;

            option.Value = textBoxValue.Text;
            option.Category = string.IsNullOrWhiteSpace(textBoxCategory.Text) ? null : textBoxCategory.Text;
            option.Description = string.IsNullOrWhiteSpace(textBoxDescription.Text) ? null : textBoxDescription.Text;
            option.OptionType = comboBoxType.SelectedItem?.ToString() ?? "string";

            try
            {
                bool success = await _optionsRepository.UpdateOptionAsync(option);
                if (success)
                {
                    await RefreshOptionsAsync();
                    MessageBox.Show("Option updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Option not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating option: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewOptions.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an option to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewOptions.SelectedRows[0];
            var option = (OptionInfo)selectedRow.DataBoundItem;

            if (option is null)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete the option '{option.Key}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool success = await _optionsRepository.DeleteOptionAsync(option.Id);
                if (success)
                {
                    await RefreshOptionsAsync();
                    MessageBox.Show("Option deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Option not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting option: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshOptionsAsync();
        }

        private void ClearFormFields()
        {
            textBoxKey.Clear();
            textBoxValue.Clear();
            textBoxCategory.Clear();
            textBoxDescription.Clear();
            comboBoxType.SelectedIndex = 0;
        }

        protected override void ApplyTheme()
        {
            base.ApplyTheme();
            if (dataGridViewOptions is not null)
            {
                dataGridViewOptions.BackgroundColor = System.Drawing.SystemColors.Control;
            }
        }

        #endregion

        #region Controls

        private DataGridView dataGridViewOptions;
        private GroupBox groupBoxOptions;
        private Label labelKey;
        private TextBox textBoxKey;
        private Label labelValue;
        private TextBox textBoxValue;
        private Label labelCategory;
        private TextBox textBoxCategory;
        private Label labelType;
        private ComboBox comboBoxType;
        private Label labelDescription;
        private TextBox textBoxDescription;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        #endregion
    }
}
