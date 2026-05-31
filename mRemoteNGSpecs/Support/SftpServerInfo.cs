namespace mRemoteNGSpecs.Support
{
    /// <summary>
    /// Connection details for the disposable SFTP server defined in
    /// <c>docker-compose.sftp.yml</c>. Kept in one place so the fixture and
    /// the step definitions cannot drift apart.
    /// </summary>
    public static class SftpServerInfo
    {
        public const string Host = "127.0.0.1";
        public const int Port = 2222;
        public const string User = "testuser";
        public const string Password = "testpass";

        /// <summary>Writable directory created by the atmoz/sftp container.</summary>
        public const string WritableRoot = "/upload";

        public const string ComposeProjectName = "mremoteng-e2e";
        public const string ComposeFileName = "docker-compose.sftp.yml";
    }
}
