Imports System.Net
Imports mRemote3G.App
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Messages
Imports mRemote3G.Tools
Imports mRemote3G.Tools.PortScan
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class PortScan
            Inherits Base

#Region "Constructors"

            Public Sub New(panel As DockContent, import As Boolean)
                InitializeComponent()

                WindowType = Type.PortScan
                DockPnl = panel
                _import = import
            End Sub

#End Region

#Region "Private Properties"

            Private ReadOnly Property IpsValid As Boolean
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

            Private Sub PortScan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
                ApplyLanguage()

                Try
                    If _import Then
                        lvHosts.Columns.AddRange(New ColumnHeader() _
                                                    {clmHost, clmSSH, clmTelnet, clmHTTP, clmHTTPS, clmRlogin, clmRDP,
                                                     clmVNC})
                        ShowImportControls(True)
                        cbProtocol.SelectedIndex = 0
                    Else
                        lvHosts.Columns.AddRange(New ColumnHeader() {clmHost, clmOpenPorts, clmClosedPorts})
                        ShowImportControls(False)
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(Language.Language.strPortScanCouldNotLoadPanel, ex)
                End Try
            End Sub

            Private Sub portStart_Enter(sender As Object, e As EventArgs) Handles portStart.Enter
                portStart.Select(0, portStart.Text.Length)
            End Sub

            Private Sub portEnd_Enter(sender As Object, e As EventArgs) Handles portEnd.Enter
                portEnd.Select(0, portEnd.Text.Length)
            End Sub

            Private Sub btnScan_Click(sender As Object, e As EventArgs) Handles btnScan.Click
                If _scanning Then
                    StopScan()
                Else
                    If IpsValid Then
                        StartScan()
                    Else
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                            Language.Language.strCannotStartPortScan)
                    End If
                End If
            End Sub

            Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
                Dim protocol As Protocols = Misc.StringToEnum(GetType(Protocols), cbProtocol.SelectedItem)

                Dim hosts As New List(Of ScanHost)
                For Each item As ListViewItem In lvHosts.SelectedItems
                    Dim scanHost = TryCast(item.Tag, ScanHost)
                    If scanHost IsNot Nothing Then hosts.Add(item.Tag)
                Next

                Import.ImportFromPortScan(hosts, protocol)

                DialogResult = DialogResult.OK
                Close()
            End Sub

#End Region

            Private Sub ApplyLanguage()
                lblStartIP.Text = String.Format("{0}:", Language.Language.strStartIP)
                lblEndIP.Text = String.Format("{0}:", Language.Language.strEndIP)
                btnScan.Text = Language.Language.strButtonScan
                btnImport.Text = Language.Language.strButtonImport
                lblOnlyImport.Text = String.Format("{0}:", Language.Language.strProtocolToImport)
                clmHost.Text = Language.Language.strColumnHostnameIP
                clmOpenPorts.Text = Language.Language.strOpenPorts
                clmClosedPorts.Text = Language.Language.strClosedPorts
                Label2.Text = String.Format("{0}:", Language.Language.strEndPort)
                Label1.Text = String.Format("{0}:", Language.Language.strStartPort)
                TabText = Language.Language.strMenuPortScan
                Text = Language.Language.strMenuPortScan
            End Sub

            Private Sub ShowImportControls(controlsVisible As Boolean)
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

                    Dim ipAddressStart As IPAddress = IPAddress.Parse(ipStart.Text)
                    Dim ipAddressEnd As IPAddress = IPAddress.Parse(ipEnd.Text)

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
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        "StartScan failed (UI.Window.PortScan)" & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

            Private Sub StopScan()
                If _portScanner IsNot Nothing Then _portScanner.StopScan()
                _scanning = False
                SwitchButtonText()
            End Sub

            Private Sub SwitchButtonText()
                If _scanning Then
                    btnScan.Text = Language.Language.strButtonStop
                Else
                    btnScan.Text = Language.Language.strButtonScan
                End If

                prgBar.Maximum = 100
                prgBar.Value = 0
            End Sub

            Private Shared Sub PortScanner_BeginHostScan(host As String)
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Scanning " & host, True)
            End Sub

            Private Delegate Sub PortScannerHostScannedDelegate _
                (host As ScanHost, scannedCount As Integer, totalCount As Integer)

            Private Sub PortScanner_HostScanned(host As ScanHost, scannedCount As Integer, totalCount As Integer)
                If InvokeRequired Then
                    Invoke(New PortScannerHostScannedDelegate(AddressOf PortScanner_HostScanned),
                           New Object() {host, scannedCount, totalCount})
                    Return
                End If

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Host scanned " & host.HostIp, True)

                Dim listViewItem As ListViewItem = host.ToListViewItem(_import)
                If listViewItem IsNot Nothing Then
                    lvHosts.Items.Add(listViewItem)
                    listViewItem.EnsureVisible()
                End If

                prgBar.Maximum = totalCount
                prgBar.Value = scannedCount
            End Sub

            Private Delegate Sub PortScannerScanComplete(hosts As List(Of ScanHost))

            Private Sub PortScanner_ScanComplete(hosts As List(Of ScanHost))
                If InvokeRequired Then
                    Invoke(New PortScannerScanComplete(AddressOf PortScanner_ScanComplete), New Object() {hosts})
                    Return
                End If

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.Language.strPortScanComplete)

                _scanning = False
                SwitchButtonText()
            End Sub

#End Region
        End Class
    End Namespace

End Namespace