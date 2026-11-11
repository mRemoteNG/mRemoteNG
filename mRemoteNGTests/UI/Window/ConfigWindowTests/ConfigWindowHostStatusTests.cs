using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Window;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Window.ConfigWindowTests
{
    /// <summary>
    /// The host status ping runs on a background thread and reports back to the window.
    /// An exception there is unhandled and terminates mRemoteNG (#3301).
    /// </summary>
    [Apartment(ApartmentState.STA)]
    public class ConfigWindowHostStatusTests
    {
        private static readonly MethodInfo CheckHostAlive =
            typeof(ConfigWindow).GetMethod("CheckHostAlive", BindingFlags.NonPublic | BindingFlags.Instance);

        private static Exception RunCheckOnBackgroundThread(ConfigWindow window, string hostName, Action whileRunning)
        {
            Exception thrown = null;
            var thread = new Thread(() =>
            {
                try
                {
                    CheckHostAlive.Invoke(window, [hostName]);
                }
                catch (TargetInvocationException ex)
                {
                    thrown = ex.InnerException;
                }
            }) { IsBackground = true };

            thread.Start();
            whileRunning();
            if (!thread.Join(TimeSpan.FromSeconds(10)))
                Assert.Fail("host status check did not finish");
            return thrown;
        }

        [Test]
        public void ClosingTheWindowWhileTheCheckReportsBackDoesNotThrow()
        {
            var window = new ConfigWindow();
            window.CreateControl();
            _ = window.Handle;

            // The check for an empty host name reports back immediately; the UI thread is not
            // pumping messages yet, so a blocking Invoke would wait until the window is disposed
            Exception thrown = RunCheckOnBackgroundThread(window, "", () =>
            {
                Thread.Sleep(300);
                window.Dispose();
            });

            Assert.That(thrown, Is.Null, () => $"background check threw {thrown?.GetType().Name}: {thrown?.Message}");
        }

        [Test]
        public void CheckAfterTheWindowWasClosedDoesNotThrow()
        {
            var window = new ConfigWindow();
            window.CreateControl();
            _ = window.Handle;
            window.Dispose();

            Exception thrown = RunCheckOnBackgroundThread(window, "", () => { });

            Assert.That(thrown, Is.Null, () => $"background check threw {thrown?.GetType().Name}: {thrown?.Message}");
        }

        [Test]
        public void CheckReportsStatusToAnOpenWindow()
        {
            var window = new ConfigWindow();
            window.CreateControl();
            _ = window.Handle;

            Exception thrown = RunCheckOnBackgroundThread(window, "", () =>
            {
                // let the posted status update run on this (UI) thread
                var deadline = DateTime.UtcNow.AddSeconds(2);
                while (DateTime.UtcNow < deadline)
                {
                    Application.DoEvents();
                    Thread.Sleep(20);
                }
            });

            Assert.That(thrown, Is.Null);
            window.Dispose();
        }
    }
}
