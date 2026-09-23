using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace mRemoteNGSpecs.Playwright
{
    /// <summary>
    /// Drives the terminal page while it runs inside a real WebView2, attached
    /// over CDP. <see cref="XtermTerminalTests"/> covers the page in a plain
    /// browser with a stubbed bridge; these tests additionally exercise the
    /// WebView2 runtime, the virtual-host resource mapping and the real
    /// <c>postMessage</c> channel in both directions - the parts a stub cannot
    /// prove.
    /// </summary>
    /// <remarks>
    /// Requires the WebView2 runtime and an interactive desktop session; the
    /// fixture reports inconclusive rather than failing when either is missing,
    /// so headless CI and contributors without WebView2 are unaffected.
    /// </remarks>
    [TestFixture]
    [Category("LiveWebView2")]
    [NonParallelizable]
    public class LiveWebView2TerminalTests
    {
        private static readonly TimeSpan StartupTimeout = TimeSpan.FromSeconds(60);

        private WebView2TerminalHost _host;
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;

        [OneTimeSetUp]
        public async Task StartHostAndAttach()
        {
            if (!Environment.UserInteractive)
                Assert.Ignore("WebView2 needs an interactive desktop session.");

            try
            {
                _host = await WebView2TerminalHost.StartAsync(StartupTimeout);
            }
            catch (Exception ex)
            {
                Assert.Ignore($"Could not start WebView2 ({ex.GetType().Name}: {ex.Message}) - skipping live terminal tests.");
            }

            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            _browser = await _playwright.Chromium.ConnectOverCDPAsync($"http://127.0.0.1:{_host.DebuggingPort}");
            _page = await FindTerminalPageAsync();
        }

        [OneTimeTearDown]
        public async Task StopHost()
        {
            if (_browser != null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
            _host?.Dispose();
        }

        [Test]
        public async Task TerminalRendersInsideWebView2()
        {
            await Assertions.Expect(_page.Locator("#terminal .xterm")).ToBeVisibleAsync();
            await Assertions.Expect(_page.Locator("#terminal .xterm-screen")).ToHaveCountAsync(1);
        }

        [Test]
        public async Task PageIsServedThroughTheVirtualHostMapping()
        {
            // The app serves the page from https://xterm.local rather than a file://
            // URL, which is what lets WebView2 apply web security to it.
            Assert.That(_page.Url, Is.EqualTo(_host.PageUrl));

            string terminalText = await _page.InnerTextAsync("#terminal");
            Assert.That(terminalText, Is.Not.Null, "The terminal element should exist once the page is served.");
        }

        [Test]
        public async Task PageAnnouncesItselfToTheHost()
        {
            string[] messages = _host.MessagesFromPage;

            Assert.That(messages.Any(m => WebView2TerminalHost.MessageType(m) == "ready"), Is.True,
                        "The page should post 'ready' to the host through the real WebView2 bridge.");

            string resize = messages.FirstOrDefault(m => WebView2TerminalHost.MessageType(m) == "resize");
            Assert.That(resize, Is.Not.Null, "The page should report its size to the host.");

            using var document = JsonDocument.Parse(resize);
            Assert.That(document.RootElement.GetProperty("cols").GetInt32(), Is.GreaterThan(0));
            Assert.That(document.RootElement.GetProperty("rows").GetInt32(), Is.GreaterThan(0));

            await Task.CompletedTask;
        }

        [Test]
        public async Task HostOutputReachesTheTerminal()
        {
            string marker = "live-webview2-" + Guid.NewGuid().ToString("N")[..8];

            _host.PostOutput(marker + "\r\n");

            await _page.WaitForFunctionAsync(
                "text => document.querySelector('#terminal').innerText.includes(text)",
                marker,
                new PageWaitForFunctionOptions { Timeout = 10_000 });
        }

        [Test]
        public async Task TypingReachesTheHost()
        {
            string typed = "whoami";
            int before = _host.MessagesFromPage.Count(m => WebView2TerminalHost.MessageType(m) == "input");

            await _page.Locator("#terminal .xterm-helper-textarea").FocusAsync();
            await _page.Keyboard.TypeAsync(typed);

            await WaitUntilAsync(() => InputPayload(_host.MessagesFromPage).Contains(typed),
                                 TimeSpan.FromSeconds(10),
                                 () => $"Host received: '{InputPayload(_host.MessagesFromPage)}'");

            Assert.That(_host.MessagesFromPage.Count(m => WebView2TerminalHost.MessageType(m) == "input"),
                        Is.GreaterThan(before));
        }

        private static string InputPayload(string[] messages)
        {
            return string.Concat(messages
                .Where(m => WebView2TerminalHost.MessageType(m) == "input")
                .Select(m =>
                {
                    using var document = JsonDocument.Parse(m);
                    return document.RootElement.GetProperty("data").GetString();
                }));
        }

        private async Task<IPage> FindTerminalPageAsync()
        {
            var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            while (DateTime.UtcNow < deadline)
            {
                IPage page = _browser.Contexts
                                     .SelectMany(context => context.Pages)
                                     .FirstOrDefault(p => p.Url.StartsWith("https://xterm.local", StringComparison.OrdinalIgnoreCase));
                if (page != null)
                    return page;

                await Task.Delay(250);
            }

            throw new InvalidOperationException(
                "Attached over CDP but no page served from https://xterm.local was found.");
        }

        private static async Task WaitUntilAsync(Func<bool> condition, TimeSpan timeout, Func<string> describeFailure)
        {
            var deadline = DateTime.UtcNow + timeout;
            while (DateTime.UtcNow < deadline)
            {
                if (condition())
                    return;

                await Task.Delay(100);
            }

            Assert.Fail($"Timed out after {timeout.TotalSeconds:0}s. {describeFailure()}");
        }
    }
}
