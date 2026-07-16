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
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel.Controls.Add(dataGridViewOptions, 0, 0);
            tableLayoutPanel.Controls.Add(groupBoxOptions, 0, 1);
            tableLayoutPanel.Controls.Add(buttonPanel, 0, 2);
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.Size = new System.Drawing.Size(200, 100);
            tableLayoutPanel.TabIndex = 0;
            // 
            // dataGridViewOptions
            // 
            dataGridViewOptions.Location = new System.Drawing.Point(3, 3);
            dataGridViewOptions.Name = "dataGridViewOptions";
            dataGridViewOptions.Size = new System.Drawing.Size(194, 1);
            dataGridViewOptions.TabIndex = 0;
            dataGridViewOptions.SelectionChanged += DataGridViewOptions_SelectionChanged;
            // 
            // groupBoxOptions
            // 
            groupBoxOptions.Controls.Add(formLayoutPanel);
            groupBoxOptions.Location = new System.Drawing.Point(3, -109);
            groupBoxOptions.Name = "groupBoxOptions";
            groupBoxOptions.Size = new System.Drawing.Size(194, 100);
            groupBoxOptions.TabIndex = 1;
            groupBoxOptions.TabStop = false;
            // 
            // formLayoutPanel
            // 
            formLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
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
            formLayoutPanel.Location = new System.Drawing.Point(0, 0);
            formLayoutPanel.Name = "formLayoutPanel";
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            formLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            formLayoutPanel.Size = new System.Drawing.Size(200, 100);
            formLayoutPanel.TabIndex = 0;
            // 
            // labelKey
            // 
            labelKey.Location = new System.Drawing.Point(3, 0);
            labelKey.Name = "labelKey";
            labelKey.Size = new System.Drawing.Size(94, 20);
            labelKey.TabIndex = 0;
            // 
            // textBoxKey
            // 
            textBoxKey.Location = new System.Drawing.Point(103, 3);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.Size = new System.Drawing.Size(94, 22);
            textBoxKey.TabIndex = 1;
            // 
            // labelValue
            // 
            labelValue.Location = new System.Drawing.Point(3, 20);
            labelValue.Name = "labelValue";
            labelValue.Size = new System.Drawing.Size(94, 20);
            labelValue.TabIndex = 2;
            // 
            // textBoxValue
            // 
            textBoxValue.Location = new System.Drawing.Point(103, 23);
            textBoxValue.Name = "textBoxValue";
            textBoxValue.Size = new System.Drawing.Size(94, 22);
            textBoxValue.TabIndex = 3;
            // 
            // labelCategory
            // 
            labelCategory.Location = new System.Drawing.Point(3, 40);
            labelCategory.Name = "labelCategory";
            labelCategory.Size = new System.Drawing.Size(94, 20);
            labelCategory.TabIndex = 4;
            // 
            // textBoxCategory
            // 
            textBoxCategory.Location = new System.Drawing.Point(103, 43);
            textBoxCategory.Name = "textBoxCategory";
            textBoxCategory.Size = new System.Drawing.Size(94, 22);
            textBoxCategory.TabIndex = 5;
            // 
            // labelType
            // 
            labelType.Location = new System.Drawing.Point(3, 60);
            labelType.Name = "labelType";
            labelType.Size = new System.Drawing.Size(94, 20);
            labelType.TabIndex = 6;
            // 
            // comboBoxType
            // 
            comboBoxType.Items.AddRange(new object[] { "string", "int", "bool", "double" });
            comboBoxType.Location = new System.Drawing.Point(103, 63);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.Size = new System.Drawing.Size(94, 21);
            comboBoxType.TabIndex = 7;
            // 
            // labelDescription
            // 
            labelDescription.Location = new System.Drawing.Point(3, 80);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new System.Drawing.Size(94, 20);
            labelDescription.TabIndex = 8;
            // 
            // textBoxDescription
            // 
            textBoxDescription.Location = new System.Drawing.Point(103, 83);
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.Size = new System.Drawing.Size(94, 22);
            textBoxDescription.TabIndex = 9;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);
            buttonPanel.Location = new System.Drawing.Point(3, -3);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(194, 100);
            buttonPanel.TabIndex = 2;
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(3, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(75, 23);
            btnAdd.TabIndex = 0;
            btnAdd.Click += BtnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new System.Drawing.Point(84, 3);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(75, 23);
            btnEdit.TabIndex = 1;
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(3, 32);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 23);
            btnDelete.TabIndex = 2;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new System.Drawing.Point(84, 32);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(75, 23);
            btnRefresh.TabIndex = 3;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // OptionsManagementPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(tableLayoutPanel);
            Name = "OptionsManagementPage";
            tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewOptions).EndInit();
            groupBoxOptions.ResumeLayout(false);
            formLayoutPanel.ResumeLayout(false);
            formLayoutPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
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
