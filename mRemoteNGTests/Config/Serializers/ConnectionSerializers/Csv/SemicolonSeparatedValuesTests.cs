using mRemoteNG.Config.Serializers.ConnectionSerializers.Csv;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Csv;

public class SemicolonSeparatedValuesTests
{
    [TestCase("plain", "plain")]
    [TestCase("pass;word", "\"pass;word\"")]
    [TestCase("say \"hi\"", "\"say \"\"hi\"\"\"")]
    [TestCase("two\r\nlines", "\"two\r\nlines\"")]
    public void ValuesAreQuotedOnlyWhenTheyNeedIt(string value, string expected)
    {
        Assert.That(SemicolonSeparatedValues.Quote(value), Is.EqualTo(expected));
    }

    [Test]
    public void QuotedFieldsKeepSeparatorsQuotesAndLineBreaks()
    {
        var records = SemicolonSeparatedValues.Parse("a;\"b;c\";\"d \"\"e\"\"\";\"f\r\ng\"\r\nh;i\r\n");

        Assert.That(records, Has.Count.EqualTo(2));
        Assert.That(records[0], Is.EqualTo(new[] { "a", "b;c", "d \"e\"", "f\r\ng" }));
        Assert.That(records[1], Is.EqualTo(new[] { "h", "i" }));
    }

    [Test]
    public void UnquotedRecordsSplitAsBefore()
    {
        var records = SemicolonSeparatedValues.Parse("Name;Hostname\nserver;10.0.0.1;\n\n");

        Assert.That(records, Has.Count.EqualTo(2));
        Assert.That(records[0], Is.EqualTo(new[] { "Name", "Hostname" }));
        Assert.That(records[1], Is.EqualTo(new[] { "server", "10.0.0.1", "" }));
    }

    [Test]
    public void AQuoteInsideAnUnquotedFieldIsKeptAsItIs()
    {
        var records = SemicolonSeparatedValues.Parse("5\" display;x");

        Assert.That(records[0], Is.EqualTo(new[] { "5\" display", "x" }));
    }
}
