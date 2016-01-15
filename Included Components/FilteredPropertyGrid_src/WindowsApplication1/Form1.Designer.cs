namespace WindowsApplication1
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.ButtonRefresh = new System.Windows.Forms.Button();
			this.TextBoxHideAttributes = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.TextBoxHideProperties = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.ControlsComboBox = new System.Windows.Forms.ComboBox();
			this.filteredPropertyGrid1 = new Azuria.Common.Controls.FilteredPropertyGrid.FilteredPropertyGrid();
			this.label4 = new System.Windows.Forms.Label();
			this.TextBoxShowProperties = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.TextBoxShowAttributes = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// ButtonRefresh
			// 
			this.ButtonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonRefresh.Location = new System.Drawing.Point(495,444);
			this.ButtonRefresh.Name = "ButtonRefresh";
			this.ButtonRefresh.Size = new System.Drawing.Size(90,25);
			this.ButtonRefresh.TabIndex = 1;
			this.ButtonRefresh.Text = "Refresh";
			this.ButtonRefresh.UseVisualStyleBackColor = true;
			this.ButtonRefresh.Click += new System.EventHandler(this.OnButtonRefreshClick);
			// 
			// TextBoxHideAttributes
			// 
			this.TextBoxHideAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TextBoxHideAttributes.Location = new System.Drawing.Point(131,323);
			this.TextBoxHideAttributes.Name = "TextBoxHideAttributes";
			this.TextBoxHideAttributes.Size = new System.Drawing.Size(454,20);
			this.TextBoxHideAttributes.TabIndex = 2;
			this.TextBoxHideAttributes.Text = "Layout";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12,326);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(110,13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Hide these attributes :";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12,352);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(113,13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Hide these properties :";
			// 
			// TextBoxHideProperties
			// 
			this.TextBoxHideProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TextBoxHideProperties.Location = new System.Drawing.Point(131,349);
			this.TextBoxHideProperties.Name = "TextBoxHideProperties";
			this.TextBoxHideProperties.Size = new System.Drawing.Size(454,20);
			this.TextBoxHideProperties.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12,299);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(94,13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Select this object :";
			// 
			// ControlsComboBox
			// 
			this.ControlsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ControlsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ControlsComboBox.Location = new System.Drawing.Point(131,296);
			this.ControlsComboBox.Name = "ControlsComboBox";
			this.ControlsComboBox.Size = new System.Drawing.Size(454,21);
			this.ControlsComboBox.TabIndex = 7;
			// 
			// filteredPropertyGrid1
			// 
			this.filteredPropertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.filteredPropertyGrid1.BrowsableProperties = null;
			this.filteredPropertyGrid1.HiddenAttributes = null;
			this.filteredPropertyGrid1.HiddenProperties = null;
			this.filteredPropertyGrid1.Location = new System.Drawing.Point(12,12);
			this.filteredPropertyGrid1.Name = "filteredPropertyGrid1";
			this.filteredPropertyGrid1.Size = new System.Drawing.Size(573,269);
			this.filteredPropertyGrid1.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12,404);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(118,13);
			this.label4.TabIndex = 11;
			this.label4.Text = "Show these properties :";
			// 
			// TextBoxShowProperties
			// 
			this.TextBoxShowProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TextBoxShowProperties.Location = new System.Drawing.Point(131,401);
			this.TextBoxShowProperties.Name = "TextBoxShowProperties";
			this.TextBoxShowProperties.Size = new System.Drawing.Size(454,20);
			this.TextBoxShowProperties.TabIndex = 10;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12,378);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(115,13);
			this.label5.TabIndex = 9;
			this.label5.Text = "Show these attributes :";
			// 
			// TextBoxShowAttributes
			// 
			this.TextBoxShowAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TextBoxShowAttributes.Location = new System.Drawing.Point(131,375);
			this.TextBoxShowAttributes.Name = "TextBoxShowAttributes";
			this.TextBoxShowAttributes.Size = new System.Drawing.Size(454,20);
			this.TextBoxShowAttributes.TabIndex = 8;
			this.TextBoxShowAttributes.Text = "Layout";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(597,481);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.TextBoxShowProperties);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.TextBoxShowAttributes);
			this.Controls.Add(this.ControlsComboBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.TextBoxHideProperties);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TextBoxHideAttributes);
			this.Controls.Add(this.ButtonRefresh);
			this.Controls.Add(this.filteredPropertyGrid1);
			this.Name = "Form1";
			this.Text = "FilterPropertyGrid test application";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Azuria.Common.Controls.FilteredPropertyGrid filteredPropertyGrid1;
		private System.Windows.Forms.Button ButtonRefresh;
		private System.Windows.Forms.TextBox TextBoxHideAttributes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TextBoxHideProperties;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox ControlsComboBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox TextBoxShowProperties;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox TextBoxShowAttributes;
	}
}

