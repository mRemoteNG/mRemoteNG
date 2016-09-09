using System.Collections.Generic;
using System.ComponentModel;

namespace mRemoteNG.Tools
{
    public class ExternalToolsTypeConverter : StringConverter
    {
        public static string[] ExternalTools
        {
            get
            {
                List<string> externalToolList = new List<string>();

                // Add a blank entry to signify that no external tool is selected
                externalToolList.Add(string.Empty);

                foreach (ExternalTool externalTool in App.Runtime.ExternalTools)
                {
                    externalToolList.Add(externalTool.DisplayName);
                }

                return externalToolList.ToArray();
            }
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(ExternalTools);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
