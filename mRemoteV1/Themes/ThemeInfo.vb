Imports System.ComponentModel
Imports System.Reflection
Imports mRemote3G.Tools

Namespace Themes
    Public Class ThemeInfo
        Implements ICloneable, INotifyPropertyChanged

#Region "Public Methods"

        Public Sub New(Optional ByVal themeName As String = Nothing)
            If themeName IsNot Nothing Then Name = themeName
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone
            Return MemberwiseClone()
        End Function

        Public Overrides Function ToString() As String
            Return Name
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Dim otherTheme = TryCast(obj, ThemeInfo)
            If otherTheme Is Nothing Then Return False

            Dim themeInfoType As Type = (New ThemeInfo).GetType()
            Dim myProperty As Object
            Dim otherProperty As Object
            For Each propertyInfo As PropertyInfo In themeInfoType.GetProperties()
                myProperty = propertyInfo.GetValue(Me, Nothing)
                otherProperty = propertyInfo.GetValue(otherTheme, Nothing)
                If Not myProperty.Equals(otherProperty) Then Return False
            Next

            Return True
        End Function

#End Region

#Region "Events"

        Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub NotifyPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

#End Region

#Region "Properties"

        Private _name As String = Language.Language.strUnnamedTheme

        <Browsable(False)>
        Public Property Name As String
            Get
                Return _name
            End Get
            Set
                If _name = value Then Return
                _name = value
                NotifyPropertyChanged("Name")
            End Set
        End Property

#Region "General"

        Private _windowBackgroundColor As Color = SystemColors.AppWorkspace

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameWindowBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionWindowBackgroundColor")>
        Public Property WindowBackgroundColor As Color
            Get
                Return (_windowBackgroundColor)
            End Get
            Set
                If _windowBackgroundColor = value Then Return
                _windowBackgroundColor = value
                NotifyPropertyChanged("WindowBackgroundColor")
            End Set
        End Property

        Private _menuBackgroundColor As Color = SystemColors.Control

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameMenuBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionMenuBackgroundColor")>
        Public Property MenuBackgroundColor As Color
            Get
                Return _menuBackgroundColor
            End Get
            Set
                If _menuBackgroundColor = value Then Return
                _menuBackgroundColor = value
                NotifyPropertyChanged("MenuBackgroundColor")
            End Set
        End Property

        Private _menuTextColor As Color = SystemColors.ControlText

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameMenuTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionMenuTextColor")>
        Public Property MenuTextColor As Color
            Get
                Return _menuTextColor
            End Get
            Set
                If _menuTextColor = value Then Return
                _menuTextColor = value
                NotifyPropertyChanged("MenuTextColor")
            End Set
        End Property

        Private _toolbarBackgroundColor As Color = SystemColors.Control

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameToolbarBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionToolbarBackgroundColor")>
        Public Property ToolbarBackgroundColor As Color
            Get
                Return _toolbarBackgroundColor
            End Get
            Set
                If _toolbarBackgroundColor = value Or value.A < 255 Then Return
                _toolbarBackgroundColor = value
                NotifyPropertyChanged("ToolbarBackgroundColor")
            End Set
        End Property

        Private _toolbarTextColor As Color = SystemColors.ControlText

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameToolbarTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionToolbarTextColor")>
        Public Property ToolbarTextColor As Color
            Get
                Return _toolbarTextColor
            End Get
            Set
                If _toolbarTextColor = value Then Return
                _toolbarTextColor = value
                NotifyPropertyChanged("ToolbarTextColor")
            End Set
        End Property

#End Region

#Region "Connections Panel"

        Private _connectionsPanelBackgroundColor As Color = SystemColors.Window

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelBackgroundColor")>
        Public Property ConnectionsPanelBackgroundColor As Color
            Get
                Return _connectionsPanelBackgroundColor
            End Get
            Set
                If _connectionsPanelBackgroundColor = value Or value.A < 255 Then Return
                _connectionsPanelBackgroundColor = value
                NotifyPropertyChanged("ConnectionsPanelBackgroundColor")
            End Set
        End Property

        Private _connectionsPanelTextColor As Color = SystemColors.WindowText

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelTextColor")>
        Public Property ConnectionsPanelTextColor As Color
            Get
                Return _connectionsPanelTextColor
            End Get
            Set
                If _connectionsPanelTextColor = value Then Return
                _connectionsPanelTextColor = value
                NotifyPropertyChanged("ConnectionsPanelTextColor")
            End Set
        End Property

        Private _connectionsPanelTreeLineColor As Color = Color.Black

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelTreeLineColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelTreeLineColor")>
        Public Property ConnectionsPanelTreeLineColor As Color
            Get
                Return _connectionsPanelTreeLineColor
            End Get
            Set
                If _connectionsPanelTreeLineColor = value Then Return
                _connectionsPanelTreeLineColor = value
                NotifyPropertyChanged("ConnectionsPanelTreeLineColor")
            End Set
        End Property

        Private _searchBoxBackgroundColor As Color = SystemColors.Window

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameSearchBoxBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionSearchBoxBackgroundColor")>
        Public Property SearchBoxBackgroundColor As Color
            Get
                Return _searchBoxBackgroundColor
            End Get
            Set
                If _searchBoxBackgroundColor = value Or value.A < 255 Then Return
                _searchBoxBackgroundColor = value
                NotifyPropertyChanged("SearchBoxBackgroundColor")
            End Set
        End Property

        Private _searchBoxTextPromptColor As Color = SystemColors.GrayText

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameSearchBoxTextPromptColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionSearchBoxTextPromptColor")>
        Public Property SearchBoxTextPromptColor As Color
            Get
                Return _searchBoxTextPromptColor
            End Get
            Set
                If _searchBoxTextPromptColor = value Then Return
                _searchBoxTextPromptColor = value
                NotifyPropertyChanged("SearchBoxTextPromptColor")
            End Set
        End Property

        Private _searchBoxTextColor As Color = SystemColors.WindowText

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameSearchBoxTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionSearchBoxTextColor")>
        Public Property SearchBoxTextColor As Color
            Get
                Return _searchBoxTextColor
            End Get
            Set
                If _searchBoxTextColor = value Then Return
                _searchBoxTextColor = value
                NotifyPropertyChanged("SearchBoxTextColor")
            End Set
        End Property

#End Region

#Region "Config Panel"

        Private _configPanelBackgroundColor As Color = SystemColors.Window

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelBackgroundColor")>
        Public Property ConfigPanelBackgroundColor As Color
            Get
                Return _configPanelBackgroundColor
            End Get
            Set
                If _configPanelBackgroundColor = value Or value.A < 255 Then Return
                _configPanelBackgroundColor = value
                NotifyPropertyChanged("ConfigPanelBackgroundColor")
            End Set
        End Property

        Private _configPanelTextColor As Color = SystemColors.WindowText

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelTextColor")>
        Public Property ConfigPanelTextColor As Color
            Get
                Return _configPanelTextColor
            End Get
            Set
                If _configPanelTextColor = value Then Return
                _configPanelTextColor = value
                NotifyPropertyChanged("ConfigPanelTextColor")
            End Set
        End Property

        Private _configPanelCategoryTextColor As Color = SystemColors.ControlText

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelCategoryTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelCategoryTextColor")>
        Public Property ConfigPanelCategoryTextColor As Color
            Get
                Return _configPanelCategoryTextColor
            End Get
            Set
                If _configPanelCategoryTextColor = value Then Return
                _configPanelCategoryTextColor = value
                NotifyPropertyChanged("ConfigPanelCategoryTextColor")
            End Set
        End Property

        Private _configPanelHelpBackgroundColor As Color = SystemColors.Control

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelHelpBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelHelpBackgroundColor")>
        Public Property ConfigPanelHelpBackgroundColor As Color
            Get
                Return _configPanelHelpBackgroundColor
            End Get
            Set
                If _configPanelHelpBackgroundColor = value Or value.A < 255 Then Return
                _configPanelHelpBackgroundColor = value
                NotifyPropertyChanged("ConfigPanelHelpBackgroundColor")
            End Set
        End Property

        Private _configPanelHelpTextColor As Color = SystemColors.ControlText

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelHelpTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelHelpTextColor")>
        Public Property ConfigPanelHelpTextColor As Color
            Get
                Return _configPanelHelpTextColor
            End Get
            Set
                If _configPanelHelpTextColor = value Then Return
                _configPanelHelpTextColor = value
                NotifyPropertyChanged("ConfigPanelHelpTextColor")
            End Set
        End Property

        Private _configPanelGridLineColor As Color = SystemColors.InactiveBorder

        <LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelGridLineColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelGridLineColor")>
        Public Property ConfigPanelGridLineColor As Color
            Get
                Return _configPanelGridLineColor
            End Get
            Set
                If _configPanelGridLineColor = value Then Return
                _configPanelGridLineColor = value
                NotifyPropertyChanged("ConfigPanelGridLineColor")
            End Set
        End Property

#End Region

#End Region
    End Class
End Namespace
