using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;

namespace mRemoteNG.Connection
{
    public class ConnectionIcon : StringConverter
    {
        public static string[] Icons = {};

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

        public static Icon FromString(string iconName)
        {
            try
            {
                var iconPath = $"{GeneralAppInfo.HomePath}\\Icons\\{iconName}.ico";

                if (File.Exists(iconPath))
                {
                    var nI = new Icon(iconPath);
                    return nI;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    $"Couldn\'t get Icon from String" + Environment.NewLine + ex.Message);
            }
            return null;
        }
    }
}