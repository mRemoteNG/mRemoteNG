Imports System.ComponentModel
Imports mRemoteNG.Tools.LocalizedAttributes
Imports mRemoteNG.My

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
            Dim otherTheme As ThemeInfo = TryCast(obj, ThemeInfo)
            If otherTheme Is Nothing Then Return False

            Dim themeInfoType As Type = (New ThemeInfo).GetType()
            Dim myProperty As Object
            Dim otherProperty As Object
            For Each propertyInfo As Reflection.PropertyInfo In themeInfoType.GetProperties()
                myProperty = propertyInfo.GetValue(Me, Nothing)
                otherProperty = propertyInfo.GetValue(otherTheme, Nothing)
                If Not myProperty.Equals(otherProperty) Then Return False
            Next

            Return True
        End Function
#End Region

#Region "Events"
        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        Protected Sub NotifyPropertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
#End Region

#Region "Properties"
        Private _name As String = Language.strUnnamedTheme
        <Browsable(False)> _
        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                If _name = value Then Return
                _name = value
                NotifyPropertyChanged("Name")
            End Set
        End Property

#Region "General"
        Private _windowBackgroundColor As Color = SystemColors.AppWorkspace
        <LocalizedCategory("strThemeCategoryGeneral", 1), _
            LocalizedDisplayName("strThemeNameWindowBackgroundColor"), _
            LocalizedDescription("strThemeDescriptionWindowBackgroundColor")> _
        Public Property WindowBackgroundColor() As Color
            Get
                Return (_windowBackgroundColor)
            End Get
            Set(value As Color)
                If _windowBackgroundColor = value Then Return
                _windowBackgroundColor = value
                NotifyPropertyChanged("WindowBackgroundColor")
            End Set
        End Property

        Private _menuBackgroundColor As Color = SystemColors.Control
        <LocalizedCategory("strThemeCategoryGeneral", 1), _
            Browsable(False),
            LocalizedDisplayName("strThemeNameMenuBackgroundColor"),
            LocalizedDescription("strThemeDescriptionMenuBackgroundColor")>
        Public Property MenuBackgroundColor() As Color
            Get
                Return _menuBackgroundColor
            End Get
            Set(value As Color)
                If _menuBackgroundColor = value Then Return
                _menuBackgroundColor = value
                NotifyPropertyChanged("MenuBackgroundColor")
            End Set
        End Property

        Private _menuTextColor As Color = SystemColors.ControlText
        <LocalizedCategory("strThemeCategoryGeneral", 1), _
            Browsable(False),
            LocalizedDisplayName("strThemeNameMenuTextColor"),
            LocalizedDescription("strThemeDescriptionMenuTextColor")>
        Public Property MenuTextColor() As Color
            Get
                Return _menuTextColor
            End Get
            Set(value As Color)
                If _menuTextColor = value Then Return
                _menuTextColor = value
                NotifyPropertyChanged("MenuTextColor")
            End Set
        End Property

        Private _toolbarBackgroundColor As Color = SystemColors.Control
        <LocalizedCategory("strThemeCategoryGeneral", 1), _
            Browsable(False), _
            LocalizedDisplayName("strThemeNameToolbarBackgroundColor"), _
            LocalizedDescription("strThemeDescriptionToolbarBackgroundColor")> _
        Public Property ToolbarBackgroundColor() As Color
            Get
                Return _toolbarBackgroundColor
            End Get
            Set(value As Color)
                If _toolbarBackgroundColor = value Or value.A < 255 Then Return
                _toolbarBackgroundColor = value
                NotifyPropertyChanged("ToolbarBackgroundColor")
            End Set
        End Property

        Private _toolbarTextColor As Color = SystemColors.ControlText
        <LocalizedCategory("strThemeCategoryGeneral", 1), _
            Browsable(False), _
            LocalizedDisplayName("strThemeNameToolbarTextColor"), _
            LocalizedDescription("strThemeDescriptionToolbarTextColor")> _
        Public Property ToolbarTextColor() As Color
            Get
                Return _toolbarTextColor
            End Get
            Set(value As Color)
                If _toolbarTextColor = value Then Return
                _toolbarTextColor = value
                NotifyPropertyChanged("ToolbarTextColor")
            End Set
        End Property
#End Region

#Region "Connections Panel"
        Private _connectionsPanelBackgroundColor As Color = SystemColors.Window
        <LocalizedCategory("strThemeCategoryConnectionsPanel", 2), _
            LocalizedDisplayName("strThemeNameConnectionsPanelBackgroundColor"), _
            LocalizedDescription("strThemeDescriptionConnectionsPanelBackgroundColor")> _
        Public Property ConnectionsPanelBackgroundColor() As Color
            Get
                Return _connectionsPanelBackgroundColor
            End Get
            Set(value As Color)
                If _connectionsPanelBackgroundColor = value Or value.A < 255 Then Return
                _connectionsPanelBackgroundColor = value
                NotifyPropertyChanged("ConnectionsPanelBackgroundColor")
            End Set
        End Property

        Private _connectionsPanelTextColor As Color = SystemColors.WindowText
        <LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedDisplayName("strThemeNameConnectionsPanelTextColor"),
            LocalizedDescription("strThemeDescriptionConnectionsPanelTextColor")>
        Public Property ConnectionsPanelTextColor() As Color
            Get
                Return _connectionsPanelTextColor
            End Get
            Set(value As Color)
                If _connectionsPanelTextColor = value Then Return
                _connectionsPanelTextColor = value
                NotifyPropertyChanged("ConnectionsPanelTextColor")
            End Set
        End Property

        Private _connectionsPanelTreeLineColor As Color = Color.Black
        <LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedDisplayName("strThemeNameConnectionsPanelTreeLineColor"),
            LocalizedDescription("strThemeDescriptionConnectionsPanelTreeLineColor")>
        Public Property ConnectionsPanelTreeLineColor() As Color
            Get
                Return _connectionsPanelTreeLineColor
            End Get
            Set(value As Color)
                If _connectionsPanelTreeLineColor = value Then Return
                _connectionsPanelTreeLineColor = value
                NotifyPropertyChanged("ConnectionsPanelTreeLineColor")
            End Set
        End Property

        Private _searchBoxBackgroundColor As Color = SystemColors.Window
        <LocalizedCategory("strThemeCategoryConnectionsPanel", 2), _
            LocalizedDisplayName("strThemeNameSearchBoxBackgroundColor"), _
            LocalizedDescription("strThemeDescriptionSearchBoxBackgroundColor")> _
        Public Property SearchBoxBackgroundColor() As Color
            Get
                Return _searchBoxBackgroundColor
            End Get
            Set(value As Color)
                If _searchBoxBackgroundColor = value Or value.A < 255 Then Return
                _searchBoxBackgroundColor = value
                NotifyPropertyChanged("SearchBoxBackgroundColor")
            End Set
        End Property

        Private _searchBoxTextPromptColor As Color = SystemColors.GrayText
        <LocalizedCategory("strThemeCategoryConnectionsPanel", 2), _
            LocalizedDisplayName("strThemeNameSearchBoxTextPromptColor"), _
            LocalizedDescription("strThemeDescriptionSearchBoxTextPromptColor")> _
        Public Property SearchBoxTextPromptColor() As Color
            Get
                Return _searchBoxTextPromptColor
            End Get
            Set(value As Color)
                If _searchBoxTextPromptColor = value Then Return
                _searchBoxTextPromptColor = value
                NotifyPropertyChanged("SearchBoxTextPromptColor")
            End Set
        End Property

        Private _searchBoxTextColor As Color = SystemColors.WindowText
        <LocalizedCategory("strThemeCategoryConnectionsPanel", 2), _
            LocalizedDisplayName("strThemeNameSearchBoxTextColor"), _
            LocalizedDescription("strThemeDescriptionSearchBoxTextColor")> _
        Public Property SearchBoxTextColor() As Color
            Get
                Return _searchBoxTextColor
            End Get
            Set(value As Color)
                If _searchBoxTextColor = value Then Return
                _searchBoxTextColor = value
                NotifyPropertyChanged("SearchBoxTextColor")
            End Set
        End Property
#End Region

#Region "Config Panel"
        Private _configPanelBackgroundColor As Color = SystemColors.Window
        <LocalizedCategory("strThemeCategoryConfigPanel", 3), _
            LocalizedDisplayName("strThemeNameConfigPanelBackgroundColor"), _
            LocalizedDescription("strThemeDescriptionConfigPanelBackgroundColor")> _
        Public Property ConfigPanelBackgroundColor() As Color
            Get
                Return _configPanelBackgroundColor
            End Get
            Set(value As Color)
                If _configPanelBackgroundColor = value Or value.A < 255 Then Return
                _configPanelBackgroundColor = value
                NotifyPropertyChanged("ConfigPanelBackgroundColor")
            End Set
        End Property

        Private _configPanelTextColor As Color = SystemColors.WindowText
        <LocalizedCategory("strThemeCategoryConfigPanel", 3), _
            LocalizedDisplayName("strThemeNameConfigPanelTextColor"), _
            LocalizedDescription("strThemeDescriptionConfigPanelTextColor")> _
        Public Property ConfigPanelTextColor() As Color
            Get
                Return _configPanelTextColor
            End Get
            Set(value As Color)
                If _configPanelTextColor = value Then Return
                _configPanelTextColor = value
                NotifyPropertyChanged("ConfigPanelTextColor")
            End Set
        End Property

        Private _configPanelCategoryTextColor As Color = SystemColors.ControlText
        <LocalizedCategory("strThemeCategoryConfigPanel", 3), _
            LocalizedDisplayName("strThemeNameConfigPanelCategoryTextColor"), _
            LocalizedDescription("strThemeDescriptionConfigPanelCategoryTextColor")> _
        Public Property ConfigPanelCategoryTextColor() As Color
            Get
                Return _configPanelCategoryTextColor
            End Get
            Set(value As Color)
                If _configPanelCategoryTextColor = value Then Return
                _configPanelCategoryTextColor = value
                NotifyPropertyChanged("ConfigPanelCategoryTextColor")
            End Set
        End Property

        Private _configPanelHelpBackgroundColor As Color = SystemColors.Control
        <LocalizedCategory("strThemeCategoryConfigPanel", 3), _
            LocalizedDisplayName("strThemeNameConfigPanelHelpBackgroundColor"), _
            LocalizedDescription("strThemeDescriptionConfigPanelHelpBackgroundColor")> _
        Public Property ConfigPanelHelpBackgroundColor() As Color
            Get
                Return _configPanelHelpBackgroundColor
            End Get
            Set(value As Color)
                If _configPanelHelpBackgroundColor = value Or value.A < 255 Then Return
                _configPanelHelpBackgroundColor = value
                NotifyPropertyChanged("ConfigPanelHelpBackgroundColor")
            End Set
        End Property

        Private _configPanelHelpTextColor As Color = SystemColors.ControlText
        <LocalizedCategory("strThemeCategoryConfigPanel", 3), _
            LocalizedDisplayName("strThemeNameConfigPanelHelpTextColor"), _
            LocalizedDescription("strThemeDescriptionConfigPanelHelpTextColor")> _
        Public Property ConfigPanelHelpTextColor() As Color
            Get
                Return _configPanelHelpTextColor
            End Get
            Set(value As Color)
                If _configPanelHelpTextColor = value Then Return
                _configPanelHelpTextColor = value
                NotifyPropertyChanged("ConfigPanelHelpTextColor")
            End Set
        End Property

        Private _configPanelGridLineColor As Color = SystemColors.InactiveBorder
        <LocalizedCategory("strThemeCategoryConfigPanel", 3), _
            LocalizedDisplayName("strThemeNameConfigPanelGridLineColor"), _
            LocalizedDescription("strThemeDescriptionConfigPanelGridLineColor")> _
        Public Property ConfigPanelGridLineColor() As Color
            Get
                Return _configPanelGridLineColor
            End Get
            Set(value As Color)
                If _configPanelGridLineColor = value Then Return
                _configPanelGridLineColor = value
                NotifyPropertyChanged("ConfigPanelGridLineColor")
            End Set
        End Property
#End Region
#End Region
    End Class
End Namespace
