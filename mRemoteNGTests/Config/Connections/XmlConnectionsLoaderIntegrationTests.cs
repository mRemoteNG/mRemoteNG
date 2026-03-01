using System;
using System.IO;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Serializers; 
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Messages;
using mRemoteNG.Tree.Root;
using mRemoteNGTests.TestHelpers;
using NSubstitute;
using NUnit.Framework;
using System.Linq; 
using mRemoteNG.Security; 
using mRemoteNG.Security.SymmetricEncryption; 
using System.Xml; 
using System.Xml.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Tools; 
using System.Text;
using System.Collections.Generic; 

namespace mRemoteNGTests.Config.Connections;

[TestFixture]
public class XmlConnectionsLoaderIntegrationTests
{
    private MessageCollector _messageCollector;

    [SetUp]
    public void Setup()
    {
        _messageCollector = new MessageCollector();
    }

    private static LegacyRijndaelCryptographyProvider CreateCryptographyProvider()
    {
        return new LegacyRijndaelCryptographyProvider();
    }

    private static string WriteXmlToString(XNode xmlDocument)
    {
        string xmlString;
        XmlWriterSettings xmlWriterSettings = new() { Indent = true, IndentChars = "    ", Encoding = Encoding.UTF8};
        MemoryStream memoryStream = new();
        using (XmlWriter xmlTextWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
        {
            xmlDocument.WriteTo(xmlTextWriter);
            xmlTextWriter.Flush();
            StreamReader streamReader = new(memoryStream, Encoding.UTF8, true);
            memoryStream.Seek(0, SeekOrigin.Begin);
            xmlString = streamReader.ReadToEnd();
        }

        return xmlString;
    }

    private static string CreateEncryptedXmlFile(SecureString masterPassword, ConnectionInfo connectionInfo)
    {
        var cryptographyProvider = new AeadCryptographyProvider();
        cryptographyProvider.KeyDerivationIterations = 1000; // Ensure valid iterations for Pkcs5S2KeyGenerator

        var saveFilter = new SaveFilter();
        var connectionNodeSerializer = new XmlConnectionNodeSerializer28(cryptographyProvider, masterPassword, saveFilter);

        var xmlConnectionsSerializer = new XmlConnectionsSerializer(cryptographyProvider, connectionNodeSerializer)
        {
            UseFullEncryption = true
        };

        var rootNode = new RootNodeInfo(RootNodeType.Connection);
        rootNode.AddChild(connectionInfo);
        rootNode.PasswordString = masterPassword.ConvertToUnsecureString();

        // Serialize the root node to get the XML document, then manually add KdfIterations
        string xmlContent = xmlConnectionsSerializer.Serialize(rootNode);
        XDocument doc = XDocument.Parse(xmlContent);
        doc.Root.SetAttributeValue("KdfIterations", cryptographyProvider.KeyDerivationIterations);
        doc.Root.SetAttributeValue("EncryptionEngine", cryptographyProvider.CipherEngine.ToString());
        doc.Root.SetAttributeValue("BlockCipherMode", cryptographyProvider.CipherMode.ToString());

        return WriteXmlToString(doc);
    }

    [Test]
    public void LoadsEncryptedFile_WithCorrectPassword_Successfully()
    {
        // Arrange
        var masterPassword = new SecureString();
        "testpass".ToCharArray().ToList().ForEach(masterPassword.AppendChar);
        masterPassword.MakeReadOnly();

        var connectionInfo = new ConnectionInfo() { Hostname = "encryptedhost", Password = "somepassword" };
        string encryptedXmlContent = CreateEncryptedXmlFile(masterPassword, connectionInfo);

        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            File.WriteAllText(filePath, encryptedXmlContent);

            // Mock the password requestor to return the correct master password
            Func<string, Optional<SecureString>> mockPasswordRequestor =
                Substitute.For<Func<string, Optional<SecureString>>>();
            mockPasswordRequestor
                .Invoke(Arg.Any<string>())
                .Returns(new Optional<SecureString>(masterPassword)); 

            var loader = new XmlConnectionsLoader(filePath, _messageCollector, mockPasswordRequestor);

            // Act
            var loadedTree = loader.Load();

            // Assert
            Assert.That(loadedTree, Is.Not.Null);
            Assert.That(loadedTree.RootNodes.Count, Is.EqualTo(1));
            Assert.That(loadedTree.RootNodes[0].Children.Count, Is.EqualTo(1));
            Assert.That(loadedTree.RootNodes[0].Children[0].Hostname, Is.EqualTo("encryptedhost"));
            Assert.That(loadedTree.RootNodes[0].Children[0].Password, Is.EqualTo("somepassword")); 
            mockPasswordRequestor.Received(1).Invoke(Path.GetFileName(filePath));
        }
    }

    [Test]
    public void LoadsEncryptedFile_WithIncorrectPassword_ReturnsNull()
    {
        // Arrange
        var correctMasterPassword = new SecureString();
        "testpass".ToCharArray().ToList().ForEach(correctMasterPassword.AppendChar);
        correctMasterPassword.MakeReadOnly();

        var incorrectMasterPassword = new SecureString();
        "wrongpass".ToCharArray().ToList().ForEach(incorrectMasterPassword.AppendChar);
        incorrectMasterPassword.MakeReadOnly();

        var connectionInfo = new ConnectionInfo() { Hostname = "encryptedhost", Password = "somepassword" };
        string encryptedXmlContent = CreateEncryptedXmlFile(correctMasterPassword, connectionInfo);

        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            File.WriteAllText(filePath, encryptedXmlContent);

            // Mock the password requestor to return an incorrect master password
            Func<string, Optional<SecureString>> mockPasswordRequestor =
                Substitute.For<Func<string, Optional<SecureString>>>();
            mockPasswordRequestor
                .Invoke(Arg.Any<string>())
                .Returns(new Optional<SecureString>(incorrectMasterPassword));

            var loader = new XmlConnectionsLoader(filePath, _messageCollector, mockPasswordRequestor);

            // Act
            var loadedTree = loader.Load();

            // Assert
            Assert.That(loadedTree, Is.Null); // Expect null on incorrect password
            mockPasswordRequestor.Received(3).Invoke(Path.GetFileName(filePath)); // Expect 3 calls due to retry logic
        }
    }

    [Test]
    public void LoadsEncryptedFile_WithNoPasswordProvided_ReturnsNull()
    {
        // Arrange
        var correctMasterPassword = new SecureString();
        "testpass".ToCharArray().ToList().ForEach(correctMasterPassword.AppendChar);
        correctMasterPassword.MakeReadOnly();

        var connectionInfo = new ConnectionInfo() { Hostname = "encryptedhost", Password = "somepassword" };
        string encryptedXmlContent = CreateEncryptedXmlFile(correctMasterPassword, connectionInfo);

        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            File.WriteAllText(filePath, encryptedXmlContent);

            // Mock the password requestor to return no password (empty optional)
            Func<string, Optional<SecureString>> mockPasswordRequestor =
                Substitute.For<Func<string, Optional<SecureString>>>();
            mockPasswordRequestor
                .Invoke(Arg.Any<string>())
                .Returns(Optional<SecureString>.Empty); 

            var loader = new XmlConnectionsLoader(filePath, _messageCollector, mockPasswordRequestor);

            // Act
            var loadedTree = loader.Load();

            // Assert
            Assert.That(loadedTree, Is.Null); // Expect null when no password is provided
            mockPasswordRequestor.Received(1).Invoke(Path.GetFileName(filePath)); 
        }
    }
}