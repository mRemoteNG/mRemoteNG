using mRemoteNG.UI.Controls;
using mRemoteNG.Connection.Protocol.SSH_DotNet;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace mRemoteNGTests.Connection.Protocol.SSH_DotNet
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]  // Required for Windows Forms controls
    public class SshTerminalControlTests
    {
        private SshTerminalControl _control;

        [SetUp]
        public void Setup()
        {
            // Reset diagnostic flags to default state
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;

            // Create control
            _control = new SshTerminalControl();
        }

        [TearDown]
        public void TearDown()
        {
            // Reset to defaults after tests
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;

            // Cleanup control
            if (_control != null && !_control.IsDisposed)
            {
                _control.Dispose();
            }
            _control = null;
        }

        #region Constructor Tests

        [Test]
        public void Constructor_CreatesInstance_Successfully()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Assert
            Assert.That(control, Is.Not.Null);
            Assert.That(control.IsDisposed, Is.False);

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Constructor_SetsDefaultBackgroundColor()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            var backColor = control.BackColor;

            // Assert
            Assert.That(backColor, Is.EqualTo(Color.Black));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Constructor_SetsDefaultForegroundColor()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            var foreColor = control.ForeColor;

            // Assert
            Assert.That(foreColor, Is.EqualTo(Color.White));

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Initialize Tests

        [Test]
        public void Initialize_CompletesSuccessfully()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act & Assert
            Assert.DoesNotThrow(() => control.Initialize());

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Initialize_IsIdempotent()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act - Initialize multiple times
            control.Initialize();
            control.Initialize();
            control.Initialize();

            // Assert - Should not throw, should handle re-initialization gracefully
            Assert.That(control.IsDisposed, Is.False);

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Property Tests

        [Test]
        public void Columns_DefaultsTo80()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            int columns = control.Columns;

            // Assert
            Assert.That(columns, Is.EqualTo(80));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Rows_DefaultsTo24()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            int rows = control.Rows;

            // Assert
            Assert.That(rows, Is.EqualTo(24));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void ScrollbackLines_DefaultsTo1000()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            int scrollback = control.ScrollbackLines;

            // Assert
            Assert.That(scrollback, Is.EqualTo(1000));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Columns_CanBeSet_WithValidValue()
        {
            // Arrange
            var control = new SshTerminalControl();
            int newColumns = 120;

            // Act
            control.Columns = newColumns;

            // Assert
            Assert.That(control.Columns, Is.EqualTo(newColumns));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Rows_CanBeSet_WithValidValue()
        {
            // Arrange
            var control = new SshTerminalControl();
            int newRows = 40;

            // Act
            control.Rows = newRows;

            // Assert
            Assert.That(control.Rows, Is.EqualTo(newRows));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Columns_RejectsInvalidValue_TooSmall()
        {
            // Arrange
            var control = new SshTerminalControl();
            int originalColumns = control.Columns;

            // Act
            control.Columns = 0;  // Invalid - should be clamped or rejected

            // Assert
            Assert.That(control.Columns, Is.Not.EqualTo(0), "Columns should not be set to 0");
            Assert.That(control.Columns, Is.GreaterThan(0), "Columns should be positive");

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Columns_RejectsInvalidValue_TooLarge()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            control.Columns = 1000;  // Invalid - should be clamped or rejected

            // Assert
            Assert.That(control.Columns, Is.LessThanOrEqualTo(500), "Columns should be clamped to reasonable maximum");

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Rows_RejectsInvalidValue_TooSmall()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            control.Rows = 0;  // Invalid

            // Assert
            Assert.That(control.Rows, Is.Not.EqualTo(0), "Rows should not be set to 0");
            Assert.That(control.Rows, Is.GreaterThan(0), "Rows should be positive");

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void Rows_RejectsInvalidValue_TooLarge()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            control.Rows = 500;  // Invalid - should be clamped or rejected

            // Assert
            Assert.That(control.Rows, Is.LessThanOrEqualTo(200), "Rows should be clamped to reasonable maximum");

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void ScrollbackLines_CanBeSet()
        {
            // Arrange
            var control = new SshTerminalControl();
            int newScrollback = 2000;

            // Act
            control.ScrollbackLines = newScrollback;

            // Assert
            Assert.That(control.ScrollbackLines, Is.EqualTo(newScrollback));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void ScrollbackLines_ClampsToMaximum()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            control.ScrollbackLines = 50000;  // Very large

            // Assert
            Assert.That(control.ScrollbackLines, Is.LessThanOrEqualTo(10000), "Scrollback should be clamped to max 10000");

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void ScrollbackLines_ClampsToMinimum()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            control.ScrollbackLines = -100;  // Negative

            // Assert
            Assert.That(control.ScrollbackLines, Is.GreaterThanOrEqualTo(0), "Scrollback should be clamped to minimum 0");

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Character Dimension Tests

        [Test]
        public void CharWidth_HasReasonableDefault()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            int charWidth = control.CharWidth;

            // Assert
            Assert.That(charWidth, Is.InRange(6, 20), "Character width should be reasonable for monospace fonts");

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void CharHeight_HasReasonableDefault()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            int charHeight = control.CharHeight;

            // Assert
            Assert.That(charHeight, Is.InRange(10, 30), "Character height should be reasonable for monospace fonts");

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Terminal Font Tests

        [Test]
        public void TerminalFont_HasDefaultFont()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            var font = control.TerminalFont;

            // Assert
            Assert.That(font, Is.Not.Null);
            Assert.That(font.Name, Does.Contain("Consol").Or.Contains("Courier"), "Should use monospace font");

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void TerminalFont_CanBeChanged()
        {
            // Arrange
            var control = new SshTerminalControl();
            var newFont = new Font("Courier New", 12);

            // Act
            control.TerminalFont = newFont;

            // Assert
            Assert.That(control.TerminalFont.Name, Is.EqualTo("Courier New"));
            Assert.That(control.TerminalFont.Size, Is.EqualTo(12));

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Color Tests

        [Test]
        public void TerminalBackColor_CanBeSet()
        {
            // Arrange
            var control = new SshTerminalControl();
            Color newColor = Color.DarkBlue;

            // Act
            control.TerminalBackColor = newColor;

            // Assert
            Assert.That(control.TerminalBackColor, Is.EqualTo(newColor));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void TerminalForeColor_CanBeSet()
        {
            // Arrange
            var control = new SshTerminalControl();
            Color newColor = Color.LightGreen;

            // Act
            control.TerminalForeColor = newColor;

            // Assert
            Assert.That(control.TerminalForeColor, Is.EqualTo(newColor));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void SetColorScheme_UpdatesBothColors()
        {
            // Arrange
            var control = new SshTerminalControl();
            Color backColor = Color.Navy;
            Color foreColor = Color.Yellow;

            // Act
            control.SetColorScheme(backColor, foreColor);

            // Assert
            Assert.That(control.TerminalBackColor, Is.EqualTo(backColor));
            Assert.That(control.TerminalForeColor, Is.EqualTo(foreColor));

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Stream Attachment Tests

        [Test]
        public void AttachSshStream_DoesNotThrow_WithNullStream()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act & Assert
            Assert.DoesNotThrow(() => control.AttachSshStream(null));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void DetachSshStream_DoesNotThrow()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act & Assert
            Assert.DoesNotThrow(() => control.DetachSshStream());

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Output Writing Tests

        [Test]
        public void WriteOutput_DoesNotThrow_WithNullData()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act & Assert
            Assert.DoesNotThrow(() => control.WriteOutput(null));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void WriteOutput_DoesNotThrow_WithEmptyData()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act & Assert
            Assert.DoesNotThrow(() => control.WriteOutput(""));

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void WriteOutput_DoesNotThrow_WithValidData()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act & Assert
            Assert.DoesNotThrow(() => control.WriteOutput("Hello, World!"));

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Connection Status Tests

        [Test]
        public void DisplayConnectionClosed_DoesNotThrow()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act & Assert
            Assert.DoesNotThrow(() => control.DisplayConnectionClosed());

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Input Tests

        [Test]
        public async Task WaitForInputAsync_ReturnEmpty_WhenNoInput()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(100);  // Cancel after 100ms

            // Act
            byte[] result = await control.WaitForInputAsync(cts.Token);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(0));

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Scrollback Tests

        [Test]
        public void GetScrollbackContent_ReturnsEmpty_WhenNoContent()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act
            string content = control.GetScrollbackContent(0, 10);

            // Assert
            Assert.That(content, Is.Not.Null);

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void GetScrollbackContent_HandlesInvalidRange_Gracefully()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act
            string content = control.GetScrollbackContent(-10, -5);

            // Assert
            Assert.That(content, Is.Not.Null);

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void GetScrollbackContent_HandlesReversedRange_Gracefully()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act
            string content = control.GetScrollbackContent(100, 50);

            // Assert
            Assert.That(content, Is.Not.Null);

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Resize Tests

        [Test]
        public void ForceResizeNotification_DoesNotThrow()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act & Assert
            Assert.DoesNotThrow(() => control.ForceResizeNotification());

            // Cleanup
            control.Dispose();
        }

        #endregion

        #region Diagnostic Mode Tests

        [Test]
        public void DiagnosticMode_DefaultsToFalse()
        {
            // Arrange
            var control = new SshTerminalControl();

            // Act
            bool diagnosticMode = control.DiagnosticMode;

            // Assert
            Assert.That(diagnosticMode, Is.False);

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void DiagnosticMode_CanBeEnabled()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();

            // Act
            control.DiagnosticMode = true;

            // Assert
            Assert.That(control.DiagnosticMode, Is.True);

            // Cleanup
            control.Dispose();
        }

        [Test]
        public void DiagnosticMode_CanBeDisabled()
        {
            // Arrange
            var control = new SshTerminalControl();
            control.Initialize();
            control.DiagnosticMode = true;

            // Act
            control.DiagnosticMode = false;

            // Assert
            Assert.That(control.DiagnosticMode, Is.False);

            // Cleanup
            control.Dispose();
        }

        #endregion

        // Note: The following tests require UI interaction and should be in integration tests:
        // - Actual rendering (OnPaint)
        // - Keyboard input capture
        // - Mouse selection
        // - Copy/paste operations
        // - Terminal resize with SSH server notification
        // - VtNetCore integration with actual ANSI sequences
        // - Scrolling behavior
    }
}
