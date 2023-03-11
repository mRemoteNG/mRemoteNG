using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class ExternalToolsService
    {
        public FullyObservableCollection<ExternalTool> ExternalTools { get; set; } = new FullyObservableCollection<ExternalTool>();

        public ExternalTool GetExtAppByName(string name)
        {
            return ExternalTools.FirstOrDefault(extA => extA.DisplayName == name);
        }
    }
}