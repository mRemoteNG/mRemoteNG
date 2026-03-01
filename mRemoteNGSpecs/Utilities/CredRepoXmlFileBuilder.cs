namespace mRemoteNGSpecs.Utilities
{
    public class CredRepoXmlFileBuilder
    {
        public static string Build(string authHeader, int kdfIterations = 600_000)
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                   $"<Credentials EncryptionEngine=\"AES\" BlockCipherMode=\"GCM\" KdfIterations=\"{kdfIterations}\" Auth=\"{authHeader}\" SchemaVersion=\"1.0\">" +
                   "</Credentials>";
        }
    }
}