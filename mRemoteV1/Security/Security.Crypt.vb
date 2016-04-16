Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports mRemote3G.App
Imports mRemote3G.App.Runtime
Imports mRemote3G.Messages

Namespace Security
    Public Class Crypt
        Public Shared Function Encrypt(StrToEncrypt As String, StrSecret As String) As String
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
                Dim data() As Byte = Encoding.UTF8.GetBytes(StrToEncrypt)

                cs.Write(data, 0, data.Length)
                cs.FlushFinalBlock()

                Dim encdata() As Byte = ms.ToArray()
                cs.Close()
                rd.Clear()

                Return Convert.ToBase64String(encdata)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(Language.Language.strErrorEncryptionFailed, ex.ToString()))
            End Try

            Return StrToEncrypt
        End Function

        Public Shared Function Decrypt(ciphertextBase64 As String, password As String) As String
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
                        Const ivLength = 16
                        Dim iv(ivLength - 1) As Byte
                        memoryStream.Read(iv, 0, ivLength)
                        rijndaelManaged.IV = iv

                        Using _
                            cryptoStream As _
                                New CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor, CryptoStreamMode.Read)
                            Using streamReader As New StreamReader(cryptoStream, Encoding.UTF8, True)
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
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(Language.Language.strErrorDecryptionFailed, ex.ToString()))
                End If
            End Try

            Return ciphertextBase64
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace

