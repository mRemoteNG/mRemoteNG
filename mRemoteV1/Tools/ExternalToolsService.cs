using System.Collections.ObjectModel;
using System.Linq;

namespace mRemoteNG.Tools
{
    public class ExternalToolsService
    {
        public ObservableCollection<ExternalTool> ExternalTools { get; set; } = new ObservableCollection<ExternalTool>();

        public ExternalTool GetExtAppByName(string name)
        {
            return ExternalTools.FirstOrDefault(extA => extA.DisplayName == name);
        }
    }
}