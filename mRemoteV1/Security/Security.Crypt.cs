using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.Messages;


namespace mRemoteNG.Security
{
	public class Crypt : ICryptographyProvider
    {
		public string Encrypt(string StrToEncrypt, SecureString StrSecret)
		{
			if (string.IsNullOrEmpty(StrToEncrypt) || StrSecret.Length == 0)
			{
				return StrToEncrypt;
			}
				
			try
			{
				var rd = new RijndaelManaged();

                var md5 = new MD5CryptoServiceProvider();
				var key = md5.ComputeHash(Encoding.UTF8.GetBytes(StrSecret.ConvertToUnsecureString()));
					
				md5.Clear();
				rd.Key = key;
				rd.GenerateIV();

                var iv = rd.IV;
                var ms = new MemoryStream();
					
				ms.Write(iv, 0, iv.Length);

                var cs = new CryptoStream(ms, rd.CreateEncryptor(), CryptoStreamMode.Write);
                var data = Encoding.UTF8.GetBytes(StrToEncrypt);
					
				cs.Write(data, 0, data.Length);
				cs.FlushFinalBlock();

                var encdata = ms.ToArray();
				cs.Close();
				rd.Clear();
					
				return Convert.ToBase64String(encdata);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strErrorEncryptionFailed, ex.Message));
			}
				
			return StrToEncrypt;
		}
			
		public string Decrypt(string ciphertextBase64, SecureString decryptionKey)
		{
			if (string.IsNullOrEmpty(ciphertextBase64) || decryptionKey.Length == 0)
				return ciphertextBase64;
				
			try
			{
				var plaintext = "";
					
				using (var rijndaelManaged = new RijndaelManaged())
				{
					using (var md5 = new MD5CryptoServiceProvider())
					{
                        var key = md5.ComputeHash(Encoding.UTF8.GetBytes(decryptionKey.ConvertToUnsecureString()));
						rijndaelManaged.Key = key;
					}


                    var ciphertext = Convert.FromBase64String(ciphertextBase64);
						
					using (var memoryStream = new MemoryStream(ciphertext))
					{
						const int ivLength = 16;
                        var iv = new byte[ivLength - 1 + 1];
						memoryStream.Read(iv, 0, ivLength);
						rijndaelManaged.IV = iv;
							
						using (var cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read))
						{
							using (var streamReader = new StreamReader(cryptoStream, Encoding.UTF8, true))
							{
								plaintext = streamReader.ReadToEnd();
							}
								
							rijndaelManaged.Clear();
						}
							
					}
						
				}
				return plaintext;
			}
			catch (Exception ex)
			{
				// Ignore CryptographicException "Padding is invalid and cannot be removed." when password is incorrect.
				if (!(ex is CryptographicException))
				{
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strErrorDecryptionFailed, ex.Message));
				}
			}
				
			return ciphertextBase64;
		}
	}
}