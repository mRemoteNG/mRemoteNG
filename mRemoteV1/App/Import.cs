using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.App
{
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
                    fileTypes.AddRange(new[] {Language.strFilterAllImportable, "*.xml;*.rdp;*.rdg;*.dat;*.csv"});
                    fileTypes.AddRange(new[] {Language.strFiltermRemoteXML, "*.xml"});
                    fileTypes.AddRange(new[] {Language.strFiltermRemoteCSV, "*.csv"});
                    fileTypes.AddRange(new[] {Language.strFilterRDP, "*.rdp"});
                    fileTypes.AddRange(new[] {Language.strFilterRdgFiles, "*.rdg"});
                    fileTypes.AddRange(new[] {Language.strFilterPuttyConnectionManager, "*.dat"});
                    fileTypes.AddRange(new[] {Language.strFilterAll, "*.*"});

                    openFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    foreach (var fileName in openFileDialog.FileNames)
                    {
                        try
                        {
                            var importer = BuildConnectionImporterFromFileExtension(fileName);
                            importer.Import(fileName, importDestinationContainer);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format(Language.strImportFileFailedContent, fileName),
                                            Language.strImportFileFailedMainInstruction,
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                                            MessageBoxDefaultButton.Button1);
                            Runtime.MessageCollector.AddExceptionMessage("Unable to import file.", ex);
                        }
                    }

                    Runtime.ConnectionsService.SaveConnectionsAsync();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Unable to import file.", ex);
            }
        }

        public static void ImportFromActiveDirectory(string ldapPath,
                                                     ContainerInfo importDestinationContainer,
                                                     bool importSubOu)
        {
            try
            {
                ActiveDirectoryImporter.Import(ldapPath, importDestinationContainer, importSubOu);
                Runtime.ConnectionsService.SaveConnectionsAsync();
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
                var importer = new PortScanImporter(protocol);
                importer.Import(hosts, importDestinationContainer);
                Runtime.ConnectionsService.SaveConnectionsAsync();
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