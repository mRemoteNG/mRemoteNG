using System;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.Config.Import;
using mRemoteNG.Container;
using mRemoteNG.Messages;

namespace mRemoteNG.Connection
{
    public class TextListConnectionImporter : IConnectionImporter<string>
    {
        public void Import(string fileName, ContainerInfo destinationContainer)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            if (!File.Exists(fileName))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Import file not found: {fileName}");
                return;
            }

            try
            {
                string content = File.ReadAllText(fileName);
                TextImporter textImporter = new();
                textImporter.Import(content, destinationContainer);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("TextListConnectionImporter.Import failed.", ex);
            }
        }
    }
}
