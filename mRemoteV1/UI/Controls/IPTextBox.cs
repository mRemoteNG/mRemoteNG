/* Source modified from here:
 * http://www.codeproject.com/Articles/11576/IP-TextBox
 * Original Author: mawnkay
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
	/** \class IPTextBox
	 * \brief An IP Address Box
	 * 
	 * A TextBox that only allows entry of a valid ip address
	 **
	 */
	public class IPTextBox: UserControl
	{
		private Panel panel1;
		public System.Windows.Forms.TextBox Octet1; 
        public System.Windows.Forms.TextBox Octet2;
        public System.Windows.Forms.TextBox Octet3;
        public System.Windows.Forms.TextBox Octet4;
        private Label label1;
        private Label label2;
		private Label label3;
		private ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		
		/** Sets and Gets the tooltiptext on toolTip1 */
		public string ToolTipText
		{
			get
			{	return toolTip1.GetToolTip(Octet1); }
			set
			{
				toolTip1.SetToolTip(Octet1,value);
				toolTip1.SetToolTip(Octet2,value);
				toolTip1.SetToolTip(Octet3,value);
				toolTip1.SetToolTip(Octet4,value);
				toolTip1.SetToolTip(label1,value);
				toolTip1.SetToolTip(label2,value);
				toolTip1.SetToolTip(label3,value);
			}
		}		

		/** Set or Get the string that represents the value in the box */
		public override string Text
		{
			get 
			{
				return Octet1.Text + @"." + Octet2.Text + @"." + Octet3.Text + @"." + Octet4.Text;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					var pieces = value.Split(@".".ToCharArray(),4);
					Octet1.Text = pieces[0];
					Octet2.Text = pieces[1];
					Octet3.Text = pieces[2];
					Octet4.Text = pieces[3];
				}
				else
				{
					Octet1.Text = "";
					Octet2.Text = "";
					Octet3.Text = "";
					Octet4.Text = "";
				}
			}
		}

		public IPTextBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			    // ReSharper disable once UseNullPropagation
                if(components != null)
			        components.Dispose();
			}
		    base.Dispose( disposing );
		}

		#region Component Designer generated code
		
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Octet4 = new System.Windows.Forms.TextBox();
            this.Octet3 = new System.Windows.Forms.TextBox();
            this.Octet2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Octet1 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Octet4);
            this.panel1.Controls.Add(this.Octet3);
            this.panel1.Controls.Add(this.Octet2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Octet1);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(128, 18);
            this.panel1.TabIndex = 0;
            this.panel1.EnabledChanged += new System.EventHandler(this.panel1_EnabledChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = ".";
            this.label3.EnabledChanged += new System.EventHandler(this.label_EnabledChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(88, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = ".";
            this.label2.EnabledChanged += new System.EventHandler(this.label_EnabledChanged);
            // 
            // Octet4
            // 
            this.Octet4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Octet4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Octet4.Location = new System.Drawing.Point(100, 0);
            this.Octet4.MaxLength = 3;
            this.Octet4.Name = "Octet4";
            this.Octet4.Size = new System.Drawing.Size(20, 16);
            this.Octet4.TabIndex = 4;
            this.Octet4.TabStop = false;
            this.Octet4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Octet4.Enter += new System.EventHandler(this.Box_Enter);
            this.Octet4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box4_KeyPress);
            // 
            // Octet3
            // 
            this.Octet3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Octet3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Octet3.Location = new System.Drawing.Point(64, 0);
            this.Octet3.MaxLength = 3;
            this.Octet3.Name = "Octet3";
            this.Octet3.Size = new System.Drawing.Size(20, 16);
            this.Octet3.TabIndex = 3;
            this.Octet3.TabStop = false;
            this.Octet3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Octet3.Enter += new System.EventHandler(this.Box_Enter);
            this.Octet3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box3_KeyPress);
            // 
            // Octet2
            // 
            this.Octet2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Octet2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Octet2.Location = new System.Drawing.Point(32, 0);
            this.Octet2.MaxLength = 3;
            this.Octet2.Name = "Octet2";
            this.Octet2.Size = new System.Drawing.Size(20, 16);
            this.Octet2.TabIndex = 2;
            this.Octet2.TabStop = false;
            this.Octet2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Octet2.Enter += new System.EventHandler(this.Box_Enter);
            this.Octet2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box2_KeyPress);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(56, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = ".";
            this.label1.EnabledChanged += new System.EventHandler(this.label_EnabledChanged);
            // 
            // Octet1
            // 
            this.Octet1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Octet1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Octet1.Location = new System.Drawing.Point(4, 0);
            this.Octet1.MaxLength = 3;
            this.Octet1.Name = "Octet1";
            this.Octet1.Size = new System.Drawing.Size(20, 16);
            this.Octet1.TabIndex = 1;
            this.Octet1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Octet1.Enter += new System.EventHandler(this.Box_Enter);
            this.Octet1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box1_KeyPress);
            // 
            // IPTextBox
            // 
            this.Controls.Add(this.panel1);
            this.Name = "IPTextBox";
            this.Size = new System.Drawing.Size(128, 18);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
		
		/** 
		 * \ifnot hide_events
		 * Checks that a string passed in resolves to an integer value between 0 and 255
		 * \param inString The string passed in for testing
		 * \return True if the string is between 0 and 255 inclusively, false otherwise
		 * \endif
		 * */
		private static bool IsValid(string inString)
		{
			try 
			{
				var theValue = int.Parse(inString);
				if(theValue >=0 && theValue <= 255)
					return true;

                MessageBox.Show("Must Be Between 0 and 255","Out Of Range");
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// \ifnot hide_events
		/// Performs KeyPress analysis and handling to ensure a valid ip octet is
		/// being entered in Box1.
		/// \endif
		private void Box1_KeyPress(object sender, KeyPressEventArgs e)
		{
			//Only Accept a '.', a numeral, or backspace
			if(e.KeyChar.ToString() == "." || char.IsDigit(e.KeyChar) || e.KeyChar == 8)
			{
				//If the key pressed is a '.'
				if(e.KeyChar.ToString() == ".")
				{
					//If the Text is a valid ip octet move to the next box
					if(Octet1.Text != "" && Octet1.Text.Length != Octet1.SelectionLength)
					{
						if(IsValid(Octet1.Text))
							Octet2.Focus();
						else
							Octet1.SelectAll();
					}
					e.Handled = true;
				}
			
				//If we are not overwriting the whole text
				else if(Octet1.SelectionLength != Octet1.Text.Length)
				{	
					//Check that the new Text value will be a valid
					// ip octet then move on to next box
					if(Octet1.Text.Length == 2)
					{
						if(!IsValid(Octet1.Text + e.KeyChar))
						{
							Octet1.SelectAll();
							e.Handled = true;
						}
						else
						{
							Octet2.Focus();
						}
					}
				}
			}
			//Do nothing if the keypress is not numeral, backspace, or '.'
			else
				e.Handled = true;
		}

		/// \ifnot hide_events
		/// Performs KeyPress analysis and handling to ensure a valid ip octet is
		/// being entered in Box2.
		/// \endif
		private void Box2_KeyPress(object sender, KeyPressEventArgs e)
		{
			//Similar to Box1_KeyPress but in special case for backspace moves cursor
			//to the previouse box (Box1)
			if(e.KeyChar.ToString() == "." || char.IsDigit(e.KeyChar) || e.KeyChar == 8)
			{
				if(e.KeyChar.ToString() == ".")
				{
					if(Octet2.Text != "" && Octet2.Text.Length != Octet2.SelectionLength)
					{
						if(IsValid(Octet1.Text))
							Octet3.Focus();
						else
							Octet2.SelectAll();
					}
					e.Handled = true;
				}			
				else if(Octet2.SelectionLength != Octet2.Text.Length)
				{
					if(Octet2.Text.Length == 2)
					{
						if(!IsValid(Octet2.Text + e.KeyChar))
						{
							Octet2.SelectAll();
							e.Handled = true;
						}
						else
						{
							Octet3.Focus();
						}
					}
				}
				else if(Octet2.Text.Length == 0 && e.KeyChar == 8)
				{
					Octet1.Focus();
					Octet1.SelectionStart = Octet1.Text.Length;
				}
			}
			else
				e.Handled = true;
		
		}

		/// \ifnot hide_events
		/// Performs KeyPress analysis and handling to ensure a valid ip octet is
		/// being entered in Box3.
		/// \endif
		private void Box3_KeyPress(object sender, KeyPressEventArgs e)
		{
			//Identical to Box2_KeyPress except that previous box is Box2 and
			//next box is Box3
			if(e.KeyChar.ToString() == "." || char.IsDigit(e.KeyChar) || e.KeyChar == 8)
			{
				if(e.KeyChar.ToString() == ".")
				{
					if(Octet3.Text != "" && Octet3.SelectionLength != Octet3.Text.Length)
					{
						if(IsValid(Octet1.Text))
							Octet4.Focus();
						else
							Octet3.SelectAll();
					}
					e.Handled = true;
				}			
				else if(Octet3.SelectionLength != Octet3.Text.Length)
				{
					if(Octet3.Text.Length == 2)
					{
						if(!IsValid(Octet3.Text + e.KeyChar))
						{
							Octet3.SelectAll();
							e.Handled = true;
						}
						else
						{
							Octet4.Focus();
						}
					}
				}
				else if(Octet3.Text.Length == 0 && e.KeyChar == 8)
				{
					Octet2.Focus();
					Octet2.SelectionStart = Octet2.Text.Length;
				}
			}
			else
				e.Handled = true;
		}

		/// \ifnot hide_events
		/// Performs KeyPress analysis and handling to ensure a valid ip octet is
		/// being entered in Box4.
		/// \endif
		private void Box4_KeyPress(object sender, KeyPressEventArgs e)
		{
			//Similar to Box3 but ignores the '.' character and does not advance
			//to the next box.  Also Box3 is previous box for backspace case.
			if(char.IsDigit(e.KeyChar) || e.KeyChar == 8)
			{
				if(Octet4.SelectionLength != Octet4.Text.Length)
				{
					if(Octet4.Text.Length == 2)
					{
						if(!IsValid(Octet4.Text + e.KeyChar))
						{
							Octet4.SelectAll();
							e.Handled = true;
						}
					}
				}
				else if(Octet4.Text.Length == 0 && e.KeyChar == 8)
				{
					Octet3.Focus();
					Octet3.SelectionStart = Octet3.Text.Length;
				}
			}
			else
				e.Handled = true;
		}

		/// \ifnot hide_events
		/// Selects All text in a box for overwriting upon entering the box
		/// \endif
		private void Box_Enter(object sender, EventArgs e)
		{
			var tb = (TextBox) sender;
			tb.SelectAll();
		}

		/// \ifnot hide_events
		/// Ensures a consistent "grayed out" look when the control is disabled
		/// \endif
		private void label_EnabledChanged(object sender, EventArgs e)
		{
		    var lbl = (Label) sender;
		    lbl.BackColor = lbl.Enabled ? SystemColors.Window : SystemColors.Control;
		}

		/// \ifnot hide_events
		/// Ensures a consistent "grayed out" look when the control is disabled
		/// \endif
		private void panel1_EnabledChanged(object sender, EventArgs e)
		{
		    var pan = (Panel) sender;
		    pan.BackColor = pan.Enabled ? SystemColors.Window : SystemColors.Control;
		}
	}
}
