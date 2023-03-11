using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Versioning;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Config.Putty
{
    [SupportedOSPlatform("windows")]
    public class PuttySessionsManager
    {
        public static PuttySessionsManager Instance { get; } = new PuttySessionsManager();

        private readonly List<AbstractPuttySessionsProvider> _providers = new List<AbstractPuttySessionsProvider>();

        public IEnumerable<AbstractPuttySessionsProvider> Providers => _providers;

        public List<RootPuttySessionsNodeInfo> RootPuttySessionsNodes { get; } = new List<RootPuttySessionsNodeInfo>();

        private PuttySessionsManager()
        {
            AddProvider(new PuttySessionsRegistryProvider());
        }


        #region Public Methods

        public void AddSessions()
        {
            foreach (var provider in Providers)
            {
                AddSessionsFromProvider(provider);
            }
        }

        private void AddSessionsFromProvider(AbstractPuttySessionsProvider puttySessionProvider)
        {
            puttySessionProvider.ThrowIfNull(nameof(puttySessionProvider));

            var rootTreeNode = puttySessionProvider.RootInfo;
            puttySessionProvider.GetSessions();

            if (!RootPuttySessionsNodes.Contains(rootTreeNode) && rootTreeNode.HasChildren())
                RootPuttySessionsNodes.Add(rootTreeNode);
            rootTreeNode.SortRecursive();
        }

        public void StartWatcher()
        {
            foreach (var provider in Providers)
            {
                provider.StartWatcher();
                provider.PuttySessionChanged += PuttySessionChanged;
            }
        }

        public void StopWatcher()
        {
            foreach (var provider in Providers)
            {
                provider.StopWatcher();
                provider.PuttySessionChanged -= PuttySessionChanged;
            }
        }

        public void AddProvider(AbstractPuttySessionsProvider newProvider)
        {
            if (_providers.Contains(newProvider)) return;
            _providers.Add(newProvider);
            newProvider.PuttySessionsCollectionChanged += RaisePuttySessionCollectionChangedEvent;
            RaiseSessionProvidersCollectionChangedEvent(
                                                        new
                                                            NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                                                                                             newProvider));
        }

        public void AddProviders(IEnumerable<AbstractPuttySessionsProvider> newProviders)
        {
            foreach (var provider in newProviders)
                AddProvider(provider);
        }

        public void RemoveProvider(AbstractPuttySessionsProvider providerToRemove)
        {
            if (!_providers.Contains(providerToRemove)) return;
            _providers.Remove(providerToRemove);
            providerToRemove.PuttySessionsCollectionChanged -= RaisePuttySessionCollectionChangedEvent;
            RaiseSessionProvidersCollectionChangedEvent(
                                                        new
                                                            NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                                                                                             providerToRemove));
        }

        public void PuttySessionChanged(object sender, PuttySessionChangedEventArgs e)
        {
            AddSessions();
        }

        #endregion

        #region Private Methods

        private string[] GetSessionNames(bool raw = false)
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

        private bool IsProviderEnabled(AbstractPuttySessionsProvider puttySessionsProvider)
        {
            var enabled = true;
            if (!(puttySessionsProvider is PuttySessionsRegistryProvider)) enabled = false;

            return enabled;
        }

        #endregion

        #region Public Classes

        public class SessionList : StringConverter
        {
            public static string[] Names => Instance.GetSessionNames();

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

        public event NotifyCollectionChangedEventHandler PuttySessionsCollectionChanged;

        protected void RaisePuttySessionCollectionChangedEvent(object sender, NotifyCollectionChangedEventArgs args)
        {
            PuttySessionsCollectionChanged?.Invoke(sender, args);
        }

        public event NotifyCollectionChangedEventHandler SessionProvidersCollectionChanged;

        protected void RaiseSessionProvidersCollectionChangedEvent(NotifyCollectionChangedEventArgs args)
        {
            SessionProvidersCollectionChanged?.Invoke(this, args);
        }
    }
}