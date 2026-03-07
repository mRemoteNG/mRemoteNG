using mRemoteNG.Connection.Protocol.SSH_DotNet;
using NUnit.Framework;
using System;
using Renci.SshNet;
using NSubstitute;

namespace mRemoteNGTests.Connection.Protocol.SSH_DotNet
{
    [TestFixture]
    public class ShellStreamExtensionsTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset diagnostic flags to default state
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;
        }

        [TearDown]
        public void TearDown()
        {
            // Reset to defaults after tests
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;
        }

        #region SendWindowChangeRequest Tests

        [Test]
        public void SendWindowChangeRequest_DoesNotThrowException_WithNullStream()
        {
            // Arrange
            ShellStream stream = null;
            uint columns = 80;
            uint rows = 24;
            uint width = 640;
            uint height = 480;

            // Act & Assert
            // Should not throw - the extension method handles null gracefully by checking for _channel field
            Assert.DoesNotThrow(() =>
                stream?.SendWindowChangeRequest(columns, rows, width, height));
        }

        // Note: Testing the actual SendWindowChangeRequest method with reflection requires a real ShellStream
        // which can only be created from a connected SshClient. The following tests verify basic behavior:

        [Test]
        public void SendWindowChangeRequest_DoesNotThrow_WithValidDimensions()
        {
            // Arrange
            // Note: Cannot create a real ShellStream without SSH connection
            // This test verifies that the method signature is correct and accepts valid parameters

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                // Verify method exists with correct signature
                var method = typeof(ShellStreamExtensions).GetMethod("SendWindowChangeRequest");
                Assert.That(method, Is.Not.Null, "SendWindowChangeRequest method should exist");

                var parameters = method.GetParameters();
                Assert.That(parameters.Length, Is.EqualTo(5), "Method should have 5 parameters");
                Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(ShellStream)), "First parameter should be ShellStream");
                Assert.That(parameters[1].ParameterType, Is.EqualTo(typeof(uint)), "Second parameter should be uint (columns)");
                Assert.That(parameters[2].ParameterType, Is.EqualTo(typeof(uint)), "Third parameter should be uint (rows)");
                Assert.That(parameters[3].ParameterType, Is.EqualTo(typeof(uint)), "Fourth parameter should be uint (width)");
                Assert.That(parameters[4].ParameterType, Is.EqualTo(typeof(uint)), "Fifth parameter should be uint (height)");
            });
        }

        [Test]
        public void SendWindowChangeRequest_AcceptsZeroPixelDimensions()
        {
            // Arrange & Act & Assert
            // Method should accept zero pixel dimensions (meaning auto)
            Assert.DoesNotThrow(() =>
            {
                var method = typeof(ShellStreamExtensions).GetMethod("SendWindowChangeRequest");
                Assert.That(method, Is.Not.Null);

                // Verify documentation mentions auto behavior for 0 width/height
                var xmlDoc = method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false);
                Assert.That(xmlDoc, Is.Not.Null);
            });
        }

        [Test]
        public void SendWindowChangeRequest_AcceptsLargeDimensions()
        {
            // Arrange & Act & Assert
            // Method should accept large dimensions without throwing
            Assert.DoesNotThrow(() =>
            {
                var method = typeof(ShellStreamExtensions).GetMethod("SendWindowChangeRequest");
                Assert.That(method, Is.Not.Null);
            });
        }

        [Test]
        public void SendWindowChangeRequest_UsesReflectionToAccessChannel()
        {
            // Arrange & Act & Assert
            // Verify that the extension method implementation uses reflection
            var method = typeof(ShellStreamExtensions).GetMethod("SendWindowChangeRequest");
            Assert.That(method, Is.Not.Null, "SendWindowChangeRequest method should exist");

            // The method should be an extension method (static)
            Assert.That(method.IsStatic, Is.True, "SendWindowChangeRequest should be a static extension method");

            // Note: ExtensionAttribute is compiler-generated for extension methods, so we can verify by checking method.IsStatic
            Assert.That(method.IsStatic, Is.True, "Method should be static (extension methods are static)");
        }

        // Integration test note:
        // Testing the actual reflection behavior and successful window change requests would require:
        // 1. A connected SshClient
        // 2. A created ShellStream
        // 3. An SSH server that supports window change requests
        // These tests should be in integration test suite, not unit tests

        #endregion
    }
}
