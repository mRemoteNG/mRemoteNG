using mRemoteNG.Connection;
using mRemoteNG.Images;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;


namespace mRemoteNG.Config.Putty
{
	public class Sessions
	{
        #region Public Methods
		private delegate void AddSessionsToTreeDelegate();
		public static void AddSessionsToTree()
		{
            TreeView treeView = ConnectionTree.TreeView;
			if (treeView == null)
			{
				return ;
			}
			if (treeView.InvokeRequired)
			{
				treeView.Invoke(new AddSessionsToTreeDelegate(AddSessionsToTree));
				return ;
			}
				
			foreach (Provider provider in Providers)
			{
				TreeNode rootTreeNode = provider.RootTreeNode;
				bool inUpdate = false;
					
				List<ConnectionInfo> savedSessions = new List<ConnectionInfo>(provider.GetSessions());
				if (!IsProviderEnabled(provider) || savedSessions == null || savedSessions.Count == 0)
				{
					if (rootTreeNode != null && treeView.Nodes.Contains(rootTreeNode))
					{
						treeView.BeginUpdate();
						treeView.Nodes.Remove(rootTreeNode);
						treeView.EndUpdate();
					}
					continue;
				}
					
				if (!treeView.Nodes.Contains(rootTreeNode))
				{
					if (!inUpdate)
					{
						treeView.BeginUpdate();
						inUpdate = true;
					}
					treeView.Nodes.Add(rootTreeNode);
				}
					
				List<TreeNode> newTreeNodes = new List<TreeNode>();
				foreach (PuttySessionInfo sessionInfo in savedSessions)
				{
					TreeNode treeNode = default(TreeNode);
					bool isNewNode = false;
					if (rootTreeNode.Nodes.ContainsKey(sessionInfo.Name))
					{
						treeNode = rootTreeNode.Nodes[sessionInfo.Name];
						isNewNode = false;
					}
					else
					{
						treeNode = ConnectionTreeNode.AddNode(TreeNodeType.PuttySession, sessionInfo.Name);
						if (treeNode == null)
						{
							continue;
						}
						treeNode.Name = treeNode.Text;
						treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
						treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
						isNewNode = true;
					}
					
					sessionInfo.RootPuttySessionsInfo = provider.RootInfo;
					sessionInfo.TreeNode = treeNode;
					//sessionInfo.Inheritance.TurnOffInheritanceCompletely();
					
					treeNode.Tag = sessionInfo;
						
					if (isNewNode)
					{
						newTreeNodes.Add(treeNode);
					}
				}
					
				foreach (TreeNode treeNode in rootTreeNode.Nodes)
				{
					if (!savedSessions.Contains((ConnectionInfo)treeNode.Tag))
					{
						if (!inUpdate)
						{
							treeView.BeginUpdate();
							inUpdate = true;
						}
						rootTreeNode.Nodes.Remove(treeNode);
					}
				}
					
				if (!(newTreeNodes.Count == 0))
				{
					if (!inUpdate)
					{
						treeView.BeginUpdate();
						inUpdate = true;
					}
					rootTreeNode.Nodes.AddRange(newTreeNodes.ToArray());
				}
					
				if (inUpdate)
				{
                    ConnectionTree.Sort(rootTreeNode, SortOrder.Ascending);
					rootTreeNode.Expand();
					treeView.EndUpdate();
				}
			}
		}
			
		public static void StartWatcher()
		{
			foreach (Provider provider in Providers)
			{
				provider.StartWatcher();
				provider.SessionChanged += SessionChanged;
			}
		}
			
		public static void StopWatcher()
		{
			foreach (Provider provider in Providers)
			{
				provider.StopWatcher();
				provider.SessionChanged -= SessionChanged;
			}
		}
			
		public static void SessionChanged(object sender, Provider.SessionChangedEventArgs e)
		{
			AddSessionsToTree();
		}
        #endregion
			
        #region Private Methods
		private static List<Provider> _providers;
        private static List<Provider> Providers
		{
			get
			{
				if (_providers == null || _providers.Count == 0)
				{
					AddProviders();
				}
				return _providers;
			}
		}
			
		private static void AddProviders()
		{
			_providers = new List<Provider>();
			_providers.Add(new RegistryProvider());
			_providers.Add(new XmingProvider());
		}
			
		private static string[] GetSessionNames(bool raw = false)
		{
			List<string> sessionNames = new List<string>();
			foreach (Provider provider in Providers)
			{
				if (!IsProviderEnabled(provider))
				{
					continue;
				}
				sessionNames.AddRange(provider.GetSessionNames(raw));
			}
			return sessionNames.ToArray();
		}
			
		private static bool IsProviderEnabled(Provider provider)
		{
			bool enabled = true;
			if (PuttyTypeDetector.GetPuttyType() == PuttyTypeDetector.PuttyType.Xming)
			{
				if ((provider) is RegistryProvider)
				{
					enabled = false;
				}
			}
			else
			{
				if ((provider) is XmingProvider)
				{
					enabled = false;
				}
			}
			return enabled;
		}
        #endregion
			
        #region Public Classes
        public class SessionList : StringConverter
        {
				
            public static string[] Names
	        {
		        get
		        {
			        return GetSessionNames();
		        }
	        }
				
	        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
	        {
		        return new StandardValuesCollection(Names);
	        }
				
	        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
	        {
		        return true;
	        }
				
	        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
	        {
		        return true;
	        }
        }
        #endregion
	}
}
