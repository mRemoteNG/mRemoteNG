Imports mRemote3G.App
Imports mRemote3G.Forms
Imports mRemote3G.UI.Window

Namespace Messages
    Public Class Collector

#Region "Public Properties"

        Private _MCForm As ErrorsAndInfos

        Public Property MCForm As ErrorsAndInfos
            Get
                Return Me._MCForm
            End Get
            Set
                Me._MCForm = value
            End Set
        End Property

#End Region

        Private ECTimer As Timer

        Public Sub New(MessageCollectorForm As ErrorsAndInfos)
            Me._MCForm = MessageCollectorForm
            CreateTimer()
        End Sub

        Private Sub CreateTimer()
            ECTimer = New Timer()
            ECTimer.Enabled = False
            ECTimer.Interval = 300
            AddHandler ECTimer.Tick, AddressOf SwitchTimerTick
        End Sub

        Delegate Sub AddToListCB(lvItem As ListViewItem)

        Private Sub AddToList(lvItem As ListViewItem)
            If Me._MCForm.lvErrorCollector.InvokeRequired Then
                Dim d As New AddToListCB(AddressOf AddToList)
                Me._MCForm.lvErrorCollector.Invoke(d, New Object() {lvItem})
            Else
                Me._MCForm.lvErrorCollector.Items.Insert(0, lvItem)
            End If
        End Sub

        Public Sub AddMessage(MsgClass As MessageClass, MsgText As String, Optional ByVal OnlyLog As Boolean = False)
            Dim nMsg As New Message
            nMsg.MsgClass = MsgClass
            nMsg.MsgText = MsgText
            nMsg.MsgDate = Now

            If My.Settings.SwitchToMCOnInformation And nMsg.MsgClass = MessageClass.InformationMsg Then
                Debug.Print("Info: " & nMsg.MsgText)
                If My.Settings.WriteLogFile Then
                    Runtime.log.Info(nMsg.MsgText)
                End If

                If OnlyLog Then
                    Exit Sub
                End If

                If My.Settings.ShowNoMessageBoxes Then
                    ECTimer.Enabled = True
                Else
                    ShowMessageBox(nMsg)
                End If
            End If

            If My.Settings.SwitchToMCOnWarning And nMsg.MsgClass = MessageClass.WarningMsg Then
                Debug.Print("Warning: " & nMsg.MsgText)
                If My.Settings.WriteLogFile Then
                    Runtime.log.Warn(nMsg.MsgText)
                End If

                If OnlyLog Then
                    Exit Sub
                End If

                If My.Settings.ShowNoMessageBoxes Then
                    ECTimer.Enabled = True
                Else
                    ShowMessageBox(nMsg)
                End If
            End If

            If My.Settings.SwitchToMCOnError And nMsg.MsgClass = MessageClass.ErrorMsg Then
                Debug.Print("Error: " & nMsg.MsgText)

                ' Always log error messages
                Runtime.Log.Error(nMsg.MsgText)

                If OnlyLog Then
                    Exit Sub
                End If

                If My.Settings.ShowNoMessageBoxes Then
                    ECTimer.Enabled = True
                Else
                    ShowMessageBox(nMsg)
                End If
            End If

            If nMsg.MsgClass = MessageClass.ReportMsg Then
                Debug.Print("Report: " & nMsg.MsgText)

                If My.Settings.WriteLogFile Then
                    Runtime.Log.Info(nMsg.MsgText)
                End If

                Exit Sub
            End If

            Dim lvItem As New ListViewItem
            lvItem.ImageIndex = CType(nMsg.MsgClass, Integer)
            lvItem.Text = nMsg.MsgText.Replace(vbNewLine, "  ")
            lvItem.Tag = nMsg

            AddToList(lvItem)
        End Sub

        Public Sub AddExceptionMessage(message As String, ex As Exception,
                                       Optional ByVal msgClass As MessageClass = MessageClass.ErrorMsg,
                                       Optional ByVal logOnly As Boolean = False)
            AddMessage(msgClass, message & vbNewLine & ex.ToString(), logOnly)
        End Sub

        Private Sub SwitchTimerTick(sender As Object, e As EventArgs)
            Me.SwitchToMessage()
            Me.ECTimer.Enabled = False
        End Sub

        Private Sub SwitchToMessage()
            Me._MCForm.PreviousActiveForm = frmMain.pnlDock.ActiveContent
            Me.ShowMCForm()
            Me._MCForm.lvErrorCollector.Focus()
            Me._MCForm.lvErrorCollector.SelectedItems.Clear()
            Me._MCForm.lvErrorCollector.Items(0).Selected = True
            Me._MCForm.lvErrorCollector.FocusedItem = Me._MCForm.lvErrorCollector.Items(0)
        End Sub

        Private Shared Sub ShowMessageBox(Msg As Message)
            Select Case Msg.MsgClass
                Case MessageClass.InformationMsg
                    MessageBox.Show(Msg.MsgText, String.Format(Language.Language.strTitleInformation, Msg.MsgDate),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                Case MessageClass.WarningMsg
                    MessageBox.Show(Msg.MsgText, String.Format(Language.Language.strTitleWarning, Msg.MsgDate),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case MessageClass.ErrorMsg
                    MessageBox.Show(Msg.MsgText, String.Format(Language.Language.strTitleError, Msg.MsgDate),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
        End Sub

#Region "Delegates"

        Private Delegate Sub ShowMCFormCB()

        Private Sub ShowMCForm()
            If frmMain.pnlDock.InvokeRequired Then
                Dim d As New ShowMCFormCB(AddressOf ShowMCForm)
                frmMain.pnlDock.Invoke(d)
            Else
                Me._MCForm.Show(frmMain.pnlDock)
            End If
        End Sub

#End Region
    End Class
End Namespace
