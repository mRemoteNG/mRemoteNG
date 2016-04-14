using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using mRemoteNG.Tools;
using mRemoteNG.App;

namespace mRemoteNG.Connection
{
    public class ExternalToolRecordImp : ExternalToolRecord
    {
        private string _preExtApp;
        private string _postExtApp;
        private string _macAddress;
        private string _userField;

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolBefore"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolBefore"),
            TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
        public string PreExtApp
        {
            get { return _preExtApp; }
            set { _preExtApp = value; }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolAfter"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolAfter"),
            TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
        public string PostExtApp
        {
            get { return _postExtApp; }
            set { _postExtApp = value; }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameMACAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionMACAddress")]
        public string MacAddress
        {
            get { return _macAddress; }
            set { _macAddress = value; }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUser1"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUser1")]
        public string UserField
        {
            get { return _userField; }
            set { _userField = value; }
        }

        public ExternalToolRecordImp()
        {
            this.SetDefaults();
        }

        private void SetDefaults()
        {
            _preExtApp = My.Settings.Default.ConDefaultPreExtApp;
            _postExtApp = My.Settings.Default.ConDefaultPostExtApp;
            _macAddress = My.Settings.Default.ConDefaultMacAddress;
            _userField = My.Settings.Default.ConDefaultUserField;
        }
    }
}
