Imports System.ComponentModel

Namespace Forms.OptionsPages
    Public Class OptionsPage
        Inherits UserControl

#Region "Public Properties"

        <Browsable(False)>
        Public Overridable Property PageName As String

        Public Overridable Property PageIcon As Icon

#End Region

#Region "Public Methods"

        Public Overridable Sub ApplyLanguage()
        End Sub

        Public Overridable Sub LoadSettings()
        End Sub

        Public Overridable Sub SaveSettings()
        End Sub

        Public Overridable Sub RevertSettings()
        End Sub

#End Region
    End Class
End Namespace