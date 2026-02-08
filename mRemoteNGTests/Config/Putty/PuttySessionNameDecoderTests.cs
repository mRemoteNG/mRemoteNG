using System.Linq;
using System.Text;
using mRemoteNG.Config.Putty;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Putty;

[TestFixture]
public class PuttySessionNameDecoderTests
{
    [Test]
    public void Decode_DecodesUtf8PercentEncodedName()
    {
        const string encodedName = "%EC%84%9C%EB%B2%84%20%EC%84%A4%EC%B9%98";

        string decodedName = PuttySessionNameDecoder.Decode(encodedName);

        Assert.That(decodedName, Is.EqualTo("서버 설치"));
    }

    [Test]
    public void Decode_PreservesPlusCharacter()
    {
        string decodedName = PuttySessionNameDecoder.Decode("Session+Name");

        Assert.That(decodedName, Is.EqualTo("Session+Name"));
    }

    [Test]
    public void Decode_FallsBackWhenUtf8DecodingFails()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Encoding koreanCodePage = Encoding.GetEncoding(949);
        string encodedName = PercentEncode("서버 설치", koreanCodePage);

        string decodedName = PuttySessionNameDecoder.Decode(encodedName, koreanCodePage);

        Assert.That(decodedName, Is.EqualTo("서버 설치"));
    }

    private static string PercentEncode(string value, Encoding encoding)
    {
        byte[] bytes = encoding.GetBytes(value);
        return string.Concat(bytes.Select(currentByte => $"%{currentByte:X2}"));
    }
}
