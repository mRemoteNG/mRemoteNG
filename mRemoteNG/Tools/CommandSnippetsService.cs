using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class CommandSnippetsService
    {
        public FullyObservableCollection<CommandSnippet> Snippets { get; set; } = [];

        public CommandSnippet? GetSnippetByName(string name)
        {
            return Snippets.FirstOrDefault(s => s.Name == name);
        }
    }
}
