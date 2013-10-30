Imports System.Runtime.InteropServices
Imports mRemoteNG.App.Runtime
Imports SharedLibraryNG

Namespace Config
    Public Class KeyboardShortcuts
#Region "Public Methods"
        Public Shared Sub RequestKeyNotifications(ByVal handle As IntPtr)
            For Each shortcutKey As ShortcutKey In Mappings.Keys
                KeyboardHook.RequestKeyNotification(handle, shortcutKey.KeyCode, shortcutKey.ModifierKeys, False)
            Next
        End Sub

        Public Shared Function CommandFromHookKeyMessage(ByVal m As Message) As Command
            Dim msgData As KeyboardHook.HookKeyMsgData = Marshal.PtrToStructure(m.LParam, GetType(KeyboardHook.HookKeyMsgData))
            Return GetCommand(msgData.KeyCode, msgData.ModifierKeys)
        End Function

        Public Shared Sub Save()
            My.Settings.KeysPreviousTab = GetShortcutKeysString(Command.PreviousTab)
            My.Settings.KeysNextTab = GetShortcutKeysString(Command.NextTab)
        End Sub
#End Region

#Region "Public Enumerations"
        Public Enum Command As Integer
            None = 0
            PreviousTab
            NextTab
        End Enum
#End Region

#Region "Private Properties"
        ' ReSharper disable InconsistentNaming
        Private Shared ReadOnly _mappings As New Dictionary(Of ShortcutKey, Command)
        ' ReSharper restore InconsistentNaming
        Private Shared ReadOnly Property Mappings As Dictionary(Of ShortcutKey, Command)
            Get
                Load()
                Return _mappings
            End Get
        End Property
#End Region

#Region "Private Fields"
        ' ReSharper disable UnusedMember.Local
        Private Shared _keyboardHook As New KeyboardHook
        ' ReSharper restore UnusedMember.Local
        Private Shared _loaded As Boolean = False
#End Region

#Region "Private Methods"
        Private Shared Sub Load()
            If _loaded Then Return
            _mappings.Clear()
            LoadFromString(Command.PreviousTab, My.Settings.KeysPreviousTab)
            LoadFromString(Command.NextTab, My.Settings.KeysNextTab)
            _loaded = True
        End Sub

        Private Shared Sub LoadFromString(ByVal command As Command, ByVal keysString As String)
            For Each shortcutKey As ShortcutKey In ParseShortcutKeysString(keysString)
                _mappings.Add(shortcutKey, command)
            Next
        End Sub

        Private Shared Function ParseShortcutKeysString(ByVal shortcutKeysString As String) As ShortcutKey()
            Dim shortcutKeys As New List(Of ShortcutKey)
            For Each shortcutKeyString As String In shortcutKeysString.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
                Try
                    shortcutKeys.Add(ShortcutKey.FromString(shortcutKeyString.Trim))
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(String.Format("KeyboardShortcuts.ParseString({0}) failed.", shortcutKeysString), ex, , True)
                    Continue For
                End Try
            Next
            Return shortcutKeys.ToArray()
        End Function

        Private Shared Function GetCommand(ByVal keyCode As Int32, ByVal modifierKeys As KeyboardHook.ModifierKeys) As Command
            Return GetCommand(New ShortcutKey(keyCode, modifierKeys))
        End Function

        Private Shared Function GetCommand(ByVal shortcutKey As ShortcutKey) As Command
            For Each keyValuePair As KeyValuePair(Of ShortcutKey, Command) In Mappings
                If ShortcutKeysMatch(keyValuePair.Key, shortcutKey) Then Return keyValuePair.Value
            Next
            Return Command.None
        End Function

        Private Shared Function ShortcutKeysMatch(ByVal wanted As ShortcutKey, ByVal pressed As ShortcutKey) As Boolean
            If Not wanted.KeyCode = pressed.KeyCode Then Return False
            Return KeyboardHook.ModifierKeysMatch(wanted.ModifierKeys, pressed.ModifierKeys)
        End Function

        Private Shared Function GetShortcutKeysString(ByVal command As Command) As String
            Dim parts As New List(Of String)
            For Each shortcutKey As ShortcutKey In GetShortcutKeys(command)
                parts.Add(shortcutKey.ToString())
            Next
            Return String.Join(", ", parts.ToArray())
        End Function

        Private Shared Function GetShortcutKeys(ByVal command As Command) As ShortcutKey()
            Dim shortcutKeys As New List(Of ShortcutKey)
            For Each keyValuePair As KeyValuePair(Of ShortcutKey, Command) In Mappings
                If keyValuePair.Value = command Then shortcutKeys.Add(keyValuePair.Key)
            Next
            Return shortcutKeys.ToArray()
        End Function
#End Region

#Region "Private Classes"
        Private Class ShortcutKey
            Public Property KeyCode As Int32
            Public Property ModifierKeys As KeyboardHook.ModifierKeys

            Public Sub New(ByVal keyCode As Int32, ByVal modifierKeys As KeyboardHook.ModifierKeys)
                Me.KeyCode = keyCode
                Me.ModifierKeys = modifierKeys
            End Sub

            Public Overrides Function ToString() As String
                Return String.Join("/", New String() {KeyCode, Convert.ToInt32(ModifierKeys)})
            End Function

            Public Shared Function FromString(ByVal shortcutKeyString As String) As ShortcutKey
                Dim parts As String() = shortcutKeyString.Split(New Char() {"/"}, 2)
                If Not parts.Length = 2 Then Throw New ArgumentException(String.Format("ShortcutKey.FromString({0}) failed. parts.Length != 2", shortcutKeyString), shortcutKeyString)
                Return New ShortcutKey(Convert.ToInt32(parts(0)), Convert.ToInt32(parts(1)))
            End Function
        End Class
#End Region
    End Class
End Namespace