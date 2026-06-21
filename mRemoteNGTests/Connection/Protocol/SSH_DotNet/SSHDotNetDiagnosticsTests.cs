using System.Linq;
using mRemoteNG.Connection.Protocol.SSH_DotNet;
using mRemoteNG.Messages;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol.SSH_DotNet
{
    [TestFixture]
    [Category("Unit")]
    public class SSHDotNetDiagnosticsTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset flags to default state
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;
            SSHDotNetDiagnostics.LogRawData = false;
            SSHDotNetDiagnostics.LogEscapeSequences = false;
            SSHDotNetDiagnostics.LogPerformanceMetrics = false;

            // Reset UI settings to default (false)
            mRemoteNG.Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs = false;
        }

        [TearDown]
        public void TearDown()
        {
            // Reset to defaults after tests
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;
            SSHDotNetDiagnostics.LogRawData = false;
            SSHDotNetDiagnostics.LogEscapeSequences = false;
            SSHDotNetDiagnostics.LogPerformanceMetrics = false;

            mRemoteNG.Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs = false;
        }

        [Test]
        public void LogTrace_GeneratesMessage_WhenTraceLoggingEnabled()
        {
            // Arrange
            SSHDotNetDiagnostics.TraceLogging = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SSHDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Trace message should be generated when TraceLogging flag is true");
        }

        [Test]
        public void LogTrace_GeneratesMessage_WhenUiNotificationPanelEnabled()
        {
            // Arrange
            SSHDotNetDiagnostics.TraceLogging = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SSHDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Trace message should be generated when UI notification panel setting is enabled");
        }

        [Test]
        public void LogTrace_GeneratesMessage_WhenUiTextLogEnabled()
        {
            // Arrange
            SSHDotNetDiagnostics.TraceLogging = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SSHDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Trace message should be generated when UI text log setting is enabled");
        }

        [Test]
        public void LogTrace_GeneratesMessage_WhenUiPopupEnabled()
        {
            // Arrange
            SSHDotNetDiagnostics.TraceLogging = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SSHDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Trace message should be generated when UI popup setting is enabled");
        }

        [Test]
        public void LogTrace_DoesNotGenerateMessage_WhenAllDisabled()
        {
            // Arrange
            SSHDotNetDiagnostics.TraceLogging = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs = false;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SSHDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.EqualTo(initialMessageCount),
                "Trace message should NOT be generated when all flags and UI settings are disabled");
        }

        [Test]
        public void LogDebug_RequiresVerboseLogging()
        {
            // Arrange
            SSHDotNetDiagnostics.VerboseLogging = false;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SSHDotNetDiagnostics.LogDebug("Test debug message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.EqualTo(initialMessageCount),
                "Debug message should NOT be generated when VerboseLogging is false");
        }

        [Test]
        public void LogInfo_AlwaysGenerates()
        {
            // Arrange
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SSHDotNetDiagnostics.LogInfo("Test info message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Info message should always be generated regardless of flags");
        }

        [Test]
        public void LogRawDataBinary_RequiresLogRawDataFlag()
        {
            // Arrange
            SSHDotNetDiagnostics.LogRawData = false;
            SSHDotNetDiagnostics.TraceLogging = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();
            byte[] data = new byte[] { 0x01, 0x02, 0x03 };

            // Act
            SSHDotNetDiagnostics.LogRawDataBinary(data, data.Length, "Test context");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.EqualTo(initialMessageCount),
                "Raw data should NOT be logged when LogRawData flag is false");
        }

        [Test]
        public void LogRawDataBinary_GeneratesMessage_WhenLogRawDataAndTraceFlagsEnabled()
        {
            // Arrange
            SSHDotNetDiagnostics.LogRawData = true;
            SSHDotNetDiagnostics.TraceLogging = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();
            byte[] data = new byte[] { 0x01, 0x02, 0x03 };

            // Act
            SSHDotNetDiagnostics.LogRawDataBinary(data, data.Length, "Test context");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Raw data should be logged when both LogRawData and TraceLogging flags are true");
        }

        [Test]
        public void LogEscapeSequence_GeneratesMessage_WhenLogEscapeSequencesEnabled()
        {
            // Arrange
            SSHDotNetDiagnostics.LogEscapeSequences = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SSHDotNetDiagnostics.LogEscapeSequence("\\x1b[H", "Move cursor to home");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Escape sequence should be logged when LogEscapeSequences flag is true");
        }

        [Test]
        public void LogEscapeSequence_GeneratesMessage_WhenUiSettingsEnabled()
        {
            // Arrange
            SSHDotNetDiagnostics.LogEscapeSequences = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SSHDotNetDiagnostics.LogEscapeSequence("\\x1b[H", "Move cursor to home");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Escape sequence should be logged when UI trace settings are enabled");
        }
    }
}
