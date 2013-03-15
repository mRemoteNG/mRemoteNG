Imports System.IO
Imports System.ComponentModel

Namespace Themes
    Public Class ThemeManager
#Region "Public Methods"
        Public Shared Function LoadTheme(ByVal themeName As String, Optional ByVal setActive As Boolean = True) As ThemeInfo
            Dim loadedTheme As ThemeInfo = DefaultTheme

            If Not String.IsNullOrEmpty(themeName) Then
                For Each theme As ThemeInfo In LoadThemes()
                    If theme.Name = themeName Then
                        loadedTheme = theme
                        Exit For
                    End If
                Next
            End If

            If setActive Then ActiveTheme = loadedTheme
            Return loadedTheme
        End Function

        Public Shared Function LoadThemes() As List(Of ThemeInfo)
            Dim themes As New List(Of ThemeInfo)
            themes.Add(DefaultTheme)
            themes.AddRange(ThemeSerializer.LoadFromXmlFile(Path.Combine(App.Info.Settings.SettingsPath, App.Info.Settings.ThemesFileName)))
            Return themes
        End Function

        Public Shared Sub SaveThemes(ByVal themes As List(Of ThemeInfo))
            themes.Remove(DefaultTheme)
            ThemeSerializer.SaveToXmlFile(themes, Path.Combine(App.Info.Settings.SettingsPath, App.Info.Settings.ThemesFileName))
        End Sub

        Public Shared Sub SaveThemes(ByVal themes As ThemeInfo())
            SaveThemes(New List(Of ThemeInfo)(themes))
        End Sub

        Public Shared Sub SaveThemes(ByVal themes As BindingList(Of ThemeInfo))
            Dim themesArray(themes.Count - 1) As ThemeInfo
            themes.CopyTo(themesArray, 0)
            SaveThemes(themesArray)
        End Sub
#End Region

#Region "Events"
        Public Shared Event ThemeChanged()
        Protected Shared Sub NotifyThemeChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs)
            If e.PropertyName = "Name" Then Return
            RaiseEvent ThemeChanged()
        End Sub
#End Region

#Region "Properties"
        ' ReSharper disable InconsistentNaming
        Private Shared ReadOnly _defaultTheme As New ThemeInfo("(Default Theme)")
        ' ReSharper restore InconsistentNaming
        Public Shared ReadOnly Property DefaultTheme As ThemeInfo
            Get
                Return _defaultTheme
            End Get
        End Property

        Private Shared _activeTheme As ThemeInfo
        Private Shared _activeThemeHandlerSet As Boolean = False
        Public Shared Property ActiveTheme As ThemeInfo
            Get
                Return _activeTheme
            End Get
            Set(value As ThemeInfo)
                If _activeTheme Is Nothing OrElse Not _activeTheme.Equals(value) Then
                    If _activeThemeHandlerSet Then RemoveHandler _activeTheme.PropertyChanged, AddressOf NotifyThemeChanged

                    _activeTheme = value

                    AddHandler _activeTheme.PropertyChanged, AddressOf NotifyThemeChanged
                    _activeThemeHandlerSet = True

                    NotifyThemeChanged(_activeTheme, New PropertyChangedEventArgs(""))
                End If
            End Set
        End Property
#End Region
    End Class
End Namespace
