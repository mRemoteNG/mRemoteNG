using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class ExternalToolsTypeConverter : StringConverter
    {
        public static string[] ExternalTools
        {
            get
            {
                var externalToolList = new List<string>();

                // Add a blank entry to signify that no external tool is selected
                externalToolList.Add(string.Empty);

                foreach (var externalTool in App.Runtime.ExternalToolsService.ExternalTools)
                {
                    externalToolList.Add(externalTool.DisplayName);
                }

                return externalToolList.ToArray();
            }
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
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