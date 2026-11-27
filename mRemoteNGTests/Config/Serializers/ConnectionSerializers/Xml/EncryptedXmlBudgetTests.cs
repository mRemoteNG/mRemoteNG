using System;
using System.Linq;
using System.Xml;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml;

[NonParallelizable]
public class EncryptedXmlBudgetTests
{
    [TestCase(false)]
    [TestCase(true)]
    public void Deserialize_EncryptedExcessiveDepth_RejectsBeforeCreatingTree(bool dpapi)
    {
        string content = string.Concat(Enumerable.Repeat("<Node Type='Container'>", 128)) +
            string.Concat(Enumerable.Repeat("</Node>", 128));
        Assert.Throws<XmlException>(() => new XmlConnectionsDeserializer().Deserialize(Encrypt(content, dpapi), true));
    }

    [Test]
    public void Deserialize_EncryptedExcessiveNodeCount_RejectsBeforeCreatingTree()
    {
        string content = string.Concat(Enumerable.Repeat("<Node/>", 250_000));
        Assert.Throws<XmlException>(() => new XmlConnectionsDeserializer().Deserialize(Encrypt(content, false), true));
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Deserialize_ValidEncryptedFragment_PreservesNamespacesAndConnections(bool dpapi)
    {
        string xml = Encrypt("<mrng:Node Type='Connection' Name='Synthetic' Id='synthetic-1'/>", dpapi);
        var model = new XmlConnectionsDeserializer().Deserialize(xml, true);
        Assert.That(model.GetRecursiveChildList().Single().Name, Is.EqualTo("Synthetic"));
        Assert.That(model.GetRecursiveChildList().Single().ConstantID, Is.EqualTo("synthetic-1"));
    }

    [TestCase("<!DOCTYPE Node [<!ENTITY x SYSTEM 'file:///unread'>]><Node>&x;</Node>")]
    [TestCase("<Node/><Node>")]
    public void ReplaceInnerXml_RejectedFragment_LeavesOriginalChildrenUntouched(string content)
    {
        XmlDocument document = SecureXmlHelper.LoadXmlFromString("<Connections><original/></Connections>");
        Assert.Throws<XmlException>(() => SecureXmlHelper.ReplaceInnerXml(document.DocumentElement, content));
        Assert.That(document.DocumentElement.InnerXml, Is.EqualTo("<original />"));
    }

    private static string Encrypt(string content, bool dpapi)
    {
        ICryptographyProvider crypto = dpapi ? new DpapiCryptographyProvider() : new AeadCryptographyProvider();
        crypto.KeyDerivationIterations = 1000;
        RootNodeInfo root = new(RootNodeType.Connection);
        var element = new XmlRootNodeSerializer().SerializeRootNodeInfo(root, crypto, new Version(2, 9), true);
        element.Value = crypto.Encrypt(content, root.PasswordString.ConvertToSecureString());
        return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + element;
    }
}
