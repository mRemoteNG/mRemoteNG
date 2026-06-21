using System.Linq;
using mRemoteNG.Connection.Protocol.SshDotNet;
using mRemoteNG.Messages;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol.SshDotNet
{
    [TestFixture]
    [Category("Unit")]
    public class SshDotNetDiagnosticsTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset flags to default state
            SshDotNetDiagnostics.VerboseLogging = false;
            SshDotNetDiagnostics.TraceLogging = false;
            SshDotNetDiagnostics.LogRawData = false;
            SshDotNetDiagnostics.LogEscapeSequences = false;
            SshDotNetDiagnostics.LogPerformanceMetrics = false;

            // Reset UI settings to default (false)
            mRemoteNG.Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs = false;
        }

        [TearDown]
        public void TearDown()
        {
            // Reset to defaults after tests
            SshDotNetDiagnostics.VerboseLogging = false;
            SshDotNetDiagnostics.TraceLogging = false;
            SshDotNetDiagnostics.LogRawData = false;
            SshDotNetDiagnostics.LogEscapeSequences = false;
            SshDotNetDiagnostics.LogPerformanceMetrics = false;

            mRemoteNG.Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs = false;
        }

        [Test]
        public void LogTrace_GeneratesMessage_WhenTraceLoggingEnabled()
        {
            // Arrange
            SshDotNetDiagnostics.TraceLogging = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SshDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Trace message should be generated when TraceLogging flag is true");
        }

        [Test]
        public void LogTrace_GeneratesMessage_WhenUiNotificationPanelEnabled()
        {
            // Arrange
            SshDotNetDiagnostics.TraceLogging = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SshDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Trace message should be generated when UI notification panel setting is enabled");
        }

        [Test]
        public void LogTrace_GeneratesMessage_WhenUiTextLogEnabled()
        {
            // Arrange
            SshDotNetDiagnostics.TraceLogging = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SshDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Trace message should be generated when UI text log setting is enabled");
        }

        [Test]
        public void LogTrace_GeneratesMessage_WhenUiPopupEnabled()
        {
            // Arrange
            SshDotNetDiagnostics.TraceLogging = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SshDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Trace message should be generated when UI popup setting is enabled");
        }

        [Test]
        public void LogTrace_DoesNotGenerateMessage_WhenAllDisabled()
        {
            // Arrange
            SshDotNetDiagnostics.TraceLogging = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs = false;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SshDotNetDiagnostics.LogTrace("Test trace message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.EqualTo(initialMessageCount),
                "Trace message should NOT be generated when all flags and UI settings are disabled");
        }

        [Test]
        public void LogDebug_RequiresVerboseLogging()
        {
            // Arrange
            SshDotNetDiagnostics.VerboseLogging = false;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SshDotNetDiagnostics.LogDebug("Test debug message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.EqualTo(initialMessageCount),
                "Debug message should NOT be generated when VerboseLogging is false");
        }

        [Test]
        public void LogInfo_AlwaysGenerates()
        {
            // Arrange
            SshDotNetDiagnostics.VerboseLogging = false;
            SshDotNetDiagnostics.TraceLogging = false;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SshDotNetDiagnostics.LogInfo("Test info message");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Info message should always be generated regardless of flags");
        }

        [Test]
        public void LogRawDataBinary_RequiresLogRawDataFlag()
        {
            // Arrange
            SshDotNetDiagnostics.LogRawData = false;
            SshDotNetDiagnostics.TraceLogging = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();
            byte[] data = new byte[] { 0x01, 0x02, 0x03 };

            // Act
            SshDotNetDiagnostics.LogRawDataBinary(data, data.Length, "Test context");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.EqualTo(initialMessageCount),
                "Raw data should NOT be logged when LogRawData flag is false");
        }

        [Test]
        public void LogRawDataBinary_GeneratesMessage_WhenLogRawDataAndTraceFlagsEnabled()
        {
            // Arrange
            SshDotNetDiagnostics.LogRawData = true;
            SshDotNetDiagnostics.TraceLogging = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();
            byte[] data = new byte[] { 0x01, 0x02, 0x03 };

            // Act
            SshDotNetDiagnostics.LogRawDataBinary(data, data.Length, "Test context");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Raw data should be logged when both LogRawData and TraceLogging flags are true");
        }

        [Test]
        public void LogEscapeSequence_GeneratesMessage_WhenLogEscapeSequencesEnabled()
        {
            // Arrange
            SshDotNetDiagnostics.LogEscapeSequences = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SshDotNetDiagnostics.LogEscapeSequence("\\x1b[H", "Move cursor to home");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Escape sequence should be logged when LogEscapeSequences flag is true");
        }

        [Test]
        public void LogEscapeSequence_GeneratesMessage_WhenUiSettingsEnabled()
        {
            // Arrange
            SshDotNetDiagnostics.LogEscapeSequences = false;
            mRemoteNG.Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs = true;
            int initialMessageCount = mRemoteNG.App.Runtime.MessageCollector.Messages.Count();

            // Act
            SshDotNetDiagnostics.LogEscapeSequence("\\x1b[H", "Move cursor to home");

            // Assert
            Assert.That(mRemoteNG.App.Runtime.MessageCollector.Messages.Count(), Is.GreaterThan(initialMessageCount),
                "Escape sequence should be logged when UI trace settings are enabled");
        }
    }
}
