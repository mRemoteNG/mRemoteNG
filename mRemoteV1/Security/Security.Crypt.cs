using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using mRemoteNG.App;


namespace mRemoteNG.Security
{
	public class Crypt
	{
		public static string Encrypt(string StrToEncrypt, string StrSecret)
		{
			if (StrToEncrypt == "" || StrSecret == "")
			{
				return StrToEncrypt;
			}
				
			try
			{
				RijndaelManaged rd = new RijndaelManaged();
					
				MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
				byte[] key = md5.ComputeHash(Encoding.UTF8.GetBytes(StrSecret));
					
				md5.Clear();
				rd.Key = key;
				rd.GenerateIV();
					
				byte[] iv = rd.IV;
				MemoryStream ms = new MemoryStream();
					
				ms.Write(iv, 0, iv.Length);
					
				CryptoStream cs = new CryptoStream(ms, rd.CreateEncryptor(), CryptoStreamMode.Write);
				byte[] data = System.Text.Encoding.UTF8.GetBytes(StrToEncrypt);
					
				cs.Write(data, 0, data.Length);
				cs.FlushFinalBlock();
					
				byte[] encdata = ms.ToArray();
				cs.Close();
				rd.Clear();
					
				return Convert.ToBase64String(encdata);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(My.Language.strErrorEncryptionFailed, ex.Message));
			}
				
			return StrToEncrypt;
		}
			
		public static string Decrypt(string ciphertextBase64, string password)
		{
			if (string.IsNullOrEmpty(ciphertextBase64) || string.IsNullOrEmpty(password))
			{
				return ciphertextBase64;
			}
				
			try
			{
				string plaintext = "";
					
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
					{
						byte[] key = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
						rijndaelManaged.Key = key;
					}
						
						
					byte[] ciphertext = Convert.FromBase64String(ciphertextBase64);
						
					using (MemoryStream memoryStream = new MemoryStream(ciphertext))
					{
						const int ivLength = 16;
						byte[] iv = new byte[ivLength - 1 + 1];
						memoryStream.Read(iv, 0, ivLength);
						rijndaelManaged.IV = iv;
							
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read))
						{
							using (StreamReader streamReader = new StreamReader(cryptoStream, System.Text.Encoding.UTF8, true))
							{
								plaintext = streamReader.ReadToEnd();
							}
								
							rijndaelManaged.Clear();
						} // cryptoStream
							
					} // memoryStream
						
				} // rijndaelManaged
					
					
				return plaintext;
			}
			catch (Exception ex)
			{
				// Ignore CryptographicException "Padding is invalid and cannot be removed." when password is incorrect.
				if (!(ex is CryptographicException))
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(My.Language.strErrorDecryptionFailed, ex.Message));
				}
			}
				
			return ciphertextBase64;
		}
	}
}
