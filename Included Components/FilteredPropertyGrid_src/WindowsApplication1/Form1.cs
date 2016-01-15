using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1
{
	public partial class Form1 : Form
	{
		public Form1() {
			InitializeComponent();
			this.ControlsComboBox.SelectedIndexChanged += new EventHandler(OnControlsComboBoxSelectedIndexChanged);
		}

		void OnControlsComboBoxSelectedIndexChanged(object sender,EventArgs e) {
			this.filteredPropertyGrid1.SelectedObject = this.Controls[this.ControlsComboBox.SelectedIndex];
			this.filteredPropertyGrid1.Refresh();
		}

		protected override void OnLoad(EventArgs e) {
			foreach(Control control in this.Controls) ControlsComboBox.Items.Add(control);
			ControlsComboBox.SelectedItem = ControlsComboBox.Items[0];
			base.OnLoad(e);
		}

		private void OnButtonRefreshClick(object sender,EventArgs e) {
			try {
				this.filteredPropertyGrid1.HiddenAttributes = ParseAttributes(ParseTextBoxText(this.TextBoxHideAttributes));
				this.filteredPropertyGrid1.HiddenProperties = ParseTextBoxText(this.TextBoxHideProperties);
				this.filteredPropertyGrid1.BrowsableAttributes = ParseAttributes(ParseTextBoxText(this.TextBoxShowAttributes));
				this.filteredPropertyGrid1.BrowsableProperties = ParseTextBoxText(this.TextBoxShowProperties);
				this.filteredPropertyGrid1.Refresh();
			} catch(ArgumentException aex) {
				MessageBox.Show(aex.Message);
			} catch { }
		}

		private string[] ParseTextBoxText(TextBox textbox) {
			return textbox.Text.Length > 0 ? textbox.Text.Replace(" ","").Split(new char[] { ',' }) : null;
		}
		private AttributeCollection ParseAttributes(string[] categorynames) {
			if(categorynames == null) return null;
			Attribute[] attributes = new Attribute[categorynames.Length];
			for(int iattribute = 0; iattribute<categorynames.Length; iattribute++) {
				attributes[iattribute] = new CategoryAttribute(categorynames[iattribute]);
			}
			return new AttributeCollection(attributes);
		}
	}
}