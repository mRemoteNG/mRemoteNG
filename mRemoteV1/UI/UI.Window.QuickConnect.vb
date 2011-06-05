Imports WeifenLuo.WinFormsUI.Docking
Imports System.Reflection
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class QuickConnect
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents btnCancel As System.Windows.Forms.Button
            Friend WithEvents flpProtocols As System.Windows.Forms.FlowLayoutPanel

            Private Sub InitializeComponent()
                Me.flpProtocols = New System.Windows.Forms.FlowLayoutPanel
                Me.btnCancel = New System.Windows.Forms.Button
                Me.SuspendLayout()
                '
                'flpProtocols
                '
                Me.flpProtocols.Dock = System.Windows.Forms.DockStyle.Fill
                Me.flpProtocols.Location = New System.Drawing.Point(0, 0)
                Me.flpProtocols.Name = "flpProtocols"
                Me.flpProtocols.Size = New System.Drawing.Size(271, 155)
                Me.flpProtocols.TabIndex = 10
                '
                'btnCancel
                '
                Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnCancel.Location = New System.Drawing.Point(-200, -200)
                Me.btnCancel.Name = "btnCancel"
                Me.btnCancel.Size = New System.Drawing.Size(75, 23)
                Me.btnCancel.TabIndex = 20
                Me.btnCancel.TabStop = False
                Me.btnCancel.Text = My.Resources.strButtonCancel
                Me.btnCancel.UseVisualStyleBackColor = True
                '
                'QuickConnect
                '
                Me.CancelButton = Me.btnCancel
                Me.ClientSize = New System.Drawing.Size(271, 155)
                Me.Controls.Add(Me.btnCancel)
                Me.Controls.Add(Me.flpProtocols)
                Me.HideOnClose = True
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.Play_Quick_Icon
                Me.Name = "QuickConnect"
                Me.TabText = My.Resources.strQuickConnect
                Me.Text = My.Resources.strQuickConnect
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.Connection
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub
#End Region

#Region "Public Properties"
            Private _ConnectionInfo As mRemoteNG.Connection.Info
            Public Property ConnectionInfo() As mRemoteNG.Connection.Info
                Get
                    Return Me._ConnectionInfo
                End Get
                Set(ByVal value As mRemoteNG.Connection.Info)
                    Me._ConnectionInfo = value
                End Set
            End Property
#End Region

            Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
                Me.Hide()
            End Sub

            Private Sub QuickConnect_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()

                Me.CreateButtons()

                Me.flpProtocols.Controls(0).Focus()
            End Sub

            Private Sub ApplyLanguage()
                btnCancel.Text = My.Resources.strButtonCancel
                TabText = My.Resources.strQuickConnect
                Text = My.Resources.strQuickConnect
            End Sub

            Private Sub CreateButtons()
                Try
                    For Each fI As FieldInfo In GetType(mRemoteNG.Connection.Protocol.Protocols).GetFields
                        If fI.Name <> "value__" And fI.Name <> "NONE" And fI.Name <> "IntApp" Then
                            Dim nBtn As New Button
                            nBtn.Text = fI.Name
                            nBtn.FlatStyle = FlatStyle.Flat
                            nBtn.Size = New Size(60, 40)
                            nBtn.Parent = Me.flpProtocols
                            AddHandler nBtn.Click, AddressOf ProtocolButton_Click
                            nBtn.Show()
                        End If
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CreateButtons (UI.Window.QuickConnect) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ProtocolButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
                Try
                    Me._ConnectionInfo.Protocol = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.Protocols), sender.Text)

                    If Me._ConnectionInfo.Port = 0 Then
                        Me._ConnectionInfo.SetDefaultPort()

                        If mRemoteNG.Connection.QuickConnect.History.Exists(Me._ConnectionInfo.Hostname) = False Then
                            mRemoteNG.Connection.QuickConnect.History.Add(Me._ConnectionInfo.Hostname)
                        End If
                    Else
                        If mRemoteNG.Connection.QuickConnect.History.Exists(Me._ConnectionInfo.Hostname) = False Then
                            mRemoteNG.Connection.QuickConnect.History.Add(Me._ConnectionInfo.Hostname & ":" & Me._ConnectionInfo.Port)
                        End If
                    End If

                    App.Runtime.OpenConnection(Me._ConnectionInfo, mRemoteNG.Connection.Info.Force.DoNotJump)

                    Me.Hide()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ProtocolButton_Click (UI.Window.QuickConnect) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
        End Class
    End Namespace
End Namespace