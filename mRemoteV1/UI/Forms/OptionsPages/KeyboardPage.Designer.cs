

namespace mRemoteNG.UI.Forms.OptionsPages
{
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class KeyboardPage : OptionsPage
	{
			
		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
			
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
			
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			System.Windows.Forms.GroupBox LineGroupBox = default(System.Windows.Forms.GroupBox);
			mRemoteNG.Controls.Alignment Alignment1 = new mRemoteNG.Controls.Alignment();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyboardPage));
			this.btnDeleteKeyboardShortcut = new System.Windows.Forms.Button();
			this.btnDeleteKeyboardShortcut.Click += new System.EventHandler(this.btnDeleteKeyboardShortcut_Click);
			this.btnNewKeyboardShortcut = new System.Windows.Forms.Button();
			this.btnNewKeyboardShortcut.Click += new System.EventHandler(this.btnNewKeyboardShortcut_Click);
			this.grpModifyKeyboardShortcut = new System.Windows.Forms.GroupBox();
			this.hotModifyKeyboardShortcut = new SharedLibraryNG.HotkeyControl();
			this.hotModifyKeyboardShortcut.TextChanged += new System.EventHandler(this.hotModifyKeyboardShortcut_TextChanged);
			this.btnResetAllKeyboardShortcuts = new System.Windows.Forms.Button();
			this.btnResetAllKeyboardShortcuts.Click += new System.EventHandler(this.btnResetAllKeyboardShortcuts_Click);
			this.btnResetKeyboardShortcuts = new System.Windows.Forms.Button();
			this.btnResetKeyboardShortcuts.Click += new System.EventHandler(this.btnResetKeyboardShortcuts_Click);
			this.lblKeyboardCommand = new System.Windows.Forms.Label();
			this.lstKeyboardShortcuts = new System.Windows.Forms.ListBox();
			this.lstKeyboardShortcuts.SelectedIndexChanged += new System.EventHandler(this.lstKeyboardShortcuts_SelectedIndexChanged);
			this.lblKeyboardShortcuts = new System.Windows.Forms.Label();
			this.lvKeyboardCommands = new mRemoteNG.Controls.ListView();
			this.lvKeyboardCommands.SelectedIndexChanged += new System.EventHandler(this.lvKeyboardCommands_SelectedIndexChanged);
			LineGroupBox = new System.Windows.Forms.GroupBox();
			this.grpModifyKeyboardShortcut.SuspendLayout();
			this.SuspendLayout();
			//
			//LineGroupBox
			//
			LineGroupBox.Location = new System.Drawing.Point(212, 20);
			LineGroupBox.Name = "LineGroupBox";
			LineGroupBox.Size = new System.Drawing.Size(398, 3);
			LineGroupBox.TabIndex = 19;
			LineGroupBox.TabStop = false;
			//
			//btnDeleteKeyboardShortcut
			//
			this.btnDeleteKeyboardShortcut.Location = new System.Drawing.Point(293, 151);
			this.btnDeleteKeyboardShortcut.Name = "btnDeleteKeyboardShortcut";
			this.btnDeleteKeyboardShortcut.Size = new System.Drawing.Size(75, 23);
			this.btnDeleteKeyboardShortcut.TabIndex = 15;
			this.btnDeleteKeyboardShortcut.Text = "&Delete";
			this.btnDeleteKeyboardShortcut.UseVisualStyleBackColor = true;
			//
			//btnNewKeyboardShortcut
			//
			this.btnNewKeyboardShortcut.Location = new System.Drawing.Point(212, 151);
			this.btnNewKeyboardShortcut.Name = "btnNewKeyboardShortcut";
			this.btnNewKeyboardShortcut.Size = new System.Drawing.Size(75, 23);
			this.btnNewKeyboardShortcut.TabIndex = 14;
			this.btnNewKeyboardShortcut.Text = "&New";
			this.btnNewKeyboardShortcut.UseVisualStyleBackColor = true;
			//
			//grpModifyKeyboardShortcut
			//
			this.grpModifyKeyboardShortcut.Controls.Add(this.hotModifyKeyboardShortcut);
			this.grpModifyKeyboardShortcut.Location = new System.Drawing.Point(212, 180);
			this.grpModifyKeyboardShortcut.Name = "grpModifyKeyboardShortcut";
			this.grpModifyKeyboardShortcut.Size = new System.Drawing.Size(398, 103);
			this.grpModifyKeyboardShortcut.TabIndex = 17;
			this.grpModifyKeyboardShortcut.TabStop = false;
			this.grpModifyKeyboardShortcut.Text = "Modify Shortcut";
			//
			//hotModifyKeyboardShortcut
			//
			this.hotModifyKeyboardShortcut.HotkeyModifiers = System.Windows.Forms.Keys.None;
			this.hotModifyKeyboardShortcut.KeyCode = System.Windows.Forms.Keys.None;
			this.hotModifyKeyboardShortcut.Location = new System.Drawing.Point(27, 41);
			this.hotModifyKeyboardShortcut.Name = "hotModifyKeyboardShortcut";
			this.hotModifyKeyboardShortcut.Size = new System.Drawing.Size(344, 20);
			this.hotModifyKeyboardShortcut.TabIndex = 0;
			this.hotModifyKeyboardShortcut.Text = "None";
			//
			//btnResetAllKeyboardShortcuts
			//
			this.btnResetAllKeyboardShortcuts.Location = new System.Drawing.Point(3, 466);
			this.btnResetAllKeyboardShortcuts.Name = "btnResetAllKeyboardShortcuts";
			this.btnResetAllKeyboardShortcuts.Size = new System.Drawing.Size(120, 23);
			this.btnResetAllKeyboardShortcuts.TabIndex = 18;
			this.btnResetAllKeyboardShortcuts.Text = "Reset &All to Default";
			this.btnResetAllKeyboardShortcuts.UseVisualStyleBackColor = true;
			//
			//btnResetKeyboardShortcuts
			//
			this.btnResetKeyboardShortcuts.Location = new System.Drawing.Point(490, 151);
			this.btnResetKeyboardShortcuts.Name = "btnResetKeyboardShortcuts";
			this.btnResetKeyboardShortcuts.Size = new System.Drawing.Size(120, 23);
			this.btnResetKeyboardShortcuts.TabIndex = 16;
			this.btnResetKeyboardShortcuts.Text = "&Reset to Default";
			this.btnResetKeyboardShortcuts.UseVisualStyleBackColor = true;
			//
			//lblKeyboardCommand
			//
			this.lblKeyboardCommand.AutoSize = true;
			this.lblKeyboardCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (10.0F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.lblKeyboardCommand.Location = new System.Drawing.Point(209, 0);
			this.lblKeyboardCommand.Name = "lblKeyboardCommand";
			this.lblKeyboardCommand.Size = new System.Drawing.Size(71, 17);
			this.lblKeyboardCommand.TabIndex = 11;
			this.lblKeyboardCommand.Text = "Command";
			//
			//lstKeyboardShortcuts
			//
			this.lstKeyboardShortcuts.FormattingEnabled = true;
			this.lstKeyboardShortcuts.Location = new System.Drawing.Point(212, 50);
			this.lstKeyboardShortcuts.Name = "lstKeyboardShortcuts";
			this.lstKeyboardShortcuts.Size = new System.Drawing.Size(398, 95);
			this.lstKeyboardShortcuts.TabIndex = 13;
			//
			//lblKeyboardShortcuts
			//
			this.lblKeyboardShortcuts.AutoSize = true;
			this.lblKeyboardShortcuts.Location = new System.Drawing.Point(209, 34);
			this.lblKeyboardShortcuts.Name = "lblKeyboardShortcuts";
			this.lblKeyboardShortcuts.Size = new System.Drawing.Size(100, 13);
			this.lblKeyboardShortcuts.TabIndex = 12;
			this.lblKeyboardShortcuts.Text = "Keyboard Shortcuts";
			//
			//lvKeyboardCommands
			//
			this.lvKeyboardCommands.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvKeyboardCommands.HideSelection = false;
			this.lvKeyboardCommands.InactiveHighlightBackColor = System.Drawing.SystemColors.Highlight;
			this.lvKeyboardCommands.InactiveHighlightBorderColor = System.Drawing.SystemColors.HotTrack;
			this.lvKeyboardCommands.InactiveHighlightForeColor = System.Drawing.SystemColors.HighlightText;
			Alignment1.Horizontal = mRemoteNG.Controls.HorizontalAlignment.Left;
			Alignment1.Vertical = mRemoteNG.Controls.VerticalAlignment.Middle;
			this.lvKeyboardCommands.LabelAlignment = Alignment1;
			this.lvKeyboardCommands.LabelWrap = false;
			this.lvKeyboardCommands.Location = new System.Drawing.Point(3, 0);
			this.lvKeyboardCommands.MultiSelect = false;
			this.lvKeyboardCommands.Name = "lvKeyboardCommands";
			this.lvKeyboardCommands.OwnerDraw = true;
			this.lvKeyboardCommands.Size = new System.Drawing.Size(200, 460);
			this.lvKeyboardCommands.TabIndex = 10;
			this.lvKeyboardCommands.TileSize = new System.Drawing.Size(196, 20);
			this.lvKeyboardCommands.UseCompatibleStateImageBehavior = false;
			this.lvKeyboardCommands.View = System.Windows.Forms.View.Tile;
			//
			//KeyboardPage
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(LineGroupBox);
			this.Controls.Add(this.btnDeleteKeyboardShortcut);
			this.Controls.Add(this.btnNewKeyboardShortcut);
			this.Controls.Add(this.grpModifyKeyboardShortcut);
			this.Controls.Add(this.btnResetAllKeyboardShortcuts);
			this.Controls.Add(this.btnResetKeyboardShortcuts);
			this.Controls.Add(this.lblKeyboardCommand);
			this.Controls.Add(this.lstKeyboardShortcuts);
			this.Controls.Add(this.lblKeyboardShortcuts);
			this.Controls.Add(this.lvKeyboardCommands);
			this.Name = "KeyboardPage";
			this.PageIcon = (System.Drawing.Icon) (resources.GetObject("$this.PageIcon"));
			this.Size = new System.Drawing.Size(610, 489);
			this.grpModifyKeyboardShortcut.ResumeLayout(false);
			this.grpModifyKeyboardShortcut.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
				
		}
		internal System.Windows.Forms.Button btnDeleteKeyboardShortcut;
		internal System.Windows.Forms.Button btnNewKeyboardShortcut;
		internal System.Windows.Forms.GroupBox grpModifyKeyboardShortcut;
        internal SharedLibraryNG.HotkeyControl hotModifyKeyboardShortcut;
		internal System.Windows.Forms.Button btnResetAllKeyboardShortcuts;
		internal System.Windows.Forms.Button btnResetKeyboardShortcuts;
		internal System.Windows.Forms.Label lblKeyboardCommand;
		internal System.Windows.Forms.ListBox lstKeyboardShortcuts;
		internal System.Windows.Forms.Label lblKeyboardShortcuts;
		internal mRemoteNG.Controls.ListView lvKeyboardCommands;
			
	}
}
