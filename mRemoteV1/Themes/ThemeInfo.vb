Imports System.ComponentModel
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
#End Region

#Region "Events"
        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        Protected Sub NotifyPropertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
#End Region

#Region "Properties"
        Private _name As String = "Unnamed Theme"
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
        <Category(vbTab & "General"), _
            DisplayNameAttribute("Window Background Color"), _
            DescriptionAttribute("Description")> _
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
        <Category(vbTab & "General"),
            Browsable(False),
            DisplayNameAttribute("Menu Background Color"),
            DescriptionAttribute("Description")>
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
        <Category(vbTab & "General"),
            Browsable(False),
            DisplayNameAttribute("Menu Text Color"),
            DescriptionAttribute("Description")>
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
        <Category(vbTab & "General"), _
            Browsable(False), _
            DisplayNameAttribute("Toolbar Background Color"), _
            DescriptionAttribute("Description")> _
        Public Property ToolbarBackgroundColor() As Color
            Get
                Return _toolbarBackgroundColor
            End Get
            Set(value As Color)
                If _toolbarBackgroundColor = value Then Return
                _toolbarBackgroundColor = value
                NotifyPropertyChanged("ToolbarBackgroundColor")
            End Set
        End Property

        Private _toolbarTextColor As Color = SystemColors.ControlText
        <Category(vbTab & "General"), _
            Browsable(False), _
            DisplayNameAttribute("Toolbar Text Color"), _
            DescriptionAttribute("Description")> _
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
        <Category("Connections Panel"), _
            DisplayNameAttribute("Connections Panel Background Color"), _
            DescriptionAttribute("Description")> _
        Public Property ConnectionsPanelBackgroundColor() As Color
            Get
                Return _connectionsPanelBackgroundColor
            End Get
            Set(value As Color)
                If _connectionsPanelBackgroundColor = value Or value = Color.Transparent Then Return
                _connectionsPanelBackgroundColor = value
                NotifyPropertyChanged("ConnectionsPanelBackgroundColor")
            End Set
        End Property

        Private _connectionsPanelTextColor As Color = SystemColors.WindowText
        <Category("Connections Panel Color"),
            DisplayNameAttribute("Connections Panel Text"),
            DescriptionAttribute("Description")>
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
        <Category("Connections Panel"),
            DisplayNameAttribute("Connections Panel Tree Line Color"),
            DescriptionAttribute("Description")>
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
        <Category("Connections Panel"), _
            DisplayNameAttribute("Search Box Background Color"), _
            DescriptionAttribute("Description")> _
        Public Property SearchBoxBackgroundColor() As Color
            Get
                Return _searchBoxBackgroundColor
            End Get
            Set(value As Color)
                If _searchBoxBackgroundColor = value Or value = Color.Transparent Then Return
                _searchBoxBackgroundColor = value
                NotifyPropertyChanged("SearchBoxBackgroundColor")
            End Set
        End Property

        Private _searchBoxTextPromptColor As Color = SystemColors.GrayText
        <Category("Connections Panel"), _
            DisplayNameAttribute("Search Box Text Prompt Color"), _
            DescriptionAttribute("Description")> _
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
        <Category("Connections Panel"), _
            DisplayNameAttribute("Search Box Text Color"), _
            DescriptionAttribute("Description")> _
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
        <Category("Config Panel"), _
            DisplayNameAttribute("Config Panel Background Color"), _
            DescriptionAttribute("Description")> _
        Public Property ConfigPanelBackgroundColor() As Color
            Get
                Return _configPanelBackgroundColor
            End Get
            Set(value As Color)
                If _configPanelBackgroundColor = value Or value = Color.Transparent Then Return
                _configPanelBackgroundColor = value
                NotifyPropertyChanged("ConfigPanelBackgroundColor")
            End Set
        End Property

        Private _configPanelTextColor As Color = SystemColors.WindowText
        <Category("Config Panel"), _
            DisplayNameAttribute("Config Panel Text Color"), _
            DescriptionAttribute("Description")> _
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
        <Category("Config Panel"), _
            DisplayNameAttribute("Config Panel Category Text Color"), _
            DescriptionAttribute("Description")> _
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
        <Category("Config Panel"), _
            DisplayNameAttribute("Config Panel Help Background Color"), _
            DescriptionAttribute("Description")> _
        Public Property ConfigPanelHelpBackgroundColor() As Color
            Get
                Return _configPanelHelpBackgroundColor
            End Get
            Set(value As Color)
                If _configPanelHelpBackgroundColor = value Or value = Color.Transparent Then Return
                _configPanelHelpBackgroundColor = value
                NotifyPropertyChanged("ConfigPanelHelpBackgroundColor")
            End Set
        End Property

        Private _configPanelHelpTextColor As Color = SystemColors.ControlText
        <Category("Config Panel"), _
            DisplayNameAttribute("Config Panel Help Text Color"), _
            DescriptionAttribute("Description")> _
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
        <Category("Config Panel"), _
            DisplayNameAttribute("Config Panel Grid Line Color"), _
            DescriptionAttribute("Description")> _
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
