using System;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Csv;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.App
{
	public class Export
	{
	    private readonly IConnectionsService _connectionsService;
	    private readonly ICredentialRepositoryList _credentialRepositoryList;
	    private readonly IWin32Window _dialogWindowParent;

	    public Export(ICredentialRepositoryList credentialRepositoryList, IConnectionsService connectionsService, IWin32Window dialogWindowParent)
	    {
	        _credentialRepositoryList = credentialRepositoryList.ThrowIfNull(nameof(credentialRepositoryList));
	        _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
	        _dialogWindowParent = dialogWindowParent.ThrowIfNull(nameof(dialogWindowParent));
	    }

		public void ExportToFile(ConnectionInfo selectedNode, ConnectionTreeModel connectionTreeModel)
		{
			try
			{
			    var saveFilter = new SaveFilter();
					
				using (var exportForm = new ExportForm())
				{
					if (selectedNode?.GetTreeNodeType() == TreeNodeType.Container)
						exportForm.SelectedFolder = selectedNode as ContainerInfo;
					else if (selectedNode?.GetTreeNodeType() == TreeNodeType.Connection)
					{
						if (selectedNode.Parent.GetTreeNodeType() == TreeNodeType.Container)
							exportForm.SelectedFolder = selectedNode.Parent;
						exportForm.SelectedConnection = selectedNode;
					}
						
					if (exportForm.ShowDialog(_dialogWindowParent) != DialogResult.OK)
						return;

				    ConnectionInfo exportTarget;
				    switch (exportForm.Scope)
					{
						case ExportForm.ExportScope.SelectedFolder:
							exportTarget = exportForm.SelectedFolder;
							break;
                        case ExportForm.ExportScope.SelectedConnection:
							exportTarget = exportForm.SelectedConnection;
							break;
						default:
							exportTarget = connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
							break;
					}
						
					saveFilter.SaveUsername = exportForm.IncludeUsername;
					saveFilter.SavePassword = exportForm.IncludePassword;
					saveFilter.SaveDomain = exportForm.IncludeDomain;
					saveFilter.SaveInheritance = exportForm.IncludeInheritance;
				    saveFilter.SaveCredentialId = exportForm.IncludeAssignedCredential;
						
					SaveExportFile(exportForm.FileName, exportForm.SaveFormat, saveFilter, exportTarget);
				}
					
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage("App.Export.ExportToFile() failed.", ex);
			}
		}
			
		private void SaveExportFile(string fileName, SaveFormat saveFormat, SaveFilter saveFilter, ConnectionInfo exportTarget)
		{
			try
			{
			    ISerializer<ConnectionInfo, string> serializer;
			    switch (saveFormat)
			    {
			        case SaveFormat.mRXML:
                        var cryptographyProvider = new CryptoProviderFactoryFromSettings().Build();
			            var rootNode = exportTarget.GetRootParent() as RootNodeInfo;
                        var connectionNodeSerializer = new XmlConnectionNodeSerializer27(
                            cryptographyProvider, 
                            rootNode?.PasswordString.ConvertToSecureString() ?? new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString(),
                            saveFilter);
			            serializer = new XmlConnectionsSerializer(cryptographyProvider, connectionNodeSerializer);
			            break;
			        case SaveFormat.mRCSV:
			            serializer = new CsvConnectionsSerializerMremotengFormat(saveFilter, _credentialRepositoryList);
                        break;
			        default:
			            throw new ArgumentOutOfRangeException(nameof(saveFormat), saveFormat, null);
			    }
			    var serializedData = serializer.Serialize(exportTarget);
			    var fileDataProvider = new FileDataProvider(fileName);
                fileDataProvider.Save(serializedData);
			}
			catch (Exception ex)
			{
			    Runtime.MessageCollector.AddExceptionStackTrace($"Export.SaveExportFile(\"{fileName}\") failed.", ex);
			}
			finally
			{
			    _connectionsService.RemoteConnectionsSyncronizer?.Enable();
			}
		}
	}
}