using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
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
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    openFileDialog.Multiselect = true;

                    var fileTypes = new List<string>();
                    fileTypes.AddRange(new[] {Language.FilterAllImportable, "*.xml;*.rdp;*.rdg;*.dat;*.csv"});
                    fileTypes.AddRange(new[] {Language.FiltermRemoteXML, "*.xml"});
                    fileTypes.AddRange(new[] {Language.FiltermRemoteCSV, "*.csv"});
                    fileTypes.AddRange(new[] {Language.FilterRDP, "*.rdp"});
                    fileTypes.AddRange(new[] {Language.FilterRdgFiles, "*.rdg"});
                    fileTypes.AddRange(new[] {Language.FilterPuttyConnectionManager, "*.dat"});
                    fileTypes.AddRange(new[] {Language.FilterAll, "*.*"});

                    openFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                        return;

					HeadlessFileImport(
						openFileDialog.FileNames,
						importDestinationContainer,
						Runtime.ConnectionsService,
						fileName => MessageBox.Show(string.Format(Language.ImportFileFailedContent, fileName), Language.AskUpdatesMainInstruction,
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1));
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
                    using (var openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.CheckFileExists = true;
                        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        openFileDialog.Multiselect = false;

                        var fileTypes = new List<string>();
                        fileTypes.AddRange(new[] {Language.FiltermRemoteRemoteDesktopManagerCSV, "*.csv"});

                        openFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                        if (openFileDialog.ShowDialog() != DialogResult.OK)
                            return;

                        var importer = new RemoteDesktopManagerImporter();
                        importer.Import(openFileDialog.FileName, importDestinationContainer);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromRemoteDesktopManagerCsv() failed.", ex);
            }
        }

        public static void HeadlessFileImport(
	        IEnumerable<string> filePaths,
	        ContainerInfo importDestinationContainer,
	        ConnectionsService connectionsService,
	        Action<string> exceptionAction = null)
        {
	        using (connectionsService.BatchedSavingContext())
	        {
		        foreach (var fileName in filePaths)
		        {
			        try
			        {
				        var importer = BuildConnectionImporterFromFileExtension(fileName);
				        importer.Import(fileName, importDestinationContainer);
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
					var importer = new PortScanImporter(protocol);
					importer.Import(hosts, importDestinationContainer);
	            }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromPortScan() failed.", ex);
            }
        }

        private static IConnectionImporter<string> BuildConnectionImporterFromFileExtension(string fileName)
        {
            // TODO: Use the file contents to determine the file type instead of trusting the extension
            var extension = Path.GetExtension(fileName) ?? "";
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
                default:
                    throw new FileFormatException("Unrecognized file format.");
            }
        }
    }
}