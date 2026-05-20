using System;
using System.IO;
using System.Linq;
using System.Reflection;
using mRemoteNG.Connection.Protocol.SSH;

namespace mRemoteNGSpecs.Playwright
{
    /// <summary>
    /// Reconstructs the self-contained xterm.js terminal page exactly as
    /// <see cref="SshTerminalBase"/> serves it to WebView2 - inlining the CSS/JS
    /// resources into the HTML - and adds a small <c>chrome.webview</c> stub so
    /// the page can run inside a plain Playwright browser instead of WebView2.
    /// </summary>
    public static class XtermPageBuilder
    {
        private const string HtmlResource = "xterm-terminal.html";
        private static readonly string[] InlineResources = { "xterm.css", "xterm.min.js", "addon-fit.min.js" };

        /// <summary>
        /// Stub for the WebView2 host bridge. Captures every message the page
        /// posts in <c>window.__sent</c> and exposes <c>window.__dispatch</c> so
        /// tests can simulate messages coming back from the C# host.
        /// </summary>
        private const string WebViewStub = @"
<script>
(function () {
  var listeners = [];
  globalThis.chrome = globalThis.chrome || {};
  globalThis.chrome.webview = {
    postMessage: function (data) { (window.__sent = window.__sent || []).push(data); },
    addEventListener: function (type, handler) { if (type === 'message') listeners.push(handler); },
    removeEventListener: function (type, handler) {
      var i = listeners.indexOf(handler); if (i >= 0) listeners.splice(i, 1);
    }
  };
  window.__dispatch = function (dataString) {
    listeners.forEach(function (h) { h({ data: dataString }); });
  };
})();
</script>";

        public static string BuildSelfContainedHtml()
        {
            string html = ReadResource(HtmlResource);

            foreach (var fileName in InlineResources)
            {
                string content = ReadResource(fileName);
                string marker = $"<!-- INLINE:{fileName} -->";
                string tag = fileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase)
                    ? $"<style>{content}</style>"
                    : $"<script>{content}</script>";
                html = html.Replace(marker, tag);
            }

            // Inject the bridge stub as the first thing in <head> so it exists
            // before the terminal bootstrap script runs.
            return html.Replace("<head>", "<head>" + WebViewStub);
        }

        private static string ReadResource(string fileName)
        {
            // Preferred: the resources embedded in the shipping mRemoteNG assembly.
            var assembly = typeof(SshTerminalBase).Assembly;
            string manifestName = assembly.GetManifestResourceNames().FirstOrDefault(n =>
                n.EndsWith(fileName, StringComparison.OrdinalIgnoreCase) ||
                n.EndsWith(fileName.Replace('-', '_'), StringComparison.OrdinalIgnoreCase));

            if (manifestName != null)
            {
                using var stream = assembly.GetManifestResourceStream(manifestName);
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }

            // Fallback: read straight from the source tree.
            string fromDisk = Path.Combine(LocateResourceFolder(), fileName);
            if (File.Exists(fromDisk))
                return File.ReadAllText(fromDisk);

            throw new FileNotFoundException(
                $"Could not locate xterm.js resource '{fileName}' as an embedded resource or on disk.");
        }

        private static string LocateResourceFolder()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "mRemoteNG.sln")))
                dir = dir.Parent;

            if (dir == null)
                throw new DirectoryNotFoundException("Could not find the repository root (mRemoteNG.sln).");

            return Path.Combine(dir.FullName, "mRemoteNG",
                                "Connection", "Protocol", "SSH", "Resources");
        }
    }
}
