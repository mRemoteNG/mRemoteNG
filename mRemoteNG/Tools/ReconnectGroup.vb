﻿Public Class ReconnectGroup
    Private _ServerReady As Boolean
    Public Property ServerReady() As Boolean
        Get
            Return _ServerReady
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                SetStatusImage(My.Resources.HostStatus_On)
            Else
                SetStatusImage(My.Resources.HostStatus_Off)
            End If

            _ServerReady = value
        End Set
    End Property

    Private Delegate Sub SetStatusImageCB(ByVal Img As Image)
    Private Sub SetStatusImage(ByVal Img As Image)
        If pbServerStatus.InvokeRequired Then
            Dim d As New SetStatusImageCB(AddressOf SetStatusImage)
            ParentForm.Invoke(d, New Object() {Img})
        Else
            pbServerStatus.Image = Img
        End If
    End Sub

    Private Sub chkReconnectWhenReady_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkReconnectWhenReady.CheckedChanged
        _ReconnectWhenReady = chkReconnectWhenReady.Checked
    End Sub

    Private _ReconnectWhenReady As Boolean
    Public Property ReconnectWhenReady() As Boolean
        Get
            Return _ReconnectWhenReady
        End Get
        Set(ByVal value As Boolean)
            _ReconnectWhenReady = value
            SetCheckbox(value)
        End Set
    End Property

    Private Delegate Sub SetCheckboxCB(ByVal Val As Boolean)
    Private Sub SetCheckbox(ByVal Val As Boolean)
        If chkReconnectWhenReady.InvokeRequired Then
            Dim d As New SetCheckboxCB(AddressOf SetCheckbox)
            ParentForm.Invoke(d, New Object() {Val})
        Else
            chkReconnectWhenReady.Checked = Val
        End If
    End Sub

    Public Event CloseClicked()

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RaiseEvent CloseClicked()
    End Sub

    Private Sub tmrAnimation_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAnimation.Tick
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

    Private Sub ReconnectGroup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ApplyLanguage()
    End Sub

    Private Sub ApplyLanguage()
        grpAutomaticReconnect.Text = My.Language.strGroupboxAutomaticReconnect
        btnClose.Text = My.Language.strButtonClose
        lblServerStatus.Text = My.Language.strLabelServerStatus
        chkReconnectWhenReady.Text = My.Language.strCheckboxReconnectWhenReady
    End Sub
End Class
