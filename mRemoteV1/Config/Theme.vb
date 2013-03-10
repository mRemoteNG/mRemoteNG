Imports System.ComponentModel

Namespace Config
    Public Class Theme
#Region "Public Methods"
        Public Sub FromTheme(ByVal theme As Theme)
            BeginUpdate()

            Name = theme.Name

            Dim themeType As Type = (New Theme).GetType()
            Dim colorType As Type = (New Color).GetType()
            Dim color As Color
            For Each propertyInfo As Reflection.PropertyInfo In themeType.GetProperties()
                If Not propertyInfo.PropertyType Is colorType Then Continue For
                color = propertyInfo.GetValue(theme, Nothing)
                propertyInfo.SetValue(Me, color, Nothing)
            Next

            EndUpdate()
        End Sub

        Public Sub BeginUpdate()
            _inUpdate = True
        End Sub

        Public Sub EndUpdate()
            _inUpdate = False
            RaiseColorChanged()
        End Sub
#End Region

#Region "Events"
        Public Event ColorChanged()
        Protected Sub RaiseColorChanged()
            If Not _inUpdate Then RaiseEvent ColorChanged()
        End Sub
#End Region

#Region "Private Fields"
        Private _inUpdate As Boolean = vbFalse
#End Region

#Region "Properties"
        <Browsable(False)> _
        Public Property Name As String = "Unnamed Theme"

#Region "General"
        Private _windowBackground As Color = SystemColors.AppWorkspace
        <Category(vbTab & "General"), _
            Browsable(True), _
            DisplayNameAttribute("Window Background"), _
            DescriptionAttribute("Description")> _
        Public Property WindowBackground() As Color
            Get
                Return (_windowBackground)
            End Get
            Set(value As Color)
                If _windowBackground = value Then Return
                _windowBackground = value
                RaiseColorChanged()
            End Set
        End Property

        Private _menuBackground As Color = SystemColors.Control
        <Category(vbTab & "General"),
            Browsable(True),
            DisplayNameAttribute("Menu Background"),
            DescriptionAttribute("Description")>
        Public Property MenuBackground() As Color
            Get
                Return _menuBackground
            End Get
            Set(value As Color)
                If _menuBackground = value Then Return
                _menuBackground = value
                RaiseColorChanged()
            End Set
        End Property

        Private _menuText As Color = SystemColors.ControlText
        <Category(vbTab & "General"),
            Browsable(True),
            DisplayNameAttribute("Menu Text"),
            DescriptionAttribute("Description")>
        Public Property MenuText() As Color
            Get
                Return _menuText
            End Get
            Set(value As Color)
                If _menuText = value Then Return
                _menuText = value
                RaiseColorChanged()
            End Set
        End Property

        Private _toolbarBackground As Color = SystemColors.Control
        <Category(vbTab & "General"), _
            Browsable(True), _
            DisplayNameAttribute("Toolbar Background"), _
            DescriptionAttribute("Description")> _
        Public Property ToolbarBackground() As Color
            Get
                Return _toolbarBackground
            End Get
            Set(value As Color)
                If _toolbarBackground = value Then Return
                _toolbarBackground = value
                RaiseColorChanged()
            End Set
        End Property

        Private _toolbarText As Color = SystemColors.ControlText
        <Category(vbTab & "General"), _
            Browsable(True), _
            DisplayNameAttribute("Toolbar Text"), _
            DescriptionAttribute("Description")> _
        Public Property ToolbarText() As Color
            Get
                Return _toolbarText
            End Get
            Set(value As Color)
                If _toolbarText = value Then Return
                _toolbarText = value
                RaiseColorChanged()
            End Set
        End Property
#End Region

#Region "Connections Panel"
        Private _connectionsPanelBackground As Color = SystemColors.Window
        <Category("Connections Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Connections Panel Background"), _
            DescriptionAttribute("Description")> _
        Public Property ConnectionsPanelBackground() As Color
            Get
                Return _connectionsPanelBackground
            End Get
            Set(value As Color)
                If _connectionsPanelBackground = value Then Return
                _connectionsPanelBackground = value
                RaiseColorChanged()
            End Set
        End Property

        Private _connectionsPanelText As Color = SystemColors.WindowText
        <Category("Connections Panel"),
            Browsable(True),
            DisplayNameAttribute("Connections Panel Text"),
            DescriptionAttribute("Description")>
        Public Property ConnectionsPanelText() As Color
            Get
                Return _connectionsPanelText
            End Get
            Set(value As Color)
                If _connectionsPanelText = value Then Return
                _connectionsPanelText = value
                RaiseColorChanged()
            End Set
        End Property

        Private _connectionsPanelTreeLines As Color = Color.Black
        <Category("Connections Panel"),
            Browsable(True),
            DisplayNameAttribute("Connections Panel Tree Lines"),
            DescriptionAttribute("Description")>
        Public Property ConnectionsPanelTreeLines() As Color
            Get
                Return _connectionsPanelTreeLines
            End Get
            Set(value As Color)
                If _connectionsPanelTreeLines = value Then Return
                _connectionsPanelTreeLines = value
                RaiseColorChanged()
            End Set
        End Property

        Private _searchBoxBackground As Color = SystemColors.Window
        <Category("Connections Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Search Box Background"), _
            DescriptionAttribute("Description")> _
        Public Property SearchBoxBackground() As Color
            Get
                Return _searchBoxBackground
            End Get
            Set(value As Color)
                If _searchBoxBackground = value Then Return
                _searchBoxBackground = value
                RaiseColorChanged()
            End Set
        End Property

        Private _searchBoxTextUnfocused As Color = SystemColors.GrayText
        <Category("Connections Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Search Box Text Unfocused"), _
            DescriptionAttribute("Description")> _
        Public Property SearchBoxTextUnfocused() As Color
            Get
                Return _searchBoxTextUnfocused
            End Get
            Set(value As Color)
                If _searchBoxTextUnfocused = value Then Return
                _searchBoxTextUnfocused = value
                RaiseColorChanged()
            End Set
        End Property

        Private _searchBoxTextFocused As Color = SystemColors.WindowText
        <Category("Connections Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Search Box Text Focused"), _
            DescriptionAttribute("Description")> _
        Public Property SearchBoxTextFocused() As Color
            Get
                Return _searchBoxTextFocused
            End Get
            Set(value As Color)
                If _searchBoxTextFocused = value Then Return
                _searchBoxTextFocused = value
                RaiseColorChanged()
            End Set
        End Property
#End Region

#Region "Config Panel"
        Private _configPanelBackground As Color = SystemColors.Window
        <Category("Config Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Config Panel Background"), _
            DescriptionAttribute("Description")> _
        Public Property ConfigPanelBackground() As Color
            Get
                Return _configPanelBackground
            End Get
            Set(value As Color)
                If _configPanelBackground = value Then Return
                _configPanelBackground = value
                RaiseColorChanged()
            End Set
        End Property

        Private _configPanelText As Color = SystemColors.WindowText
        <Category("Config Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Config Panel Text"), _
            DescriptionAttribute("Description")> _
        Public Property ConfigPanelText() As Color
            Get
                Return _configPanelText
            End Get
            Set(value As Color)
                If _configPanelText = value Then Return
                _configPanelText = value
                RaiseColorChanged()
            End Set
        End Property

        Private _configPanelCategoryText As Color = SystemColors.ControlText
        <Category("Config Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Config Panel Category Text"), _
            DescriptionAttribute("Description")> _
        Public Property ConfigPanelCategoryText() As Color
            Get
                Return _configPanelCategoryText
            End Get
            Set(value As Color)
                If _configPanelCategoryText = value Then Return
                _configPanelCategoryText = value
                RaiseColorChanged()
            End Set
        End Property

        Private _configPanelHelpBackground As Color = SystemColors.Control
        <Category("Config Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Config Panel Help Background"), _
            DescriptionAttribute("Description")> _
        Public Property ConfigPanelHelpBackground() As Color
            Get
                Return _configPanelHelpBackground
            End Get
            Set(value As Color)
                If _configPanelHelpBackground = value Then Return
                _configPanelHelpBackground = value
                RaiseColorChanged()
            End Set
        End Property

        Private _configPanelHelpText As Color = SystemColors.ControlText
        <Category("Config Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Config Panel Help Text"), _
            DescriptionAttribute("Description")> _
        Public Property ConfigPanelHelpText() As Color
            Get
                Return _configPanelHelpText
            End Get
            Set(value As Color)
                If _configPanelHelpText = value Then Return
                _configPanelHelpText = value
                RaiseColorChanged()
            End Set
        End Property

        Private _configPanelGridLines As Color = SystemColors.InactiveBorder
        <Category("Config Panel"), _
            Browsable(True), _
            DisplayNameAttribute("Config Panel Grid Lines"), _
            DescriptionAttribute("Description")> _
        Public Property ConfigPanelGridLines() As Color
            Get
                Return _configPanelGridLines
            End Get
            Set(value As Color)
                If _configPanelGridLines = value Then Return
                _configPanelGridLines = value
                RaiseColorChanged()
            End Set
        End Property
#End Region
#End Region
    End Class
End Namespace
