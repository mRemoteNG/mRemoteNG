using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class QuickConnectComboBox : ToolStripComboBox
    {
        private readonly ComboBox _comboBox;
        private bool _ignoreEnter;

        public QuickConnectComboBox()
        {
            _comboBox = ComboBox;
            if (_comboBox == null) return;
            _comboBox.PreviewKeyDown += ComboBox_PreviewKeyDown;
            _comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            _comboBox.DrawItem += ComboBox_DrawItem;
            _comboBox.DrawMode = DrawMode.OwnerDrawFixed;
            CausesValidation = false;

            // This makes it so that _ignoreEnter works correctly before any items are added to the combo box
            _comboBox.Items.Clear();
        }

        private void ComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & _comboBox.DroppedDown)
            {
                _ignoreEnter = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                // Only connect if Enter was not pressed while the combo box was dropped down
                if (!_ignoreEnter)
                {
                    OnConnectRequested(new ConnectRequestedEventArgs(_comboBox.Text));
                }

                _ignoreEnter = false;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete & _comboBox.DroppedDown)
            {
                if (_comboBox.SelectedIndex != -1)
                {
                    // Items can't be removed from the ComboBox while it is dropped down without possibly causing
                    // an exception so we must close it, delete the item, and then drop it down again. When we
                    // close it programmatically, the SelectedItem may revert to Nothing, so we must save it first.
                    var item = _comboBox.SelectedItem;
                    _comboBox.DroppedDown = false;
                    _comboBox.Items.Remove(item);
                    _comboBox.SelectedIndex = -1;
                    if (_comboBox.Items.Count != 0)
                    {
                        _comboBox.DroppedDown = true;
                    }
                }

                e.Handled = true;
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(_comboBox.SelectedItem is HistoryItem))
            {
                return;
            }

            var historyItem = (HistoryItem)_comboBox.SelectedItem;
            OnProtocolChanged(new ProtocolChangedEventArgs(historyItem.ConnectionInfo.Protocol));
        }

        private static void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }

            var drawItem = comboBox.Items[e.Index];

            string drawString;
            if (drawItem is HistoryItem)
            {
                var historyItem = (HistoryItem)drawItem;
                drawString = historyItem.ToString(true);
            }
            else
            {
                drawString = drawItem.ToString();
            }

            e.DrawBackground();
            e.Graphics.DrawString(drawString, e.Font, new SolidBrush(e.ForeColor),
                                  new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        private struct HistoryItem : IEquatable<HistoryItem>
        {
            public ConnectionInfo ConnectionInfo { get; set; }

            public bool Equals(HistoryItem other)
            {
                if (ConnectionInfo.Hostname != other.ConnectionInfo.Hostname)
                {
                    return false;
                }

                if (ConnectionInfo.Port != other.ConnectionInfo.Port)
                {
                    return false;
                }

                return ConnectionInfo.Protocol == other.ConnectionInfo.Protocol;
            }

            public override string ToString()
            {
                return ToString(false);
            }

            public string ToString(bool includeProtocol)
            {
                var port = string.Empty;
                if (ConnectionInfo.Port != ConnectionInfo.GetDefaultPort())
                {
                    port = $":{ConnectionInfo.Port}";
                }

                return includeProtocol
                    ? $"{ConnectionInfo.Hostname}{port} ({ConnectionInfo.Protocol})"
                    : $"{ConnectionInfo.Hostname}{port}";
            }
        }

        private bool Exists(HistoryItem searchItem)
        {
            foreach (var item in _comboBox.Items)
            {
                if (!(item is HistoryItem))
                {
                    continue;
                }

                var historyItem = (HistoryItem)item;
                if (historyItem.Equals(searchItem))
                {
                    return true;
                }
            }

            return false;
        }

        public void Add(ConnectionInfo connectionInfo)
        {
            try
            {
                var historyItem = new HistoryItem {ConnectionInfo = connectionInfo};
                if (!Exists(historyItem))
                {
                    _comboBox.Items.Insert(0, historyItem);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.QuickConnectAddFailed, ex);
            }
        }

        #region Events

        public class ConnectRequestedEventArgs : EventArgs
        {
            public ConnectRequestedEventArgs(string connectionString)
            {
                ConnectionString = connectionString;
            }

            public string ConnectionString { get; }
        }

        public delegate void ConnectRequestedEventHandler(object sender, ConnectRequestedEventArgs e);

        private ConnectRequestedEventHandler ConnectRequestedEvent;

        public event ConnectRequestedEventHandler ConnectRequested
        {
            add => ConnectRequestedEvent = (ConnectRequestedEventHandler)Delegate.Combine(ConnectRequestedEvent, value);
            remove => ConnectRequestedEvent = (ConnectRequestedEventHandler)Delegate.Remove(ConnectRequestedEvent, value);
        }


        private void OnConnectRequested(ConnectRequestedEventArgs e)
        {
            // TODO: Any reason to not jsut pass "e"?
            ConnectRequestedEvent?.Invoke(this, new ConnectRequestedEventArgs(e.ConnectionString));
        }

        public class ProtocolChangedEventArgs : EventArgs
        {
            public ProtocolChangedEventArgs(ProtocolType protocol)
            {
                Protocol = protocol;
            }

            public ProtocolType Protocol { get; }
        }

        public delegate void ProtocolChangedEventHandler(object sender, ProtocolChangedEventArgs e);

        private ProtocolChangedEventHandler ProtocolChangedEvent;

        public event ProtocolChangedEventHandler ProtocolChanged
        {
            add => ProtocolChangedEvent = (ProtocolChangedEventHandler)Delegate.Combine(ProtocolChangedEvent, value);
            remove => ProtocolChangedEvent = (ProtocolChangedEventHandler)Delegate.Remove(ProtocolChangedEvent, value);
        }


        private void OnProtocolChanged(ProtocolChangedEventArgs e)
        {
            // TODO: Any reason to not jsut pass "e"?
            ProtocolChangedEvent?.Invoke(this, new ProtocolChangedEventArgs(e.Protocol));
        }

        #endregion
    }
}