using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.Config.Import;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public static class Import
    {
        public static void ImportFromFile(ContainerInfo importDestinationContainer)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new())
                {
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    openFileDialog.Multiselect = true;

                    List<string> fileTypes = new();
                    fileTypes.AddRange(new[] {Language.FilterAllImportable, "*.xml;*.rdp;*.rdg;*.dat;*.csv;*.html;*.htm;*.txt;*.ini;*.crt"});
                    fileTypes.AddRange(new[] {Language.FiltermRemoteXML, "*.xml"});
                    fileTypes.AddRange(new[] {Language.FiltermRemoteCSV, "*.csv"});
                    fileTypes.AddRange(new[] {Language.FilterRDP, "*.rdp"});
                    fileTypes.AddRange(new[] {Language.FilterRdgFiles, "*.rdg"});
                    fileTypes.AddRange(new[] {Language.FilterPuttyConnectionManager, "*.dat"});
                    fileTypes.AddRange(new[] {Language.FilterNetscapeBookmarks, "*.html;*.htm"});
                    fileTypes.AddRange(new[] {"Text List Files (*.txt)", "*.txt"});
                    fileTypes.AddRange(new[] {Language.FilterAll, "*.*"});
                    fileTypes.AddRange(new[] { Language.FilterSecureCRT, "*.crt" });

                    openFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                        return;

					HeadlessFileImport(
						openFileDialog.FileNames,
						importDestinationContainer,
						Runtime.ConnectionsService,
						fileName => MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Language.ImportFileFailedContent, fileName), Language.AskUpdatesMainInstruction,
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1),
                        (nodes) =>
                        {
                            var repos = Runtime.CredentialProviderCatalog.CredentialProviders;
                            if (!repos.Any()) return;

                            if (nodes.Any(CredentialImportHelper.HasCredentials))
                            {
                                var repo = repos.First();
                                var result = MessageBox.Show(
                                    $"The imported file contains credentials.{Environment.NewLine}Do you want to extract them to the Credential Repository '{repo.Config.Title}'?",
                                    "Import Credentials",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    foreach (var node in nodes)
                                    {
                                        CredentialImportHelper.ExtractCredentials(node, repo);
                                    }
                                    repo.SaveCredentials(repo.Config.Key);
                                }
                            }
                        });
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Unable to import file.", ex);
            }
        }

        public static void ImportFromRemoteDesktopManagerCsv(ContainerInfo importDestinationContainer)
        {
            try
            {
                using (Runtime.ConnectionsService.BatchedSavingContext())
                {
                    using (OpenFileDialog openFileDialog = new())
                    {
                        openFileDialog.CheckFileExists = true;
                        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        openFileDialog.Multiselect = false;

                        List<string> fileTypes = new();
                        fileTypes.AddRange(new[] {Language.FiltermRemoteRemoteDesktopManagerCSV, "*.csv"});

                        openFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                        if (openFileDialog.ShowDialog() != DialogResult.OK)
                            return;

                        RemoteDesktopManagerImporter importer = new();
                        importer.Import(openFileDialog.FileName, importDestinationContainer);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromRemoteDesktopManagerCsv() failed.", ex);
            }
        }

        public static void ImportFromTextList(ContainerInfo importDestinationContainer)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new())
                {
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    openFileDialog.Multiselect = true;
                    openFileDialog.Filter = $"Text List Files (*.txt)|*.txt|{Language.FilterAll}|*.*";

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    HeadlessFileImport(
                        openFileDialog.FileNames,
                        importDestinationContainer,
                        Runtime.ConnectionsService,
                        fileName => MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Language.ImportFileFailedContent, fileName), Language.AskUpdatesMainInstruction,
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1));
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromTextList() failed.", ex);
            }
        }

        public static void HeadlessFileImport(
	        IEnumerable<string> filePaths,
	        ContainerInfo importDestinationContainer,
	        ConnectionsService connectionsService,
	        Action<string>? exceptionAction = null,
            Action<IEnumerable<ConnectionInfo>>? credentialHandler = null)
        {
	        using (connectionsService.BatchedSavingContext())
	        {
		        foreach (string fileName in filePaths)
		        {
			        try
			        {
                        int childrenBefore = importDestinationContainer.Children.Count;
                        IConnectionImporter<string> importer = BuildConnectionImporterFromFileExtension(fileName);
				        importer.Import(fileName, importDestinationContainer);

                        int childrenAfter = importDestinationContainer.Children.Count;
                        if (childrenAfter > childrenBefore && credentialHandler != null)
                        {
                            var newNodes = new List<ConnectionInfo>();
                            for (int i = childrenBefore; i < childrenAfter; i++)
                            {
                                newNodes.Add(importDestinationContainer.Children[i]);
                            }
                            credentialHandler(newNodes);
                        }
			        }
			        catch (Exception ex)
			        {
				        exceptionAction?.Invoke(fileName);
				        Runtime.MessageCollector.AddExceptionMessage($"Error occurred while importing file '{fileName}'.", ex);
			        }
		        }
	        }
		}

        public static void ImportFromActiveDirectory(string ldapPath,
                                                     ContainerInfo importDestinationContainer,
                                                     bool importSubOu)
        {
            try
            {
	            using (Runtime.ConnectionsService.BatchedSavingContext())
	            {
					ActiveDirectoryImporter.Import(ldapPath, importDestinationContainer, importSubOu);
	            }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromActiveDirectory() failed.", ex);
            }
        }

        public static void ImportFromPortScan(IEnumerable<ScanHost> hosts,
                                              ProtocolType protocol,
                                              ContainerInfo importDestinationContainer)
        {
            try
            {
	            using (Runtime.ConnectionsService.BatchedSavingContext())
	            {
                    PortScanImporter importer = new(protocol);
					importer.Import(hosts, importDestinationContainer);
	            }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromPortScan() failed.", ex);
            }
        }

        internal static void ImportFromPutty(ContainerInfo selectedNodeAsContainer)
        {
            try
            {
                using (Runtime.ConnectionsService.BatchedSavingContext())
                {
                    RegistryImporter.Import("Software\\SimonTatham\\PuTTY\\Sessions", selectedNodeAsContainer);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromPutty() failed.", ex);
            }
        }

        public static void ImportFromSecureCRT(ContainerInfo importDestinationContainer)
        {
            try
            {
                using (Runtime.ConnectionsService.BatchedSavingContext())
                {
                    string? defaultPath = SecureCRTFolderImporter.GetDefaultSessionsPath();

                    using FolderBrowserDialog folderDialog = new()
                    {
                        Description = "Select SecureCRT Sessions folder",
                        ShowNewFolderButton = false
                    };

                    if (defaultPath != null)
                        folderDialog.SelectedPath = defaultPath;

                    if (folderDialog.ShowDialog() != DialogResult.OK)
                        return;

                    SecureCRTFolderImporter.Import(folderDialog.SelectedPath, importDestinationContainer);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromSecureCRT() failed.", ex);
            }
        }

        public static void ImportFromMtputty(ContainerInfo importDestinationContainer)
        {
            try
            {
                using (Runtime.ConnectionsService.BatchedSavingContext())
                {
                    using (OpenFileDialog openFileDialog = new())
                    {
                        openFileDialog.CheckFileExists = true;
                        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        openFileDialog.Multiselect = false;
                        openFileDialog.Filter = $"mTTY Files (*.xml)|*.xml|{Language.FilterAll}|*.*";

                        if (openFileDialog.ShowDialog() != DialogResult.OK)
                            return;

                        MtputtyImporter importer = new();
                        importer.Import(openFileDialog.FileName, importDestinationContainer);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromMtputty() failed.", ex);
            }
        }

        public static void ImportFromGuacamole(IDatabaseConnector connector, ContainerInfo destinationContainer)
        {
            try
            {
                using (Runtime.ConnectionsService.BatchedSavingContext())
                {
                    var importer = new GuacamoleImporter(connector);
                    importer.Import(destinationContainer);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromGuacamole() failed.", ex);
                throw; // Rethrow to let the UI know
            }
        }

        private static IConnectionImporter<string> BuildConnectionImporterFromFileExtension(string fileName)
        {
            // TODO: Use the file contents to determine the file type instead of trusting the extension
            string extension = Path.GetExtension(fileName) ?? "";
            switch (extension.ToLowerInvariant())
            {
                case ".xml":
                    return new MRemoteNGXmlImporter();
                case ".csv":
                    return new MRemoteNGCsvImporter();
                case ".rdp":
                    return new RemoteDesktopConnectionImporter();
                case ".rdg":
                    return new RemoteDesktopConnectionManagerImporter();
                case ".dat":
                    return new PuttyConnectionManagerImporter();
                case ".crt":
                    return new SecureCRTImporter();
                case ".html":
                case ".htm":
                    return new BookmarksHtmlImporter();
                case ".txt":
                    return new TextListConnectionImporter();
                case ".ini":
                    return new SecureCRTIniFileImporter();
                default:
                    throw new FileFormatException("Unrecognized file format.");
            }
        }
    }
}