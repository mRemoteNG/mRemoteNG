Imports mRemoteNG.App.Runtime
Imports System.IO
Imports System.ComponentModel

Namespace Tools
    Public Class ExternalApp
#Region "Properties"
        Private _DisplayName As String
        Public Property DisplayName() As String
            Get
                Return _DisplayName
            End Get
            Set(ByVal value As String)
                _DisplayName = value
            End Set
        End Property

        Private _FileName As String
        Public Property FileName() As String
            Get
                Return _FileName
            End Get
            Set(ByVal value As String)
                _FileName = value
            End Set
        End Property

        Private _WaitForExit As Boolean
        Public Property WaitForExit() As Boolean
            Get
                Return _WaitForExit
            End Get
            Set(ByVal value As Boolean)
                _WaitForExit = value
            End Set
        End Property

        Private _Arguments As String
        Public Property Arguments() As String
            Get
                Return _Arguments
            End Get
            Set(ByVal value As String)
                _Arguments = value
            End Set
        End Property

        Private _TryIntegrate As Boolean
        Public Property TryIntegrate() As Boolean
            Get
                Return _TryIntegrate
            End Get
            Set(ByVal value As Boolean)
                _TryIntegrate = value
            End Set
        End Property



        Private _ConnectionInfo As Connection.Info
        Public Property ConnectionInfo() As Connection.Info
            Get
                Return _ConnectionInfo
            End Get
            Set(ByVal value As Connection.Info)
                _ConnectionInfo = value
            End Set
        End Property

        Public ReadOnly Property Icon() As Icon
            Get
                If File.Exists(Me._FileName) Then
                    Return Tools.Misc.GetIconFromFile(Me._FileName)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public ReadOnly Property Image() As Image
            Get
                Dim iC As Icon = Me.Icon
                If iC IsNot Nothing Then
                    Return iC.ToBitmap
                Else
                    Return Nothing
                End If
            End Get
        End Property
#End Region

        Public Sub New()
            Me.New("")
        End Sub

        Public Sub New(ByVal DisplayName As String)
            Me.New(DisplayName, "", "")
        End Sub

        Public Sub New(ByVal DisplayName As String, ByVal Filename As String, ByVal Arguments As String)
            _DisplayName = DisplayName
            _FileName = Filename
            _Arguments = Arguments
        End Sub

        ' Start external app
        Public Function Start(Optional ByVal ConnectionInfo As Connection.Info = Nothing) As Process
            Try
                If _FileName = "" Then
                    Throw New Exception("No Filename specified!")
                End If

                If _TryIntegrate = True Then
                    StartIntApp(ConnectionInfo)
                    Return Nothing
                End If

                _ConnectionInfo = ConnectionInfo

                Dim p As New Process()
                Dim pI As New ProcessStartInfo()

                pI.FileName = ParseText(_FileName)
                pI.Arguments = ParseText(_Arguments)

                p.StartInfo = pI

                p.Start()

                If _WaitForExit Then
                    p.WaitForExit()
                End If

                Return p
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't start external application." & vbNewLine & ex.Message)
                Return Nothing
            End Try
        End Function

        ' Start external app integrated
        Public Sub StartIntApp(Optional ByVal ConnectionInfo As Connection.Info = Nothing)
            Try
                _ConnectionInfo = ConnectionInfo

                Dim nCI As New Connection.Info

                nCI.Protocol = Connection.Protocol.Protocols.IntApp
                nCI.ExtApp = Me.DisplayName
                nCI.Name = Me.DisplayName
                nCI.Panel = "Int. Apps"
                nCI.Hostname = _ConnectionInfo.Hostname
                nCI.Port = _ConnectionInfo.Port
                nCI.Username = _ConnectionInfo.Username
                nCI.Password = _ConnectionInfo.Password
                nCI.Domain = _ConnectionInfo.Domain
                nCI.Description = _ConnectionInfo.Description
                nCI.MacAddress = _ConnectionInfo.MacAddress
                nCI.UserField = _ConnectionInfo.UserField
                nCI.Description = _ConnectionInfo.Description
                nCI.PreExtApp = _ConnectionInfo.PreExtApp
                nCI.PostExtApp = _ConnectionInfo.PostExtApp

                OpenConnection(nCI)
            Catch ex As Exception

            End Try
        End Sub

        Public Function ParseText(ByVal Text As String) As String
            Dim pText As String = Text

            Try
                If _ConnectionInfo IsNot Nothing Then
                    pText = Replace(pText, "%Name%", _ConnectionInfo.Name, , , CompareMethod.Text)
                    pText = Replace(pText, "%HostName%", _ConnectionInfo.Hostname, , , CompareMethod.Text)
                    pText = Replace(pText, "%Port%", _ConnectionInfo.Port, , , CompareMethod.Text)
                    pText = Replace(pText, "%UserName%", _ConnectionInfo.Username, , , CompareMethod.Text)
                    pText = Replace(pText, "%Password%", _ConnectionInfo.Password, , , CompareMethod.Text)
                    pText = Replace(pText, "%Domain%", _ConnectionInfo.Domain, , , CompareMethod.Text)
                    pText = Replace(pText, "%Description%", _ConnectionInfo.Description, , , CompareMethod.Text)
                    pText = Replace(pText, "%MacAddress%", _ConnectionInfo.MacAddress, , , CompareMethod.Text)
                    pText = Replace(pText, "%UserField%", _ConnectionInfo.UserField, , , CompareMethod.Text)
                Else
                    pText = Replace(pText, "%Name%", "", , , CompareMethod.Text)
                    pText = Replace(pText, "%HostName%", "", , , CompareMethod.Text)
                    pText = Replace(pText, "%Port%", "", , , CompareMethod.Text)
                    pText = Replace(pText, "%UserName%", "", , , CompareMethod.Text)
                    pText = Replace(pText, "%Password%", "", , , CompareMethod.Text)
                    pText = Replace(pText, "%Domain%", "", , , CompareMethod.Text)
                    pText = Replace(pText, "%Description%", "", , , CompareMethod.Text)
                    pText = Replace(pText, "%MacAddress%", "", , , CompareMethod.Text)
                    pText = Replace(pText, "%UserField%", "", , , CompareMethod.Text)
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.WarningMsg, "ParseText failed (Tools.ExternalApp)" & vbNewLine & ex.Message, True)
            End Try

            Return pText
        End Function
    End Class

    Public Class ExternalAppsTypeConverter
        Inherits StringConverter

        Public Shared ExternalApps As String() = New String() {}

        Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
            Return New StandardValuesCollection(ExternalApps)
        End Function

        Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function
    End Class
End Namespace