namespace mRemoteNG.Properties
{
    internal sealed partial class OptionsSecurityPage
    {
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string MasterPasswordVerifier
        {
            get => (string)this[nameof(MasterPasswordVerifier)];
            set => this[nameof(MasterPasswordVerifier)] = value;
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string MasterPasswordHint
        {
            get => (string)this[nameof(MasterPasswordHint)];
            set => this[nameof(MasterPasswordHint)] = value;
        }
    }
}
