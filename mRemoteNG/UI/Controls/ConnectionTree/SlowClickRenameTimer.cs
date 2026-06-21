using System;
using System.Windows.Forms;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Controls.ConnectionTree
{
    /// <summary>
    /// Concrete <see cref="ISlowClickRenameTimer"/> backed by a <see cref="System.Windows.Forms.Timer"/>.
    /// Runs on the UI thread — no cross-thread marshalling required.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class SlowClickRenameTimer : ISlowClickRenameTimer
    {
        private readonly Timer _timer;

        /// <inheritdoc />
        public int Interval
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }

        /// <inheritdoc />
        public bool Enabled => _timer.Enabled;

        /// <inheritdoc />
        public event EventHandler Tick
        {
            add => _timer.Tick += value;
            remove => _timer.Tick -= value;
        }

        /// <summary>
        /// Initializes a new <see cref="SlowClickRenameTimer"/>.
        /// </summary>
        /// <param name="intervalMs">
        /// Milliseconds to wait after the slow second click before the <see cref="Tick"/>
        /// event fires. Pass <see cref="SystemInformation.DoubleClickTime"/> to match the
        /// Windows system double-click threshold.
        /// </param>
        public SlowClickRenameTimer(int intervalMs)
        {
            _timer = new Timer { Interval = Math.Max(1, intervalMs) };
        }

        /// <inheritdoc />
        public void Start() => _timer.Start();

        /// <inheritdoc />
        public void Stop() => _timer.Stop();

        /// <inheritdoc />
        public void Dispose() => _timer.Dispose();
    }
}