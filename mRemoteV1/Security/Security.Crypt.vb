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
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(My.Language.strErrorEncryptionFailed, ex.Message))
            End Try

            Return StrToEncrypt
        End Function

        Public Shared Function Decrypt(ByVal ciphertextBase64 As String, ByVal password As String) As String
            If String.IsNullOrEmpty(ciphertextBase64) Or String.IsNullOrEmpty(password) Then
                Return ciphertextBase64
            End If

            Try
                Dim plaintext As String

                Using rijndaelManaged As New RijndaelManaged
                    Using md5 As New MD5CryptoServiceProvider
                        Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(password))
                        rijndaelManaged.Key = key
                    End Using

                    Dim ciphertext() As Byte = Convert.FromBase64String(ciphertextBase64)

                    Using memoryStream As New MemoryStream(ciphertext)
                        Const ivLength As Integer = 16
                        Dim iv(ivLength - 1) As Byte
                        memoryStream.Read(iv, 0, ivLength)
                        rijndaelManaged.IV = iv

                        Using cryptoStream As New CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor, CryptoStreamMode.Read)
                            Using streamReader As New StreamReader(cryptoStream, System.Text.Encoding.UTF8, True)
                                plaintext = streamReader.ReadToEnd()
                            End Using
                            rijndaelManaged.Clear()
                        End Using ' cryptoStream
                    End Using ' memoryStream
                End Using ' rijndaelManaged

                Return plaintext
            Catch ex As Exception
                ' Ignore CryptographicException "Padding is invalid and cannot be removed." when password is incorrect.
                If Not TypeOf ex Is CryptographicException Then
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(My.Language.strErrorDecryptionFailed, ex.Message))
                End If
            End Try

            Return ciphertextBase64
        End Function
    End Class
End Namespace

