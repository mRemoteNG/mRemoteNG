using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.Controls
{
	public class QuickConnectComboBox : ToolStripComboBox
	{

		private ComboBox withEventsField__comboBox;
		private ComboBox _comboBox {
			get { return withEventsField__comboBox; }
			set {
				if (withEventsField__comboBox != null) {
					withEventsField__comboBox.PreviewKeyDown -= ComboBox_PreviewKeyDown;
					withEventsField__comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
					withEventsField__comboBox.DrawItem -= ComboBox_DrawItem;
				}
				withEventsField__comboBox = value;
				if (withEventsField__comboBox != null) {
					withEventsField__comboBox.PreviewKeyDown += ComboBox_PreviewKeyDown;
					withEventsField__comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
					withEventsField__comboBox.DrawItem += ComboBox_DrawItem;
				}
			}
		}

		private bool _ignoreEnter = false;
		public QuickConnectComboBox()
		{
			_comboBox = ComboBox;
			_comboBox.DrawMode = DrawMode.OwnerDrawFixed;

			// This makes it so that _ignoreEnter works correctly before any items are added to the combo box
			_comboBox.Items.Clear();
		}

		private void ComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter & _comboBox.DroppedDown)
				_ignoreEnter = true;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Enter) {
				// Only connect if Enter was not pressed while the combo box was dropped down
				if (!_ignoreEnter)
					OnConnectRequested(new ConnectRequestedEventArgs(_comboBox.Text));
				_ignoreEnter = false;
				e.Handled = true;
			} else if (e.KeyCode == Keys.Delete & _comboBox.DroppedDown) {
				if (!(_comboBox.SelectedIndex == -1)) {
					// Items can't be removed from the ComboBox while it is dropped down without possibly causing
					// an exception so we must close it, delete the item, and then drop it down again. When we
					// close it programmatically, the SelectedItem may revert to Nothing, so we must save it first.
					object item = _comboBox.SelectedItem;
					_comboBox.DroppedDown = false;
					_comboBox.Items.Remove(item);
					_comboBox.SelectedIndex = -1;
					if (!(_comboBox.Items.Count == 0)) {
						_comboBox.DroppedDown = true;
					}
				}
				e.Handled = true;
			}
		}

		private void ComboBox_SelectedIndexChanged(System.Object sender, EventArgs e)
		{
			if (!_comboBox.SelectedItem is HistoryItem)
				return;
			HistoryItem historyItem = (HistoryItem)_comboBox.SelectedItem;
			OnProtocolChanged(new ProtocolChangedEventArgs(historyItem.ConnectionInfo.Protocol));
		}

		private static void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			if (comboBox == null)
				return;
			object drawItem = comboBox.Items[e.Index];

			string drawString = null;
			if (drawItem is HistoryItem) {
				HistoryItem historyItem = (HistoryItem)drawItem;
				drawString = historyItem.ToString(includeProtocol: true);
			} else {
				drawString = drawItem.ToString();
			}

			e.DrawBackground();
			e.Graphics.DrawString(drawString, e.Font, new SolidBrush(e.ForeColor), new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
			e.DrawFocusRectangle();
		}

		private struct HistoryItem : IEquatable<HistoryItem>
		{

			public Connection.Info ConnectionInfo { get; set; }

			public bool Equals(HistoryItem other)
			{
				if (!(ConnectionInfo.Hostname == other.ConnectionInfo.Hostname))
					return false;
				if (!(ConnectionInfo.Port == other.ConnectionInfo.Port))
					return false;
				if (!(ConnectionInfo.Protocol == other.ConnectionInfo.Protocol))
					return false;
				return true;
			}

			public override string ToString()
			{
				return ToString(false);
			}

			public string ToString(bool includeProtocol)
			{
				string port = string.Empty;
				if (!(ConnectionInfo.Port == ConnectionInfo.GetDefaultPort())) {
					port = string.Format(":{0}", ConnectionInfo.Port);
				}
				if (includeProtocol) {
					return string.Format("{0}{1} ({2})", ConnectionInfo.Hostname, port, ConnectionInfo.Protocol);
				} else {
					return string.Format("{0}{1}", ConnectionInfo.Hostname, port);
				}
			}
		}

		private bool Exists(HistoryItem searchItem)
		{
			foreach (object item in _comboBox.Items) {
				if (!item is HistoryItem)
					continue;
				HistoryItem historyItem = (HistoryItem)item;
				if (historyItem.Equals(searchItem))
					return true;
			}
			return false;
		}

		public void Add(Connection.Info connectionInfo)
		{
			try {
				HistoryItem historyItem = new HistoryItem();
				historyItem.ConnectionInfo = connectionInfo;
				if (!Exists(historyItem))
					_comboBox.Items.Insert(0, historyItem);
			} catch (Exception ex) {
				mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strQuickConnectAddFailed, ex, mRemoteNG.Messages.MessageClass.ErrorMsg, true);
			}
		}

		#region "Events"
		public class ConnectRequestedEventArgs : EventArgs
		{

			public ConnectRequestedEventArgs(string connectionString)
			{
				_connectionString = connectionString;
			}


			private readonly string _connectionString;
			public string ConnectionString {
				get { return _connectionString; }
			}
		}

		public event ConnectRequestedEventHandler ConnectRequested;
		public delegate void ConnectRequestedEventHandler(object sender, ConnectRequestedEventArgs e);

		protected virtual void OnConnectRequested(ConnectRequestedEventArgs e)
		{
			if (ConnectRequested != null) {
				ConnectRequested(this, new ConnectRequestedEventArgs(e.ConnectionString));
			}
		}

		public class ProtocolChangedEventArgs : EventArgs
		{

			public ProtocolChangedEventArgs(Connection.Protocol.Protocols protocol)
			{
				_protocol = protocol;
			}


			private readonly Connection.Protocol.Protocols _protocol;
			public Connection.Protocol.Protocols Protocol {
				get { return _protocol; }
			}
		}

		public event ProtocolChangedEventHandler ProtocolChanged;
		public delegate void ProtocolChangedEventHandler(object sender, ProtocolChangedEventArgs e);

		protected virtual void OnProtocolChanged(ProtocolChangedEventArgs e)
		{
			if (ProtocolChanged != null) {
				ProtocolChanged(this, new ProtocolChangedEventArgs(e.Protocol));
			}
		}
		#endregion
	}
}

