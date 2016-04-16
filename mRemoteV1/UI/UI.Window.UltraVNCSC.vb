Imports System.ComponentModel
Imports mRemote3G.App
Imports mRemote3G.Messages
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class UltraVNCSC
            Inherits Base

#Region "Form Init"

            Friend WithEvents tsMain As ToolStrip
            Friend WithEvents pnlContainer As Panel
            Friend WithEvents btnDisconnect As ToolStripButton

            Private Sub InitializeComponent()
                Dim resources = New ComponentResourceManager(GetType(UltraVNCSC))
                Me.tsMain = New ToolStrip
                Me.btnDisconnect = New ToolStripButton
                Me.pnlContainer = New Panel
                Me.tsMain.SuspendLayout()
                Me.SuspendLayout()
                '
                'tsMain
                '
                Me.tsMain.GripStyle = ToolStripGripStyle.Hidden
                Me.tsMain.Items.AddRange(New ToolStripItem() {Me.btnDisconnect})
                Me.tsMain.Location = New Point(0, 0)
                Me.tsMain.Name = "tsMain"
                Me.tsMain.Size = New Size(446, 25)
                Me.tsMain.TabIndex = 0
                Me.tsMain.Text = "ToolStrip1"
                '
                'btnDisconnect
                '
                Me.btnDisconnect.DisplayStyle = ToolStripItemDisplayStyle.Text
                Me.btnDisconnect.Image = CType(resources.GetObject("btnDisconnect.Image"), Image)
                Me.btnDisconnect.ImageTransparentColor = Color.Magenta
                Me.btnDisconnect.Name = "btnDisconnect"
                Me.btnDisconnect.Size = New Size(63, 22)
                Me.btnDisconnect.Text = "Disconnect"
                '
                'pnlContainer
                '
                Me.pnlContainer.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                                 Or AnchorStyles.Left) _
                                                Or AnchorStyles.Right),
                                               AnchorStyles)
                Me.pnlContainer.Location = New Point(0, 27)
                Me.pnlContainer.Name = "pnlContainer"
                Me.pnlContainer.Size = New Size(446, 335)
                Me.pnlContainer.TabIndex = 1
                '
                'UltraVNCSC
                '
                Me.ClientSize = New Size(446, 362)
                Me.Controls.Add(Me.pnlContainer)
                Me.Controls.Add(Me.tsMain)
                Me.Icon = My.Resources.Resources.UVNC_SC_Icon
                Me.Name = "UltraVNCSC"
                Me.TabText = "UltraVNC SC"
                Me.Text = "UltraVNC SC"
                Me.tsMain.ResumeLayout(False)
                Me.tsMain.PerformLayout()
                Me.ResumeLayout(False)
                Me.PerformLayout()
            End Sub

#End Region

#Region "Declarations"
            'Private WithEvents vnc As AxCSC_ViewerXControl

#End Region

#Region "Public Methods"

            Public Sub New(Panel As DockContent)
                Me.WindowType = Type.UltraVNCSC
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub

#End Region

#Region "Private Methods"

            Private Sub UltraVNCSC_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()

                StartListening()
            End Sub

            Private Sub ApplyLanguage()
                btnDisconnect.Text = Language.Language.strButtonDisconnect
            End Sub

            Private Sub StartListening()
                Try
                    'If vnc IsNot Nothing Then
                    '    vnc.Dispose()
                    '    vnc = Nothing
                    'End If

                    'vnc = New AxCSC_ViewerXControl()
                    'SetupLicense()

                    'vnc.Parent = pnlContainer
                    'vnc.Dock = DockStyle.Fill
                    'vnc.Show()

                    'vnc.StretchMode = ViewerX.ScreenStretchMode.SSM_ASPECT
                    'vnc.ListeningText = My.Language.strInheritListeningForIncomingVNCConnections & " " & My.Settings.UVNCSCPort

                    'vnc.ListenEx(My.Settings.UVNCSCPort)
                Catch ex As Exception
                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "StartListening (UI.Window.UltraVNCSC) failed" & vbNewLine & ex.ToString(), False)
                    Close()
                End Try
            End Sub

            Private Sub SetupLicense()
                Try
                    'Dim f As System.Reflection.FieldInfo
                    'f = GetType(AxHost).GetField("licenseKey", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
                    'f.SetValue(vnc, "{072169039103041044176252035252117103057101225235137221179204110241121074}")
                Catch ex As Exception
                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "VNC SetupLicense failed (UI.Window.UltraVNCSC)" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            'Private Sub vnc_ConnectionAccepted(ByVal sender As Object, ByVal e As AxViewerX._ISmartCodeVNCViewerEvents_ConnectionAcceptedEvent) Handles vnc.ConnectionAccepted
            '    mC.AddMessage(Messages.MessageClass.InformationMsg, e.bstrServerAddress & " is now connected to your UltraVNC SingleClick panel!")
            'End Sub

            'Private Sub vnc_Disconnected(ByVal sender As Object, ByVal e As System.EventArgs) Handles vnc.Disconnected
            '    StartListening()
            'End Sub

            Private Sub btnDisconnect_Click(sender As Object, e As EventArgs) Handles btnDisconnect.Click
                'vnc.Dispose()
                Dispose()
                Runtime.Windows.Show(Type.UltraVNCSC)
            End Sub

#End Region
        End Class
    End Namespace

End Namespace