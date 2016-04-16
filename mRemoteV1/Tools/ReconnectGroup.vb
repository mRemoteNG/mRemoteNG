Imports mRemote3G.My.Resources

Namespace Tools
    Public Class ReconnectGroup
        Private _ServerReady As Boolean

        Public Property ServerReady As Boolean
            Get
                Return _ServerReady
            End Get
            Set
                If value = True Then
                    SetStatusImage(HostStatus_On)
                Else
                    SetStatusImage(HostStatus_Off)
                End If

                _ServerReady = value
            End Set
        End Property

        Private Delegate Sub SetStatusImageCB(Img As Image)

        Private Sub SetStatusImage(Img As Image)
            If pbServerStatus.InvokeRequired Then
                Dim d As New SetStatusImageCB(AddressOf SetStatusImage)
                ParentForm.Invoke(d, New Object() {Img})
            Else
                pbServerStatus.Image = Img
            End If
        End Sub

        Private Sub chkReconnectWhenReady_CheckedChanged(sender As Object, e As EventArgs) _
            Handles chkReconnectWhenReady.CheckedChanged
            _ReconnectWhenReady = chkReconnectWhenReady.Checked
        End Sub

        Private _ReconnectWhenReady As Boolean

        Public Property ReconnectWhenReady As Boolean
            Get
                Return _ReconnectWhenReady
            End Get
            Set
                _ReconnectWhenReady = value
                SetCheckbox(value)
            End Set
        End Property

        Private Delegate Sub SetCheckboxCB(Val As Boolean)

        Private Sub SetCheckbox(Val As Boolean)
            If chkReconnectWhenReady.InvokeRequired Then
                Dim d As New SetCheckboxCB(AddressOf SetCheckbox)
                ParentForm.Invoke(d, New Object() {Val})
            Else
                chkReconnectWhenReady.Checked = Val
            End If
        End Sub

        Public Event CloseClicked()

        Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
            RaiseEvent CloseClicked()
        End Sub

        Private Sub tmrAnimation_Tick(sender As Object, e As EventArgs) Handles tmrAnimation.Tick
            Select Case lblAnimation.Text
                Case ""
                    lblAnimation.Text = "»"
                Case "»"
                    lblAnimation.Text = "»»"
                Case "»»"
                    lblAnimation.Text = "»»»"
                Case "»»»"
                    lblAnimation.Text = ""
            End Select
        End Sub

        Private Delegate Sub DisposeReconnectGroupCB()

        Public Sub DisposeReconnectGroup()
            If Me.InvokeRequired Then
                Dim d As New DisposeReconnectGroupCB(AddressOf DisposeReconnectGroup)
                ParentForm.Invoke(d)
            Else
                Me.Dispose()
            End If
        End Sub

        Private Sub ReconnectGroup_Load(sender As Object, e As EventArgs) Handles Me.Load
            ApplyLanguage()
        End Sub

        Private Sub ApplyLanguage()
            grpAutomaticReconnect.Text = Language.Language.strGroupboxAutomaticReconnect
            btnClose.Text = Language.Language.strButtonClose
            lblServerStatus.Text = Language.Language.strLabelServerStatus
            chkReconnectWhenReady.Text = Language.Language.strCheckboxReconnectWhenReady
        End Sub
    End Class
End NameSpace