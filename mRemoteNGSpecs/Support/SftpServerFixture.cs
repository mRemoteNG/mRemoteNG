using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Renci.SshNet;
using Reqnroll;

namespace mRemoteNGSpecs.Support
{
    /// <summary>
    /// Starts a disposable atmoz/sftp Docker container before the @sftp feature
    /// runs and tears it down afterwards. If Docker is unavailable the whole
    /// feature is ignored rather than failed, so contributors without Docker can
    /// still build and run the rest of the suite.
    /// </summary>
    [Binding]
    public static class SftpServerFixture
    {
        private static string ComposeFilePath =>
            Path.Combine(AppContext.BaseDirectory, SftpServerInfo.ComposeFileName);

        [BeforeFeature("sftp")]
        public static void StartSftpServer()
        {
            if (!DockerIsAvailable())
                Assert.Ignore("Docker is not available - skipping @sftp end-to-end scenarios.");

            if (!File.Exists(ComposeFilePath))
                Assert.Ignore($"Compose file not found next to the test assembly: {ComposeFilePath}");

            var (exitCode, output) = RunDocker($"compose -p {SftpServerInfo.ComposeProjectName} " +
                                               $"-f \"{ComposeFilePath}\" up -d", TimeSpan.FromMinutes(3));
            if (exitCode != 0)
                Assert.Ignore($"Could not start the SFTP container (exit {exitCode}):{Environment.NewLine}{output}");

            WaitUntilSftpAccepts(TimeSpan.FromSeconds(45));
        }

        [AfterFeature("sftp")]
        public static void StopSftpServer()
        {
            if (!DockerIsAvailable())
                return;

            RunDocker($"compose -p {SftpServerInfo.ComposeProjectName} " +
                      $"-f \"{ComposeFilePath}\" down -v", TimeSpan.FromMinutes(2));
        }

        private static bool DockerIsAvailable()
        {
            try
            {
                var (exitCode, _) = RunDocker("version --format \"{{.Server.Version}}\"", TimeSpan.FromSeconds(20));
                return exitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private static void WaitUntilSftpAccepts(TimeSpan timeout)
        {
            var deadline = DateTime.UtcNow + timeout;
            Exception last = null;

            while (DateTime.UtcNow < deadline)
            {
                try
                {
                    using var probe = new SftpClient(
                        SftpServerInfo.Host, SftpServerInfo.Port,
                        SftpServerInfo.User, SftpServerInfo.Password);
                    probe.ConnectionInfo.Timeout = TimeSpan.FromSeconds(5);
                    probe.Connect();
                    if (probe.IsConnected)
                    {
                        probe.Disconnect();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    last = ex;
                }

                Thread.Sleep(1000);
            }

            Assert.Ignore($"SFTP server did not become reachable within {timeout.TotalSeconds:F0}s. " +
                          $"Last error: {last?.Message}");
        }

        private static (int ExitCode, string Output) RunDocker(string arguments, TimeSpan timeout)
        {
            var psi = new ProcessStartInfo("docker", arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using var process = Process.Start(psi)
                                ?? throw new InvalidOperationException("Failed to start the docker process.");

            string stdout = process.StandardOutput.ReadToEnd();
            string stderr = process.StandardError.ReadToEnd();

            if (!process.WaitForExit((int)timeout.TotalMilliseconds))
            {
                try { process.Kill(true); } catch { /* best effort */ }
                throw new TimeoutException($"docker {arguments} timed out after {timeout.TotalSeconds:F0}s.");
            }

            return (process.ExitCode, (stdout + stderr).Trim());
        }
    }
}
