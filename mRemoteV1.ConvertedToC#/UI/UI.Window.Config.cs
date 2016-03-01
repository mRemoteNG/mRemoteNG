using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using mRemoteNG.Messages;
using mRemoteNG.My;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Root;
using WeifenLuo.WinFormsUI.Docking;
using System.Net.NetworkInformation;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.UI
{
	namespace Window
	{
		public class Config : UI.Window.Base
		{


			#region "Form Init"
			private System.Windows.Forms.ToolStripButton withEventsField_btnShowProperties;
			internal System.Windows.Forms.ToolStripButton btnShowProperties {
				get { return withEventsField_btnShowProperties; }
				set {
					if (withEventsField_btnShowProperties != null) {
						withEventsField_btnShowProperties.Click -= btnShowProperties_Click;
					}
					withEventsField_btnShowProperties = value;
					if (withEventsField_btnShowProperties != null) {
						withEventsField_btnShowProperties.Click += btnShowProperties_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripButton withEventsField_btnShowDefaultProperties;
			internal System.Windows.Forms.ToolStripButton btnShowDefaultProperties {
				get { return withEventsField_btnShowDefaultProperties; }
				set {
					if (withEventsField_btnShowDefaultProperties != null) {
						withEventsField_btnShowDefaultProperties.Click -= btnShowDefaultProperties_Click;
					}
					withEventsField_btnShowDefaultProperties = value;
					if (withEventsField_btnShowDefaultProperties != null) {
						withEventsField_btnShowDefaultProperties.Click += btnShowDefaultProperties_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripButton withEventsField_btnShowInheritance;
			internal System.Windows.Forms.ToolStripButton btnShowInheritance {
				get { return withEventsField_btnShowInheritance; }
				set {
					if (withEventsField_btnShowInheritance != null) {
						withEventsField_btnShowInheritance.Click -= btnShowInheritance_Click;
					}
					withEventsField_btnShowInheritance = value;
					if (withEventsField_btnShowInheritance != null) {
						withEventsField_btnShowInheritance.Click += btnShowInheritance_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripButton withEventsField_btnShowDefaultInheritance;
			internal System.Windows.Forms.ToolStripButton btnShowDefaultInheritance {
				get { return withEventsField_btnShowDefaultInheritance; }
				set {
					if (withEventsField_btnShowDefaultInheritance != null) {
						withEventsField_btnShowDefaultInheritance.Click -= btnShowDefaultInheritance_Click;
					}
					withEventsField_btnShowDefaultInheritance = value;
					if (withEventsField_btnShowDefaultInheritance != null) {
						withEventsField_btnShowDefaultInheritance.Click += btnShowDefaultInheritance_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripButton withEventsField_btnIcon;
			internal System.Windows.Forms.ToolStripButton btnIcon {
				get { return withEventsField_btnIcon; }
				set {
					if (withEventsField_btnIcon != null) {
						withEventsField_btnIcon.MouseUp -= btnIcon_Click;
					}
					withEventsField_btnIcon = value;
					if (withEventsField_btnIcon != null) {
						withEventsField_btnIcon.MouseUp += btnIcon_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripButton withEventsField_btnHostStatus;
			internal System.Windows.Forms.ToolStripButton btnHostStatus {
				get { return withEventsField_btnHostStatus; }
				set {
					if (withEventsField_btnHostStatus != null) {
						withEventsField_btnHostStatus.Click -= btnHostStatus_Click;
					}
					withEventsField_btnHostStatus = value;
					if (withEventsField_btnHostStatus != null) {
						withEventsField_btnHostStatus.Click += btnHostStatus_Click;
					}
				}
			}
			internal System.Windows.Forms.ContextMenuStrip cMenIcons;
			private System.ComponentModel.IContainer components;
			private System.Windows.Forms.ContextMenuStrip withEventsField_propertyGridContextMenu;
			internal System.Windows.Forms.ContextMenuStrip propertyGridContextMenu {
				get { return withEventsField_propertyGridContextMenu; }
				set {
					if (withEventsField_propertyGridContextMenu != null) {
						withEventsField_propertyGridContextMenu.Opening -= propertyGridContextMenu_Opening;
					}
					withEventsField_propertyGridContextMenu = value;
					if (withEventsField_propertyGridContextMenu != null) {
						withEventsField_propertyGridContextMenu.Opening += propertyGridContextMenu_Opening;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_propertyGridContextMenuShowHelpText;
			internal System.Windows.Forms.ToolStripMenuItem propertyGridContextMenuShowHelpText {
				get { return withEventsField_propertyGridContextMenuShowHelpText; }
				set {
					if (withEventsField_propertyGridContextMenuShowHelpText != null) {
						withEventsField_propertyGridContextMenuShowHelpText.Click -= propertyGridContextMenuShowHelpText_Click;
						withEventsField_propertyGridContextMenuShowHelpText.CheckedChanged -= propertyGridContextMenuShowHelpText_CheckedChanged;
					}
					withEventsField_propertyGridContextMenuShowHelpText = value;
					if (withEventsField_propertyGridContextMenuShowHelpText != null) {
						withEventsField_propertyGridContextMenuShowHelpText.Click += propertyGridContextMenuShowHelpText_Click;
						withEventsField_propertyGridContextMenuShowHelpText.CheckedChanged += propertyGridContextMenuShowHelpText_CheckedChanged;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_propertyGridContextMenuReset;
			internal System.Windows.Forms.ToolStripMenuItem propertyGridContextMenuReset {
				get { return withEventsField_propertyGridContextMenuReset; }
				set {
					if (withEventsField_propertyGridContextMenuReset != null) {
						withEventsField_propertyGridContextMenuReset.Click -= propertyGridContextMenuReset_Click;
					}
					withEventsField_propertyGridContextMenuReset = value;
					if (withEventsField_propertyGridContextMenuReset != null) {
						withEventsField_propertyGridContextMenuReset.Click += propertyGridContextMenuReset_Click;
					}
				}
			}
			internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
			private Azuria.Common.Controls.FilteredPropertyGrid withEventsField_pGrid;
			internal Azuria.Common.Controls.FilteredPropertyGrid pGrid {
				get { return withEventsField_pGrid; }
				set {
					if (withEventsField_pGrid != null) {
						withEventsField_pGrid.PropertyValueChanged -= pGrid_PropertyValueChanged;
						withEventsField_pGrid.PropertySortChanged -= pGrid_PropertySortChanged;
					}
					withEventsField_pGrid = value;
					if (withEventsField_pGrid != null) {
						withEventsField_pGrid.PropertyValueChanged += pGrid_PropertyValueChanged;
						withEventsField_pGrid.PropertySortChanged += pGrid_PropertySortChanged;
					}
				}
			}
			private void InitializeComponent()
			{
				this.components = new System.ComponentModel.Container();
				this.pGrid = new Azuria.Common.Controls.FilteredPropertyGrid();
				this.propertyGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
				this.propertyGridContextMenuReset = new System.Windows.Forms.ToolStripMenuItem();
				this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
				this.propertyGridContextMenuShowHelpText = new System.Windows.Forms.ToolStripMenuItem();
				this.btnShowInheritance = new System.Windows.Forms.ToolStripButton();
				this.btnShowDefaultInheritance = new System.Windows.Forms.ToolStripButton();
				this.btnShowProperties = new System.Windows.Forms.ToolStripButton();
				this.btnShowDefaultProperties = new System.Windows.Forms.ToolStripButton();
				this.btnIcon = new System.Windows.Forms.ToolStripButton();
				this.btnHostStatus = new System.Windows.Forms.ToolStripButton();
				this.cMenIcons = new System.Windows.Forms.ContextMenuStrip(this.components);
				this.propertyGridContextMenu.SuspendLayout();
				this.SuspendLayout();
				//
				//pGrid
				//
				this.pGrid.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.pGrid.BrowsableProperties = null;
				this.pGrid.ContextMenuStrip = this.propertyGridContextMenu;
				this.pGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
				this.pGrid.HiddenAttributes = null;
				this.pGrid.HiddenProperties = null;
				this.pGrid.Location = new System.Drawing.Point(0, 0);
				this.pGrid.Name = "pGrid";
				this.pGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
				this.pGrid.Size = new System.Drawing.Size(226, 530);
				this.pGrid.TabIndex = 0;
				this.pGrid.UseCompatibleTextRendering = true;
				//
				//propertyGridContextMenu
				//
				this.propertyGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.propertyGridContextMenuReset,
					this.ToolStripSeparator1,
					this.propertyGridContextMenuShowHelpText
				});
				this.propertyGridContextMenu.Name = "propertyGridContextMenu";
				this.propertyGridContextMenu.Size = new System.Drawing.Size(157, 76);
				//
				//propertyGridContextMenuReset
				//
				this.propertyGridContextMenuReset.Name = "propertyGridContextMenuReset";
				this.propertyGridContextMenuReset.Size = new System.Drawing.Size(156, 22);
				this.propertyGridContextMenuReset.Text = "&Reset";
				//
				//ToolStripSeparator1
				//
				this.ToolStripSeparator1.Name = "ToolStripSeparator1";
				this.ToolStripSeparator1.Size = new System.Drawing.Size(153, 6);
				//
				//propertyGridContextMenuShowHelpText
				//
				this.propertyGridContextMenuShowHelpText.Name = "propertyGridContextMenuShowHelpText";
				this.propertyGridContextMenuShowHelpText.Size = new System.Drawing.Size(156, 22);
				this.propertyGridContextMenuShowHelpText.Text = "&Show Help Text";
				//
				//btnShowInheritance
				//
				this.btnShowInheritance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.btnShowInheritance.Image = global::mRemoteNG.My.Resources.Resources.Inheritance;
				this.btnShowInheritance.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.btnShowInheritance.Name = "btnShowInheritance";
				this.btnShowInheritance.Size = new System.Drawing.Size(23, 22);
				this.btnShowInheritance.Text = "Inheritance";
				//
				//btnShowDefaultInheritance
				//
				this.btnShowDefaultInheritance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.btnShowDefaultInheritance.Image = global::mRemoteNG.My.Resources.Resources.Inheritance_Default;
				this.btnShowDefaultInheritance.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.btnShowDefaultInheritance.Name = "btnShowDefaultInheritance";
				this.btnShowDefaultInheritance.Size = new System.Drawing.Size(23, 22);
				this.btnShowDefaultInheritance.Text = "Default Inheritance";
				//
				//btnShowProperties
				//
				this.btnShowProperties.Checked = true;
				this.btnShowProperties.CheckState = System.Windows.Forms.CheckState.Checked;
				this.btnShowProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.btnShowProperties.Image = global::mRemoteNG.My.Resources.Resources.Properties;
				this.btnShowProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.btnShowProperties.Name = "btnShowProperties";
				this.btnShowProperties.Size = new System.Drawing.Size(23, 22);
				this.btnShowProperties.Text = "Properties";
				//
				//btnShowDefaultProperties
				//
				this.btnShowDefaultProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.btnShowDefaultProperties.Image = global::mRemoteNG.My.Resources.Resources.Properties_Default;
				this.btnShowDefaultProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.btnShowDefaultProperties.Name = "btnShowDefaultProperties";
				this.btnShowDefaultProperties.Size = new System.Drawing.Size(23, 22);
				this.btnShowDefaultProperties.Text = "Default Properties";
				//
				//btnIcon
				//
				this.btnIcon.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
				this.btnIcon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.btnIcon.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.btnIcon.Name = "btnIcon";
				this.btnIcon.Size = new System.Drawing.Size(23, 22);
				this.btnIcon.Text = "Icon";
				//
				//btnHostStatus
				//
				this.btnHostStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
				this.btnHostStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
				this.btnHostStatus.Image = global::mRemoteNG.My.Resources.Resources.HostStatus_Check;
				this.btnHostStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
				this.btnHostStatus.Name = "btnHostStatus";
				this.btnHostStatus.Size = new System.Drawing.Size(23, 22);
				this.btnHostStatus.Tag = "checking";
				this.btnHostStatus.Text = "Status";
				//
				//cMenIcons
				//
				this.cMenIcons.Name = "cMenIcons";
				this.cMenIcons.Size = new System.Drawing.Size(61, 4);
				//
				//Config
				//
				this.ClientSize = new System.Drawing.Size(226, 530);
				this.Controls.Add(this.pGrid);
				this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
				this.HideOnClose = true;
				this.Icon = global::mRemoteNG.My.Resources.Resources.Config_Icon;
				this.Name = "Config";
				this.TabText = "Config";
				this.Text = "Config";
				this.propertyGridContextMenu.ResumeLayout(false);
				this.ResumeLayout(false);

			}
			#endregion

			#region "Private Properties"
				#endregion
			private bool ConfigLoading = false;

			#region "Public Properties"
			public bool PropertiesVisible {
				get {
					if (this.btnShowProperties.Checked) {
						return true;
					} else {
						return false;
					}
				}
				set {
					this.btnShowProperties.Checked = value;

					if (value == true) {
						this.btnShowInheritance.Checked = false;
						this.btnShowDefaultInheritance.Checked = false;
						this.btnShowDefaultProperties.Checked = false;
					}
				}
			}

			public bool InheritanceVisible {
				get {
					if (this.btnShowInheritance.Checked) {
						return true;
					} else {
						return false;
					}
				}
				set {
					this.btnShowInheritance.Checked = value;

					if (value == true) {
						this.btnShowProperties.Checked = false;
						this.btnShowDefaultInheritance.Checked = false;
						this.btnShowDefaultProperties.Checked = false;
					}
				}
			}

			public bool DefaultPropertiesVisible {
				get {
					if (this.btnShowDefaultProperties.Checked) {
						return true;
					} else {
						return false;
					}
				}
				set {
					this.btnShowDefaultProperties.Checked = value;

					if (value == true) {
						this.btnShowProperties.Checked = false;
						this.btnShowDefaultInheritance.Checked = false;
						this.btnShowInheritance.Checked = false;
					}
				}
			}

			public bool DefaultInheritanceVisible {
				get {
					if (this.btnShowDefaultInheritance.Checked) {
						return true;
					} else {
						return false;
					}
				}
				set {
					this.btnShowDefaultInheritance.Checked = value;

					if (value == true) {
						this.btnShowProperties.Checked = false;
						this.btnShowDefaultProperties.Checked = false;
						this.btnShowInheritance.Checked = false;
					}
				}
			}
			#endregion

			#region "Public Methods"
			public Config(DockContent Panel)
			{
				SystemColorsChanged += Config_SystemColorsChanged;
				Load += Config_Load;
				this.WindowType = Type.Config;
				this.DockPnl = Panel;
				this.InitializeComponent();
			}

			// Main form handle command key events
			// Adapted from http://kiwigis.blogspot.com/2009/05/adding-tab-key-support-to-propertygrid.html
			protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
			{
				if ((keyData & Keys.KeyCode) == Keys.Tab) {
					GridItem selectedItem = pGrid.SelectedGridItem;
					GridItem gridRoot = selectedItem;
					while (gridRoot.GridItemType != GridItemType.Root) {
						gridRoot = gridRoot.Parent;
					}

					List<GridItem> gridItems = new List<GridItem>();
					FindChildGridItems(gridRoot, ref gridItems);

					if (!ContainsGridItemProperty(gridItems))
						return true;

					GridItem newItem = selectedItem;

					if (keyData == (Keys.Tab | Keys.Shift)) {
						newItem = FindPreviousGridItemProperty(gridItems, selectedItem);
					} else if (keyData == Keys.Tab) {
						newItem = FindNextGridItemProperty(gridItems, selectedItem);
					}

					pGrid.SelectedGridItem = newItem;

					return true;
					// Handled
				} else {
					return base.ProcessCmdKey(ref msg, keyData);
				}
			}

			private void FindChildGridItems(GridItem item, ref List<GridItem> gridItems)
			{
				gridItems.Add(item);

				if (!item.Expandable | item.Expanded) {
					foreach (GridItem child in item.GridItems) {
						FindChildGridItems(child, ref gridItems);
					}
				}
			}

			private bool ContainsGridItemProperty(List<GridItem> gridItems)
			{
				foreach (GridItem item in gridItems) {
					if (item.GridItemType == GridItemType.Property)
						return true;
				}
				return false;
			}

			private GridItem FindPreviousGridItemProperty(List<GridItem> gridItems, GridItem startItem)
			{
				if (gridItems.Count == 0)
					return null;
				if (startItem == null)
					return null;

				int startIndex = gridItems.IndexOf(startItem);

				if (startItem.GridItemType == GridItemType.Property) {
					startIndex = startIndex - 1;
					if (startIndex < 0)
						startIndex = gridItems.Count - 1;
				}

				int previousIndex = 0;
				bool previousIndexValid = false;
				for (int index = startIndex; index >= 0; index += -1) {
					if (gridItems[index].GridItemType == GridItemType.Property) {
						previousIndex = index;
						previousIndexValid = true;
						break; // TODO: might not be correct. Was : Exit For
					}
				}

				if (previousIndexValid)
					return gridItems[previousIndex];

				for (int index = gridItems.Count - 1; index >= startIndex + 1; index += -1) {
					if (gridItems[index].GridItemType == GridItemType.Property) {
						previousIndex = index;
						previousIndexValid = true;
						break; // TODO: might not be correct. Was : Exit For
					}
				}

				if (!previousIndexValid)
					return null;

				return gridItems[previousIndex];
			}

			private GridItem FindNextGridItemProperty(List<GridItem> gridItems, GridItem startItem)
			{
				if (gridItems.Count == 0)
					return null;
				if (startItem == null)
					return null;

				int startIndex = gridItems.IndexOf(startItem);

				if (startItem.GridItemType == GridItemType.Property) {
					startIndex = startIndex + 1;
					if (startIndex >= gridItems.Count)
						startIndex = 0;
				}

				int nextIndex = 0;
				bool nextIndexValid = false;
				for (int index = startIndex; index <= gridItems.Count - 1; index++) {
					if (gridItems[index].GridItemType == GridItemType.Property) {
						nextIndex = index;
						nextIndexValid = true;
						break; // TODO: might not be correct. Was : Exit For
					}
				}

				if (nextIndexValid)
					return gridItems[nextIndex];

				for (int index = 0; index <= startIndex - 1; index++) {
					if (gridItems[index].GridItemType == GridItemType.Property) {
						nextIndex = index;
						nextIndexValid = true;
						break; // TODO: might not be correct. Was : Exit For
					}
				}

				if (!nextIndexValid)
					return null;

				return gridItems[nextIndex];
			}

			public void SetPropertyGridObject(object Obj)
			{
				try {
					this.btnShowProperties.Enabled = false;
					this.btnShowInheritance.Enabled = false;
					this.btnShowDefaultProperties.Enabled = false;
					this.btnShowDefaultInheritance.Enabled = false;
					this.btnIcon.Enabled = false;
					this.btnHostStatus.Enabled = false;

					this.btnIcon.Image = null;

					//CONNECTION INFO
					if (Obj is mRemoteNG.Connection.Info) {
						//NO CONTAINER
						if ((Obj as mRemoteNG.Connection.Info).IsContainer == false) {
							//Properties selected
							if (this.PropertiesVisible) {
								this.pGrid.SelectedObject = Obj;

								this.btnShowProperties.Enabled = true;
								if ((Obj as mRemoteNG.Connection.Info).Parent != null) {
									this.btnShowInheritance.Enabled = true;
								} else {
									this.btnShowInheritance.Enabled = false;
								}
								this.btnShowDefaultProperties.Enabled = false;
								this.btnShowDefaultInheritance.Enabled = false;
								btnIcon.Enabled = true;
								this.btnHostStatus.Enabled = true;
							//Defaults selected
							} else if (this.DefaultPropertiesVisible) {
								this.pGrid.SelectedObject = Obj;

								//Is the default connection
								if ((Obj as mRemoteNG.Connection.Info).IsDefault) {
									this.btnShowProperties.Enabled = true;
									this.btnShowInheritance.Enabled = false;
									this.btnShowDefaultProperties.Enabled = true;
									this.btnShowDefaultInheritance.Enabled = true;
									this.btnIcon.Enabled = true;
									this.btnHostStatus.Enabled = false;
								//is not the default connection
								} else {
									this.btnShowProperties.Enabled = true;
									this.btnShowInheritance.Enabled = true;
									this.btnShowDefaultProperties.Enabled = false;
									this.btnShowDefaultInheritance.Enabled = false;
									this.btnIcon.Enabled = true;
									this.btnHostStatus.Enabled = true;

									this.PropertiesVisible = true;
								}
							//Inheritance selected
							} else if (this.InheritanceVisible) {
								this.pGrid.SelectedObject = (Obj as mRemoteNG.Connection.Info).Inherit;

								this.btnShowProperties.Enabled = true;
								this.btnShowInheritance.Enabled = true;
								this.btnShowDefaultProperties.Enabled = false;
								this.btnShowDefaultInheritance.Enabled = false;
								this.btnIcon.Enabled = true;
								this.btnHostStatus.Enabled = true;
							//Default Inhertiance selected
							} else if (this.DefaultInheritanceVisible) {
								pGrid.SelectedObject = Obj;

								this.btnShowProperties.Enabled = true;
								this.btnShowInheritance.Enabled = true;
								this.btnShowDefaultProperties.Enabled = false;
								this.btnShowDefaultInheritance.Enabled = false;
								this.btnIcon.Enabled = true;
								this.btnHostStatus.Enabled = true;

								this.PropertiesVisible = true;
							}
						//CONTAINER
						} else if ((Obj as mRemoteNG.Connection.Info).IsContainer) {
							this.pGrid.SelectedObject = Obj;

							this.btnShowProperties.Enabled = true;
							if (((Obj as mRemoteNG.Connection.Info).Parent as mRemoteNG.Container.Info).Parent != null) {
								this.btnShowInheritance.Enabled = true;
							} else {
								this.btnShowInheritance.Enabled = false;
							}
							this.btnShowDefaultProperties.Enabled = false;
							this.btnShowDefaultInheritance.Enabled = false;
							this.btnIcon.Enabled = true;
							this.btnHostStatus.Enabled = false;

							this.PropertiesVisible = true;
						}

						Icon conIcon = mRemoteNG.Connection.Icon.FromString((Obj as mRemoteNG.Connection.Info).Icon);
						if (conIcon != null) {
							this.btnIcon.Image = conIcon.ToBitmap();
						}
					//ROOT
					} else if (Obj is Root.Info) {
						Root.Info rootInfo = (Root.Info)Obj;
						switch (rootInfo.Type) {
							case mRemoteNG.Root.Info.RootType.Connection:
								PropertiesVisible = true;
								DefaultPropertiesVisible = false;
								btnShowProperties.Enabled = true;
								btnShowInheritance.Enabled = false;
								btnShowDefaultProperties.Enabled = true;
								btnShowDefaultInheritance.Enabled = true;
								btnIcon.Enabled = false;
								btnHostStatus.Enabled = false;
								break;
							case mRemoteNG.Root.Info.RootType.Credential:
								throw new NotImplementedException();
							case mRemoteNG.Root.Info.RootType.PuttySessions:
								PropertiesVisible = true;
								DefaultPropertiesVisible = false;
								btnShowProperties.Enabled = true;
								btnShowInheritance.Enabled = false;
								btnShowDefaultProperties.Enabled = false;
								btnShowDefaultInheritance.Enabled = false;
								btnIcon.Enabled = false;
								btnHostStatus.Enabled = false;
								break;
						}
						pGrid.SelectedObject = Obj;
					//INHERITANCE
					} else if (Obj is mRemoteNG.Connection.Info.Inheritance) {
						this.pGrid.SelectedObject = Obj;

						if (this.InheritanceVisible) {
							this.InheritanceVisible = true;
							this.btnShowProperties.Enabled = true;
							this.btnShowInheritance.Enabled = true;
							this.btnShowDefaultProperties.Enabled = false;
							this.btnShowDefaultInheritance.Enabled = false;
							this.btnIcon.Enabled = true;
							this.btnHostStatus.Enabled = !((Obj as mRemoteNG.Connection.Info.Inheritance).Parent as mRemoteNG.Connection.Info).IsContainer;

							this.InheritanceVisible = true;

							Icon conIcon = mRemoteNG.Connection.Icon.FromString(((Obj as mRemoteNG.Connection.Info.Inheritance).Parent as mRemoteNG.Connection.Info).Icon);
							if (conIcon != null) {
								this.btnIcon.Image = conIcon.ToBitmap();
							}
						} else if (this.DefaultInheritanceVisible) {
							this.btnShowProperties.Enabled = true;
							this.btnShowInheritance.Enabled = false;
							this.btnShowDefaultProperties.Enabled = true;
							this.btnShowDefaultInheritance.Enabled = true;
							this.btnIcon.Enabled = false;
							this.btnHostStatus.Enabled = false;

							this.DefaultInheritanceVisible = true;
						}

					}

					this.ShowHideGridItems();
					this.SetHostStatus(Obj);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConfigPropertyGridObjectFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public void pGrid_SelectedObjectChanged()
			{
				this.ShowHideGridItems();
			}
			#endregion

			#region "Private Methods"
			private void ApplyLanguage()
			{
				btnShowInheritance.Text = mRemoteNG.My.Language.strButtonInheritance;
				btnShowDefaultInheritance.Text = mRemoteNG.My.Language.strButtonDefaultInheritance;
				btnShowProperties.Text = mRemoteNG.My.Language.strButtonProperties;
				btnShowDefaultProperties.Text = mRemoteNG.My.Language.strButtonDefaultProperties;
				btnIcon.Text = mRemoteNG.My.Language.strButtonIcon;
				btnHostStatus.Text = mRemoteNG.My.Language.strStatus;
				Text = mRemoteNG.My.Language.strMenuConfig;
				TabText = mRemoteNG.My.Language.strMenuConfig;
				propertyGridContextMenuShowHelpText.Text = Language.strMenuShowHelpText;
			}

			private void ApplyTheme()
			{
				var _with1 = mRemoteNG.Themes.ThemeManager.ActiveTheme;
				pGrid.BackColor = _with1.ToolbarBackgroundColor;
				pGrid.ForeColor = _with1.ToolbarTextColor;
				pGrid.ViewBackColor = _with1.ConfigPanelBackgroundColor;
				pGrid.ViewForeColor = _with1.ConfigPanelTextColor;
				pGrid.LineColor = _with1.ConfigPanelGridLineColor;
				pGrid.HelpBackColor = _with1.ConfigPanelHelpBackgroundColor;
				pGrid.HelpForeColor = _with1.ConfigPanelHelpTextColor;
				pGrid.CategoryForeColor = _with1.ConfigPanelCategoryTextColor;
			}

			private bool _originalPropertyGridToolStripItemCountValid;

			private int _originalPropertyGridToolStripItemCount;
			private void AddToolStripItems()
			{
				try {
					ToolStrip customToolStrip = new ToolStrip();
					customToolStrip.Items.Add(btnShowProperties);
					customToolStrip.Items.Add(btnShowInheritance);
					customToolStrip.Items.Add(btnShowDefaultProperties);
					customToolStrip.Items.Add(btnShowDefaultInheritance);
					customToolStrip.Items.Add(btnHostStatus);
					customToolStrip.Items.Add(btnIcon);
					customToolStrip.Show();

					ToolStrip propertyGridToolStrip = new ToolStrip();

					ToolStrip toolStrip = null;
					foreach (System.Windows.Forms.Control control in pGrid.Controls) {
						toolStrip = control as ToolStrip;

						if (toolStrip != null) {
							propertyGridToolStrip = toolStrip;
							break; // TODO: might not be correct. Was : Exit For
						}
					}

					if (toolStrip == null) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, Language.strCouldNotFindToolStripInFilteredPropertyGrid, true);
						return;
					}

					if (!_originalPropertyGridToolStripItemCountValid) {
						_originalPropertyGridToolStripItemCount = propertyGridToolStrip.Items.Count;
						_originalPropertyGridToolStripItemCountValid = true;
					}
					Debug.Assert(_originalPropertyGridToolStripItemCount == 5);

					// Hide the "Property Pages" button
					propertyGridToolStrip.Items[_originalPropertyGridToolStripItemCount - 1].Visible = false;

					int expectedToolStripItemCount = _originalPropertyGridToolStripItemCount + customToolStrip.Items.Count;
					if (propertyGridToolStrip.Items.Count != expectedToolStripItemCount) {
						propertyGridToolStrip.AllowMerge = true;
						ToolStripManager.Merge(customToolStrip, propertyGridToolStrip);
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, Language.strConfigUiLoadFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void Config_Load(object sender, System.EventArgs e)
			{
				ApplyLanguage();

				mRemoteNG.Themes.ThemeManager.ThemeChanged += ApplyTheme;
				ApplyTheme();

				AddToolStripItems();

				pGrid.HelpVisible = Settings.ShowConfigHelpText;
			}

			private void Config_SystemColorsChanged(System.Object sender, System.EventArgs e)
			{
				AddToolStripItems();
			}

			private void pGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
			{
				try {
					if (this.pGrid.SelectedObject is mRemoteNG.Connection.Info) {
						switch (e.ChangedItem.Label) {
							case mRemoteNG.My.Language.strPropertyNameProtocol:
								(this.pGrid.SelectedObject as mRemoteNG.Connection.Info).SetDefaultPort();
								break;
							case mRemoteNG.My.Language.strPropertyNameName:
								mRemoteNG.App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Text = this.pGrid.SelectedObject.Name;
								if (mRemoteNG.My.Settings.SetHostnameLikeDisplayName & this.pGrid.SelectedObject is mRemoteNG.Connection.Info) {
									mRemoteNG.Connection.Info connectionInfo = (mRemoteNG.Connection.Info)this.pGrid.SelectedObject;
									if (!string.IsNullOrEmpty(connectionInfo.Name)) {
										connectionInfo.Hostname = connectionInfo.Name;
									}
								}
								break;
							case mRemoteNG.My.Language.strPropertyNameIcon:
								Icon conIcon = mRemoteNG.Connection.Icon.FromString((this.pGrid.SelectedObject as mRemoteNG.Connection.Info).Icon);
								if (conIcon != null) {
									this.btnIcon.Image = conIcon.ToBitmap();
								}
								break;
							case mRemoteNG.My.Language.strPropertyNameAddress:
								this.SetHostStatus(this.pGrid.SelectedObject);
								break;
						}

						if ((this.pGrid.SelectedObject as mRemoteNG.Connection.Info).IsDefault) {
							mRemoteNG.App.Runtime.DefaultConnectionToSettings();
						}
					}

					Info rootInfo = pGrid.SelectedObject as Info;
					if ((rootInfo != null)) {
						switch (e.ChangedItem.PropertyDescriptor.Name) {
							case "Password":
								if (rootInfo.Password == true) {
									string passwordName = null;
									if (Settings.UseSQLServer) {
										passwordName = Language.strSQLServer.TrimEnd(":");
									} else {
										passwordName = Path.GetFileName(mRemoteNG.App.Runtime.GetStartupConnectionFileName());
									}

									string password = mRemoteNG.Tools.Misc.PasswordDialog(passwordName);

									if (string.IsNullOrEmpty(password)) {
										rootInfo.Password = false;
									} else {
										rootInfo.PasswordString = password;
									}
								}
								break;
							case "Name":
								break;
							//Windows.treeForm.tvConnections.SelectedNode.Text = pGrid.SelectedObject.Name
						}
					}

					if (this.pGrid.SelectedObject is mRemoteNG.Connection.Info.Inheritance) {
						if ((this.pGrid.SelectedObject as mRemoteNG.Connection.Info.Inheritance).IsDefault) {
							mRemoteNG.App.Runtime.DefaultInheritanceToSettings();
						}
					}

					this.ShowHideGridItems();
					mRemoteNG.App.Runtime.SaveConnectionsBG();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConfigPropertyGridValueFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void pGrid_PropertySortChanged(object sender, EventArgs e)
			{
				if (pGrid.PropertySort == PropertySort.CategorizedAlphabetical) {
					pGrid.PropertySort = PropertySort.Categorized;
				}
			}

			private void ShowHideGridItems()
			{
				try {
					List<string> strHide = new List<string>();

					if (this.pGrid.SelectedObject is mRemoteNG.Connection.Info) {
						mRemoteNG.Connection.Info conI = pGrid.SelectedObject;

						switch (conI.Protocol) {
							case mRemoteNG.Connection.Protocol.Protocols.RDP:
								strHide.Add("ExtApp");
								strHide.Add("ICAEncryption");
								strHide.Add("PuttySession");
								strHide.Add("RenderingEngine");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								if (conI.RDGatewayUsageMethod == mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod.Never) {
									strHide.Add("RDGatewayDomain");
									strHide.Add("RDGatewayHostname");
									strHide.Add("RDGatewayPassword");
									strHide.Add("RDGatewayUseConnectionCredentials");
									strHide.Add("RDGatewayUsername");
								} else if (conI.RDGatewayUseConnectionCredentials) {
									strHide.Add("RDGatewayDomain");
									strHide.Add("RDGatewayPassword");
									strHide.Add("RDGatewayUsername");
								}
								if (!(conI.Resolution == RDP.RDPResolutions.FitToWindow | conI.Resolution == RDP.RDPResolutions.Fullscreen)) {
									strHide.Add("AutomaticResize");
								}
								break;
							case mRemoteNG.Connection.Protocol.Protocols.VNC:
								strHide.Add("CacheBitmaps");
								strHide.Add("Colors");
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("ExtApp");
								strHide.Add("ICAEncryption");
								strHide.Add("PuttySession");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound");
								strHide.Add("RenderingEngine");
								strHide.Add("Resolution");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								if (conI.VNCAuthMode == mRemoteNG.Connection.Protocol.VNC.AuthMode.AuthVNC) {
									strHide.Add("Username");
									strHide.Add("Domain");
								}
								if (conI.VNCProxyType == mRemoteNG.Connection.Protocol.VNC.ProxyType.ProxyNone) {
									strHide.Add("VNCProxyIP");
									strHide.Add("VNCProxyPassword");
									strHide.Add("VNCProxyPort");
									strHide.Add("VNCProxyUsername");
								}
								break;
							case mRemoteNG.Connection.Protocol.Protocols.SSH1:
								strHide.Add("CacheBitmaps");
								strHide.Add("Colors");
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("Domain");
								strHide.Add("ExtApp");
								strHide.Add("ICAEncryption");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound");
								strHide.Add("RenderingEngine");
								strHide.Add("Resolution");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								break;
							case mRemoteNG.Connection.Protocol.Protocols.SSH2:
								strHide.Add("CacheBitmaps");
								strHide.Add("Colors");
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("Domain");
								strHide.Add("ExtApp");
								strHide.Add("ICAEncryption");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound");
								strHide.Add("RenderingEngine");
								strHide.Add("Resolution");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								break;
							case mRemoteNG.Connection.Protocol.Protocols.Telnet:
								strHide.Add("CacheBitmaps");
								strHide.Add("Colors");
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("Domain");
								strHide.Add("ExtApp");
								strHide.Add("ICAEncryption");
								strHide.Add("Password");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound");
								strHide.Add("RenderingEngine");
								strHide.Add("Resolution");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								strHide.Add("Username");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								break;
							case mRemoteNG.Connection.Protocol.Protocols.Rlogin:
								strHide.Add("CacheBitmaps");
								strHide.Add("Colors");
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("Domain");
								strHide.Add("ExtApp");
								strHide.Add("ICAEncryption");
								strHide.Add("Password");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound");
								strHide.Add("RenderingEngine");
								strHide.Add("Resolution");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								strHide.Add("Username");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								break;
							case mRemoteNG.Connection.Protocol.Protocols.RAW:
								strHide.Add("CacheBitmaps");
								strHide.Add("Colors");
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("Domain");
								strHide.Add("ExtApp");
								strHide.Add("ICAEncryption");
								strHide.Add("Password");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound");
								strHide.Add("RenderingEngine");
								strHide.Add("Resolution");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								strHide.Add("Username");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								break;
							case mRemoteNG.Connection.Protocol.Protocols.HTTP:
								strHide.Add("CacheBitmaps");
								strHide.Add("Colors");
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("Domain");
								strHide.Add("ExtApp");
								strHide.Add("ICAEncryption");
								strHide.Add("PuttySession");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound");
								strHide.Add("Resolution");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								break;
							case mRemoteNG.Connection.Protocol.Protocols.HTTPS:
								strHide.Add("CacheBitmaps");
								strHide.Add("Colors");
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("Domain");
								strHide.Add("ExtApp");
								strHide.Add("ICAEncryption");
								strHide.Add("PuttySession");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound;Resolution");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								break;
							case mRemoteNG.Connection.Protocol.Protocols.ICA:
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("ExtApp");
								strHide.Add("Port");
								strHide.Add("PuttySession");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound");
								strHide.Add("RenderingEngine");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								break;
							case mRemoteNG.Connection.Protocol.Protocols.IntApp:
								strHide.Add("CacheBitmaps");
								strHide.Add("Colors");
								strHide.Add("DisplayThemes");
								strHide.Add("DisplayWallpaper");
								strHide.Add("EnableFontSmoothing");
								strHide.Add("EnableDesktopComposition");
								strHide.Add("Domain");
								strHide.Add("ICAEncryption");
								strHide.Add("PuttySession");
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsageMethod");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
								strHide.Add("RDPAuthenticationLevel");
								strHide.Add("LoadBalanceInfo");
								strHide.Add("RedirectDiskDrives");
								strHide.Add("RedirectKeys");
								strHide.Add("RedirectPorts");
								strHide.Add("RedirectPrinters");
								strHide.Add("RedirectSmartCards");
								strHide.Add("RedirectSound");
								strHide.Add("RenderingEngine");
								strHide.Add("Resolution");
								strHide.Add("AutomaticResize");
								strHide.Add("UseConsoleSession");
								strHide.Add("UseCredSsp");
								strHide.Add("VNCAuthMode");
								strHide.Add("VNCColors");
								strHide.Add("VNCCompression");
								strHide.Add("VNCEncoding");
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyType");
								strHide.Add("VNCProxyUsername");
								strHide.Add("VNCSmartSizeMode");
								strHide.Add("VNCViewOnly");
								break;
						}

						if (conI.IsDefault == false) {
							var _with2 = conI.Inherit;
							if (_with2.CacheBitmaps) {
								strHide.Add("CacheBitmaps");
							}

							if (_with2.Colors) {
								strHide.Add("Colors");
							}

							if (_with2.Description) {
								strHide.Add("Description");
							}

							if (_with2.DisplayThemes) {
								strHide.Add("DisplayThemes");
							}

							if (_with2.DisplayWallpaper) {
								strHide.Add("DisplayWallpaper");
							}

							if (_with2.EnableFontSmoothing) {
								strHide.Add("EnableFontSmoothing");
							}

							if (_with2.EnableDesktopComposition) {
								strHide.Add("EnableDesktopComposition");
							}

							if (_with2.Domain) {
								strHide.Add("Domain");
							}

							if (_with2.Icon) {
								strHide.Add("Icon");
							}

							if (_with2.Password) {
								strHide.Add("Password");
							}

							if (_with2.Port) {
								strHide.Add("Port");
							}

							if (_with2.Protocol) {
								strHide.Add("Protocol");
							}

							if (_with2.PuttySession) {
								strHide.Add("PuttySession");
							}

							if (_with2.RedirectDiskDrives) {
								strHide.Add("RedirectDiskDrives");
							}

							if (_with2.RedirectKeys) {
								strHide.Add("RedirectKeys");
							}

							if (_with2.RedirectPorts) {
								strHide.Add("RedirectPorts");
							}

							if (_with2.RedirectPrinters) {
								strHide.Add("RedirectPrinters");
							}

							if (_with2.RedirectSmartCards) {
								strHide.Add("RedirectSmartCards");
							}

							if (_with2.RedirectSound) {
								strHide.Add("RedirectSound");
							}

							if (_with2.Resolution) {
								strHide.Add("Resolution");
							}

							if (_with2.AutomaticResize)
								strHide.Add("AutomaticResize");

							if (_with2.UseConsoleSession) {
								strHide.Add("UseConsoleSession");
							}

							if (_with2.UseCredSsp) {
								strHide.Add("UseCredSsp");
							}

							if (_with2.RenderingEngine) {
								strHide.Add("RenderingEngine");
							}

							if (_with2.ICAEncryption) {
								strHide.Add("ICAEncryption");
							}

							if (_with2.RDPAuthenticationLevel) {
								strHide.Add("RDPAuthenticationLevel");
							}

							if (_with2.LoadBalanceInfo)
								strHide.Add("LoadBalanceInfo");

							if (_with2.Username) {
								strHide.Add("Username");
							}

							if (_with2.Panel) {
								strHide.Add("Panel");
							}

							if (conI.IsContainer) {
								strHide.Add("Hostname");
							}

							if (_with2.PreExtApp) {
								strHide.Add("PreExtApp");
							}

							if (_with2.PostExtApp) {
								strHide.Add("PostExtApp");
							}

							if (_with2.MacAddress) {
								strHide.Add("MacAddress");
							}

							if (_with2.UserField) {
								strHide.Add("UserField");
							}

							if (_with2.VNCAuthMode) {
								strHide.Add("VNCAuthMode");
							}

							if (_with2.VNCColors) {
								strHide.Add("VNCColors");
							}

							if (_with2.VNCCompression) {
								strHide.Add("VNCCompression");
							}

							if (_with2.VNCEncoding) {
								strHide.Add("VNCEncoding");
							}

							if (_with2.VNCProxyIP) {
								strHide.Add("VNCProxyIP");
							}

							if (_with2.VNCProxyPassword) {
								strHide.Add("VNCProxyPassword");
							}

							if (_with2.VNCProxyPort) {
								strHide.Add("VNCProxyPort");
							}

							if (_with2.VNCProxyType) {
								strHide.Add("VNCProxyType");
							}

							if (_with2.VNCProxyUsername) {
								strHide.Add("VNCProxyUsername");
							}

							if (_with2.VNCViewOnly) {
								strHide.Add("VNCViewOnly");
							}

							if (_with2.VNCSmartSizeMode) {
								strHide.Add("VNCSmartSizeMode");
							}

							if (_with2.ExtApp) {
								strHide.Add("ExtApp");
							}

							if (_with2.RDGatewayUsageMethod) {
								strHide.Add("RDGatewayUsageMethod");
							}

							if (_with2.RDGatewayHostname) {
								strHide.Add("RDGatewayHostname");
							}

							if (_with2.RDGatewayUsername) {
								strHide.Add("RDGatewayUsername");
							}

							if (_with2.RDGatewayPassword) {
								strHide.Add("RDGatewayPassword");
							}

							if (_with2.RDGatewayDomain) {
								strHide.Add("RDGatewayDomain");
							}

							if (_with2.RDGatewayUseConnectionCredentials) {
								strHide.Add("RDGatewayUseConnectionCredentials");
							}

							if (_with2.RDGatewayHostname) {
								strHide.Add("RDGatewayHostname");
							}
						} else {
							strHide.Add("Hostname");
							strHide.Add("Name");
						}
					} else if (pGrid.SelectedObject is Root.Info) {
						Root.Info rootInfo = (Root.Info)pGrid.SelectedObject;
						if (rootInfo.Type == mRemoteNG.Root.Info.RootType.PuttySessions) {
							strHide.Add("Password");
						}
					}

					this.pGrid.HiddenProperties = strHide.ToArray();

					this.pGrid.Refresh();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConfigPropertyGridHideItemsFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void btnShowProperties_Click(object sender, System.EventArgs e)
			{
				if (this.pGrid.SelectedObject is mRemoteNG.Connection.Info.Inheritance) {
					if ((this.pGrid.SelectedObject as mRemoteNG.Connection.Info.Inheritance).IsDefault) {
						this.PropertiesVisible = true;
						this.InheritanceVisible = false;
						this.DefaultPropertiesVisible = false;
						this.DefaultInheritanceVisible = false;
						this.SetPropertyGridObject(mRemoteNG.App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Tag as mRemoteNG.Root.Info);
					} else {
						this.PropertiesVisible = true;
						this.InheritanceVisible = false;
						this.DefaultPropertiesVisible = false;
						this.DefaultInheritanceVisible = false;
						this.SetPropertyGridObject((this.pGrid.SelectedObject as mRemoteNG.Connection.Info.Inheritance).Parent);
					}
				} else if (this.pGrid.SelectedObject is mRemoteNG.Connection.Info) {
					if ((this.pGrid.SelectedObject as mRemoteNG.Connection.Info).IsDefault) {
						this.PropertiesVisible = true;
						this.InheritanceVisible = false;
						this.DefaultPropertiesVisible = false;
						this.DefaultInheritanceVisible = false;
						this.SetPropertyGridObject(mRemoteNG.App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Tag as mRemoteNG.Root.Info);
					}
				}
			}

			private void btnShowDefaultProperties_Click(object sender, System.EventArgs e)
			{
				if (this.pGrid.SelectedObject is mRemoteNG.Root.Info | this.pGrid.SelectedObject is mRemoteNG.Connection.Info.Inheritance) {
					this.PropertiesVisible = false;
					this.InheritanceVisible = false;
					this.DefaultPropertiesVisible = true;
					this.DefaultInheritanceVisible = false;
					this.SetPropertyGridObject(mRemoteNG.App.Runtime.DefaultConnectionFromSettings());
				}
			}

			private void btnShowInheritance_Click(object sender, System.EventArgs e)
			{
				if (this.pGrid.SelectedObject is mRemoteNG.Connection.Info) {
					this.PropertiesVisible = false;
					this.InheritanceVisible = true;
					this.DefaultPropertiesVisible = false;
					this.DefaultInheritanceVisible = false;
					this.SetPropertyGridObject((this.pGrid.SelectedObject as mRemoteNG.Connection.Info).Inherit);
				}
			}

			private void btnShowDefaultInheritance_Click(object sender, System.EventArgs e)
			{
				if (this.pGrid.SelectedObject is mRemoteNG.Root.Info | this.pGrid.SelectedObject is mRemoteNG.Connection.Info) {
					this.PropertiesVisible = false;
					this.InheritanceVisible = false;
					this.DefaultPropertiesVisible = false;
					this.DefaultInheritanceVisible = true;
					this.SetPropertyGridObject(mRemoteNG.App.Runtime.DefaultInheritanceFromSettings());
				}
			}

			private void btnHostStatus_Click(object sender, System.EventArgs e)
			{
				SetHostStatus(this.pGrid.SelectedObject);
			}

			private void btnIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
			{
				try {
					if (pGrid.SelectedObject is mRemoteNG.Connection.Info & !pGrid.SelectedObject is mRemoteNG.Connection.PuttySession.Info) {
						this.cMenIcons.Items.Clear();

						foreach (string iStr in mRemoteNG.Connection.Icon.Icons) {
							ToolStripMenuItem tI = new ToolStripMenuItem();
							tI.Text = iStr;
							tI.Image = mRemoteNG.Connection.Icon.FromString(iStr).ToBitmap();
							tI.Click += IconMenu_Click;

							this.cMenIcons.Items.Add(tI);
						}

						Point mPos = new Point(PointToScreen(new Point(e.Location.X + this.pGrid.Width - 100, e.Location.Y)));
						this.cMenIcons.Show(mPos);
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConfigPropertyGridButtonIconClickFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void IconMenu_Click(object sender, EventArgs e)
			{
				try {
					mRemoteNG.Connection.Info connectionInfo = pGrid.SelectedObject as mRemoteNG.Connection.Info;
					if (connectionInfo == null)
						return;

					ToolStripMenuItem selectedMenuItem = sender as ToolStripMenuItem;
					if (selectedMenuItem == null)
						return;

					string iconName = selectedMenuItem.Text;
					if (string.IsNullOrEmpty(iconName))
						return;

					Icon connectionIcon = mRemoteNG.Connection.Icon.FromString(iconName);
					if (connectionIcon == null)
						return;

					btnIcon.Image = connectionIcon.ToBitmap();

					connectionInfo.Icon = iconName;
					pGrid.Refresh();

					mRemoteNG.App.Runtime.SaveConnectionsBG();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, Language.strConfigPropertyGridMenuClickFailed + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Host Status (Ping)"
			private string HostName;

			private System.Threading.Thread pThread;
			private void CheckHostAlive()
			{
				Ping pingSender = new Ping();
				PingReply pReply = null;

				try {
					pReply = pingSender.Send(HostName);

					if (pReply.Status == IPStatus.Success) {
						if (this.btnHostStatus.Tag == "checking") {
							ShowStatusImage(mRemoteNG.My.Resources.HostStatus_On);
						}
					} else {
						if (this.btnHostStatus.Tag == "checking") {
							ShowStatusImage(mRemoteNG.My.Resources.HostStatus_Off);
						}
					}
				} catch (Exception ex) {
					if (this.btnHostStatus.Tag == "checking") {
						ShowStatusImage(mRemoteNG.My.Resources.HostStatus_Off);
					}
				}
			}

			public delegate void ShowStatusImageCB(Image Image);
			private void ShowStatusImage(Image Image)
			{
				if (this.pGrid.InvokeRequired) {
					ShowStatusImageCB d = new ShowStatusImageCB(ShowStatusImage);
					this.pGrid.Invoke(d, new object[] { Image });
				} else {
					this.btnHostStatus.Image = Image;
					this.btnHostStatus.Tag = "checkfinished";
				}
			}

			public void SetHostStatus(object ConnectionInfo)
			{
				try {
					this.btnHostStatus.Image = mRemoteNG.My.Resources.HostStatus_Check;

					// To check status, ConnectionInfo must be an mRemoteNG.Connection.Info that is not a container
					if (ConnectionInfo is mRemoteNG.Connection.Info) {
						if ((ConnectionInfo as mRemoteNG.Connection.Info).IsContainer)
							return;
					} else {
						return;
					}

					this.btnHostStatus.Tag = "checking";
					HostName = (ConnectionInfo as mRemoteNG.Connection.Info).Hostname;
					pThread = new System.Threading.Thread(CheckHostAlive);
					pThread.SetApartmentState(System.Threading.ApartmentState.STA);
					pThread.IsBackground = true;
					pThread.Start();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConfigPropertyGridSetHostStatusFailed + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			private void propertyGridContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
			{
				try {
					propertyGridContextMenuShowHelpText.Checked = Settings.ShowConfigHelpText;
					GridItem gridItem = pGrid.SelectedGridItem;
					propertyGridContextMenuReset.Enabled = (pGrid.SelectedObject != null && gridItem != null && gridItem.PropertyDescriptor != null && gridItem.PropertyDescriptor.CanResetValue(pGrid.SelectedObject));
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("UI.Window.Config.propertyGridContextMenu_Opening() failed.", ex, MessageClass.ErrorMsg, true);
				}
			}

			private void propertyGridContextMenuReset_Click(System.Object sender, EventArgs e)
			{
				try {
					GridItem gridItem = pGrid.SelectedGridItem;
					if (pGrid.SelectedObject != null && gridItem != null && gridItem.PropertyDescriptor != null && gridItem.PropertyDescriptor.CanResetValue(pGrid.SelectedObject)) {
						pGrid.ResetSelectedProperty();
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("UI.Window.Config.propertyGridContextMenuReset_Click() failed.", ex, MessageClass.ErrorMsg, true);
				}
			}

			private void propertyGridContextMenuShowHelpText_Click(object sender, EventArgs e)
			{
				propertyGridContextMenuShowHelpText.Checked = !propertyGridContextMenuShowHelpText.Checked;
			}

			private void propertyGridContextMenuShowHelpText_CheckedChanged(object sender, EventArgs e)
			{
				Settings.ShowConfigHelpText = propertyGridContextMenuShowHelpText.Checked;
				pGrid.HelpVisible = propertyGridContextMenuShowHelpText.Checked;
			}
		}
	}
}
