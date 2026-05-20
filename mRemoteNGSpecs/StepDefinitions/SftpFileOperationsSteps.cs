using System.IO;
using System.Linq;
using System.Threading.Tasks;
using mRemoteNG.Tools;
using mRemoteNGSpecs.Support;
using NUnit.Framework;
using Reqnroll;

namespace mRemoteNGSpecs.StepDefinitions
{
    /// <summary>
    /// Drives <see cref="SftpFileService"/> against the live atmoz/sftp container
    /// started by <see cref="SftpServerFixture"/>.
    /// </summary>
    [Binding]
    public class SftpFileOperationsSteps
    {
        private readonly SftpWorld _world;
        private string _stagedLocalFile;

        public SftpFileOperationsSteps(SftpWorld world) => _world = world;

        private SftpFileService Service => _world.Service;

        private static string JoinRemote(string directory, string name) =>
            directory.TrimEnd('/') + "/" + name;

        // --- Background -----------------------------------------------------

        [Given("a running SFTP server")]
        public void GivenARunningSftpServer()
        {
            // The container lifecycle is owned by SftpServerFixture's
            // BeforeFeature/AfterFeature hooks; this step documents the
            // precondition so the scenario reads as a complete story.
        }

        [Given("I connect the SFTP file service")]
        public async Task GivenIConnectTheSftpFileService()
        {
            Service.Connect(SftpServerInfo.Host, SftpServerInfo.User,
                            SftpServerInfo.Password, SftpServerInfo.Port);
            Assert.That(Service.IsConnected, Is.True, "SFTP service failed to connect.");
            await CleanDirectoryAsync(SftpServerInfo.WritableRoot);
        }

        // --- Connection assertions -----------------------------------------

        [Then("the SFTP service reports it is connected")]
        public void ThenTheServiceIsConnected() =>
            Assert.That(Service.IsConnected, Is.True);

        [Then("the home path is known")]
        public void ThenTheHomePathIsKnown() =>
            Assert.That(Service.HomePath, Is.Not.Null.And.Not.Empty);

        // --- Arrange remote/local state ------------------------------------

        [Given("a remote file {string} containing {string} exists in {string}")]
        public async Task GivenARemoteFileExists(string name, string content, string directory)
        {
            string local = Path.Combine(_world.LocalTempDir, "seed-" + name);
            await File.WriteAllTextAsync(local, content);
            await Service.UploadFileAsync(local, JoinRemote(directory, name));
        }

        [Given("a local file {string} containing {string}")]
        public async Task GivenALocalFile(string name, string content)
        {
            _stagedLocalFile = Path.Combine(_world.LocalTempDir, name);
            await File.WriteAllTextAsync(_stagedLocalFile, content);
        }

        [Given("a remote directory {string} exists")]
        public async Task GivenARemoteDirectoryExists(string path) =>
            await Service.CreateDirectoryAsync(path);

        // --- Actions --------------------------------------------------------

        [When("I list the directory {string}")]
        public async Task WhenIListTheDirectory(string path) =>
            _world.LastListing = await Service.ListDirectoryAsync(path);

        [When("I upload it to {string}")]
        public async Task WhenIUploadItTo(string remotePath) =>
            await Service.UploadFileAsync(_stagedLocalFile, remotePath);

        [When("I download {string}")]
        public async Task WhenIDownload(string remotePath)
        {
            string local = Path.Combine(_world.LocalTempDir, "dl-" + Path.GetFileName(remotePath));
            await Service.DownloadFileAsync(remotePath, local);
            _world.LastDownloadedFile = local;
        }

        [When("I create the directory {string}")]
        public async Task WhenICreateTheDirectory(string path) =>
            await Service.CreateDirectoryAsync(path);

        [When("I rename {string} to {string}")]
        public async Task WhenIRename(string oldPath, string newPath) =>
            await Service.RenameAsync(oldPath, newPath);

        [When("I delete the file {string}")]
        public async Task WhenIDeleteTheFile(string path) =>
            await Service.DeleteAsync(path, isDirectory: false);

        [When("I delete the directory {string}")]
        public async Task WhenIDeleteTheDirectory(string path) =>
            await Service.DeleteAsync(path, isDirectory: true);

        // --- Assertions -----------------------------------------------------

        [Then("the listing contains a file named {string}")]
        public void ThenTheLastListingContainsFile(string name) =>
            Assert.That(_world.LastListing.Any(i => i.Name == name && !i.IsDirectory),
                        Is.True, $"Expected a file named '{name}' in the last listing.");

        [Then("the listing of {string} contains a file named {string}")]
        public async Task ThenListingContainsFile(string directory, string name)
        {
            var items = await Service.ListDirectoryAsync(directory);
            Assert.That(items.Any(i => i.Name == name && !i.IsDirectory),
                        Is.True, $"Expected a file named '{name}' in '{directory}'.");
        }

        [Then("the listing of {string} contains a directory named {string}")]
        public async Task ThenListingContainsDirectory(string directory, string name)
        {
            var items = await Service.ListDirectoryAsync(directory);
            Assert.That(items.Any(i => i.Name == name && i.IsDirectory),
                        Is.True, $"Expected a directory named '{name}' in '{directory}'.");
        }

        [Then("the listing of {string} does not contain a file named {string}")]
        public async Task ThenListingDoesNotContainFile(string directory, string name)
        {
            var items = await Service.ListDirectoryAsync(directory);
            Assert.That(items.Any(i => i.Name == name && !i.IsDirectory),
                        Is.False, $"Did not expect a file named '{name}' in '{directory}'.");
        }

        [Then("the listing of {string} does not contain a directory named {string}")]
        public async Task ThenListingDoesNotContainDirectory(string directory, string name)
        {
            var items = await Service.ListDirectoryAsync(directory);
            Assert.That(items.Any(i => i.Name == name && i.IsDirectory),
                        Is.False, $"Did not expect a directory named '{name}' in '{directory}'.");
        }

        [Then("the downloaded file contains {string}")]
        public async Task ThenTheDownloadedFileContains(string expected)
        {
            Assert.That(_world.LastDownloadedFile, Is.Not.Null, "Nothing was downloaded.");
            string actual = await File.ReadAllTextAsync(_world.LastDownloadedFile);
            Assert.That(actual, Is.EqualTo(expected));
        }

        // --- Helpers --------------------------------------------------------

        /// <summary>Recursively empties a remote directory so scenarios start clean.</summary>
        private async Task CleanDirectoryAsync(string directory)
        {
            var items = await Service.ListDirectoryAsync(directory);
            foreach (var item in items)
            {
                if (item.Name is "." or "..")
                    continue;

                string full = JoinRemote(directory, item.Name);
                if (item.IsDirectory)
                {
                    await CleanDirectoryAsync(full);
                    await Service.DeleteAsync(full, isDirectory: true);
                }
                else
                {
                    await Service.DeleteAsync(full, isDirectory: false);
                }
            }
        }
    }
}
