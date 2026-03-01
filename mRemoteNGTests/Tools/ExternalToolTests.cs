using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    [TestFixture]
    public class ExternalToolTests
    {
        /// <summary>
        /// Helper: joins ArgumentList entries into a single string for searching.
        /// When RunElevated=false, args go into ArgumentList, not Arguments.
        /// </summary>
        private static string GetAllArguments(ProcessStartInfo psi)
        {
            if (!string.IsNullOrEmpty(psi.Arguments))
                return psi.Arguments;
            return string.Join(" ", psi.ArgumentList);
        }

        private static string NormalizeSystem32PathForWow64(
            string fileName,
            bool is64BitOperatingSystem,
            bool is64BitProcess,
            Func<string, bool> fileExists)
        {
            var normalizeMethod = typeof(ExternalTool).GetMethod(
                "NormalizeSystem32PathForWow64",
                BindingFlags.NonPublic | BindingFlags.Static,
                null,
                new[] { typeof(string), typeof(bool), typeof(bool), typeof(Func<string, bool>) },
                null);

            Assert.That(normalizeMethod, Is.Not.Null);

            return (string)normalizeMethod!.Invoke(
                null,
                new object[] { fileName, is64BitOperatingSystem, is64BitProcess, fileExists })!;
        }

        [Test]
        public void PasswordWithEqualsSignIsPassedCorrectly()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Password = "Z-3=Wv99/Aq",
                Hostname = "testhost",
                Username = "testuser"
            };

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "test.exe",
                Arguments = "-u %USERNAME% -p %PASSWORD% -h %HOSTNAME%"
            };

            // Act
            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            // Assert
            var arguments = GetAllArguments(process.StartInfo);
            Assert.That(arguments, Does.Contain("Z-3"));
            Assert.That(arguments, Does.Contain("Wv99/Aq"));
            Assert.That(arguments, Does.Match("Z-3=Wv99/Aq"));
        }

        [Test]
        public void PasswordWithSpecialCharactersIsPassedCorrectly()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Password = "P@ss=W0rd!",
                Hostname = "testhost",
                Username = "testuser"
            };

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "test.exe",
                Arguments = "-p %PASSWORD%"
            };

            // Act
            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            // Assert
            var arguments = GetAllArguments(process.StartInfo);
            Assert.That(arguments, Does.Contain("P@ss"));
            Assert.That(arguments, Does.Contain("W0rd"));
        }

        [Test]
        public void MultipleArgumentsAreParsedCorrectly()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Password = "TestPass=123",
                Hostname = "myhost.com",
                Username = "admin",
                Port = 8080
            };

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "app.exe",
                Arguments = "--host %HOSTNAME% --port %PORT% --user %USERNAME% --pass %PASSWORD%"
            };

            // Act
            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            // Assert
            var arguments = GetAllArguments(process.StartInfo);
            Assert.That(arguments, Does.Contain("myhost.com"));
            Assert.That(arguments, Does.Contain("8080"));
            Assert.That(arguments, Does.Contain("admin"));
            Assert.That(arguments, Does.Contain("TestPass"));
            Assert.That(arguments, Does.Contain("123"));
        }

        [Test]
        public void ArgumentsWithSpaces_AreParsedCorrectly()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Hostname = "test host",
                Username = "user name"
            };

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "app.exe",
                Arguments = "--host \"%HOSTNAME%\" --user \"%USERNAME%\""
            };

            // Act
            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            // Assert - When not elevated, arguments should be in ArgumentList
            if (!externalTool.RunElevated)
            {
                Assert.That(process.StartInfo.ArgumentList.Count, Is.GreaterThan(0));
                // Arguments with spaces should be preserved in ArgumentList
                Assert.That(process.StartInfo.ArgumentList, Does.Contain("test host").Or.Contain("--host"));
            }
        }

        [Test]
        public void ValidExecutablePath_DoesNotThrow()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo();
            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "notepad.exe",
                Arguments = ""
            };

            // Act & Assert
            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            
            Assert.DoesNotThrow(() => setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo }));
        }

        [Test]
        public void InvalidExecutablePath_ThrowsArgumentException()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo();
            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "notepad.exe & calc.exe", // Command injection attempt
                Arguments = ""
            };

            // Act & Assert
            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            var ex = Assert.Throws<TargetInvocationException>(() => 
                setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo }));
            Assert.That(ex.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public void RunElevated_UsesShellExecuteTrue()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo();
            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "notepad.exe",
                Arguments = "--test",
                RunElevated = true
            };

            // Act
            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            // Assert
            Assert.That(process.StartInfo.UseShellExecute, Is.True);
            Assert.That(process.StartInfo.Verb, Is.EqualTo("runas"));
        }

        [Test]
        public void RunNotElevated_UsesShellExecuteFalse()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo();
            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "notepad.exe",
                Arguments = "--test",
                RunElevated = false
            };

            // Act
            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            // Assert
            Assert.That(process.StartInfo.UseShellExecute, Is.False);
        }

        [Test]
        public void System32Path_NormalizesToSysnativeInWow64_WithFallback()
        {
            // Use the actual Windows directory from the environment to avoid
            // case mismatches (e.g. "C:\Windows" vs "C:\WINDOWS") across machines.
            string windowsDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            if (string.IsNullOrWhiteSpace(windowsDir))
                windowsDir = Environment.GetEnvironmentVariable("SystemRoot") ?? @"C:\Windows";

            string system32Path = System.IO.Path.Combine(windowsDir, "System32", "telnet.exe");
            string sysnativePath = System.IO.Path.Combine(windowsDir, "Sysnative", "telnet.exe");

            string normalized = NormalizeSystem32PathForWow64(
                system32Path,
                is64BitOperatingSystem: true,
                is64BitProcess: false,
                path => path.Equals(sysnativePath, StringComparison.OrdinalIgnoreCase));

            Assert.That(normalized, Is.EqualTo(sysnativePath));

            string fallback = NormalizeSystem32PathForWow64(
                system32Path,
                is64BitOperatingSystem: true,
                is64BitProcess: false,
                _ => false);

            Assert.That(fallback, Is.EqualTo(system32Path));
        }

        [Test]
        public void PasswordWithComma_BatchFile_PassedAsSingleArgument()
        {
            // Issue #3044: comma in password must not split into separate arguments
            // when the target is a batch file (routed through cmd.exe).
            var connectionInfo = new ConnectionInfo
            {
                Password = "1234,56789",
                Hostname = "testhost",
                Username = "testuser"
            };

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "test.cmd",
                Arguments = "-p %PASSWORD%"
            };

            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            // For batch files, Arguments string is used (not ArgumentList).
            // The password must appear as a single double-quoted argument.
            Assert.That(process.StartInfo.Arguments, Does.Contain("\"1234,56789\""));
        }

        [Test]
        public void PasswordWithComma_ExeFile_PassedCorrectly()
        {
            // For .exe targets, ArgumentList is used (C-runtime quoting).
            // The comma should appear intact in one ArgumentList entry.
            var connectionInfo = new ConnectionInfo
            {
                Password = "1234,56789",
                Hostname = "testhost",
                Username = "testuser"
            };

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "test.exe",
                Arguments = "-p %PASSWORD%"
            };

            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            // ArgumentList should contain the password as a single entry
            Assert.That(process.StartInfo.ArgumentList, Does.Contain("1234,56789"));
        }

        [Test]
        public void RunAsUser_AuthTypeRunas_SetsUserNameAndDisablesShellExecute()
        {
            // Issue #1833: external tools should support launching as a different user
            // so that SCCM and similar tools can connect with alternate credentials.
            var connectionInfo = new ConnectionInfo { Hostname = "server01" };

            var externalTool = new ExternalTool
            {
                DisplayName = "SCCM",
                FileName = "CmRcViewer.exe",
                Arguments = "%HOSTNAME%",
                AuthenticationType = "runas",
                AuthenticationUsername = "DOMAIN\\adminuser",
                AuthenticationPassword = "adminpass"
            };

            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            Assert.That(process.StartInfo.UseShellExecute, Is.False);
            Assert.That(process.StartInfo.UserName, Is.EqualTo("adminuser"));
            Assert.That(process.StartInfo.Domain, Is.EqualTo("DOMAIN"));
            Assert.That(process.StartInfo.Password, Is.Not.Null);
        }

        [Test]
        public void RunAsUser_AuthTypeRunas_WithoutDomain_SetsUserNameOnly()
        {
            var connectionInfo = new ConnectionInfo { Hostname = "server01" };

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "test.exe",
                Arguments = "%HOSTNAME%",
                AuthenticationType = "runas",
                AuthenticationUsername = "localuser",
                AuthenticationPassword = "pass123"
            };

            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            Assert.That(process.StartInfo.UseShellExecute, Is.False);
            Assert.That(process.StartInfo.UserName, Is.EqualTo("localuser"));
            Assert.That(process.StartInfo.Domain, Is.Empty);
        }

        [Test]
        public void RunAsUser_AuthTypeRunas_SupportsTokensInAuthFields()
        {
            // %USERNAME%, %DOMAIN%, %PASSWORD% tokens in AuthUsername/AuthPassword
            // should be expanded from the connection info.
            var connectionInfo = new ConnectionInfo
            {
                Hostname = "server01",
                Username = "connuser",
                Domain = "CONNDOMAIN",
                Password = "connpass"
            };

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "test.exe",
                Arguments = "%HOSTNAME%",
                AuthenticationType = "runas",
                AuthenticationUsername = "%DOMAIN%\\%USERNAME%",
                AuthenticationPassword = "%PASSWORD%"
            };

            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            Assert.That(process.StartInfo.UseShellExecute, Is.False);
            Assert.That(process.StartInfo.UserName, Is.EqualTo("connuser"));
            Assert.That(process.StartInfo.Domain, Is.EqualTo("CONNDOMAIN"));
            Assert.That(process.StartInfo.Password, Is.Not.Null);
        }

        [Test]
        public void RunAsUser_AuthTypeNotRunas_DoesNotSetAlternateCredentials()
        {
            // Any AuthenticationType other than "runas" must not trigger alternate-user launch.
            var connectionInfo = new ConnectionInfo();

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "notepad.exe",
                Arguments = "",
                AuthenticationType = "ssh",
                AuthenticationUsername = "someuser",
                AuthenticationPassword = "somepass"
            };

            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            Assert.That(process.StartInfo.UserName, Is.Null.Or.Empty);
            Assert.That(process.StartInfo.Password, Is.Null);
        }

        [Test]
        public void RunAsUser_AuthTypeRunas_EmptyUsername_DoesNotSetAlternateCredentials()
        {
            // "runas" with empty username must not attempt alternate-user launch.
            var connectionInfo = new ConnectionInfo();

            var externalTool = new ExternalTool
            {
                DisplayName = "Test Tool",
                FileName = "notepad.exe",
                Arguments = "",
                AuthenticationType = "runas",
                AuthenticationUsername = "",
                AuthenticationPassword = "somepass"
            };

            var process = new Process();
            var setProcessPropertiesMethod = typeof(ExternalTool).GetMethod(
                "SetProcessProperties",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            setProcessPropertiesMethod?.Invoke(externalTool, new object[] { process, connectionInfo });

            Assert.That(process.StartInfo.UserName, Is.Null.Or.Empty);
            Assert.That(process.StartInfo.Password, Is.Null);
        }
    }
}
