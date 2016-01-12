﻿Imports mRemoteNG.Tools.PortScan
Imports mRemoteNG.My
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class PortScan
            Inherits Base
#Region "Constructors"
            Public Sub New(ByVal panel As DockContent, ByVal import As Boolean)
                InitializeComponent()

                WindowType = Type.PortScan
                DockPnl = panel
                _import = import
            End Sub
#End Region

#Region "Private Properties"
            Private ReadOnly Property IpsValid() As Boolean
                Get
                    If String.IsNullOrEmpty(ipStart.Octet1) Then Return False
                    If String.IsNullOrEmpty(ipStart.Octet2) Then Return False
                    If String.IsNullOrEmpty(ipStart.Octet3) Then Return False
                    If String.IsNullOrEmpty(ipStart.Octet4) Then Return False

                    If String.IsNullOrEmpty(ipEnd.Octet1) Then Return False
                    If String.IsNullOrEmpty(ipEnd.Octet2) Then Return False
                    If String.IsNullOrEmpty(ipEnd.Octet3) Then Return False
                    If String.IsNullOrEmpty(ipEnd.Octet4) Then Return False

                    Return True
                End Get
            End Property
#End Region

#Region "Private Fields"
            Private ReadOnly _import As Boolean
            Private _portScanner As Scanner
            Private _scanning As Boolean = False
#End Region

#Region "Private Methods"
#Region "Event Handlers"
            Private Sub PortScan_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
                ApplyLanguage()

                Try
                    If _import Then
                        lvHosts.Columns.AddRange(New ColumnHeader() {clmHost, clmSSH, clmTelnet, clmHTTP, clmHTTPS, clmRlogin, clmRDP, clmVNC})
                        ShowImportControls(True)
                        cbProtocol.SelectedIndex = 0
                    Else
                        lvHosts.Columns.AddRange(New ColumnHeader() {clmHost, clmOpenPorts, clmClosedPorts})
                        ShowImportControls(False)
                    End If
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(My.Language.strPortScanCouldNotLoadPanel, ex)
                End Try
            End Sub

            Private Sub portStart_Enter(sender As System.Object, e As EventArgs) Handles portStart.Enter
                portStart.Select(0, portStart.Text.Length)
            End Sub

            Private Sub portEnd_Enter(sender As System.Object, e As EventArgs) Handles portEnd.Enter
                portEnd.Select(0, portEnd.Text.Length)
            End Sub

            Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnScan.Click
                If _scanning Then
                    StopScan()
                Else
                    If IpsValid Then
                        StartScan()
                    Else
                        MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strCannotStartPortScan)
                    End If
                End If
            End Sub

            Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnImport.Click
                Dim protocol As mRemoteNG.Connection.Protocol.Protocols = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.Protocols), cbProtocol.SelectedItem)

                Dim hosts As New List(Of ScanHost)
                For Each item As ListViewItem In lvHosts.SelectedItems
                    Dim scanHost As ScanHost = TryCast(item.Tag, ScanHost)
                    If scanHost IsNot Nothing Then hosts.Add(item.Tag)
                Next

                App.Import.ImportFromPortScan(hosts, protocol)

                DialogResult = DialogResult.OK
                Close()
            End Sub
#End Region

            Private Sub ApplyLanguage()
                lblStartIP.Text = String.Format("{0}:", My.Language.strStartIP)
                lblEndIP.Text = String.Format("{0}:", My.Language.strEndIP)
                btnScan.Text = My.Language.strButtonScan
                btnImport.Text = My.Language.strButtonImport
                lblOnlyImport.Text = String.Format("{0}:", My.Language.strProtocolToImport)
                clmHost.Text = My.Language.strColumnHostnameIP
                clmOpenPorts.Text = My.Language.strOpenPorts
                clmClosedPorts.Text = My.Language.strClosedPorts
                Label2.Text = String.Format("{0}:", My.Language.strEndPort)
                Label1.Text = String.Format("{0}:", My.Language.strStartPort)
                TabText = My.Language.strMenuPortScan
                Text = My.Language.strMenuPortScan
            End Sub

            Private Sub ShowImportControls(ByVal controlsVisible As Boolean)
                pnlPorts.Visible = controlsVisible
                pnlImport.Visible = controlsVisible
                If controlsVisible Then
                    lvHosts.Height = pnlImport.Top - lvHosts.Top
                Else
                    lvHosts.Height = pnlImport.Bottom - lvHosts.Top
                End If
            End Sub

            Private Sub StartScan()
                Try
                    _scanning = True
                    SwitchButtonText()
                    lvHosts.Items.Clear()

                    Dim ipAddressStart As Net.IPAddress = Net.IPAddress.Parse(ipStart.Text)
                    Dim ipAddressEnd As Net.IPAddress = Net.IPAddress.Parse(ipEnd.Text)

                    If _import Then
                        _portScanner = New Scanner(ipAddressStart, ipAddressEnd)
                    Else
                        _portScanner = New Scanner(ipAddressStart, ipAddressEnd, portStart.Value, portEnd.Value)
                    End If

                    AddHandler _portScanner.BeginHostScan, AddressOf PortScanner_BeginHostScan
                    AddHandler _portScanner.HostScanned, AddressOf PortScanner_HostScanned
                    AddHandler _portScanner.ScanComplete, AddressOf PortScanner_ScanComplete

                    _portScanner.StartScan()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "StartScan failed (UI.Window.PortScan)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub StopScan()
                If _portScanner IsNot Nothing Then _portScanner.StopScan()
                _scanning = False
                SwitchButtonText()
            End Sub

            Private Sub SwitchButtonText()
                If _scanning Then
                    btnScan.Text = My.Language.strButtonStop
                Else
                    btnScan.Text = My.Language.strButtonScan
                End If

                prgBar.Maximum = 100
                prgBar.Value = 0
            End Sub

            Private Shared Sub PortScanner_BeginHostScan(ByVal host As String)
                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, "Scanning " & host, True)
            End Sub

            Private Delegate Sub PortScannerHostScannedDelegate(ByVal host As ScanHost, ByVal scannedCount As Integer, ByVal totalCount As Integer)
            Private Sub PortScanner_HostScanned(ByVal host As ScanHost, ByVal scannedCount As Integer, ByVal totalCount As Integer)
                If InvokeRequired Then
                    Invoke(New PortScannerHostScannedDelegate(AddressOf PortScanner_HostScanned), New Object() {host, scannedCount, totalCount})
                    Return
                End If

                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, "Host scanned " & host.HostIp, True)

                Dim listViewItem As ListViewItem = host.ToListViewItem(_import)
                If listViewItem IsNot Nothing Then
                    lvHosts.Items.Add(listViewItem)
                    listViewItem.EnsureVisible()
                End If

                prgBar.Maximum = totalCount
                prgBar.Value = scannedCount
            End Sub

            Private Delegate Sub PortScannerScanComplete(ByVal hosts As List(Of ScanHost))
            Private Sub PortScanner_ScanComplete(ByVal hosts As List(Of ScanHost))
                If InvokeRequired Then
                    Invoke(New PortScannerScanComplete(AddressOf PortScanner_ScanComplete), New Object() {hosts})
                    Return
                End If

                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, Language.strPortScanComplete)

                _scanning = False
                SwitchButtonText()
            End Sub
#End Region
        End Class
    End Namespace
End Namespace