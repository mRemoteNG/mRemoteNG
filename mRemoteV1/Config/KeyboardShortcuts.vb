Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports mRemoteNG.App.Runtime
Imports SharedLibraryNG

Namespace Config
    Public Class KeyboardShortcuts
#Region "Public Properties"
        Private Shared _defaultMap As KeyboardShortcutMap = Nothing
        Public Shared ReadOnly Property DefaultMap() As KeyboardShortcutMap
            Get
                LoadDefaultMap()
                Return _defaultMap
            End Get
        End Property

        Private Shared _map As KeyboardShortcutMap
        Public Shared Property Map() As KeyboardShortcutMap
            Get
                Load()
                Return _map
            End Get
            Set(value As KeyboardShortcutMap)
                CancelKeyNotifications()
                _map = value
                Save()
                RequestKeyNotifications(_handle)
            End Set
        End Property
#End Region

#Region "Public Methods"
        Public Shared Sub RequestKeyNotifications(ByVal handle As IntPtr)
            ' ReSharper disable LocalizableElement
            If handle = IntPtr.Zero Then Throw New ArgumentException("The handle cannot be null.", "handle")
            If Not _handle = IntPtr.Zero And Not _handle = handle Then Throw New ArgumentException("The handle must match the handle that was specified the first time this function was called.", "handle")
            ' ReSharper restore LocalizableElement
            _handle = handle
            For Each shortcutMapping As ShortcutMapping In Map.Mappings
                KeyboardHook.RequestKeyNotification(handle, shortcutMapping.Key.KeyCode, shortcutMapping.Key.ModifierKeys, False)
            Next
        End Sub

        Public Shared Function CommandFromHookKeyMessage(ByVal m As Message) As ShortcutCommand
            Dim msgData As KeyboardHook.HookKeyMsgData = Marshal.PtrToStructure(m.LParam, GetType(KeyboardHook.HookKeyMsgData))
            Return Map.GetCommand(msgData.KeyCode, msgData.ModifierKeys)
        End Function
#End Region

#Region "Private Fields"
        ' ReSharper disable once UnusedMember.Local
        Private Shared _keyboardHook As New KeyboardHook
        Private Shared _mapLoaded As Boolean = False
        Private Shared _handle As IntPtr = IntPtr.Zero
#End Region

#Region "Private Methods"
        Private Shared Sub LoadDefaultMap()
            If _defaultMap IsNot Nothing Then Return
            _defaultMap = New KeyboardShortcutMap()
            _defaultMap.AddFromConfigString(ShortcutCommand.PreviousTab, My.Settings.Properties("KeysPreviousTab").DefaultValue)
            _defaultMap.AddFromConfigString(ShortcutCommand.NextTab, My.Settings.Properties("KeysNextTab").DefaultValue)
        End Sub

        Private Shared Sub Load()
            If _mapLoaded Then Return
            _map = New KeyboardShortcutMap()
            _map.AddFromConfigString(ShortcutCommand.PreviousTab, My.Settings.KeysPreviousTab)
            _map.AddFromConfigString(ShortcutCommand.NextTab, My.Settings.KeysNextTab)
            _mapLoaded = True
        End Sub

        Private Shared Sub Save()
            My.Settings.KeysPreviousTab = _map.GetConfigString(ShortcutCommand.PreviousTab)
            My.Settings.KeysNextTab = _map.GetConfigString(ShortcutCommand.NextTab)
        End Sub

        Private Shared Sub CancelKeyNotifications()
            If _handle = IntPtr.Zero Then Return
            For Each shortcutMapping As ShortcutMapping In Map.Mappings
                KeyboardHook.CancelKeyNotification(_handle, shortcutMapping.Key.KeyCode, shortcutMapping.Key.ModifierKeys, False)
            Next
        End Sub
#End Region
    End Class

    Public Class KeyboardShortcutMap
        Implements ICloneable

#Region "Public Properties"
        Private ReadOnly _mappings As List(Of ShortcutMapping)
        Public ReadOnly Property Mappings As List(Of ShortcutMapping)
            Get
                Return _mappings
            End Get
        End Property
#End Region

#Region "Constructors"
        Public Sub New()
            _mappings = New List(Of ShortcutMapping)()
        End Sub

        Public Sub New(ByVal mappings As List(Of ShortcutMapping))
            _mappings = mappings
        End Sub
#End Region

#Region "Public Methods"
        Public Sub Add(ByVal command As ShortcutCommand, ByVal shortcutKey As ShortcutKey)
            If Mappings.Contains(New ShortcutMapping(command, shortcutKey)) Then Return
            Mappings.Add(New ShortcutMapping(command, shortcutKey))
        End Sub

        Public Sub AddRange(ByVal command As ShortcutCommand, ByVal shortcutKeys As IEnumerable(Of ShortcutKey))
            For Each shortcutKey As ShortcutKey In shortcutKeys
                Add(command, shortcutKey)
            Next
        End Sub

        Public Sub Remove(ByVal command As ShortcutCommand, ByVal shortcutKey As ShortcutKey)
            Mappings.Remove(New ShortcutMapping(command, shortcutKey))
        End Sub

        Public Sub AddFromConfigString(ByVal command As ShortcutCommand, ByVal configString As String)
            For Each shortcutKey As ShortcutKey In ParseConfigString(configString)
                Add(command, shortcutKey)
            Next
        End Sub

        Public Function GetConfigString(ByVal command As ShortcutCommand) As String
            Dim parts As New List(Of String)
            For Each shortcutKey As ShortcutKey In GetShortcutKeys(command)
                parts.Add(shortcutKey.ToConfigString())
            Next
            Return String.Join(", ", parts.ToArray())
        End Function

        Public Function GetShortcutKeys(ByVal command As ShortcutCommand) As ShortcutKey()
            Dim shortcutKeys As New List(Of ShortcutKey)
            For Each shortcutMapping As ShortcutMapping In Mappings
                If shortcutMapping.Command = command Then shortcutKeys.Add(shortcutMapping.Key)
            Next
            Return shortcutKeys.ToArray()
        End Function

        Public Sub SetShortcutKeys(ByVal command As ShortcutCommand, ByVal shortcutKeys As IEnumerable(Of ShortcutKey))
            ClearShortcutKeys(command)
            AddRange(command, shortcutKeys)
        End Sub

        Public Function GetCommand(ByVal keyCode As Int32, ByVal modifierKeys As KeyboardHook.ModifierKeys) As ShortcutCommand
            Return GetCommand(New ShortcutKey(keyCode, modifierKeys))
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim newMappings As New List(Of ShortcutMapping)()
            newMappings.AddRange(Mappings)
            Return New KeyboardShortcutMap(newMappings)
        End Function
#End Region

#Region "Private Methods"
        Private Shared Function ParseConfigString(ByVal shortcutKeysString As String) As ShortcutKey()
            Dim shortcutKeys As New List(Of ShortcutKey)
            For Each shortcutKeyString As String In shortcutKeysString.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
                Try
                    shortcutKeys.Add(ShortcutKey.FromConfigString(shortcutKeyString.Trim))
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(String.Format("KeyboardShortcuts.ParseShortcutKeysString({0}) failed at {1}.", shortcutKeysString, shortcutKeyString), ex, , True)
                    Continue For
                End Try
            Next
            Return shortcutKeys.ToArray()
        End Function

        Private Function GetCommand(ByVal shortcutKey As ShortcutKey) As ShortcutCommand
            For Each shortcutMapping As ShortcutMapping In Mappings
                If ShortcutKeysMatch(shortcutMapping.Key, shortcutKey) Then Return shortcutMapping.Command
            Next
            Return ShortcutCommand.None
        End Function

        Private Shared Function ShortcutKeysMatch(ByVal wanted As ShortcutKey, ByVal pressed As ShortcutKey) As Boolean
            If Not wanted.KeyCode = pressed.KeyCode Then Return False
            Return KeyboardHook.ModifierKeysMatch(wanted.ModifierKeys, pressed.ModifierKeys)
        End Function

        Private Sub ClearShortcutKeys(ByVal command As ShortcutCommand)
            Dim mappingsToRemove As New List(Of ShortcutMapping)
            For Each mapping As ShortcutMapping In Mappings
                If mapping.Command = command Then mappingsToRemove.Add(mapping)
            Next

            For Each mapping As ShortcutMapping In mappingsToRemove
                Mappings.Remove(mapping)
            Next
        End Sub
#End Region
    End Class

    <ImmutableObject(True)> _
    Public Class ShortcutMapping
        Implements IEquatable(Of ShortcutMapping)

#Region "Public Properties"
        Private ReadOnly _command As ShortcutCommand
        Public ReadOnly Property Command As ShortcutCommand
            Get
                Return _command
            End Get
        End Property

        Private ReadOnly _key As ShortcutKey
        Public ReadOnly Property Key As ShortcutKey
            Get
                Return _key
            End Get
        End Property
#End Region

#Region "Constructors"
        Public Sub New(ByVal command As ShortcutCommand, ByVal key As ShortcutKey)
            _command = command
            _key = Key
        End Sub
#End Region

#Region "Public Methods"
        Public Overloads Function Equals(other As ShortcutMapping) As Boolean Implements IEquatable(Of ShortcutMapping).Equals
            If Not Command = other.Command Then Return False
            If Not Key = other.Key Then Return False
            Return True
        End Function
#End Region
    End Class

    <ImmutableObject(True)> _
    Public Class ShortcutKey
        Implements IEquatable(Of ShortcutKey)

#Region "Public Properties"
        Private ReadOnly _keyCode As Int32
        Public ReadOnly Property KeyCode As Int32
            Get
                Return _keyCode
            End Get
        End Property

        Private ReadOnly _modifierKeys As KeyboardHook.ModifierKeys
        Public ReadOnly Property ModifierKeys As KeyboardHook.ModifierKeys
            Get
                Return _modifierKeys
            End Get
        End Property
#End Region

#Region "Constructors"
        Public Sub New(ByVal keyCode As Int32, ByVal modifierKeys As KeyboardHook.ModifierKeys)
            _keyCode = keyCode
            _modifierKeys = modifierKeys
        End Sub

        Public Sub New(ByVal keysValue As Keys)
            _keyCode = keysValue And Keys.KeyCode
            If Not (keysValue And Keys.Shift) = 0 Then _modifierKeys = _modifierKeys Or KeyboardHook.ModifierKeys.Shift
            If Not (keysValue And Keys.Control) = 0 Then _modifierKeys = _modifierKeys Or KeyboardHook.ModifierKeys.Control
            If Not (keysValue And Keys.Alt) = 0 Then _modifierKeys = _modifierKeys Or KeyboardHook.ModifierKeys.Alt
        End Sub
#End Region

#Region "Public Methods"
        Public Function ToConfigString() As String
            Return String.Join("/", New String() {KeyCode, Convert.ToInt32(ModifierKeys)})
        End Function

        Public Shared Function FromConfigString(ByVal shortcutKeyString As String) As ShortcutKey
            Dim parts As String() = shortcutKeyString.Split(New Char() {"/"}, 2)
            If Not parts.Length = 2 Then Throw New ArgumentException(String.Format("ShortcutKey.FromString({0}) failed. parts.Length != 2", shortcutKeyString), shortcutKeyString)
            Return New ShortcutKey(Convert.ToInt32(parts(0)), Convert.ToInt32(parts(1)))
        End Function

        Public Overrides Function ToString() As String
            Return HotkeyControl.KeysToString(Me)
        End Function

        Public Overloads Function Equals(other As ShortcutKey) As Boolean Implements IEquatable(Of ShortcutKey).Equals
            If Not KeyCode = other.KeyCode Then Return False
            If Not ModifierKeys = other.ModifierKeys Then Return False
            Return True
        End Function
#End Region

#Region "Operators"
        Public Shared Operator =(ByVal shortcutKey1 As ShortcutKey, ByVal shortcutKey2 As ShortcutKey) As Boolean
            Return shortcutKey1.Equals(shortcutKey2)
        End Operator

        Public Shared Operator <>(ByVal shortcutKey1 As ShortcutKey, ByVal shortcutKey2 As ShortcutKey) As Boolean
            Return Not shortcutKey1.Equals(shortcutKey2)
        End Operator

        ' This is a narrowing conversion because (Keys Or Keys.Modifiers) cannot hold all possible values of KeyboardHook.ModifierKeys
        Public Shared Narrowing Operator CType(ByVal shortcutKey As ShortcutKey) As Keys
            Dim keysValue As Keys = shortcutKey.KeyCode And Keys.KeyCode
            If Not (shortcutKey.ModifierKeys And KeyboardHook.ModifierKeys.Shift) = 0 Then keysValue = keysValue Or Keys.Shift
            If Not (shortcutKey.ModifierKeys And KeyboardHook.ModifierKeys.Control) = 0 Then keysValue = keysValue Or Keys.Control
            If Not (shortcutKey.ModifierKeys And KeyboardHook.ModifierKeys.Alt) = 0 Then keysValue = keysValue Or Keys.Alt
            Return keysValue
        End Operator

        Public Shared Widening Operator CType(ByVal keys As Keys) As ShortcutKey
            Return New ShortcutKey(keys)
        End Operator
#End Region
    End Class

    Public Enum ShortcutCommand As Integer
        None = 0
        PreviousTab
        NextTab
    End Enum
End Namespace