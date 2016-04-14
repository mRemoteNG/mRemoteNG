using mRemoteNG.App;
using mRemoteNG.Messages;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Timers;

namespace mRemoteNG.Config.Connections
{
    public class SqlUpdateTimer : IDisposable
    {
        private Timer _sqlUpdateTimer;

        public SqlUpdateTimer()
        {
            Initialize();
        }

        ~SqlUpdateTimer()
        {
            Dispose(false);
        }

        private void Initialize()
        {
            _sqlUpdateTimer = new Timer();
            _sqlUpdateTimer.Interval = 3000;
            _sqlUpdateTimer.Elapsed += sqlUpdateTimer_Elapsed;
        }

        public void Enable()
        {
            _sqlUpdateTimer.Start();
        }

        public void Disable()
        {
            _sqlUpdateTimer.Stop();
        }

        public bool IsUpdateCheckingEnabled()
        {
            return _sqlUpdateTimer.Enabled;
        }

        private static void sqlUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SqlUpdateTimerElapsedEvent();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool itIsSafeToAlsoFreeManagedObjects)
        {
            if (itIsSafeToAlsoFreeManagedObjects)
            {
                StopTimer();
            }
        }
        private void StopTimer()
        {
            try
            {
                _sqlUpdateTimer.Stop();
                _sqlUpdateTimer.Close();
            }
            catch (Exception)
            {
            }
        }


        public delegate void SqlUpdateTimerElapsedEventHandler();
        private static SqlUpdateTimerElapsedEventHandler SqlUpdateTimerElapsedEvent;
        public static event SqlUpdateTimerElapsedEventHandler SqlUpdateTimerElapsed
        {
            add { SqlUpdateTimerElapsedEvent = (SqlUpdateTimerElapsedEventHandler)Delegate.Combine(SqlUpdateTimerElapsedEvent, value); }
            remove { SqlUpdateTimerElapsedEvent = (SqlUpdateTimerElapsedEventHandler)Delegate.Remove(SqlUpdateTimerElapsedEvent, value); }
        }
    }
}