Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports mRemoteNG.App.Runtime

Namespace Security
    Public Class Crypt
        Public Shared Function Encrypt(ByVal StrToEncrypt As String, ByVal StrSecret As String) As String
            If StrToEncrypt = "" Or StrSecret = "" Then
                Return StrToEncrypt
            End If

            Try
                Dim rd As New RijndaelManaged

                Dim md5 As New MD5CryptoServiceProvider
                Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(StrSecret))

                md5.Clear()
                rd.Key = key
                rd.GenerateIV()

                Dim iv() As Byte = rd.IV
                Dim ms As New MemoryStream

                ms.Write(iv, 0, iv.Length)

                Dim cs As New CryptoStream(ms, rd.CreateEncryptor, CryptoStreamMode.Write)
                Dim data() As Byte = System.Text.Encoding.UTF8.GetBytes(StrToEncrypt)

                cs.Write(data, 0, data.Length)
                cs.FlushFinalBlock()

                Dim encdata() As Byte = ms.ToArray()
                cs.Close()
                rd.Clear()

                Return Convert.ToBase64String(encdata)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Encryption failed" & vbNewLine & ex.Message, True)
            End Try

            Return StrToEncrypt
        End Function

        Public Shared Function Decrypt(ByVal StrToDecrypt As String, ByVal StrSecret As String) As String
            If StrToDecrypt = "" Or StrSecret = "" Then
                Return StrToDecrypt
            End If

            Try
                Dim rd As New RijndaelManaged
                Dim rijndaelIvLength As Integer = 16
                Dim md5 As New MD5CryptoServiceProvider
                Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(StrSecret))

                md5.Clear()

                Dim encdata() As Byte = Convert.FromBase64String(StrToDecrypt)
                Dim ms As New MemoryStream(encdata)
                Dim iv(15) As Byte

                ms.Read(iv, 0, rijndaelIvLength)
                rd.IV = iv
                rd.Key = key

                Dim cs As New CryptoStream(ms, rd.CreateDecryptor, CryptoStreamMode.Read)

                Dim data(ms.Length - rijndaelIvLength) As Byte
                Dim i As Integer = cs.Read(data, 0, data.Length)

                cs.Close()
                rd.Clear()

                Return System.Text.Encoding.UTF8.GetString(data, 0, i)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Decryption failed" & vbNewLine & ex.Message, True)
            End Try

            Return StrToDecrypt
        End Function
    End Class
End Namespace

