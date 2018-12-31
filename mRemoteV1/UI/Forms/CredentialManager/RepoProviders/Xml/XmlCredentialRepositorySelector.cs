using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManager.RepoProviders.Xml
{
    public class XmlCredentialRepositorySelector : ISelectionTarget<ICredentialRepositoryConfig>
    {
        private readonly CredentialService _credentialService;

        public string Text { get; set; } = "XML";
        public Image Image { get; } = Resources.xml;
        public ICredentialRepositoryConfig Config { get; } = new CredentialRepositoryConfig {TypeName = "Xml"};

        public XmlCredentialRepositorySelector(CredentialService credentialService)
        {
            _credentialService = credentialService.ThrowIfNull(nameof(credentialService));
        }

        public SequencedControl BuildEditorPage(ICredentialRepositoryList repositoryList)
        {
            var repositoryFactory = _credentialService.GetRepositoryFactoryForConfig(Config);

            if (!repositoryFactory.Any())
                throw new CredentialRepositoryTypeNotSupportedException(Config.TypeName);

            return new XmlCredentialRepositoryEditorPage(Config, repositoryList, repositoryFactory.First())
            {
                Dock = DockStyle.Fill
            };
        }
    }
}