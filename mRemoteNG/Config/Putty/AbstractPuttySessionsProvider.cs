using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Tree.Root;
using System.Net;
using System.Runtime.Versioning;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Config.Putty
{
    [SupportedOSPlatform("windows")]
    public abstract class AbstractPuttySessionsProvider
    {
        public virtual RootPuttySessionsNodeInfo RootInfo { get; } = new RootPuttySessionsNodeInfo();

        protected virtual List<PuttySessionInfo> Sessions => RootInfo.Children.OfType<PuttySessionInfo>().ToList();

        #region Public Methods

        public abstract string[] GetSessionNames(bool raw = false);
        public abstract PuttySessionInfo GetSession(string sessionName);

        public virtual IEnumerable<PuttySessionInfo> GetSessions()
        {
            var sessionNamesFromProvider = GetSessionNames(true);
            foreach (var sessionName in GetSessionNamesToAdd(sessionNamesFromProvider))
            {
                var sessionInfo = GetSession(sessionName);
                AddSession(sessionInfo);
            }

            foreach (var session in GetSessionToRemove(sessionNamesFromProvider))
            {
                RemoveSession(session);
            }

            RootInfo.SortRecursive();
            return Sessions;
        }

        private IEnumerable<string> GetSessionNamesToAdd(IEnumerable<string> sessionNamesFromProvider)
        {
            var currentlyKnownSessionNames = Sessions.Select(session => session.Name);
            var sessionNamesToAdd = sessionNamesFromProvider.Except(currentlyKnownSessionNames);
            return sessionNamesToAdd;
        }

        private IEnumerable<PuttySessionInfo> GetSessionToRemove(IEnumerable<string> sessionNamesFromProvider)
        {
            var currentlyKnownSessionNames = Sessions.Select(session => session.Name);
            var normalizedSessionNames =
                sessionNamesFromProvider.Select(name =>
                                                    WebUtility.UrlDecode(name));
            var sessionNamesToRemove = currentlyKnownSessionNames.Except(normalizedSessionNames);
            return Sessions.Where(session => sessionNamesToRemove.Contains(session.Name));
        }

        protected virtual void AddSession(PuttySessionInfo sessionInfo)
        {
            if (string.IsNullOrEmpty(sessionInfo?.Name) || Sessions.Any(child => child.Name == sessionInfo.Name))
                return;
            RootInfo.AddChild(sessionInfo);
            RaisePuttySessionCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, sessionInfo));
        }

        protected virtual void RemoveSession(PuttySessionInfo sessionInfo)
        {
            if (!Sessions.Contains(sessionInfo)) return;
            RootInfo.RemoveChild(sessionInfo);
            RaisePuttySessionCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, sessionInfo));
        }

        public virtual void StartWatcher()
        {
        }

        public virtual void StopWatcher()
        {
        }

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