Imports System.Reflection
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Runtime.InteropServices
Imports System.Collections.Specialized
Imports System.Text.RegularExpressions
Imports mRemote.App.Runtime
Imports System.IO
Imports System.Data.SqlClient

Namespace Tools
    Public Class Misc
        Private Structure SHFILEINFO
            Public hIcon As IntPtr            ' : icon
            Public iIcon As Integer           ' : icondex
            Public dwAttributes As Integer    ' : SFGAO_ flags
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> _
            Public szDisplayName As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)> _
            Public szTypeName As String
        End Structure

        <DllImport("shell32.dll")> _
        Private Shared Function SHGetFileInfo(ByVal pszPath As String, ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As IntPtr
        End Function

        Private Const SHGFI_ICON As Integer = &H100
        Private Const SHGFI_SMALLICON As Integer = &H1
        'Private Const SHGFI_LARGEICON = &H0    ' Large icon

        Public Shared Function GetIconFromFile(ByVal FileName As String) As Icon
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
                Dim myIcon As System.Drawing.Icon
                myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon)

                Return myIcon
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.WarningMsg, "GetIconFromFile failed (Tools.Misc)" & vbNewLine & ex.Message, True)
                Return Nothing
            End Try
        End Function

        Public Shared Event SQLUpdateCheckFinished(ByVal UpdateAvailable As Boolean)
        Public Shared Sub IsSQLUpdateAvailableBG()
            Dim t As New Threading.Thread(AddressOf IsSQLUpdateAvailable)
            t.Start()
        End Sub

        Public Shared Function IsSQLUpdateAvailable() As Boolean
            Try
                Dim sqlCon As SqlConnection
                Dim sqlQuery As SqlCommand
                Dim sqlRd As SqlDataReader

                Dim sqlDB As String = "mRemote"
                Dim LastUpdateInDB As Date

                If My.Settings.SQLUser <> "" Then
                    sqlCon = New SqlConnection("Data Source=" & My.Settings.SQLHost & ";Initial Catalog=" & sqlDB & ";User Id=" & My.Settings.SQLUser & ";Password=" & Security.Crypt.Decrypt(My.Settings.SQLPass, App.Info.General.EncryptionKey))
                Else
                    sqlCon = New SqlConnection("Data Source=" & My.Settings.SQLHost & ";Initial Catalog=" & sqlDB & ";Integrated Security=True")
                End If

                sqlCon.Open()

                sqlQuery = New SqlCommand("SELECT * FROM tblUpdate", sqlCon)
                sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection)

                sqlRd.Read()

                If sqlRd.HasRows Then
                    LastUpdateInDB = sqlRd.Item("LastUpdate")

                    If LastUpdateInDB > LastSQLUpdate Then
                        RaiseEvent SQLUpdateCheckFinished(True)
                        Return True
                    End If
                End If

                RaiseEvent SQLUpdateCheckFinished(False)
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.WarningMsg, "IsSQLUpdateAvailable failed (Tools.Misc)" & vbNewLine & ex.Message, True)
            End Try
            
            Return False
        End Function

        Public Shared Function PasswordDialog(Optional ByVal Verify As Boolean = True) As String
            Dim nPwFrm As New frmPassword()

            nPwFrm.Verify = Verify

            If nPwFrm.ShowDialog = DialogResult.OK Then
                Return nPwFrm.Password
            Else
                Return ""
            End If
        End Function

        Private Shared rndNums As New ArrayList
        Public Shared Function CreateConstantID() As String
            Dim cID As String
            Dim rnd As New Random(Now.Second)
            Dim iRnd As Integer
            Dim NewFound As Boolean = False

            Do Until NewFound
                iRnd = rnd.Next(10000, 99999)

                If rndNums.Contains(iRnd) = False Then
                    rndNums.Add(iRnd)
                    NewFound = True
                End If
            Loop

            cID = Now.Year & LeadingZero(Now.Month) & LeadingZero(Now.Day) & LeadingZero(Now.Hour) & LeadingZero(Now.Minute) & LeadingZero(Now.Second) & LeadingZero(Now.Millisecond & iRnd)

            Return cID
        End Function

        Public Shared Function LeadingZero(ByVal Number As String) As String
            If Number < 10 Then
                Return "0" & Number
            Else
                Return Number
            End If
        End Function

        Public Shared Function DBDate(ByVal Dt As Date) As String
            Dim strDate As String

            strDate = Dt.Year & LeadingZero(Dt.Month) & LeadingZero(Dt.Day) & " " & LeadingZero(Dt.Hour) & ":" & LeadingZero(Dt.Minute) & ":" & LeadingZero(Dt.Second)

            Return strDate
        End Function

        Public Shared Function PrepareForDB(ByVal Text As String) As String
            Text = Replace(Text, "'True'", "1", , , CompareMethod.Text)
            Text = Replace(Text, "'False'", "0", , , CompareMethod.Text)

            Return Text
        End Function

        Public Shared Function PrepareValueForDB(ByVal Text As String) As String
            Text = Replace(Text, "'", "''", , , CompareMethod.Text)

            Return Text
        End Function

        Public Shared Function StringToEnum(ByVal t As Type, ByVal value As String) As Object
            For Each fI As FieldInfo In t.GetFields
                If fI.Name = value Then
                    Return fI.GetValue(vbNull)
                End If
            Next

            Dim ex As New Exception(String.Format("Can't convert {0} to {1}", value, t.ToString))
            mC.AddMessage(Messages.MessageClass.ErrorMsg, "StringToEnum failed" & vbNewLine & ex.Message, True)
            Throw ex
        End Function

        Public Shared Function TakeScreenshot(ByVal sender As UI.Window.Connection) As Image
            Try
                Dim LeftStart As Integer = sender.TabController.SelectedTab.PointToScreen(New Point(sender.TabController.SelectedTab.Left)).X  'Me.Left + Splitter.SplitterDistance + 11
                Dim TopStart As Integer = sender.TabController.SelectedTab.PointToScreen(New Point(sender.TabController.SelectedTab.Top)).Y 'Me.Top + Splitter.Top + TabController.Top + TabController.SelectedTab.Top * 2 - 3
                Dim LeftWidth As Integer = sender.TabController.SelectedTab.Width  'Me.Width - (Splitter.SplitterDistance + 16)
                Dim TopHeight As Integer = sender.TabController.SelectedTab.Height  'Me.Height - (Splitter.Top + TabController.Top + TabController.SelectedTab.Top * 2 + 2)

                Dim currentFormSize As Size = New Size(LeftWidth, TopHeight)
                Dim ScreenToBitmap As New Bitmap(LeftWidth, TopHeight)
                Dim gGraphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(ScreenToBitmap)

                gGraphics.CopyFromScreen(New Point(LeftStart, TopStart), New Point(0, 0), currentFormSize)

                Return ScreenToBitmap
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, "Taking Screenshot failed" & vbNewLine & ex.Message, True)
            End Try

            Return Nothing
        End Function

        Public Class PropertyGridCategory
            Public Const Category1 As String = vbCr & vbCr & vbCr & vbCr & vbCr & vbCr & vbCr
            Public Const Category2 As String = vbCr & vbCr & vbCr & vbCr & vbCr & vbCr
            Public Const Category3 As String = vbCr & vbCr & vbCr & vbCr & vbCr
            Public Const Category4 As String = vbCr & vbCr & vbCr & vbCr
            Public Const Category5 As String = vbCr & vbCr & vbCr
            Public Const Category6 As String = vbCr & vbCr
            Public Const Category7 As String = vbCr
        End Class

        Public Class PropertyGridValue
            Public Const Value1 As String = vbCr & vbCr & vbCr & vbCr & vbCr & vbCr
            Public Const Value2 As String = vbCr & vbCr & vbCr & vbCr & vbCr
            Public Const Value3 As String = vbCr & vbCr & vbCr & vbCr
            Public Const Value4 As String = vbCr & vbCr & vbCr
            Public Const Value5 As String = vbCr & vbCr
            Public Const Value6 As String = vbCr
        End Class

        Public Class EnumTypeConverter
            Inherits EnumConverter
            Private _enumType As System.Type

            Public Sub New(ByVal type As System.Type)
                MyBase.New(type)
                _enumType = type
            End Sub

            Public Overloads Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destType As System.Type) As Boolean
                Return destType Is GetType(String)
            End Function

            Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As Globalization.CultureInfo, ByVal value As Object, ByVal destType As System.Type) As Object
                Dim fi As FieldInfo = _enumType.GetField([Enum].GetName(_enumType, value))
                Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)

                If dna IsNot Nothing Then
                    Return dna.Description
                Else
                    Return value.ToString()
                End If
            End Function

            Public Overloads Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal srcType As System.Type) As Boolean
                Return srcType Is GetType(String)
            End Function

            Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As Globalization.CultureInfo, ByVal value As Object) As Object
                For Each fi As FieldInfo In _enumType.GetFields()
                    Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)

                    If (dna IsNot Nothing) AndAlso (DirectCast(value, String) = dna.Description) Then
                        Return [Enum].Parse(_enumType, fi.Name)
                    End If
                Next

                Return [Enum].Parse(_enumType, DirectCast(value, String))
            End Function
        End Class

        Public Class YesNoTypeConverter
            Inherits TypeConverter

            Public Overloads Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal sourceType As Type) As Boolean
                If sourceType Is GetType(String) Then
                    Return True
                End If

                Return MyBase.CanConvertFrom(context, sourceType)
            End Function

            Public Overloads Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destinationType As Type) As Boolean
                If destinationType Is GetType(String) Then
                    Return True
                End If

                Return MyBase.CanConvertTo(context, destinationType)
            End Function

            Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object
                If value.GetType() Is GetType(String) Then
                    If CStr(value).ToLower() = Language.Base.Yes.ToLower Then
                        Return True
                    End If

                    If CStr(value).ToLower() = Language.Base.No.ToLower Then
                        Return False
                    End If

                    Throw New Exception("Values must be ""Yes"" or ""No""")
                End If

                Return MyBase.ConvertFrom(context, culture, value)
            End Function

            Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
                If destinationType Is GetType(String) Then
                    Return IIf(CBool(value), Language.Base.Yes, Language.Base.No)
                End If

                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End Function

            Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
                Dim bools() As Boolean = {True, False}

                Dim svc As New System.ComponentModel.TypeConverter.StandardValuesCollection(bools)

                Return svc
            End Function
        End Class

        Public Class Fullscreen
            Private Shared winState As FormWindowState
            Private Shared brdStyle As FormBorderStyle
            Private Shared topMost As Boolean
            Private Shared bounds As Rectangle

            Public Shared targetForm As Form = frmMain
            Public Shared FullscreenActive As Boolean = False

            Public Shared Sub EnterFullscreen()
                Try
                    If Not FullscreenActive Then
                        FullscreenActive = True
                        Save()
                        targetForm.WindowState = FormWindowState.Maximized
                        targetForm.FormBorderStyle = FormBorderStyle.None
                        targetForm.MainMenuStrip.Visible = False
                        SetWinFullScreen(targetForm.Handle)
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Entering Fullscreen failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Shared Sub Save()
                winState = targetForm.WindowState
                brdStyle = targetForm.FormBorderStyle
                bounds = targetForm.Bounds
            End Sub

            Public Shared Sub ExitFullscreen()
                Try
                    targetForm.WindowState = winState
                    targetForm.FormBorderStyle = brdStyle
                    targetForm.MainMenuStrip.Visible = True
                    targetForm.Bounds = bounds
                    FullscreenActive = False
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Exiting Fullscreen failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            <DllImport("user32.dll", EntryPoint:="GetSystemMetrics")> Public Shared Function GetSystemMetrics(ByVal which As Integer) As Integer
            End Function

            <DllImport("user32.dll")> Public Shared Sub SetWindowPos(ByVal hwnd As IntPtr, ByVal hwndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal flags As UInteger)
            End Sub

            Private Shared HWND_TOP As IntPtr = IntPtr.Zero
            Private Const SWP_SHOWWINDOW As Integer = 64
            ' 0×0040

            Public Shared Sub SetWinFullScreen(ByVal hwnd As IntPtr)
                Try
                    Dim curScreen As Screen = Screen.FromHandle(targetForm.Handle)
                    SetWindowPos(hwnd, HWND_TOP, curScreen.Bounds.Left, curScreen.Bounds.Top, curScreen.Bounds.Right, curScreen.Bounds.Bottom, SWP_SHOWWINDOW)
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "SetWindowPos failed" & vbNewLine & ex.Message, True)
                End Try
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
            Private Parameters As StringDictionary

            ' Retrieve a parameter value if it exists
            Default Public ReadOnly Property Item(ByVal Param As String) As String
                Get
                    Return (Parameters(Param))
                End Get
            End Property

            Public Sub New(ByVal Args As String())
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
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Creating new Args failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
        End Class
    End Class
End Namespace
