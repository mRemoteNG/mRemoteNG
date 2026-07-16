using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.Config.Settings;
using mRemoteNG.Resources.Language;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptionsManagementPage : OptionsPage
    {
        private IOptionsRepository _optionsRepository;
        private List<OptionInfo> _currentOptions;
        private bool _isLoading;

        public OptionsManagementPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);
            _currentOptions = [];
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
            SuspendLayout();

            // Main layout
            var tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10)
            };
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // DataGridView for options
            dataGridViewOptions = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoGenerateColumns = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                BackgroundColor = System.Drawing.SystemColors.Control
            };

            SetupDataGridViewColumns();
            dataGridViewOptions.SelectionChanged += DataGridViewOptions_SelectionChanged;
            tableLayoutPanel.Controls.Add(dataGridViewOptions, 0, 0);

            // Form controls panel
            groupBoxOptions = new GroupBox
            {
                Text = "Option Details",
                Dock = DockStyle.Fill,
                AutoSize = false,
                Height = 150
            };

            var formLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(5)
            };
            formLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            formLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Key
            labelKey = new Label { Text = "Key:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            textBoxKey = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(5) };
            formLayoutPanel.Controls.Add(labelKey, 0, 0);
            formLayoutPanel.Controls.Add(textBoxKey, 1, 0);

            // Value
            labelValue = new Label { Text = "Value:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            textBoxValue = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(5), Multiline = true, Height = 40 };
            formLayoutPanel.Controls.Add(labelValue, 0, 1);
            formLayoutPanel.Controls.Add(textBoxValue, 1, 1);

            // Category
            labelCategory = new Label { Text = "Category:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            textBoxCategory = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(5) };
            formLayoutPanel.Controls.Add(labelCategory, 0, 2);
            formLayoutPanel.Controls.Add(textBoxCategory, 1, 2);

            // Type
            labelType = new Label { Text = "Type:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            comboBoxType = new ComboBox 
            { 
                Dock = DockStyle.Fill, 
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBoxType.Items.AddRange(new[] { "string", "int", "bool", "double" });
            comboBoxType.SelectedIndex = 0;
            formLayoutPanel.Controls.Add(labelType, 0, 3);
            formLayoutPanel.Controls.Add(comboBoxType, 1, 3);

            // Description
            labelDescription = new Label { Text = "Description:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.TopLeft };
            textBoxDescription = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(5), Multiline = true, Height = 40 };
            formLayoutPanel.Controls.Add(labelDescription, 0, 4);
            formLayoutPanel.Controls.Add(textBoxDescription, 1, 4);

            groupBoxOptions.Controls.Add(formLayoutPanel);
            tableLayoutPanel.Controls.Add(groupBoxOptions, 0, 1);

            // Buttons panel
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = new Padding(5)
            };

            btnAdd = new Button { Text = "Add", Width = 80, Height = 30 };
            btnEdit = new Button { Text = "Edit", Width = 80, Height = 30 };
            btnDelete = new Button { Text = "Delete", Width = 80, Height = 30 };
            btnRefresh = new Button { Text = "Refresh", Width = 80, Height = 30 };

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;

            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            tableLayoutPanel.Controls.Add(buttonPanel, 0, 2);

            Controls.Add(tableLayoutPanel);

            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Name = "OptionsManagementPage";
            ResumeLayout(false);
        }

        private void SetupDataGridViewColumns()
        {
            dataGridViewOptions.Columns.Clear();

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50,
                Visible = false
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Key",
                HeaderText = "Key",
                Width = 150
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Value",
                HeaderText = "Value",
                Width = 200
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Category",
                HeaderText = "Category",
                Width = 120
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OptionType",
                HeaderText = "Type",
                Width = 80
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                Width = 200
            });

            dataGridViewOptions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ModifiedDate",
                HeaderText = "Modified",
                Width = 150
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
