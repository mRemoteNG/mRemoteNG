Imports System.Text.RegularExpressions

Namespace Tools
    ' Adapted from http://qntm.org/cmd
    Public Class CommandLineArguments

#Region "Protected Fields"

        Protected Arguments As New List(Of Argument)

#End Region

#Region "Public Properties"

        Public Property EscapeForShell As Boolean

#End Region

#Region "Public Methods"

        Public Sub Add(argument As String, Optional ByVal forceQuotes As Boolean = False)
            Arguments.Add(New Argument(argument, False, forceQuotes))
        End Sub

        Public Sub Add(ByVal ParamArray argumentArray As String())
            For Each argument As String In argumentArray
                Add(argument)
            Next
        End Sub

        Public Sub AddFileName(fileName As String, Optional ByVal forceQuotes As Boolean = False)
            Arguments.Add(New Argument(fileName, True, forceQuotes))
        End Sub

        Public Overrides Function ToString() As String
            Dim processedArguments As New List(Of String)

            For Each argument As Argument In Arguments
                processedArguments.Add(ProcessArgument(argument, EscapeForShell))
            Next

            Return String.Join(" ", processedArguments.ToArray())
        End Function

        Public Shared Function PrefixFileName(argument As String) As String
            If String.IsNullOrEmpty(argument) Then Return argument

            If argument.StartsWith("-") Then argument = ".\" & argument

            Return argument
        End Function

        Public Shared Function EscapeBackslashes(argument As String) As String
            If String.IsNullOrEmpty(argument) Then Return argument

            ' Sequence of backslashes followed by a double quote:
            '     double up all the backslashes and escape the double quote
            Return Regex.Replace(argument, "(\\*)""", "$1$1\""")
        End Function

        Public Shared Function EscapeBackslashesForTrailingQuote(argument As String) As String
            If String.IsNullOrEmpty(argument) Then Return argument

            ' Sequence of backslashes followed by the end of the string
            ' (which will become a double quote):
            '     double up all the backslashes
            Return Regex.Replace(argument, "(\\*)$", "$1$1")
        End Function

        Public Shared Function QuoteArgument(argument As String, Optional ByVal forceQuotes As Boolean = False) _
            As String
            If Not forceQuotes And Not String.IsNullOrEmpty(argument) And Not argument.Contains(" ") Then _
                Return argument

            Return """" & EscapeBackslashesForTrailingQuote(argument) & """"
        End Function

        Public Shared Function EscapeShellMetacharacters(argument As String) As String
            If String.IsNullOrEmpty(argument) Then Return argument

            Return Regex.Replace(argument, "([()%!^""<>&|])", "^$1")
        End Function

#End Region

#Region "Protected Methods"

        Protected Shared Function ProcessArgument(argument As Argument, Optional ByVal escapeForShell As Boolean = False) _
            As String
            Dim text As String = argument.Text

            If argument.IsFileName Then text = PrefixFileName(text)
            text = EscapeBackslashes(text)
            text = QuoteArgument(text, argument.ForceQuotes)
            If escapeForShell Then text = EscapeShellMetacharacters(text)

            Return text
        End Function

#End Region

#Region "Protected Classes"

        Protected Class Argument
            Public Sub New(text As String, Optional ByVal isFileName As Boolean = False,
                           Optional ByVal forceQuotes As Boolean = False)
                Me.Text = text
                Me.IsFileName = isFileName
                Me.ForceQuotes = forceQuotes
            End Sub

            Public Property Text As String
            Public Property IsFileName As Boolean
            Public Property ForceQuotes As Boolean
        End Class

#End Region
    End Class
End Namespace