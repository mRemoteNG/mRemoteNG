using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Root.PuttySessions;


namespace mRemoteNG.Config.Putty
{
    public abstract class AbstractPuttySessionsProvider
	{
        public virtual RootPuttySessionsNodeInfo RootInfo { get; } = new RootPuttySessionsNodeInfo();

        #region Public Methods
        public abstract string[] GetSessionNames(bool raw = false);
		public abstract PuttySessionInfo GetSession(string sessionName);
		
		public virtual IEnumerable<PuttySessionInfo> GetSessions()
		{
			foreach (var sessionName in GetSessionNames(true))
			{
				var sessionInfo = GetSession(sessionName);
			    AddSession(sessionInfo);
			}
			return RootInfo.Children.OfType<PuttySessionInfo>();
		}

        protected virtual void AddSession(PuttySessionInfo sessionInfo)
        {
            if (string.IsNullOrEmpty(sessionInfo?.Name) || RootInfo.Children.Any(child => child.Name == sessionInfo.Name))
                return;
            RootInfo.AddChild(sessionInfo);
            RaisePuttySessionCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, sessionInfo));
        }
		
		public virtual void StartWatcher() { }
		
		public virtual void StopWatcher() { }
        #endregion
		
		public delegate void PuttySessionChangedEventHandler(object sender, PuttySessionChangedEventArgs e);
        public event PuttySessionChangedEventHandler PuttySessionChanged;
		
		protected virtual void RaiseSessionChangedEvent(PuttySessionChangedEventArgs args)
		{
            PuttySessionChanged?.Invoke(this, args);
		}

        public event NotifyCollectionChangedEventHandler PuttySessionsCollectionChanged;

        protected void RaisePuttySessionCollectionChangedEvent(NotifyCollectionChangedEventArgs args)
        {
            PuttySessionsCollectionChanged?.Invoke(this, args);
        }
    }
}