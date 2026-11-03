using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Import
{
    /// <summary>
    /// Imports a Royal TS / Royal TSX document into a new folder named after the document.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class RoyalTsImporter : IConnectionImporter<string>
    {
        // A .rtsz file is either the document's XML or a zip archive holding it under this name.
        private const string ZippedDocumentName = "Document.rtsz";

        public void Import(string fileName, ContainerInfo destinationContainer)
        {
            RoyalTsDocumentDeserializer deserializer = new();
            ConnectionTreeModel connectionTreeModel = deserializer.Deserialize(ReadDocument(fileName));

            foreach ((string objectType, int count) in deserializer.SkippedObjectTypes)
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    $"Royal TS import: {count} {objectType} object(s) skipped, mRemoteNG has no equivalent.");
            if (deserializer.PasswordsNotImported > 0)
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"Royal TS import: {deserializer.PasswordsNotImported} password(s) not imported. Royal TS stores them encrypted with the document's key.");

            ContainerInfo importContainer = new()
            {
                Name = deserializer.DocumentName.Length > 0 ? deserializer.DocumentName : Path.GetFileNameWithoutExtension(fileName)
            };
            importContainer.AddChildRange(connectionTreeModel.RootNodes[0].Children.ToArray());
            destinationContainer.AddChild(importContainer);
        }

        internal static string ReadDocument(string fileName)
        {
            if (!IsZipArchive(fileName))
                return File.ReadAllText(fileName);

            using ZipArchive archive = ZipFile.OpenRead(fileName);
            ZipArchiveEntry entry = archive.GetEntry(ZippedDocumentName) ?? archive.Entries.FirstOrDefault()
                                    ?? throw new FileFormatException("The Royal TS document archive is empty.");
            using StreamReader reader = new(entry.Open());
            return reader.ReadToEnd();
        }

        private static bool IsZipArchive(string fileName)
        {
            using FileStream stream = File.OpenRead(fileName);
            return stream.ReadByte() == 'P' && stream.ReadByte() == 'K';
        }
    }
}
