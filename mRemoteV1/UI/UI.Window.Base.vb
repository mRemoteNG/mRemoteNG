Imports System.Windows.Forms
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

        End Class
    End Namespace
End Namespace