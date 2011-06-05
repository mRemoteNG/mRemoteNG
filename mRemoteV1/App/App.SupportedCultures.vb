Namespace App
    Public Class SupportedCultures
        Inherits Dictionary(Of String, String)

        Private Sub New()
            Dim CultureInfo As Globalization.CultureInfo
            For Each CultureName As String In My.Settings.SupportedUICultures.Split(",")
                Try
                    CultureInfo = New Globalization.CultureInfo(CultureName.Trim)
                    Add(CultureInfo.Name, CultureInfo.TextInfo.ToTitleCase(CultureInfo.NativeName))
                Catch ex As Exception
                    Debug.Print(String.Format("An exception occurred while adding the culture '{0}' to the list of supported cultures. {1}", CultureName, ex.ToString))
                End Try
            Next
        End Sub

        Private Shared _Instance As SupportedCultures = Nothing
        Public Shared Sub InstantiateSingleton()
            If _Instance Is Nothing Then
                _Instance = New SupportedCultures
            End If
        End Sub

        Public Shared Function IsNameSupported(ByVal CultureName As String) As Boolean
            Return _Instance.ContainsKey(CultureName)
        End Function

        Public Shared Function IsNativeNameSupported(ByVal CultureNativeName As String) As Boolean
            Return _Instance.ContainsValue(CultureNativeName)
        End Function

        Public Shared ReadOnly Property CultureName(ByVal CultureNativeName As String) As String
            Get
                Dim Names(_Instance.Count) As String
                Dim NativeNames(_Instance.Count) As String

                _Instance.Keys.CopyTo(Names, 0)
                _Instance.Values.CopyTo(NativeNames, 0)

                For Index As Integer = 0 To _Instance.Count
                    If NativeNames(Index) = CultureNativeName Then
                        Return Names(Index)
                    End If
                Next

                Throw New System.Collections.Generic.KeyNotFoundException()
            End Get
        End Property

        Public Shared ReadOnly Property CultureNativeName(ByVal CultureName As String) As String
            Get
                Return _Instance.Item(CultureName)
            End Get
        End Property

        Public Shared ReadOnly Property CultureNativeNames() As List(Of String)
            Get
                Dim ValueList As New List(Of String)
                For Each Value As String In _Instance.Values
                    ValueList.Add(Value)
                Next
                Return ValueList
            End Get
        End Property
    End Class
End Namespace
