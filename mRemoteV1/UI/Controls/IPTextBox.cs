/* Source modified from here:
 * http://www.codeproject.com/Articles/11576/IP-TextBox
 * Original Author: mawnkay
 */
using System;
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
		public  Base.NGTextBox Octet1; 
        public  Base.NGTextBox Octet2;
        public  Base.NGTextBox Octet3;
        public  Base.NGTextBox Octet4;
        private Base.NGLabel label1;
        private Base.NGLabel label2;
		private Base.NGLabel label3;
		private ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		
		/** Sets and Gets the tooltiptext on toolTip1 */
		public string ToolTipText
		{
			get => toolTip1.GetToolTip(Octet1);
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
			get => Octet1.Text + @"." + Octet2.Text + @"." + Octet3.Text + @"." + Octet4.Text;
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
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyTheme();
            Themes.ThemeManager.getInstance().ThemeChanged += ApplyTheme;
        }

        private void ApplyTheme()
        { 
            if (Themes.ThemeManager.getInstance().ThemingActive)
                panel1.BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
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
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            label3 = new Base.NGLabel();
            label2 = new Base.NGLabel();
            Octet4 = new Base.NGTextBox();
            Octet3 = new Base.NGTextBox();
            Octet2 = new Base.NGTextBox();
            label1 = new Base.NGLabel();
            Octet1 = new Base.NGTextBox();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.Window;
            panel1.Controls.Add(Octet4);
            panel1.Controls.Add(Octet3);
            panel1.Controls.Add(Octet2);
            panel1.Controls.Add(Octet1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label3);
            panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(124, 18);
            panel1.TabIndex = 0;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(23, 1);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(8, 13);
            label3.TabIndex = 6;
            label3.Text = ".";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(86, 2);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(8, 13);
            label2.TabIndex = 5;
            label2.Text = ".";
            // 
            // Octet4
            // 
            Octet4.BackColor = System.Drawing.SystemColors.Menu;
            Octet4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Octet4.Font = new System.Drawing.Font("Segoe UI", 9F);
            Octet4.Location = new System.Drawing.Point(95, 1);
            Octet4.MaxLength = 3;
            Octet4.Name = "Octet4";
            Octet4.Size = new System.Drawing.Size(24, 16);
            Octet4.TabIndex = 4;
            Octet4.TabStop = false;
            Octet4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Octet4.Enter += new System.EventHandler(Box_Enter);
            Octet4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Box4_KeyPress);
            // 
            // Octet3
            // 
            Octet3.BackColor = System.Drawing.SystemColors.Menu;
            Octet3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Octet3.Font = new System.Drawing.Font("Segoe UI", 9F);
            Octet3.Location = new System.Drawing.Point(63, 1);
            Octet3.MaxLength = 3;
            Octet3.Name = "Octet3";
            Octet3.Size = new System.Drawing.Size(24, 16);
            Octet3.TabIndex = 3;
            Octet3.TabStop = false;
            Octet3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Octet3.Enter += new System.EventHandler(Box_Enter);
            Octet3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Box3_KeyPress);
            // 
            // Octet2
            // 
            Octet2.BackColor = System.Drawing.SystemColors.Menu;
            Octet2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Octet2.Font = new System.Drawing.Font("Segoe UI", 9F);
            Octet2.Location = new System.Drawing.Point(32, 1);
            Octet2.MaxLength = 3;
            Octet2.Name = "Octet2";
            Octet2.Size = new System.Drawing.Size(24, 16);
            Octet2.TabIndex = 2;
            Octet2.TabStop = false;
            Octet2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Octet2.Enter += new System.EventHandler(Box_Enter);
            Octet2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Box2_KeyPress);
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(55, 2);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(8, 13);
            label1.TabIndex = 1;
            label1.Text = ".";
            // 
            // Octet1
            // 
            Octet1.BackColor = System.Drawing.SystemColors.Menu;
            Octet1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Octet1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Octet1.Location = new System.Drawing.Point(1, 1);
            Octet1.MaxLength = 3;
            Octet1.Name = "Octet1";
            Octet1.Size = new System.Drawing.Size(24, 16);
            Octet1.TabIndex = 1;
            Octet1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Octet1.Enter += new System.EventHandler(Box_Enter);
            Octet1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Box1_KeyPress);
            // 
            // IPTextBox
            // 
            Controls.Add(panel1);
            Name = "IPTextBox";
            Size = new System.Drawing.Size(124, 18);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

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

                MessageBox.Show(Language.strIPRange,Language.strOutOfRange);
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
				    if (Octet1.Text.Length != 2) return;
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
				    if (Octet2.Text.Length != 2) return;
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
				    if (Octet3.Text.Length != 2) return;
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
				    if (Octet4.Text.Length != 2) return;
				    if (IsValid(Octet4.Text + e.KeyChar)) return;
				    Octet4.SelectAll();
				    e.Handled = true;
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

    }
}
