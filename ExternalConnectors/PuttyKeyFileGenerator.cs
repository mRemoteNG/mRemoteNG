using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ExternalConnectors;

public class PuttyKeyFileGenerator
{
    private const int prefixSize = 4;
    private const int paddedPrefixSize = prefixSize + 1;
    private const int lineLength = 64;
    private const string keyType = "ssh-rsa";
    private const string encryptionType = "none";

    // source from
    // https://gist.github.com/canton7/5670788?permalink_comment_id=3240331
    // https://gist.github.com/bosima/ee6630d30b533c7d7b2743a849e9b9d0

    public static string ToPuttyPrivateKey(RSACryptoServiceProvider cryptoServiceProvider, string Comment = "imported-openssh-key")
    {
        var publicParameters = cryptoServiceProvider.ExportParameters(false);
        byte[] publicBuffer = new byte[3 + keyType.Length + GetPrefixSize(publicParameters.Exponent) + publicParameters.Exponent.Length + GetPrefixSize(publicParameters.Modulus) + publicParameters.Modulus.Length + 1];

        using (var bw = new BinaryWriter(new MemoryStream(publicBuffer)))
        {
            bw.Write(new byte[] { 0x00, 0x00, 0x00 });
            bw.Write(keyType);
            PutPrefixed(bw, publicParameters.Exponent, CheckIsNeddPadding(publicParameters.Exponent));
            PutPrefixed(bw, publicParameters.Modulus, CheckIsNeddPadding(publicParameters.Modulus));
        }
        var publicBlob = System.Convert.ToBase64String(publicBuffer);

        var privateParameters = cryptoServiceProvider.ExportParameters(true);

        byte[] privateBuffer = new byte[paddedPrefixSize + privateParameters.D.Length + paddedPrefixSize + privateParameters.P!.Length + paddedPrefixSize + privateParameters.Q.Length + paddedPrefixSize + privateParameters.InverseQ.Length];

        using (var bw = new BinaryWriter(new MemoryStream(privateBuffer)))
        {
            PutPrefixed(bw, privateParameters.D, true);
            PutPrefixed(bw, privateParameters.P, true);
            PutPrefixed(bw, privateParameters.Q, true);
            PutPrefixed(bw, privateParameters.InverseQ, true);
        }
        var privateBlob = System.Convert.ToBase64String(privateBuffer);

        HMACSHA1 hmacSha1 = new(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes("putty-private-key-file-mac-key")));
        //byte[] bytesToHash = new byte[4 + 7 + 4 + 4 + 4 + Comment.Length + 4 + publicBuffer.Length + 4 + privateBuffer.Length];
        byte[] bytesToHash = new byte[prefixSize + keyType.Length + prefixSize + encryptionType.Length + prefixSize + Comment.Length + prefixSize + publicBuffer.Length + prefixSize + privateBuffer.Length];

        using (var bw = new BinaryWriter(new MemoryStream(bytesToHash)))
        {
            PutPrefixed(bw, Encoding.ASCII.GetBytes("ssh-rsa"));
            PutPrefixed(bw, Encoding.ASCII.GetBytes("none"));
            PutPrefixed(bw, Encoding.ASCII.GetBytes(Comment));
            PutPrefixed(bw, publicBuffer);
            PutPrefixed(bw, privateBuffer);
        }

        var hash = string.Join("", hmacSha1.ComputeHash(bytesToHash).Select(x => $"{x:x2}"));

        var sb = new StringBuilder();
        sb.AppendLine("PuTTY-User-Key-File-2: " + keyType);
        sb.AppendLine("Encryption: " + encryptionType);
        sb.AppendLine("Comment: " + Comment);

        var publicLines = SpliceText(publicBlob, lineLength);
        sb.AppendLine("Public-Lines: " + publicLines.Length);
        foreach (var line in publicLines)
        {
            sb.AppendLine(line);
        }

        var privateLines = SpliceText(privateBlob, lineLength);
        sb.AppendLine("Private-Lines: " + privateLines.Length);
        foreach (var line in privateLines)
        {
            sb.AppendLine(line);
        }

        sb.AppendLine("Private-MAC: " + hash);

        return sb.ToString();
    }

    private static void PutPrefixed(BinaryWriter bw, byte[] bytes, bool addLeadingNull = false)
    {
        bw.Write(BitConverter.GetBytes(bytes.Length + (addLeadingNull ? 1 : 0)).Reverse().ToArray());
        if (addLeadingNull)
            bw.Write(new byte[] { 0x00 });
        bw.Write(bytes);
    }
    
    private static string[] SpliceText(string text, int lineLength)
    {
        return Regex.Matches(text, ".{1," + lineLength + "}").Cast<Match>().Select(m => m.Value).ToArray();
    }
    private static int GetPrefixSize(byte[] bytes)
    {
        return CheckIsNeddPadding(bytes) ? paddedPrefixSize : prefixSize;
    }
    private static bool CheckIsNeddPadding(byte[] bytes)
    {
        // 128 == 10000000
        // This means that the number of bits can be divided by 8.
        // According to the algorithm in putty, you need to add a padding.
        return bytes[0] >= 128;
    }

}