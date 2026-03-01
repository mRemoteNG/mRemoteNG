using System;
using System.Collections.Generic;
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
        private readonly ComboBox? _comboBox;
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
            if (e.KeyCode == Keys.Enter & (_comboBox?.DroppedDown ?? false))
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
                    OnConnectRequested(new ConnectRequestedEventArgs(_comboBox?.Text ?? string.Empty));
                }

                _ignoreEnter = false;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape && !(_comboBox?.DroppedDown ?? false))
            {
                Text = string.Empty;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && _comboBox != null && _comboBox.DroppedDown)
            {
                if (_comboBox.SelectedIndex != -1)
                {
                    // Items can't be removed from the ComboBox while it is dropped down without possibly causing
                    // an exception so we must close it, delete the item, and then drop it down again. When we
                    // close it programmatically, the SelectedItem may revert to Nothing, so we must save it first.
                    object? item = _comboBox.SelectedItem;
                    _comboBox.DroppedDown = false;
                    if (item != null) _comboBox.Items.Remove(item);
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
            if (_comboBox == null || _comboBox.SelectedItem is not HistoryItem)
            {
                return;
            }

            HistoryItem historyItem = (HistoryItem)_comboBox.SelectedItem;
            OnProtocolChanged(new ProtocolChangedEventArgs(historyItem.ConnectionInfo.Protocol));
        }

        private static void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender is not ComboBox comboBox)
            {
                return;
            }

            object? drawItem = comboBox.Items[e.Index];

            string drawString;
            if (drawItem is HistoryItem historyItem)
            {
                drawString = historyItem.ToString(true);
            }
            else
            {
                drawString = drawItem?.ToString() ?? string.Empty;
            }

            e.DrawBackground();
            Font drawFont = e.Font ?? SystemFonts.DefaultFont;
            e.Graphics.DrawString(drawString, drawFont, new SolidBrush(e.ForeColor),
                                  new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Public data class for persisting history items.
        /// </summary>
        public struct HistoryItemData
        {
            public string Hostname { get; set; }
            public int Port { get; set; }
            public ProtocolType Protocol { get; set; }
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
                string port = string.Empty;
                if (ConnectionInfo.Port != ConnectionInfo.GetDefaultPort())
                {
                    port = $":{ConnectionInfo.Port}";
                }

                return includeProtocol
                    ? $"{ConnectionInfo.Hostname}{port} ({ConnectionInfo.Protocol})"
                    : $"{ConnectionInfo.Hostname}{port}";
            }
        }

        private const int MaxHistoryItems = 20;

        public void Add(ConnectionInfo connectionInfo)
        {
            try
            {
                if (_comboBox == null) return;
                HistoryItem historyItem = new() { ConnectionInfo = connectionInfo };

                // Remove existing entry so the item is promoted to the top (MRU behaviour).
                for (int i = _comboBox.Items.Count - 1; i >= 0; i--)
                {
                    if (_comboBox.Items[i] is HistoryItem existing && existing.Equals(historyItem))
                    {
                        _comboBox.Items.RemoveAt(i);
                        break;
                    }
                }

                _comboBox.Items.Insert(0, historyItem);

                // Trim to the maximum history size.
                while (_comboBox.Items.Count > MaxHistoryItems)
                    _comboBox.Items.RemoveAt(_comboBox.Items.Count - 1);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.QuickConnectAddFailed, ex);
            }
        }

        public IEnumerable<HistoryItemData> GetHistoryItems()
        {
            List<HistoryItemData> items = new();
            if (_comboBox == null) return items;
            foreach (object item in _comboBox.Items)
            {
                if (item is not HistoryItem historyItem) continue;
                items.Add(new HistoryItemData
                {
                    Hostname = historyItem.ConnectionInfo.Hostname,
                    Port = historyItem.ConnectionInfo.Port,
                    Protocol = historyItem.ConnectionInfo.Protocol
                });
            }
            return items;
        }

        #region Events

        public class ConnectRequestedEventArgs(string connectionString) : EventArgs
        {
            public string ConnectionString { get; } = connectionString;
        }

        public delegate void ConnectRequestedEventHandler(object sender, ConnectRequestedEventArgs e);

        private ConnectRequestedEventHandler? ConnectRequestedEvent;

        public event ConnectRequestedEventHandler ConnectRequested
        {
            add => ConnectRequestedEvent = (ConnectRequestedEventHandler?)Delegate.Combine(ConnectRequestedEvent, value);
            remove => ConnectRequestedEvent = (ConnectRequestedEventHandler?)Delegate.Remove(ConnectRequestedEvent, value);
        }


        private void OnConnectRequested(ConnectRequestedEventArgs e)
        {
            // TODO: Any reason to not jsut pass "e"?
            ConnectRequestedEvent?.Invoke(this, new ConnectRequestedEventArgs(e.ConnectionString));
        }

        public class ProtocolChangedEventArgs(ProtocolType protocol) : EventArgs
        {
            public ProtocolType Protocol { get; } = protocol;
        }

        public delegate void ProtocolChangedEventHandler(object sender, ProtocolChangedEventArgs e);

        private ProtocolChangedEventHandler? ProtocolChangedEvent;

        public event ProtocolChangedEventHandler ProtocolChanged
        {
            add => ProtocolChangedEvent = (ProtocolChangedEventHandler?)Delegate.Combine(ProtocolChangedEvent, value);
            remove => ProtocolChangedEvent = (ProtocolChangedEventHandler?)Delegate.Remove(ProtocolChangedEvent, value);
        }


        private void OnProtocolChanged(ProtocolChangedEventArgs e)
        {
            // TODO: Any reason to not jsut pass "e"?
            ProtocolChangedEvent?.Invoke(this, new ProtocolChangedEventArgs(e.Protocol));
        }

        #endregion
    }
}