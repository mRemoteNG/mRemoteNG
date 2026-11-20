using System;

namespace mRemoteNG.Connection.Protocol.VNC
{
    /// <summary>
    /// What <see cref="VncStallWatchdog.Check"/> asks the caller to do.
    /// </summary>
    public enum VncStallAction
    {
        None,
        Reconnect,
        GiveUp
    }

    /// <summary>
    /// Decides when a VNC session has stopped drawing and needs a fresh connection.
    /// </summary>
    /// <remarks>
    /// VncSharpCore can lose its place in the server's byte stream. Once that happens it
    /// reads the rest of the stream as junk, blocks in a read that never completes, and
    /// stops requesting updates, so the tab freezes while the TCP connection stays up.
    /// A screen refresh cannot recover it: <c>FullScreenUpdate</c> only sets a flag that
    /// the stuck worker thread would read. Only a new connection does.
    ///
    /// A healthy session is never silent for long, even on a static screen: VncSharp's
    /// 15 second socket read timeout makes it request a full frame, which the server
    /// answers. So no frame for <see cref="StallTimeout"/> means the session is stuck,
    /// not idle.
    ///
    /// The clock is injected so the policy can be tested without timers or sockets.
    /// </remarks>
    public sealed class VncStallWatchdog
    {
        public static readonly TimeSpan DefaultStallTimeout = TimeSpan.FromSeconds(30);
        public const int DefaultMaxReconnects = 3;

        private readonly Func<TimeSpan> _now;
        private TimeSpan _lastFrame;
        private int _reconnectsWithoutFrame;
        private bool _armed;

        public VncStallWatchdog(Func<TimeSpan> now, TimeSpan stallTimeout, int maxReconnects)
        {
            if (stallTimeout <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(stallTimeout), "Stall timeout must be positive.");
            if (maxReconnects < 0)
                throw new ArgumentOutOfRangeException(nameof(maxReconnects), "Reconnect limit cannot be negative.");

            _now = now ?? throw new ArgumentNullException(nameof(now));
            StallTimeout = stallTimeout;
            MaxReconnects = maxReconnects;
        }

        public TimeSpan StallTimeout { get; }

        public int MaxReconnects { get; }

        public bool IsArmed => _armed;

        /// <summary>
        /// Starts watching a connection that has just been established. Reconnects made
        /// without a frame arriving in between still count towards <see cref="MaxReconnects"/>,
        /// so a server that accepts connections but never draws cannot cause an endless loop.
        /// </summary>
        public void Start()
        {
            _armed = true;
            _lastFrame = _now();
        }

        public void Stop()
        {
            _armed = false;
        }

        /// <summary>
        /// Records that the session drew something.
        /// </summary>
        public void FrameReceived()
        {
            _lastFrame = _now();
            _reconnectsWithoutFrame = 0;
        }

        /// <summary>
        /// Called periodically. Returns <see cref="VncStallAction.Reconnect"/> once per stall,
        /// or <see cref="VncStallAction.GiveUp"/> when the reconnect limit is used up, after
        /// which the watchdog disarms itself.
        /// </summary>
        public VncStallAction Check()
        {
            if (!_armed || _now() - _lastFrame < StallTimeout)
                return VncStallAction.None;

            if (_reconnectsWithoutFrame >= MaxReconnects)
            {
                _armed = false;
                return VncStallAction.GiveUp;
            }

            _reconnectsWithoutFrame++;
            _armed = false; // re-armed by Start() once the new connection completes
            return VncStallAction.Reconnect;
        }
    }
}
