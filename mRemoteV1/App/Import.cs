using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;

namespace mRemoteNG.App
{
    public class Import
    {
        private static FileType DetermineFileType(string fileName)
        {
            // TODO: Use the file contents to determine the file type instead of trusting the extension
            var fileExtension = Convert.ToString(Path.GetExtension(fileName).ToLowerInvariant());
            switch (fileExtension)
            {
                case ".xml":
                    return FileType.mRemoteXml;
                case ".rdp":
                    return FileType.RemoteDesktopConnection;
                case ".rdg":
                    return FileType.RemoteDesktopConnectionManager;
                case ".dat":
                    return FileType.PuttyConnectionManager;
                default:
                    return FileType.Unknown;
            }
        }

        private enum FileType
        {
            Unknown = 0,
            // ReSharper disable once InconsistentNaming
            mRemoteXml,
            RemoteDesktopConnection,
            RemoteDesktopConnectionManager,
            PuttyConnectionManager
        }

        #region Public Methods

        public static void ImportFromFile(ContainerInfo importDestinationContainer,
            bool alwaysUseSelectedTreeNode = false)
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    openFileDialog.Multiselect = true;

                    var fileTypes = new List<string>();
                    fileTypes.AddRange(new[] {Language.strFilterAllImportable, "*.xml;*.rdp;*.rdg;*.dat"});
                    fileTypes.AddRange(new[] {Language.strFiltermRemoteXML, "*.xml"});
                    fileTypes.AddRange(new[] {Language.strFilterRDP, "*.rdp"});
                    fileTypes.AddRange(new[] {Language.strFilterRdgFiles, "*.rdg"});
                    fileTypes.AddRange(new[] {Language.strFilterPuttyConnectionManager, "*.dat"});
                    fileTypes.AddRange(new[] {Language.strFilterAll, "*.*"});

                    openFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    foreach (var fileName in openFileDialog.FileNames)
                        try
                        {
                            IConnectionImporter importer;
                            switch (DetermineFileType(fileName))
                            {
                                case FileType.mRemoteXml:
                                    importer = new mRemoteNGImporter();
                                    break;
                                case FileType.RemoteDesktopConnection:
                                    importer = new RemoteDesktopConnectionImporter();
                                    break;
                                case FileType.RemoteDesktopConnectionManager:
                                    importer = new RemoteDesktopConnectionManagerImporter();
                                    break;
                                case FileType.PuttyConnectionManager:
                                    importer = new PuttyConnectionManagerImporter();
                                    break;
                                default:
                                    throw new FileFormatException("Unrecognized file format.");
                            }
                            importer.Import(fileName, importDestinationContainer);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format(Language.strImportFileFailedContent, fileName),
                                Language.strImportFileFailedMainInstruction,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromFile() failed:1", ex,
                                logOnly: true);
                        }

                    Runtime.SaveConnectionsAsync();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromFile() failed:2", ex, logOnly: true);
            }
        }

        public static void ImportFromActiveDirectory(string ldapPath, ContainerInfo importDestinationContainer)
        {
            try
            {
                var importer = new ActiveDirectoryImporter();
                importer.Import(ldapPath, importDestinationContainer);
                Runtime.SaveConnectionsAsync();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromActiveDirectory() failed.", ex,
                    logOnly: true);
            }
        }

        public static void ImportFromPortScan(IEnumerable<ScanHost> hosts, ProtocolType protocol,
            ContainerInfo importDestinationContainer)
        {
            try
            {
                var importer = new PortScanImporter(protocol);
                importer.Import(hosts, importDestinationContainer);
                Runtime.SaveConnectionsAsync();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromPortScan() failed.", ex,
                    logOnly: true);
            }
        }

        #endregion
    }
}