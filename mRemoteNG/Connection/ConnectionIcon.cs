using System;
using System.ComponentModel;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.App.Info;


namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public class ConnectionIcon : StringConverter
    {
        public static string[] Icons = { };

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Icons);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public static System.Drawing.Icon FromString(string iconName)
        {
            try
            {
                var iconPath = $"{GeneralAppInfo.HomePath}\\Icons\\{iconName}.ico";

                if (System.IO.File.Exists(iconPath))
                {
                    var nI = new System.Drawing.Icon(iconPath);
                    return nI;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    $"Couldn\'t get Icon from String" + Environment.NewLine +
                                                    ex.Message);
            }

            return null;
        }
    }
}