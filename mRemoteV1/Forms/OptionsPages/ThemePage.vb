Imports System.ComponentModel
Imports mRemote3G.Themes

Namespace Forms.OptionsPages
    Public Class ThemePage
        Public Overrides Property PageName As String
            Get
                Return Language.Language.strOptionsTabTheme
            End Get
            Set
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            btnThemeDelete.Text = Language.Language.strOptionsThemeButtonDelete
            btnThemeNew.Text = Language.Language.strOptionsThemeButtonNew
        End Sub

        Public Overrides Sub LoadSettings()
            MyBase.SaveSettings()

            _themeList = New BindingList(Of ThemeInfo)(ThemeManager.LoadThemes())
            cboTheme.DataSource = _themeList
            cboTheme.SelectedItem = ThemeManager.ActiveTheme
            cboTheme_SelectionChangeCommitted(Me, New EventArgs())

            ThemePropertyGrid.PropertySort = PropertySort.Categorized

            _originalTheme = ThemeManager.ActiveTheme
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            ThemeManager.SaveThemes(_themeList)
            My.Settings.ThemeName = ThemeManager.ActiveTheme.Name
        End Sub

        Public Overrides Sub RevertSettings()
            ThemeManager.ActiveTheme = _originalTheme
        End Sub

#Region "Private Fields"

        Private _themeList As BindingList(Of ThemeInfo)
        Private _originalTheme As ThemeInfo

#End Region

#Region "Private Methods"

#Region "Event Handlers"

        Private Sub cboTheme_DropDown(sender As Object, e As EventArgs) Handles cboTheme.DropDown
            If ThemeManager.ActiveTheme Is ThemeManager.DefaultTheme Then Return
            ThemeManager.ActiveTheme.Name = cboTheme.Text
        End Sub

        Private Sub cboTheme_SelectionChangeCommitted(sender As Object, e As EventArgs) _
            Handles cboTheme.SelectionChangeCommitted
            If cboTheme.SelectedItem Is Nothing Then cboTheme.SelectedItem = ThemeManager.DefaultTheme

            If cboTheme.SelectedItem Is ThemeManager.DefaultTheme Then
                cboTheme.DropDownStyle = ComboBoxStyle.DropDownList
                btnThemeDelete.Enabled = False
                ThemePropertyGrid.Enabled = False
            Else
                cboTheme.DropDownStyle = ComboBoxStyle.DropDown
                btnThemeDelete.Enabled = True
                ThemePropertyGrid.Enabled = True
            End If

            ThemeManager.ActiveTheme = cboTheme.SelectedItem
            ThemePropertyGrid.SelectedObject = ThemeManager.ActiveTheme
            ThemePropertyGrid.Refresh()
        End Sub

        Private Sub btnThemeNew_Click(sender As Object, e As EventArgs) Handles btnThemeNew.Click
            Dim newTheme As ThemeInfo = ThemeManager.ActiveTheme.Clone()
            newTheme.Name = Language.Language.strUnnamedTheme

            _themeList.Add(newTheme)

            cboTheme.SelectedItem = newTheme
            cboTheme_SelectionChangeCommitted(Me, New EventArgs())

            cboTheme.Focus()
        End Sub

        Private Sub btnThemeDelete_Click(sender As Object, e As EventArgs) Handles btnThemeDelete.Click
            Dim theme As ThemeInfo = cboTheme.SelectedItem
            If theme Is Nothing Then Return

            _themeList.Remove(theme)

            cboTheme.SelectedItem = ThemeManager.DefaultTheme
            cboTheme_SelectionChangeCommitted(Me, New EventArgs())
        End Sub

#End Region

#End Region
    End Class
End Namespace