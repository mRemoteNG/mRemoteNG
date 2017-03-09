using System;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.App
{
	public static class Export
	{
		public static void ExportToFile(ConnectionInfo selectedNode, ConnectionTreeModel connectionTreeModel)
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
						
					if (exportForm.ShowDialog(FrmMain.Default) != DialogResult.OK)
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
			
		private static void SaveExportFile(string fileName, ConnectionsSaver.Format saveFormat, SaveFilter saveFilter, ConnectionInfo exportTarget)
		{
			try
			{
			    ISerializer<string> serializer;
			    switch (saveFormat)
			    {
			        case ConnectionsSaver.Format.mRXML:
                        var factory = new CryptographyProviderFactory();
                        var cryptographyProvider = factory.CreateAeadCryptographyProvider(Settings.Default.EncryptionEngine, Settings.Default.EncryptionBlockCipherMode);
                        cryptographyProvider.KeyDerivationIterations = Settings.Default.EncryptionKeyDerivationIterations;
			            var rootNode = exportTarget.GetRootParent() as RootNodeInfo;
                        var connectionNodeSerializer = new XmlConnectionNodeSerializer27(
                            cryptographyProvider, 
                            rootNode?.PasswordString.ConvertToSecureString() ?? new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString(),
                            saveFilter);
			            serializer = new XmlConnectionsSerializer(cryptographyProvider, connectionNodeSerializer);
			            break;
			        case ConnectionsSaver.Format.mRCSV:
			            serializer = new CsvConnectionsSerializerMremotengFormat(saveFilter);
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
			    Runtime.RemoteConnectionsSyncronizer?.Enable();
			}
		}
	}
}