Imports WeifenLuo.WinFormsUI.Docking

Namespace UI
    Namespace Window
        Public Class Base
            Inherits DockContent

#Region "Public Properties"
            Private _WindowType As UI.Window.Type
            Public Property WindowType() As UI.Window.Type
                Get
                    Return Me._WindowType
                End Get
                Set(ByVal value As UI.Window.Type)
                    Me._WindowType = value
                End Set
            End Property

            Private _DockPnl As DockContent
            Public Property DockPnl() As DockContent
                Get
                    Return Me._DockPnl
                End Get
                Set(ByVal value As DockContent)
                    Me._DockPnl = value
                End Set
            End Property
#End Region

#Region "Public Methods"
            Public Sub SetFormText(ByVal Text As String)
                Me.Text = Text
                Me.TabText = Text
            End Sub
#End Region

#Region "Private Methods"
            Private Sub Base_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
                If TypeOf Me Is Connection Then
                    frmMain.pnlDock.DocumentStyle = DocumentStyle.DockingSdi
                Else
                    frmMain.pnlDock.DocumentStyle = DocumentStyle.DockingWindow
                End If
            End Sub

            Private Sub Base_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
                Dim nonConnectionPanelCount As Integer = 0
                For Each document As DockContent In frmMain.pnlDock.Documents
                    If document IsNot Me And Not TypeOf document Is Connection Then
                        nonConnectionPanelCount = nonConnectionPanelCount + 1
                    End If
                Next

                If nonConnectionPanelCount = 0 Then
                    frmMain.pnlDock.DocumentStyle = DocumentStyle.DockingSdi
                Else
                    frmMain.pnlDock.DocumentStyle = DocumentStyle.DockingWindow
                End If
            End Sub
#End Region
        End Class
    End Namespace
End Namespace