using System;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Csv;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Json;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Rdp;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public static class Export
    {
        public static void ExportToFile(ConnectionInfo selectedNode, ConnectionTreeModel connectionTreeModel)
        {
            try
            {
                SaveFilter saveFilter = new();

                using (FrmExport exportForm = new())
                {
                    if (selectedNode?.GetTreeNodeType() == TreeNodeType.Container)
                        exportForm.SelectedFolder = selectedNode as ContainerInfo;
                    else if (selectedNode?.GetTreeNodeType() == TreeNodeType.Connection)
                    {
                        if (selectedNode.Parent?.GetTreeNodeType() == TreeNodeType.Container)
                            exportForm.SelectedFolder = selectedNode.Parent;
                        exportForm.SelectedConnection = selectedNode;
                    }

                    if (exportForm.ShowDialog(FrmMain.Default) != DialogResult.OK)
                        return;

                    ConnectionInfo defaultTarget = connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
                    ConnectionInfo exportTarget = exportForm.Scope switch
                    {
                        FrmExport.ExportScope.SelectedFolder => exportForm.SelectedFolder ?? defaultTarget,
                        FrmExport.ExportScope.SelectedConnection => exportForm.SelectedConnection ?? defaultTarget,
                        _ => defaultTarget
                    };

                    saveFilter.SaveUsername = exportForm.IncludeUsername;
                    saveFilter.SavePassword = exportForm.IncludePassword;
                    saveFilter.SaveDomain = exportForm.IncludeDomain;
                    saveFilter.SaveInheritance = exportForm.IncludeInheritance;
                    saveFilter.SaveCredentialId = exportForm.IncludeAssignedCredential;

                    SaveExportFile(exportForm.FileName, exportForm.SaveFormat, saveFilter, exportTarget, exportForm.IsEncrypted, exportForm.Password);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Export.ExportToFile() failed.", ex);
            }
        }

        private static void SaveExportFile(string fileName,
                                           SaveFormat saveFormat,
                                           SaveFilter saveFilter,
                                           ConnectionInfo exportTarget,
                                           bool isEncrypted = false,
                                           string password = "")
        {
            try
            {
                ISerializer<ConnectionInfo, string> serializer;
                switch (saveFormat)
                {
                    case SaveFormat.mRXML:
                        if (isEncrypted)
                        {
                            RootNodeInfo tempRoot = new(RootNodeType.Connection)
                            {
                                PasswordString = password
                            };

                            ConnectionInfo clonedTarget = exportTarget.Clone();

                            if (exportTarget is RootNodeInfo)
                            {
                                if (clonedTarget is ContainerInfo container)
                                {
                                    tempRoot.AddChildRange(container.Children.ToArray());
                                }
                                exportTarget = tempRoot;
                            }
                            else
                            {
                                tempRoot.AddChild(clonedTarget);
                                exportTarget = clonedTarget;
                            }
                        }

                        ICryptographyProvider cryptographyProvider = new CryptoProviderFactoryFromSettings().Build();
                        RootNodeInfo? rootNode = exportTarget.GetRootParent() as RootNodeInfo;
                        XmlConnectionNodeSerializer28 connectionNodeSerializer = new(
                                                                                         cryptographyProvider,
                                                                                         rootNode?.PasswordString
                                                                                                 .ConvertToSecureString() ??
                                                                                         new RootNodeInfo(RootNodeType
                                                                                                              .Connection)
                                                                                             .PasswordString
                                                                                             .ConvertToSecureString(),
                                                                                         saveFilter);
                        serializer = new XmlConnectionsSerializer(cryptographyProvider, connectionNodeSerializer)
                        {
                            UseFullEncryption = isEncrypted
                        };
                        break;
                    case SaveFormat.mRCSV:
                        serializer =
                            new CsvConnectionsSerializerMremotengFormat(saveFilter, Runtime.CredentialProviderCatalog);
                        break;
                    case SaveFormat.mRJSON:
                        serializer = new JsonConnectionsSerializer(saveFilter);
                        break;
                    case SaveFormat.RDP:
                        serializer = new RdpConnectionSerializer(saveFilter);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(saveFormat), saveFormat, null);
                }

                string serializedData = serializer.Serialize(exportTarget);
                FileDataProvider fileDataProvider = new(fileName);
                fileDataProvider.Save(serializedData);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Export.SaveExportFile(\"{fileName}\") failed.", ex);
            }
            finally
            {
                Runtime.ConnectionsService.RemoteConnectionsSyncronizer?.Enable();
            }
        }
    }
}