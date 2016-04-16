Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports mRemote3G.App
Imports mRemote3G.App.Info
Imports mRemote3G.App.Runtime
Imports mRemote3G.Forms
Imports mRemote3G.Messages
Imports mRemote3G.My.Resources
Imports mRemote3G.Security

Namespace Tools
    Public Class Misc
        Private Structure SHFILEINFO
            Public hIcon As IntPtr            ' : icon
            Public iIcon As Integer           ' : icondex
            Public dwAttributes As Integer    ' : SFGAO_ flags
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst := 260)> Public szDisplayName As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst := 80)> Public szTypeName As String
        End Structure

        <DllImport("shell32.dll", CharSet := CharSet.Unicode)>
        Private Shared Function SHGetFileInfo(pszPath As String, dwFileAttributes As Integer, ByRef psfi As SHFILEINFO,
                                              cbFileInfo As Integer, uFlags As Integer) As IntPtr
        End Function

        Private Const SHGFI_ICON As Integer = &H100
        Private Const SHGFI_SMALLICON As Integer = &H1
        'Private Const SHGFI_LARGEICON = &H0    ' Large icon

        Public Shared Function GetIconFromFile(FileName As String) As Icon
            Try
                If File.Exists(FileName) = False Then
                    Return Nothing
                End If

                Dim hImgSmall As IntPtr  'The handle to the system image list.
                'Dim hImgLarge As IntPtr  'The handle to the system image list.
                Dim shinfo As SHFILEINFO
                shinfo = New SHFILEINFO()

                shinfo.szDisplayName = New String(Chr(0), 260)
                shinfo.szTypeName = New String(Chr(0), 80)

                'Use this to get the small icon.
                hImgSmall = SHGetFileInfo(FileName, 0, shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON Or SHGFI_SMALLICON)

                'Use this to get the large icon.
                'hImgLarge = SHGetFileInfo(fName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);

                'The icon is returned in the hIcon member of the
                'shinfo struct.
                Dim myIcon As Icon
                myIcon = Icon.FromHandle(shinfo.hIcon)

                Return myIcon
            Catch SAEx As ArgumentException
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "GetIconFromFile failed (Tools.Misc) - using default icon. Exception:" &
                                                    vbNewLine & SAEx.ToString(), True)
                Return mRemote_Icon
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "GetIconFromFile failed (Tools.Misc)" & vbNewLine & ex.ToString(),
                                                    True)
                Return Nothing
            End Try
        End Function

        Public Shared Event SQLUpdateCheckFinished(UpdateAvailable As Boolean)

        Public Shared Sub IsSQLUpdateAvailableBG()
            Dim t As New Thread(AddressOf IsSQLUpdateAvailable)
            t.SetApartmentState(ApartmentState.STA)
            t.Start()
        End Sub

        Public Shared Function IsSQLUpdateAvailable() As Boolean
            Try
                Dim sqlCon As SqlConnection
                Dim sqlQuery As SqlCommand
                Dim sqlRd As SqlDataReader

                Dim LastUpdateInDB As Date

                If My.Settings.SQLUser <> "" Then
                    sqlCon =
                        New SqlConnection(
                            "Data Source=" & My.Settings.SQLHost & ";Initial Catalog=" & My.Settings.SQLDatabaseName &
                            ";User Id=" & My.Settings.SQLUser & ";Password=" &
                            Crypt.Decrypt(My.Settings.SQLPass, General.EncryptionKey))
                Else
                    sqlCon =
                        New SqlConnection(
                            "Data Source=" & My.Settings.SQLHost & ";Initial Catalog=" & My.Settings.SQLDatabaseName &
                            ";Integrated Security=True")
                End If

                sqlCon.Open()

                sqlQuery = New SqlCommand("SELECT * FROM tblUpdate", sqlCon)
                sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection)

                sqlRd.Read()

                If sqlRd.HasRows Then
                    LastUpdateInDB = sqlRd.Item("LastUpdate")

                    If LastUpdateInDB > Runtime.LastSqlUpdate Then
                        RaiseEvent SQLUpdateCheckFinished(True)
                        Return True
                    End If
                End If

                RaiseEvent SQLUpdateCheckFinished(False)
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "IsSQLUpdateAvailable failed (Tools.Misc)" & vbNewLine &
                                                    ex.ToString(), True)
            End Try

            Return False
        End Function

        Public Shared Function PasswordDialog(Optional ByVal passwordName As String = Nothing,
                                              Optional ByVal verify As Boolean = True) As String
            Dim passwordForm As New PasswordForm(passwordName, verify)

            If passwordForm.ShowDialog = DialogResult.OK Then
                Return passwordForm.Password
            Else
                Return ""
            End If
        End Function

        Public Shared Function CreateConstantID() As String
            Return Guid.NewGuid().ToString()
        End Function

        Public Shared Function LeadingZero(Number As String) As String
            If Number < 10 Then
                Return "0" & Number
            Else
                Return Number
            End If
        End Function

        Public Shared Function DBDate(Dt As Date) As String
            Dim strDate As String

            strDate = Dt.Year & LeadingZero(Dt.Month) & LeadingZero(Dt.Day) & " " & LeadingZero(Dt.Hour) & ":" &
                      LeadingZero(Dt.Minute) & ":" & LeadingZero(Dt.Second)

            Return strDate
        End Function

        Public Shared Function PrepareForDB(Text As String) As String
            Text = Replace(Text, "'True'", "1", , , CompareMethod.Text)
            Text = Replace(Text, "'False'", "0", , , CompareMethod.Text)

            Return Text
        End Function

        Public Shared Function PrepareValueForDB(Text As String) As String
            Text = Replace(Text, "'", "''", , , CompareMethod.Text)

            Return Text
        End Function

        Public Shared Function StringToEnum(t As Type, value As String) As Object
            Return [Enum].Parse(t, value)
        End Function

        Public Shared Function GetExceptionMessageRecursive(ex As Exception,
                                                            Optional ByVal separator As String = vbNewLine) As String
            Dim message As String = ex.ToString()
            If ex.InnerException IsNot Nothing Then
                Dim innerMessage As String = GetExceptionMessageRecursive(ex.InnerException, separator)
                message = String.Join(separator, New String() {message, innerMessage})
            End If
            Return message
        End Function

        Public Shared Function TakeScreenshot(sender As UI.Window.Connection) As Image
            Try
                Dim LeftStart As Integer =
                        sender.TabController.SelectedTab.PointToScreen(New Point(sender.TabController.SelectedTab.Left)) _
                        .X  'Me.Left + Splitter.SplitterDistance + 11
                Dim TopStart As Integer =
                        sender.TabController.SelectedTab.PointToScreen(New Point(sender.TabController.SelectedTab.Top)).
                        Y 'Me.Top + Splitter.Top + TabController.Top + TabController.SelectedTab.Top * 2 - 3
                Dim LeftWidth As Integer = sender.TabController.SelectedTab.Width _
                'Me.Width - (Splitter.SplitterDistance + 16)
                Dim TopHeight As Integer = sender.TabController.SelectedTab.Height _
                'Me.Height - (Splitter.Top + TabController.Top + TabController.SelectedTab.Top * 2 + 2)

                Dim currentFormSize As New Size(LeftWidth, TopHeight)
                Dim ScreenToBitmap As New Bitmap(LeftWidth, TopHeight)
                Dim gGraphics As Graphics = Graphics.FromImage(ScreenToBitmap)

                gGraphics.CopyFromScreen(New Point(LeftStart, TopStart), New Point(0, 0), currentFormSize)

                Return ScreenToBitmap
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Taking Screenshot failed" & vbNewLine & ex.ToString(), True)
            End Try

            Return Nothing
        End Function

        Public Class EnumTypeConverter
            Inherits EnumConverter
            Private ReadOnly _enumType As Type

            Public Sub New(type As Type)
                MyBase.New(type)
                _enumType = type
            End Sub

            Public Overloads Overrides Function CanConvertTo(context As ITypeDescriptorContext, destType As Type) _
                As Boolean
                Return destType Is GetType(String)
            End Function

            Public Overloads Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo,
                                                          value As Object, destType As Type) As Object
                Dim fi As FieldInfo = _enumType.GetField([Enum].GetName(_enumType, value))
                Dim dna = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)),
                                     DescriptionAttribute)

                If dna IsNot Nothing Then
                    Return dna.Description
                Else
                    Return value.ToString()
                End If
            End Function

            Public Overloads Overrides Function CanConvertFrom(context As ITypeDescriptorContext, srcType As Type) _
                As Boolean
                Return srcType Is GetType(String)
            End Function

            Public Overloads Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As CultureInfo,
                                                            value As Object) As Object
                For Each fi As FieldInfo In _enumType.GetFields()
                    Dim dna = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)),
                                         DescriptionAttribute)

                    If (dna IsNot Nothing) AndAlso (DirectCast(value, String) = dna.Description) Then
                        Return [Enum].Parse(_enumType, fi.Name)
                    End If
                Next

                Return [Enum].Parse(_enumType, DirectCast(value, String))
            End Function
        End Class

        Public Class YesNoTypeConverter
            Inherits TypeConverter

            Public Overloads Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) _
                As Boolean
                If sourceType Is GetType(String) Then
                    Return True
                End If

                Return MyBase.CanConvertFrom(context, sourceType)
            End Function

            Public Overloads Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) _
                As Boolean
                If destinationType Is GetType(String) Then
                    Return True
                End If

                Return MyBase.CanConvertTo(context, destinationType)
            End Function

            Public Overloads Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As CultureInfo,
                                                            value As Object) As Object
                If value.GetType() Is GetType(String) Then
                    If CStr(value).ToLower() = Language.Language.strYes.ToLower Then
                        Return True
                    End If

                    If CStr(value).ToLower() = Language.Language.strNo.ToLower Then
                        Return False
                    End If

                    Throw New Exception("Values must be ""Yes"" or ""No""")
                End If

                Return MyBase.ConvertFrom(context, culture, value)
            End Function

            Public Overloads Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo,
                                                          value As Object, destinationType As Type) As Object
                If destinationType Is GetType(String) Then
                    Return IIf(CBool(value), Language.Language.strYes, Language.Language.strNo)
                End If

                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End Function

            Public Overloads Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overloads Overrides Function GetStandardValues(context As ITypeDescriptorContext) _
                As StandardValuesCollection
                Dim bools() As Boolean = {True, False}

                Dim svc As New StandardValuesCollection(bools)

                Return svc
            End Function
        End Class

        Public Class Fullscreen
            Public Sub New(handledForm As Form)
                _handledForm = handledForm
            End Sub

            Private ReadOnly _handledForm As Form
            Private _savedWindowState As FormWindowState
            Private _savedBorderStyle As FormBorderStyle
            Private _savedBounds As Rectangle

            Private _value As Boolean = False

            Public Property Value As Boolean
                Get
                    Return _value
                End Get
                Set(newValue As Boolean)
                    If _value = newValue Then Return
                    If Not _value Then
                        EnterFullscreen()
                    Else
                        ExitFullscreen()
                    End If
                    _value = newValue
                End Set
            End Property

            Private Sub EnterFullscreen()
                _savedBorderStyle = _handledForm.FormBorderStyle
                _savedWindowState = _handledForm.WindowState
                _savedBounds = _handledForm.Bounds

                _handledForm.FormBorderStyle = FormBorderStyle.None
                If _handledForm.WindowState = FormWindowState.Maximized Then
                    _handledForm.WindowState = FormWindowState.Normal
                End If
                _handledForm.WindowState = FormWindowState.Maximized
            End Sub

            Private Sub ExitFullscreen()
                _handledForm.FormBorderStyle = _savedBorderStyle
                _handledForm.WindowState = _savedWindowState
                _handledForm.Bounds = _savedBounds
            End Sub
        End Class


        '
        '* Arguments class: application arguments interpreter
        '*
        '* Authors:		R. LOPES
        '* Contributors:	R. LOPES
        '* Created:		25 October 2002
        '* Modified:		28 October 2002
        '*
        '* Version:		1.0
        '
        Public Class CMDArguments
            Private ReadOnly Parameters As StringDictionary

            ' Retrieve a parameter value if it exists
            Default Public ReadOnly Property Item(Param As String) As String
                Get
                    Return (Parameters(Param))
                End Get
            End Property

            Public Sub New(Args As String())
                Parameters = New StringDictionary()
                Dim Spliter As New Regex("^-{1,2}|^/|=|:", RegexOptions.IgnoreCase Or RegexOptions.Compiled)
                Dim Remover As New Regex("^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase Or RegexOptions.Compiled)
                Dim Parameter As String = Nothing
                Dim Parts As String()

                ' Valid parameters forms:
                ' {-,/,--}param{ ,=,:}((",')value(",'))
                ' Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'

                Try
                    For Each Txt As String In Args
                        ' Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
                        Parts = Spliter.Split(Txt, 3)
                        Select Case Parts.Length
                            Case 1
                                ' Found a value (for the last parameter found (space separator))
                                If Parameter IsNot Nothing Then
                                    If Not Parameters.ContainsKey(Parameter) Then
                                        Parts(0) = Remover.Replace(Parts(0), "$1")
                                        Parameters.Add(Parameter, Parts(0))
                                    End If
                                    Parameter = Nothing
                                End If
                                ' else Error: no parameter waiting for a value (skipped)
                                Exit Select
                            Case 2
                                ' Found just a parameter
                                ' The last parameter is still waiting. With no value, set it to true.
                                If Parameter IsNot Nothing Then
                                    If Not Parameters.ContainsKey(Parameter) Then
                                        Parameters.Add(Parameter, "true")
                                    End If
                                End If
                                Parameter = Parts(1)
                                Exit Select
                            Case 3
                                ' Parameter with enclosed value
                                ' The last parameter is still waiting. With no value, set it to true.
                                If Parameter IsNot Nothing Then
                                    If Not Parameters.ContainsKey(Parameter) Then
                                        Parameters.Add(Parameter, "true")
                                    End If
                                End If
                                Parameter = Parts(1)
                                ' Remove possible enclosing characters (",')
                                If Not Parameters.ContainsKey(Parameter) Then
                                    Parts(2) = Remover.Replace(Parts(2), "$1")
                                    Parameters.Add(Parameter, Parts(2))
                                End If
                                Parameter = Nothing
                                Exit Select
                        End Select
                    Next
                    ' In case a parameter is still waiting
                    If Parameter IsNot Nothing Then
                        If Not Parameters.ContainsKey(Parameter) Then
                            Parameters.Add(Parameter, "true")
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Creating new Args failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub
        End Class

        Private Sub New()
        End Sub
    End Class
End Namespace
