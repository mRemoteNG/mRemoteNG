using System.Collections.Generic;
using System;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Root.PuttySessions;


namespace mRemoteNG.Config.Putty
{
    public abstract class AbstractPuttySessionsProvider
	{
        public RootPuttySessionsNodeInfo RootInfo { get; } = new RootPuttySessionsNodeInfo();

        #region Public Methods
        public abstract string[] GetSessionNames(bool raw = false);
		public abstract PuttySessionInfo GetSession(string sessionName);
			
		public virtual IEnumerable<PuttySessionInfo> GetSessions()
		{
			foreach (var sessionName in GetSessionNames(true))
			{
				var sessionInfo = GetSession(sessionName);
				if (string.IsNullOrEmpty(sessionInfo?.Hostname) || RootInfo.Children.Any(child => child.Name == sessionInfo.Name))
					continue;
                RootInfo.AddChild(sessionInfo);
			}
			return RootInfo.Children.OfType<PuttySessionInfo>();
		}
			
		public virtual void StartWatcher() { }
			
		public virtual void StopWatcher() { }
        #endregion
			
        #region Public Events
		public delegate void SessionChangedEventHandler(object sender, SessionChangedEventArgs e);
		private SessionChangedEventHandler SessionChangedEvent;
			
		public event SessionChangedEventHandler SessionChanged
		{
			add
			{
				SessionChangedEvent = (SessionChangedEventHandler) Delegate.Combine(SessionChangedEvent, value);
			}
			remove
			{
				SessionChangedEvent = (SessionChangedEventHandler) Delegate.Remove(SessionChangedEvent, value);
			}
		}
        #endregion
			
        #region Public Classes
        #endregion
			
        #region Protected Methods
		protected virtual void OnSessionChanged(SessionChangedEventArgs e)
		{
		    SessionChangedEvent?.Invoke(this, new SessionChangedEventArgs());
		}
        #endregion
	}
}