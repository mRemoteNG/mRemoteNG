using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using static VerifyNUnit.Verifier;

namespace mRemoteNGSpecs.Playwright
{
    /// <summary>
    /// End-to-end tests for the xterm.js SSH terminal page. The page is the
    /// exact resource shipped to WebView2 (see <see cref="XtermPageBuilder"/>);
    /// a stubbed <c>chrome.webview</c> bridge lets it run in a real browser so
    /// rendering, the host message protocol, and input handling are exercised
    /// the same way they are inside mRemoteNG.
    /// </summary>
    [TestFixture]
    public class XtermTerminalTests : PageTest
    {
        private static readonly string TerminalHtml = XtermPageBuilder.BuildSelfContainedHtml();

        /// <summary>Fixed viewport + light scheme so screenshots are deterministic.</summary>
        public override BrowserNewContextOptions ContextOptions() => new()
        {
            ViewportSize = new ViewportSize { Width = 900, Height = 500 },
            ColorScheme = ColorScheme.Light,
        };

        // --- Behavior -------------------------------------------------------

        [Test]
        public async Task Loading_PostsReadyMessage()
        {
            await LoadTerminalAsync();

            var messages = await SentMessagesAsync();
            Assert.That(messages.Any(m => TypeOf(m) == "ready"), Is.True,
                        "The terminal should post a 'ready' message once initialised.");
        }

        [Test]
        public async Task Loading_RendersTheTerminalElement()
        {
            await LoadTerminalAsync();

            await Expect(Page.Locator("#terminal .xterm")).ToBeVisibleAsync();
            await Expect(Page.Locator("#terminal .xterm-screen")).ToHaveCountAsync(1);
        }

        [Test]
        public async Task Loading_PostsResizeWithPositiveDimensions()
        {
            await LoadTerminalAsync();
            await Page.WaitForFunctionAsync(
                "() => (window.__sent || []).some(m => JSON.parse(m).type === 'resize')");

            var resize = (await SentMessagesAsync()).First(m => TypeOf(m) == "resize");
            Assert.That(resize.GetProperty("cols").GetInt32(), Is.GreaterThan(0));
            Assert.That(resize.GetProperty("rows").GetInt32(), Is.GreaterThan(0));
        }

        [Test]
        public async Task HostOutput_IsRenderedInTheTerminal()
        {
            await LoadTerminalAsync();

            await SendHostOutputAsync("Hello from mRemoteNG E2E");

            await Expect(Page.Locator("#terminal")).ToContainTextAsync("Hello from mRemoteNG E2E");
        }

        [Test]
        public async Task AnsiColorOutput_IsRenderedWithColor()
        {
            await LoadTerminalAsync();

            // SGR 31 = red foreground, SGR 0 = reset.
            await SendHostOutputAsync("[31mRED-TEXT[0m plain");
            await Expect(Page.Locator("#terminal")).ToContainTextAsync("RED-TEXT");

            string color = await Page.EvaluateAsync<string>(@"() => {
                const spans = [...document.querySelectorAll('#terminal .xterm-rows span')];
                const hit = spans.find(s => s.textContent.includes('RED-TEXT'));
                return hit ? getComputedStyle(hit).color : '';
            }");

            var (r, g, b) = ParseRgb(color);
            Assert.That(r, Is.GreaterThan(g + 40).And.GreaterThan(b + 40),
                        $"Expected a red-dominant colour for ANSI red, got '{color}'.");
        }

        [Test]
        public async Task KeyboardInput_PostsInputMessages()
        {
            await LoadTerminalAsync();

            await Page.Locator("#terminal .xterm-screen").ClickAsync();
            await Page.Keyboard.TypeAsync("ls");
            await Page.WaitForFunctionAsync(
                "() => (window.__sent || []).some(m => JSON.parse(m).type === 'input')");

            string typed = string.Concat((await SentMessagesAsync())
                .Where(m => TypeOf(m) == "input")
                .Select(m => m.GetProperty("data").GetString()));
            Assert.That(typed, Does.Contain("l").And.Contain("s"));
        }

        [Test]
        public async Task Loading_ProducesNoConsoleErrors()
        {
            var consoleErrors = new List<string>();
            Page.Console += (_, msg) =>
            {
                if (msg.Type == "error")
                    consoleErrors.Add(msg.Text);
            };

            await LoadTerminalAsync();
            await SendHostOutputAsync("post-load output");
            await Expect(Page.Locator("#terminal")).ToContainTextAsync("post-load output");

            Assert.That(consoleErrors, Is.Empty,
                        "Console errors: " + string.Join(" | ", consoleErrors));
        }

        // --- Visual regression ---------------------------------------------
        // Playwright for .NET only captures screenshots; visual *comparison* is
        // delegated to Verify (Playwright's own ToHaveScreenshot assertion is
        // exclusive to the JavaScript @playwright/test runner).

        [Test]
        [Category("Visual")]
        public async Task TerminalRendering_MatchesVerifiedScreenshot()
        {
            await LoadFixedTerminalAsync();

            // Verify does an exact image compare, so freeze the blinking cursor.
            await Page.AddStyleTagAsync(new PageAddStyleTagOptions
            {
                Content = ".xterm-cursor-layer, .xterm-cursor { visibility: hidden !important; }"
            });

            // Playwright captures the terminal element as a PNG; Verify owns the
            // image comparison. (Verifying an IPage/ILocator directly would also
            // snapshot font-dependent, non-portable HTML.)
            byte[] screenshot = await Page.Locator("#terminal").ScreenshotAsync(
                new LocatorScreenshotOptions { Animations = ScreenshotAnimations.Disabled });

            await Verify(screenshot, extension: "png");
        }

        [Test]
        public async Task TerminalContent_MatchesVerifiedTextSnapshot()
        {
            await LoadFixedTerminalAsync();

            // Structural snapshot: the text the terminal actually rendered.
            // Deterministic and portable - unaffected by fonts or anti-aliasing.
            string rendered = await Page.EvaluateAsync<string>(@"() => {
                const rows = [...document.querySelectorAll('#terminal .xterm-rows > div')];
                return rows
                    .map(r => r.textContent.replace(/ /g, ' ').replace(/\s+$/, ''))
                    .join('\n')
                    .replace(/\n+$/, '');
            }");

            await Verify(rendered);
        }

        // --- Helpers --------------------------------------------------------

        /// <summary>Loads the terminal and renders a fixed banner used by the snapshot tests.</summary>
        private async Task LoadFixedTerminalAsync()
        {
            await LoadTerminalAsync();
            await SendHostOutputAsync(
                "mRemoteNG xterm.js terminal E2E\r\n$ echo visual-regression\r\nvisual-regression\r\n$ ");
            await Expect(Page.Locator("#terminal")).ToContainTextAsync("visual-regression");
        }

        private async Task LoadTerminalAsync()
        {
            await Page.SetContentAsync(TerminalHtml);
            await Page.WaitForFunctionAsync(
                "() => (window.__sent || []).some(m => JSON.parse(m).type === 'ready')");
        }

        /// <summary>Simulates a host-&gt;page 'output' message (Base64 UTF-8, as C# sends it).</summary>
        private async Task SendHostOutputAsync(string text)
        {
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
            string json = JsonSerializer.Serialize(new { type = "output", data = base64 });
            await Page.EvaluateAsync("payload => window.__dispatch(payload)", json);
        }

        private async Task<IReadOnlyList<JsonElement>> SentMessagesAsync()
        {
            var raw = await Page.EvaluateAsync<string[]>("() => window.__sent || []");
            return raw.Select(s => JsonDocument.Parse(s).RootElement).ToList();
        }

        private static string TypeOf(JsonElement message) =>
            message.TryGetProperty("type", out var t) ? t.GetString() : null;

        private static (int R, int G, int B) ParseRgb(string cssColor)
        {
            // Accepts "rgb(r, g, b)" / "rgba(r, g, b, a)".
            var numbers = cssColor
                .Split('(', ')', ',')
                .Select(p => p.Trim())
                .Where(p => int.TryParse(p, out _))
                .Select(int.Parse)
                .ToArray();

            return numbers.Length >= 3 ? (numbers[0], numbers[1], numbers[2]) : (0, 0, 0);
        }
    }
}
