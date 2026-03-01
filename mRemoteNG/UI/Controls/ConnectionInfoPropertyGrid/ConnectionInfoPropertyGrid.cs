using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Attributes;
using mRemoteNG.Tree.Root;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid {
    [SupportedOSPlatform("windows")]
    public partial class ConnectionInfoPropertyGrid : FilteredPropertyGrid.FilteredPropertyGrid {
        private readonly Dictionary<Type, IEnumerable<PropertyInfo>> _propertyCache = [];
        private ConnectionInfo? _selectedConnectionInfo;
        private IEnumerable<ConnectionInfo>? _selectedConnectionInfos;
        private PropertyMode _propertyMode;

        public IEnumerable<ConnectionInfo>? SelectedConnectionInfos
        {
            get => _selectedConnectionInfos;
            set
            {
                if (_selectedConnectionInfos == value) return;
                _selectedConnectionInfos = value;
                _selectedConnectionInfo = _selectedConnectionInfos?.FirstOrDefault();
                RootNodeSelected = _selectedConnectionInfos?.Any(c => c is RootNodeInfo) == true;
                SetGridObject();
            }
        }

        /// <summary>
        /// The <see cref="ConnectionInfo"/> currently being shown by this
        /// property grid.
        /// </summary>
        public ConnectionInfo? SelectedConnectionInfo {
            get => _selectedConnectionInfo;
            set {
                if (_selectedConnectionInfo == value)
                    return;

                _selectedConnectionInfo = value;
                _selectedConnectionInfos = value != null ? new[] { value } : null;
                RootNodeSelected = SelectedConnectionInfo is RootNodeInfo;
                SetGridObject();
            }
        }

        /// <summary>
        /// Determines which set of properties this grid will display.
        /// </summary>
        public PropertyMode PropertyMode {
            get => _propertyMode;
            set {
                if (_propertyMode == value)
                    return;
                _propertyMode = value;
                SetGridObject();
            }
        }

        /// <summary>
        /// Is the property grid showing the selected connection's
        /// inheritance info? If false, the connection's normal
        /// properties are shown instead.
        /// </summary>
        public bool IsShowingInheritance => PropertyMode == PropertyMode.Inheritance ||
                                            PropertyMode == PropertyMode.DefaultInheritance;

        /// <summary>
        /// This indicates whether the current <see cref="SelectedConnectionInfo"/>
        /// is a <see cref="DefaultConnectionInfo"/>.
        /// </summary>
        public bool IsShowingDefaultProperties => PropertyMode == PropertyMode.DefaultConnection ||
                                                  PropertyMode == PropertyMode.DefaultInheritance;

        /// <summary>
        /// True when the <see cref="SelectedConnectionInfo"/> is
        /// of type <see cref="RootNodeInfo"/>.
        /// </summary>
        public bool RootNodeSelected { get; private set; }

        public ConnectionInfoPropertyGrid() {
            InitializeComponent();
            PropertyValueChanged += pGrid_PropertyValueChanged;
        }

        private void SetGridObject() {
            ClearFilters();

            if (_selectedConnectionInfos == null || !_selectedConnectionInfos.Any())
            {
                SelectedObjects = null;
                return;
            }

            switch (PropertyMode) {
                case PropertyMode.Connection:
                default:
                    SelectedObjects = _selectedConnectionInfos.ToArray();
                    break;
                case PropertyMode.Inheritance:
                    SelectedObjects = _selectedConnectionInfos.Select(c => c.Inheritance).ToArray();
                    break;
                case PropertyMode.DefaultConnection:
                    SelectedObject = DefaultConnectionInfo.Instance;
                    break;
                case PropertyMode.DefaultInheritance:
                    SelectedObject = DefaultConnectionInheritance.Instance;
                    break;
            }

            if ((SelectedObjects != null && SelectedObjects.Length > 0) || SelectedObject != null)
                ShowHideGridItems();
        }

        private void ShowHideGridItems() {
            try {
                if (IsShowingDefaultProperties)
                {
                    ShowHideGridItemsDefault();
                    return;
                }

                if (_selectedConnectionInfos == null || !_selectedConnectionInfos.Any())
                    return;

                if (RootNodeSelected && PropertyMode == PropertyMode.Connection) {
                    if (SelectedConnectionInfo is RootPuttySessionsNodeInfo) {
                        BrowsableProperties = new[]
                        {
                            nameof(RootPuttySessionsNodeInfo.Name)
                        };
                    } else if (SelectedConnectionInfo is RootNodeInfo) {
                        RootNodeInfo rootInfo = (RootNodeInfo)SelectedConnectionInfo;
                        List<string> rootProperties =
                        [
                            nameof(RootNodeInfo.Name),
                            nameof(RootNodeInfo.Password)
                        ];

                        if (rootInfo.Password)
                            rootProperties.Add(nameof(RootNodeInfo.AutoLockOnMinimize));

                        BrowsableProperties = rootProperties.ToArray();
                    }

                    Refresh();
                    return;
                }

                // Gather valid properties for ALL selected connections (Intersection)
                List<string>? commonValidProperties = null;
                foreach (var info in _selectedConnectionInfos)
                {
                    object gridObject = IsShowingInheritance ? info.Inheritance : info;
                    var props = GetPropertiesForGridObject(gridObject)
                                .Where(property => IsValidForProtocol(property, info.Protocol, IsShowingInheritance))
                                .Select(property => property.Name);

                    if (commonValidProperties == null)
                        commonValidProperties = props.ToList();
                    else
                        commonValidProperties = commonValidProperties.Intersect(props, StringComparer.Ordinal).ToList();
                }
                BrowsableProperties = commonValidProperties?.ToArray() ?? Array.Empty<string>();

                // Gather exclusions for ALL selected connections (Intersection)
                // If a property is excluded in ALL, then hide it.
                // If it is needed by ANY, keep it visible.
                List<string>? commonExclusions = null;

                foreach (var info in _selectedConnectionInfos)
                {
                    var exclusions = new List<string>();

                    if (PropertyMode == PropertyMode.Connection) {
                        // hide any inherited properties
                        exclusions.AddRange(info.Inheritance.GetEnabledInheritanceProperties());

                        // hide external provider fields
                        exclusions.AddRange(SpecialExternalAddressProviderExclusions(info));
                        exclusions.AddRange(SpecialExternalCredentialProviderExclusions(info));
                        if (info.Protocol == ProtocolType.IntApp)
                            exclusions.Remove(nameof(AbstractConnectionRecord.Username));

                        // ReSharper disable once SwitchStatementMissingSomeCases
                        switch (info.Protocol) {
                            case ProtocolType.RDP:
                                exclusions.AddRange(SpecialRdpExclusions(info));
                                break;
                            case ProtocolType.VNC:
                            case ProtocolType.ARD:
                                exclusions.AddRange(SpecialVncExclusions(info));
                                break;
                        }

                        if (info is PuttySessionInfo)
                            exclusions.Add(nameof(AbstractConnectionRecord.Favorite));
                    }

                    // Hide credential fields when registry policy denies saving
                    if (!CommonRegistrySettings.AllowSavePasswords)
                        exclusions.Add(nameof(AbstractConnectionRecord.Password));

                    if (!CommonRegistrySettings.AllowSaveUsernames)
                        exclusions.Add(nameof(AbstractConnectionRecord.Username));

                    if (commonExclusions == null)
                        commonExclusions = exclusions;
                    else
                        commonExclusions = commonExclusions.Intersect(exclusions, StringComparer.Ordinal).ToList();
                }

                HiddenProperties = commonExclusions?.ToArray();
                Refresh();
            } catch (Exception ex) {
                Runtime.MessageCollector.AddMessage(
                    MessageClass.ErrorMsg,
                    Language.ConfigPropertyGridHideItemsFailed +
                    Environment.NewLine + ex.Message, true);
            }
        }

        private void ShowHideGridItemsDefault()
        {
             List<string> strHide = new();
             if (PropertyMode == PropertyMode.DefaultConnection) {
                    strHide.Add(nameof(AbstractConnectionRecord.Hostname));
                    strHide.Add(nameof(AbstractConnectionRecord.AlternativeAddress));
                    strHide.Add(nameof(AbstractConnectionRecord.Name));
             }
             HiddenProperties = strHide.ToArray();
             Refresh();
        }

        private IEnumerable<PropertyInfo> GetPropertiesForGridObject(object currentGridObject) {
            if (_propertyCache.TryGetValue(currentGridObject.GetType(), out IEnumerable<PropertyInfo>? properties))
                return properties;

            Type type = currentGridObject.GetType();
            PropertyInfo[] props = type.GetProperties();
            _propertyCache.Add(type, props);

            return props;
        }

        private static bool IsValidForProtocol(PropertyInfo property, ProtocolType protocol, bool skipProtocolCheck) {
            return
                property.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false &&
                (skipProtocolCheck || property.GetCustomAttribute<AttributeUsedInProtocol>()?
                    .SupportedProtocolTypes
                    .Contains(protocol) != false);
        }

        private static List<string> SpecialExternalAddressProviderExclusions(ConnectionInfo info) {
            List<string> strHide = new();
            if (info == null)
                return strHide;

            // aws
            if (info.ExternalAddressProvider != ExternalAddressProvider.AmazonWebServices) {
                strHide.Add(nameof(AbstractConnectionRecord.EC2InstanceId));
                strHide.Add(nameof(AbstractConnectionRecord.EC2Region));
            }
            return strHide;
        }

        private static List<string> SpecialExternalCredentialProviderExclusions(ConnectionInfo info) {
            List<string> strHide = new();
            if (info == null)
                return strHide;

            if (info.ExternalCredentialProvider == ExternalCredentialProvider.None) {
                strHide.Add(nameof(AbstractConnectionRecord.UserViaAPI));
                strHide.Add(nameof(AbstractConnectionRecord.VaultOpenbaoSecretEngine));
                strHide.Add(nameof(AbstractConnectionRecord.VaultOpenbaoMount));
                strHide.Add(nameof(AbstractConnectionRecord.VaultOpenbaoRole));
            } else if (info.ExternalCredentialProvider == ExternalCredentialProvider.DelineaSecretServer
                  || info.ExternalCredentialProvider == ExternalCredentialProvider.ClickstudiosPasswordState) {
                strHide.Add(nameof(AbstractConnectionRecord.Username));
                strHide.Add(nameof(AbstractConnectionRecord.Password));
                strHide.Add(nameof(AbstractConnectionRecord.Domain));
                strHide.Add(nameof(AbstractConnectionRecord.VaultOpenbaoSecretEngine));
                strHide.Add(nameof(AbstractConnectionRecord.VaultOpenbaoMount));
                strHide.Add(nameof(AbstractConnectionRecord.VaultOpenbaoRole));
            } else if (info.ExternalCredentialProvider == ExternalCredentialProvider.OnePassword) {
                strHide.Add(nameof(AbstractConnectionRecord.VaultOpenbaoSecretEngine));
                strHide.Add(nameof(AbstractConnectionRecord.VaultOpenbaoMount));
                strHide.Add(nameof(AbstractConnectionRecord.VaultOpenbaoRole));
            } else if (info.ExternalCredentialProvider == ExternalCredentialProvider.VaultOpenbao) {
                strHide.Add(nameof(AbstractConnectionRecord.UserViaAPI));
                if (info.VaultOpenbaoSecretEngine != VaultOpenbaoSecretEngine.Kv
                    && info.VaultOpenbaoSecretEngine != VaultOpenbaoSecretEngine.SSHOTP)
                    strHide.Add(nameof(AbstractConnectionRecord.Username));
                strHide.Add(nameof(AbstractConnectionRecord.Password));
            }
            return strHide;
        }

        /// <summary>
        /// 
        /// </summary>
        private static List<string> SpecialRdpExclusions(ConnectionInfo info) {
            List<string> strHide = new();
            if (info == null)
                return strHide;

            if (info.RDPMinutesToIdleTimeout <= 0) {
                strHide.Add(nameof(AbstractConnectionRecord.RDPAlertIdleTimeout));
            }

            if (info.RDGatewayUsageMethod == RDGatewayUsageMethod.Never) {
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayDomain));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayHostname));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayPassword));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayUseConnectionCredentials));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayUsername));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayAccessToken));
            } else if (info.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.Yes ||
                       info.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard) {
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayDomain));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayPassword));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayUsername));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayExternalCredentialProvider));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayUserViaAPI));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayAccessToken));
            } else if (info.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.ExternalCredentialProvider) {
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayDomain));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayPassword));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayUsername));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayAccessToken));
            } else if (info.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.AccessToken) {
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayDomain));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayPassword));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayUsername));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayExternalCredentialProvider));
                strHide.Add(nameof(AbstractConnectionRecord.RDGatewayUserViaAPI));
            }

            if (!(info.Resolution == RDPResolutions.FitToWindow ||
                  info.Resolution == RDPResolutions.Fullscreen)) {
                strHide.Add(nameof(AbstractConnectionRecord.AutomaticResize));
            }

            if (info.Resolution != RDPResolutions.Custom) {
                strHide.Add(nameof(AbstractConnectionRecord.ResolutionWidth));
                strHide.Add(nameof(AbstractConnectionRecord.ResolutionHeight));
            }

            if (info.RedirectDiskDrives != RDPDiskDrives.Custom) {
                strHide.Add(nameof(AbstractConnectionRecord.RedirectDiskDrivesCustom));
            }

            if (info.RedirectSound != RDPSounds.BringToThisComputer) {
                strHide.Add(nameof(AbstractConnectionRecord.SoundQuality));
            }

            if (!info.UseVmId) {
                strHide.Add(nameof(AbstractConnectionRecord.VmId));
                strHide.Add(nameof(AbstractConnectionRecord.UseEnhancedMode));
            }

            return strHide;
        }

        private static List<string> SpecialVncExclusions(ConnectionInfo info) {
            List<string> strHide = new();
            if (info == null)
                return strHide;
            if (info.VNCAuthMode == ProtocolVNC.AuthMode.AuthVNC) {
                strHide.Add(nameof(AbstractConnectionRecord.Username));
                strHide.Add(nameof(AbstractConnectionRecord.Domain));
            }

            if (info.VNCProxyType == ProtocolVNC.ProxyType.ProxyNone) {
                strHide.Add(nameof(AbstractConnectionRecord.VNCProxyIP));
                strHide.Add(nameof(AbstractConnectionRecord.VNCProxyPassword));
                strHide.Add(nameof(AbstractConnectionRecord.VNCProxyPort));
                strHide.Add(nameof(AbstractConnectionRecord.VNCProxyUsername));
            }

            return strHide;
        }

        private void UpdateConnectionInfoNode(PropertyValueChangedEventArgs e) {
            if (IsShowingInheritance)
                return;

            if (_selectedConnectionInfos != null) {
                foreach (var info in _selectedConnectionInfos) {
                    if (e.ChangedItem?.Label == Language.Protocol) {
                        info.SetDefaultPort();
                    } else if (e.ChangedItem?.Label == Language.Name) {
                        if (Settings.Default.SetHostnameLikeDisplayName) {
                            if (!string.IsNullOrEmpty(info.Name))
                                info.Hostname = info.Name;
                        }
                    }
                }
            }

            if (IsShowingDefaultProperties)
                DefaultConnectionInfo.Instance.SaveTo(Settings.Default, a => "ConDefault" + a);
        }

        private void UpdateRootInfoNode(PropertyValueChangedEventArgs e) {
            if (!(SelectedObject is RootNodeInfo rootInfo))
                return;

            string changedProperty = e.ChangedItem?.PropertyDescriptor?.Name ?? "";
            if (changedProperty == nameof(RootNodeInfo.AutoLockOnMinimize) && !rootInfo.Password)
            {
                rootInfo.AutoLockOnMinimize = false;
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Autolock requires password protection to be enabled.");
                return;
            }

            if (changedProperty != nameof(RootNodeInfo.Password))
                return;

            if (rootInfo.Password) {
                string passwordName = Properties.OptionsDBsPage.Default.UseSQLServer ? Language.SQLServer.TrimEnd(':') : Path.GetFileName(ConnectionsService.GetStartupConnectionFileName());
                Optional<System.Security.SecureString> password = MiscTools.PasswordDialog(passwordName);

                // operation cancelled, dont set a password
                if (!password.Any() || password.First().Length == 0) {
                    rootInfo.Password = false;
                    return;
                }

                rootInfo.PasswordString = password.First().ConvertToUnsecureString();
            } else {
                if (!CurrentPasswordVerified(rootInfo))
                {
                    rootInfo.Password = true;
                    return;
                }

                rootInfo.AutoLockOnMinimize = false;
                rootInfo.PasswordString = "";
            }
        }

        private static bool CurrentPasswordVerified(RootNodeInfo rootInfo)
        {
            string passwordName = Properties.OptionsDBsPage.Default.UseSQLServer
                ? Language.SQLServer.TrimEnd(':')
                : Path.GetFileName(ConnectionsService.GetStartupConnectionFileName());

            Optional<System.Security.SecureString> password = MiscTools.PasswordDialog(passwordName, false);
            if (!password.Any() || password.First().Length == 0)
                return false;

            bool matches = rootInfo.IsPasswordMatch(password.First());
            if (!matches)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Password protection disable request rejected: provided password did not match.");
            }

            return matches;
        }

        private void UpdateInheritanceNode() {
            if (IsShowingDefaultProperties && IsShowingInheritance)
                DefaultConnectionInheritance.SaveTo(Settings.Default, a => "InhDefault" + a);
        }

        private void pGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
            try {
                UpdateConnectionInfoNode(e);
                UpdateRootInfoNode(e);
                UpdateInheritanceNode();
                ShowHideGridItems();
            } catch (Exception ex) {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    Language.ConfigPropertyGridValueFailed + Environment.NewLine +
                    ex.Message, true);
            }
        }
    }
}
