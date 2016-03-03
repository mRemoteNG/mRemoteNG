// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

//using mRemoteNG.App.Runtime;
using System.DirectoryServices;


namespace mRemoteNG.Tree
{
	public class Node
		{
			public enum Type
			{
				None = 0,
				Root = 1,
				Container = 2,
				Connection = 3,
				PuttyRoot = 4,
				PuttySession = 5
			}
			
			private static TreeView _TreeView;
public static TreeView TreeView
			{
				get
				{
					return _TreeView;
				}
				set
				{
					_TreeView = value;
				}
			}
			
public static TreeNode SelectedNode
			{
				get
				{
					return _TreeView.SelectedNode;
				}
				set
				{
					treeNodeToBeSelected = value;
					SelectNode();
				}
			}
			
			private static TreeNode treeNodeToBeSelected;
			private delegate void SelectNodeCB();
			private static void SelectNode()
			{
				if (_TreeView.InvokeRequired == true)
				{
					SelectNodeCB d = new SelectNodeCB(SelectNode);
					_TreeView.Invoke(d);
				}
				else
				{
					_TreeView.SelectedNode = treeNodeToBeSelected;
				}
			}
			
			
			public static string GetConstantID(TreeNode node)
			{
				if (GetNodeType(node) == Type.Connection)
				{
					return (node.Tag as mRemoteNG.Connection.Info).ConstantID;
				}
				else if (GetNodeType(node) == Type.Container)
				{
					return (node.Tag as mRemoteNG.Container.Info).ConnectionInfo.ConstantID;
				}
				
				return null;
			}
			
			public static TreeNode GetNodeFromPositionID(int id)
			{
				foreach (Connection.Info conI in ConnectionList)
				{
					if (conI.PositionID == id)
					{
						if (conI.IsContainer)
						{
							return (conI.Parent as Container.Info).TreeNode;
						}
						else
						{
							return conI.TreeNode;
						}
					}
				}
				
				return null;
			}
			
			public static TreeNode GetNodeFromConstantID(string id)
			{
				foreach (Connection.Info conI in ConnectionList)
				{
					if (conI.ConstantID == id)
					{
						if (conI.IsContainer)
						{
							return (conI.Parent as Container.Info).TreeNode;
						}
						else
						{
							return conI.TreeNode;
						}
					}
				}
				
				return null;
			}
			
			public static Tree.Node.Type GetNodeType(TreeNode treeNode)
			{
				try
				{
					if (treeNode == null)
					{
						return Type.None;
					}
					
					if (treeNode.Tag == null)
					{
						return Type.None;
					}
					
					if (treeNode.Tag is Root.PuttySessions.Info)
					{
						return Type.PuttyRoot;
					}
					else if (treeNode.Tag is Root.Info)
					{
						return Type.Root;
					}
					else if (treeNode.Tag is Container.Info)
					{
						return Type.Container;
					}
					else if (treeNode.Tag is Connection.PuttySession.Info)
					{
						return Type.PuttySession;
					}
					else if (treeNode.Tag is Connection.Info)
					{
						return Type.Connection;
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t get node type" + Constants.vbNewLine + ex.Message, true);
				}
				
				return Type.None;
			}
			
			public static Tree.Node.Type GetNodeTypeFromString(string str)
			{
				try
				{
					switch (str.ToLower())
					{
						case "root":
							return Type.Root;
						case "container":
							return Type.Container;
						case "connection":
							return Type.Connection;
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t get node type from string" + Constants.vbNewLine + ex.Message, true);
				}
				
				return Type.None;
			}
			
			public static TreeNode Find(TreeNode treeNode, string searchFor)
			{
				TreeNode tmpNode = default(TreeNode);
				
				try
				{
					if ((treeNode.Text.ToLower()).IndexOf(searchFor.ToLower()) + 1 > 0)
					{
						return treeNode;
					}
					else
					{
						foreach (TreeNode childNode in treeNode.Nodes)
						{
							tmpNode = Find(childNode, searchFor);
							if (!(tmpNode == null))
							{
								return tmpNode;
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Find node failed" + Constants.vbNewLine + ex.Message, true);
				}
				
				return null;
			}
			
			public static TreeNode Find(TreeNode treeNode, Connection.Info conInfo)
			{
				TreeNode tmpNode = default(TreeNode);
				
				try
				{
					if (treeNode.Tag == conInfo)
					{
						return treeNode;
					}
					else
					{
						foreach (TreeNode childNode in treeNode.Nodes)
						{
							tmpNode = Find(childNode, conInfo);
							if (!(tmpNode == null))
							{
								return tmpNode;
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Find node failed" + Constants.vbNewLine + ex.Message, true);
				}
				
				return null;
			}
			
			public static bool IsEmpty(TreeNode treeNode)
			{
				try
				{
					if (treeNode.Nodes.Count <= 0)
					{
						return false;
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "IsEmpty (Tree.Node) failed" + Constants.vbNewLine + ex.Message, true);
				}
				
				return true;
			}
			
			
			
			public static TreeNode AddNode(Type nodeType, string name = null)
			{
				try
				{
					TreeNode treeNode = new TreeNode();
					string defaultName = "";
					
					switch (nodeType)
					{
						case Type.Connection:
						case Type.PuttySession:
							defaultName = My.Language.strNewConnection;
							treeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed;
							treeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed;
							break;
						case Type.Container:
							defaultName = My.Language.strNewFolder;
							treeNode.ImageIndex = Images.Enums.TreeImage.Container;
							treeNode.SelectedImageIndex = Images.Enums.TreeImage.Container;
							break;
						case Type.Root:
							defaultName = My.Language.strNewRoot;
							treeNode.ImageIndex = Images.Enums.TreeImage.Root;
							treeNode.SelectedImageIndex = Images.Enums.TreeImage.Root;
							break;
					}
					
					if (!string.IsNullOrEmpty(name))
					{
						treeNode.Name = name;
					}
					else
					{
						treeNode.Name = defaultName;
					}
					treeNode.Text = treeNode.Name;
					
					return treeNode;
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddNode failed" + Constants.vbNewLine + ex.Message, true);
				}
				
				return null;
			}
			
			public static void CloneNode(TreeNode oldTreeNode, TreeNode parentNode = null)
			{
				try
				{
					if (GetNodeType(oldTreeNode) == Type.Connection)
					{
						Connection.Info oldConnectionInfo = (Connection.Info) oldTreeNode.Tag;
						
						Connection.Info newConnectionInfo = oldConnectionInfo.Copy();
						Connection.Info.Inheritance newInheritance = oldConnectionInfo.Inherit.Copy();
						newInheritance.Parent = newConnectionInfo;
						newConnectionInfo.Inherit = newInheritance;
						
						ConnectionList.Add(newConnectionInfo);
						
						TreeNode newTreeNode = new TreeNode(newConnectionInfo.Name);
						newTreeNode.Tag = newConnectionInfo;
						newTreeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed;
						newTreeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed;
						
						newConnectionInfo.TreeNode = newTreeNode;
						
						if (parentNode == null)
						{
							oldTreeNode.Parent.Nodes.Insert(oldTreeNode.Index + 1, newTreeNode);
							TreeView.SelectedNode = newTreeNode;
						}
						else
						{
							Container.Info parentContainerInfo = parentNode.Tag as Container.Info;
							if (parentContainerInfo != null)
							{
								newConnectionInfo.Parent = parentContainerInfo;
							}
							parentNode.Nodes.Add(newTreeNode);
						}
					}
					else if (GetNodeType(oldTreeNode) == Type.Container)
					{
						Container.Info oldContainerInfo = (Container.Info) oldTreeNode.Tag;
						
						Container.Info newContainerInfo = oldContainerInfo.Copy();
						Connection.Info newConnectionInfo = oldContainerInfo.ConnectionInfo.Copy();
						newContainerInfo.ConnectionInfo = newConnectionInfo;
						
						TreeNode newTreeNode = new TreeNode(newContainerInfo.Name);
						newTreeNode.Tag = newContainerInfo;
						newTreeNode.ImageIndex = Images.Enums.TreeImage.Container;
						newTreeNode.SelectedImageIndex = Images.Enums.TreeImage.Container;
						newContainerInfo.ConnectionInfo.Parent = newContainerInfo;
						
						ContainerList.Add(newContainerInfo);
						
						if (parentNode == null)
						{
							oldTreeNode.Parent.Nodes.Insert(oldTreeNode.Index + 1, newTreeNode);
							TreeView.SelectedNode = newTreeNode;
						}
						else
						{
							parentNode.Nodes.Add(newTreeNode);
						}
						
						foreach (TreeNode childTreeNode in oldTreeNode.Nodes)
						{
							CloneNode(childTreeNode, newTreeNode);
						}
						
						newTreeNode.Expand();
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, string.Format(My.Language.strErrorCloneNodeFailed, ex.Message));
				}
			}
			
			public static void SetNodeImage(TreeNode treeNode, Images.Enums.TreeImage Img)
			{
				SetNodeImageIndex(treeNode, Img);
			}
			
			private delegate void SetNodeImageIndexDelegate(TreeNode treeNode, int imageIndex);
			private static void SetNodeImageIndex(TreeNode treeNode, int imageIndex)
			{
				if (treeNode == null || treeNode.TreeView == null)
				{
					return ;
				}
				if (treeNode.TreeView.InvokeRequired)
				{
					treeNode.TreeView.Invoke(new SetNodeImageIndexDelegate(SetNodeImageIndex), new object[] {treeNode, imageIndex});
					return ;
				}
				
				treeNode.ImageIndex = imageIndex;
				treeNode.SelectedImageIndex = imageIndex;
			}
			
			// VBConversions Note: Former VB static variables moved to class level because they aren't supported in C#.
			static TreeNode SetNodeToolTip_old_node = default(TreeNode);
			
			public static void SetNodeToolTip(MouseEventArgs e, ToolTip tTip)
			{
				try
				{
					if (My.Settings.Default.ShowDescriptionTooltipsInTree)
					{
						//Find the node under the mouse.
						// static TreeNode old_node = default(TreeNode); VBConversions Note: Static variable moved to class level and renamed SetNodeToolTip_old_node. Local static variables are not supported in C#.
						TreeNode new_node = _TreeView.GetNodeAt(e.X, e.Y);
						if (new_node == SetNodeToolTip_old_node)
						{
							return;
						}
						SetNodeToolTip_old_node = new_node;
						
						//See if we have a node.
						if (SetNodeToolTip_old_node == null)
						{
							tTip.SetToolTip(_TreeView, "");
						}
						else
						{
							//Get this node's object data.
							if (GetNodeType(SetNodeToolTip_old_node) == Type.Connection)
							{
								tTip.SetToolTip(_TreeView, (SetNodeToolTip_old_node.Tag as mRemoteNG.Connection.Info).Description);
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SetNodeToolTip failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
			
			
			public static void DeleteSelectedNode()
			{
				try
				{
					if (SelectedNode == null)
					{
						return ;
					}
					
					if (Tree.Node.GetNodeType(SelectedNode) == Type.Root)
					{
						MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "The root item cannot be deleted!");
					}
					else if (Tree.Node.GetNodeType(SelectedNode) == Type.Container)
					{
						if (Tree.Node.IsEmpty(SelectedNode) == false)
						{
							if (Interaction.MsgBox(string.Format(My.Language.strConfirmDeleteNodeFolder, SelectedNode.Text), (Microsoft.VisualBasic.MsgBoxStyle) (MsgBoxStyle.YesNo | MsgBoxStyle.Question), null) == MsgBoxResult.Yes)
							{
								SelectedNode.Remove();
							}
						}
						else
						{
							if (Interaction.MsgBox(string.Format(My.Language.strConfirmDeleteNodeFolderNotEmpty, SelectedNode.Text), (Microsoft.VisualBasic.MsgBoxStyle) (MsgBoxStyle.YesNo | MsgBoxStyle.Question), null) == MsgBoxResult.Yes)
							{
								foreach (TreeNode tNode in SelectedNode.Nodes)
								{
									tNode.Remove();
								}
								SelectedNode.Remove();
							}
						}
					}
					else if (Tree.Node.GetNodeType(SelectedNode) == Type.Connection)
					{
						if (Interaction.MsgBox(string.Format(My.Language.strConfirmDeleteNodeConnection, SelectedNode.Text), (Microsoft.VisualBasic.MsgBoxStyle) (MsgBoxStyle.YesNo | MsgBoxStyle.Question), null) == MsgBoxResult.Yes)
						{
							SelectedNode.Remove();
						}
					}
					else
					{
						MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Tree item type is unknown so it cannot be deleted!");
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Deleting selected node failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
			
			public static void StartRenameSelectedNode()
			{
				if (SelectedNode != null)
				{
					SelectedNode.BeginEdit();
				}
			}
			
			public static void FinishRenameSelectedNode(string newName)
			{
				if (newName == null)
				{
					return ;
				}
				
				if (newName.Length > 0)
				{
					SelectedNode.Tag.Name = newName;
					
					if (My.Settings.Default.SetHostnameLikeDisplayName)
					{
						Connection.Info connectionInfo = SelectedNode.Tag as Connection.Info;
						if (connectionInfo != null)
						{
							connectionInfo.Hostname = newName;
						}
					}
				}
			}
			
			public static void MoveNodeUp()
			{
				try
				{
					if (SelectedNode != null)
					{
						if (!(SelectedNode.PrevNode == null))
						{
							TreeView.BeginUpdate();
							TreeView.Sorted = false;
							
							TreeNode newNode = SelectedNode.Clone();
							SelectedNode.Parent.Nodes.Insert(SelectedNode.Index - 1, newNode);
							SelectedNode.Remove();
							SelectedNode = newNode;
							
							TreeView.EndUpdate();
						}
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "MoveNodeUp failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
			
			public static void MoveNodeDown()
			{
				try
				{
					if (SelectedNode != null)
					{
						if (!(SelectedNode.NextNode == null))
						{
							TreeView.BeginUpdate();
							TreeView.Sorted = false;
							
							TreeNode newNode = SelectedNode.Clone();
							SelectedNode.Parent.Nodes.Insert(SelectedNode.Index + 2, newNode);
							SelectedNode.Remove();
							SelectedNode = newNode;
							
							TreeView.EndUpdate();
						}
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "MoveNodeDown failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
			
			public static void ExpandAllNodes()
			{
				TreeView.BeginUpdate();
				TreeView.ExpandAll();
				TreeView.EndUpdate();
			}
			
			public static void CollapseAllNodes()
			{
				TreeView.BeginUpdate();
				foreach (TreeNode treeNode in TreeView.Nodes[0].Nodes)
				{
					treeNode.Collapse(false);
				}
				TreeView.EndUpdate();
			}
			
			public static void Sort(TreeNode treeNode, System.Windows.Forms.SortOrder sorting)
			{
				if (TreeView == null)
				{
					return ;
				}
				
				TreeView.BeginUpdate();
				
				if (treeNode == null)
				{
					if (TreeView.Nodes.Count > 0)
					{
						treeNode = TreeView.Nodes[0];
					}
					else
					{
						return ;
					}
				}
				else if (GetNodeType(treeNode) == Type.Connection)
				{
					treeNode = treeNode.Parent;
					if (treeNode == null)
					{
						return ;
					}
				}
				
				Sort(treeNode, new Tools.Controls.TreeNodeSorter(sorting));
				
				TreeView.EndUpdate();
			}
			
			// Adapted from http://www.codeproject.com/Tips/252234/ASP-NET-TreeView-Sort
			private static void Sort(TreeNode treeNode, Tools.Controls.TreeNodeSorter nodeSorter)
			{
				foreach (TreeNode childNode in treeNode.Nodes)
				{
					Sort(childNode, nodeSorter);
				}
				
				try
				{
					List<TreeNode> sortedNodes = new List<TreeNode>();
					TreeNode currentNode = null;
					while (treeNode.Nodes.Count > 0)
					{
						foreach (TreeNode childNode in treeNode.Nodes)
						{
							if (currentNode == null || nodeSorter.Compare(childNode, currentNode) < 0)
							{
								currentNode = childNode;
							}
						}
						treeNode.Nodes.Remove(currentNode);
						sortedNodes.Add(currentNode);
						currentNode = null;
					}
					
					foreach (TreeNode childNode in sortedNodes)
					{
						treeNode.Nodes.Add(childNode);
					}
				}
				catch (Exception ex)
				{
					MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Sort nodes failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
			
			private delegate void ResetTreeDelegate();
			public static void ResetTree()
			{
				if (TreeView.InvokeRequired)
				{
					ResetTreeDelegate resetTreeDelegate = new ResetTreeDelegate(ResetTree);
					Windows.treeForm.Invoke(resetTreeDelegate);
				}
				else
				{
					TreeView.BeginUpdate();
					TreeView.Nodes.Clear();
					TreeView.Nodes.Add(My.Language.strConnections);
					TreeView.EndUpdate();
				}
			}
		}
}
