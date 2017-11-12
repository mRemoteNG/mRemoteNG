using System.Linq;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.Tools
{
    public class ExternalToolsService
    {
        public FullyObservableCollection<ExternalTool> ExternalTools { get; set; } = new FullyObservableCollection<ExternalTool>();

        public ExternalTool GetExtAppByName(string name)
        {
            return ExternalTools.FirstOrDefault(extA => extA.DisplayName == name);
        }
    }
}