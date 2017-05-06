namespace mRemoteNG.Specs.Utilities
{
    public class CredRepoXmlFileBuilder
    {
        public string Build(string authHeader)
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    $"<Credentials EncryptionEngine=\"AES\" BlockCipherMode=\"GCM\" KdfIterations=\"1000\" Auth=\"{authHeader}\" SchemaVersion=\"1.0\">" +
                    "</Credentials>";
        }
    }
}