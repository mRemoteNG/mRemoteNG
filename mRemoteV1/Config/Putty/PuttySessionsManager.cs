using mRemoteNG.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Root.PuttySessions;


namespace mRemoteNG.Config.Putty
{
	public class PuttySessionsManager
	{
        private static List<AbstractPuttySessionsProvider> _providers;
        public static IEnumerable<AbstractPuttySessionsProvider> Providers
        {
            get
            {
                if (_providers == null || _providers.Count == 0)
                    AddProviders();
                return _providers;
            }
        }

        public static List<RootPuttySessionsNodeInfo> RootPuttySessionsNodes { get; } = new List<RootPuttySessionsNodeInfo>();

        #region Public Methods
		public static void AddSessionsToTree()
		{
			foreach (var provider in Providers)
			{
			    AddSessionsToTreeForProvider(provider);
			}
		}

	    private static void AddSessionsToTreeForProvider(AbstractPuttySessionsProvider provider)
	    {
            var rootTreeNode = provider.RootInfo;
	        provider.GetSessions();

            if (!RootPuttySessionsNodes.Contains(rootTreeNode) && rootTreeNode.HasChildren())
                RootPuttySessionsNodes.Add(rootTreeNode);
            rootTreeNode.SortRecursive();
        }
		
		public static void StartWatcher()
		{
			foreach (var provider in Providers)
			{
				provider.StartWatcher();
				provider.SessionChanged += SessionChanged;
			}
		}
		
		public static void StopWatcher()
		{
			foreach (var provider in Providers)
			{
				provider.StopWatcher();
				provider.SessionChanged -= SessionChanged;
			}
		}
		
		public static void SessionChanged(object sender, AbstractPuttySessionsProvider.SessionChangedEventArgs e)
		{
			AddSessionsToTree();
		}
        #endregion
		
        #region Private Methods
	    public static void AddProvider(AbstractPuttySessionsProvider provider)
	    {
	        _providers.Add(provider);
	    }
			
		private static void AddProviders()
		{
		    _providers = new List<AbstractPuttySessionsProvider> {new PuttySessionsRegistryProvider(), new PuttySessionsXmingProvider()};
		}
			
		private static string[] GetSessionNames(bool raw = false)
		{
			var sessionNames = new List<string>();
			foreach (var provider in Providers)
			{
				if (!IsProviderEnabled(provider))
				{
					continue;
				}
				sessionNames.AddRange(provider.GetSessionNames(raw));
			}
			return sessionNames.ToArray();
		}
			
		private static bool IsProviderEnabled(AbstractPuttySessionsProvider puttySessionsProvider)
		{
            var enabled = true;
			if (PuttyTypeDetector.GetPuttyType() == PuttyTypeDetector.PuttyType.Xming)
			{
				if (puttySessionsProvider is PuttySessionsRegistryProvider)
					enabled = false;
			}
			else
			{
				if (puttySessionsProvider is PuttySessionsXmingProvider)
					enabled = false;
			}
			return enabled;
		}
        #endregion
			
        #region Public Classes
        public class SessionList : StringConverter
        {
            public static string[] Names => GetSessionNames();

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
	        {
		        return new StandardValuesCollection(Names);
	        }
				
	        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
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