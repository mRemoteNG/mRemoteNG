using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace mRemoteNG.UI
{
	namespace Window
	{
		public partial class ActiveDirectoryImport : UI.Window.Base
		{
			#region " Windows Form Designer generated code "

			private void InitializeComponent()
			{
				this.btnImport = new System.Windows.Forms.Button();
				this.txtDomain = new System.Windows.Forms.TextBox();
				this.lblDomain = new System.Windows.Forms.Label();
				this.btnChangeDomain = new System.Windows.Forms.Button();
				this.ActiveDirectoryTree = new ADTree.ADtree();
				this.SuspendLayout();
				//
				//btnImport
				//
				this.btnImport.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
				this.btnImport.Location = new System.Drawing.Point(443, 338);
				this.btnImport.Name = "btnImport";
				this.btnImport.Size = new System.Drawing.Size(75, 23);
				this.btnImport.TabIndex = 4;
				this.btnImport.Text = "&Import";
				this.btnImport.UseVisualStyleBackColor = true;
				//
				//txtDomain
				//
				this.txtDomain.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.txtDomain.Location = new System.Drawing.Point(12, 25);
				this.txtDomain.Name = "txtDomain";
				this.txtDomain.Size = new System.Drawing.Size(425, 20);
				this.txtDomain.TabIndex = 1;
				//
				//lblDomain
				//
				this.lblDomain.AutoSize = true;
				this.lblDomain.Location = new System.Drawing.Point(9, 9);
				this.lblDomain.Name = "lblDomain";
				this.lblDomain.Size = new System.Drawing.Size(46, 13);
				this.lblDomain.TabIndex = 0;
				this.lblDomain.Text = "Domain:";
				//
				//btnChangeDomain
				//
				this.btnChangeDomain.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
				this.btnChangeDomain.Location = new System.Drawing.Point(443, 23);
				this.btnChangeDomain.Name = "btnChangeDomain";
				this.btnChangeDomain.Size = new System.Drawing.Size(75, 23);
				this.btnChangeDomain.TabIndex = 2;
				this.btnChangeDomain.Text = "Change";
				this.btnChangeDomain.UseVisualStyleBackColor = true;
				//
				//ActiveDirectoryTree
				//
				this.ActiveDirectoryTree.ADPath = null;
				this.ActiveDirectoryTree.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.ActiveDirectoryTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				this.ActiveDirectoryTree.Location = new System.Drawing.Point(12, 52);
				this.ActiveDirectoryTree.Name = "ActiveDirectoryTree";
				this.ActiveDirectoryTree.SelectedNode = null;
				this.ActiveDirectoryTree.Size = new System.Drawing.Size(506, 280);
				this.ActiveDirectoryTree.TabIndex = 3;
				//
				//ADImport
				//
				this.AcceptButton = this.btnImport;
				this.ClientSize = new System.Drawing.Size(530, 373);
				this.Controls.Add(this.ActiveDirectoryTree);
				this.Controls.Add(this.lblDomain);
				this.Controls.Add(this.txtDomain);
				this.Controls.Add(this.btnChangeDomain);
				this.Controls.Add(this.btnImport);
				this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
				this.Icon = global::mRemoteNG.My.Resources.Resources.ActiveDirectory_Icon;
				this.Name = "ActiveDirectoryImport";
				this.TabText = "Active Directory Import";
				this.Text = "Active Directory Import";
				this.ResumeLayout(false);
				this.PerformLayout();

			}
			private System.Windows.Forms.Button withEventsField_btnImport;
			private System.Windows.Forms.Button btnImport {
				get { return withEventsField_btnImport; }
				set {
					if (withEventsField_btnImport != null) {
						withEventsField_btnImport.Click -= btnImport_Click;
					}
					withEventsField_btnImport = value;
					if (withEventsField_btnImport != null) {
						withEventsField_btnImport.Click += btnImport_Click;
					}
				}
			}
			private System.Windows.Forms.TextBox withEventsField_txtDomain;
			private System.Windows.Forms.TextBox txtDomain {
				get { return withEventsField_txtDomain; }
				set {
					if (withEventsField_txtDomain != null) {
						withEventsField_txtDomain.PreviewKeyDown -= txtDomain_PreviewKeyDown;
						withEventsField_txtDomain.KeyDown -= txtDomain_KeyDown;
					}
					withEventsField_txtDomain = value;
					if (withEventsField_txtDomain != null) {
						withEventsField_txtDomain.PreviewKeyDown += txtDomain_PreviewKeyDown;
						withEventsField_txtDomain.KeyDown += txtDomain_KeyDown;
					}
				}
			}
			private System.Windows.Forms.Label lblDomain;
			private System.Windows.Forms.Button withEventsField_btnChangeDomain;
			private System.Windows.Forms.Button btnChangeDomain {
				get { return withEventsField_btnChangeDomain; }
				set {
					if (withEventsField_btnChangeDomain != null) {
						withEventsField_btnChangeDomain.Click -= btnChangeDomain_Click;
					}
					withEventsField_btnChangeDomain = value;
					if (withEventsField_btnChangeDomain != null) {
						withEventsField_btnChangeDomain.Click += btnChangeDomain_Click;
					}
				}
			}
			private ADTree.ADtree withEventsField_ActiveDirectoryTree;
			private ADTree.ADtree ActiveDirectoryTree {
				get { return withEventsField_ActiveDirectoryTree; }
				set {
					if (withEventsField_ActiveDirectoryTree != null) {
						withEventsField_ActiveDirectoryTree.ADPathChanged -= ActiveDirectoryTree_ADPathChanged;
					}
					withEventsField_ActiveDirectoryTree = value;
					if (withEventsField_ActiveDirectoryTree != null) {
						withEventsField_ActiveDirectoryTree.ADPathChanged += ActiveDirectoryTree_ADPathChanged;
					}
				}
				#endregion
			}
		}
	}
}

