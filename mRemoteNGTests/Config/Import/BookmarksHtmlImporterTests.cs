using System;
using System.IO;
using System.Linq;
using System.Text;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Import
{
    [TestFixture]
    public class BookmarksHtmlImporterTests
    {
        [Test]
        public void Import_ValidNetscapeBookmarks_CreatesStructure()
        {
            string htmlContent = @"<!DOCTYPE NETSCAPE-Bookmark-file-1>
<!-- This is an automatically generated file.
     It will be read and overwritten.
     DO NOT EDIT! -->
<TITLE>Bookmarks</TITLE>
<H1>Bookmarks</H1>
<DL><p>
    <DT><H3 ADD_DATE=""1588665600"" LAST_MODIFIED=""1588665600"" PERSONAL_TOOLBAR_FOLDER=""true"">My Folder</H3>
    <DL><p>
        <DT><A HREF=""https://example.com"" ADD_DATE=""1588665600"">Example</A>
    </DL><p>
    <DT><A HREF=""http://google.com"">Google</A>
</DL><p>";
            
            string filePath = Path.GetTempFileName() + ".html";
            File.WriteAllText(filePath, htmlContent, Encoding.UTF8);

            var destination = new ContainerInfo();
            var importer = new BookmarksHtmlImporter();

            try
            {
                importer.Import(filePath, destination);

                Assert.That(destination.Children, Has.Count.EqualTo(2), "Expected 2 children at root level");
                
                var folder = destination.Children.OfType<ContainerInfo>().FirstOrDefault(c => string.Equals(c.Name, "My Folder", StringComparison.Ordinal));
                Assert.That(folder, Is.Not.Null, "My Folder not found");
                Assert.That(folder.Children, Has.Count.EqualTo(1), "Expected 1 child in My Folder");
                
                var exampleConn = folder.Children.OfType<ConnectionInfo>().FirstOrDefault();
                Assert.That(exampleConn, Is.Not.Null, "Example connection not found");
                Assert.That(exampleConn.Name, Is.EqualTo("Example"));
                Assert.That(exampleConn.Hostname, Is.EqualTo("https://example.com"));
                Assert.That(exampleConn.Protocol, Is.EqualTo(ProtocolType.HTTPS));

                var googleConn = destination.Children.OfType<ConnectionInfo>().FirstOrDefault(c => string.Equals(c.Name, "Google", StringComparison.Ordinal));
                Assert.That(googleConn, Is.Not.Null, "Google connection not found");
                Assert.That(googleConn.Hostname, Is.EqualTo("http://google.com"));
                Assert.That(googleConn.Protocol, Is.EqualTo(ProtocolType.HTTP));
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
        
        [Test]
        public void Import_MissingFile_DoesNothing()
        {
            string filePath = "non_existent_file.html";
            var destination = new ContainerInfo();
            var importer = new BookmarksHtmlImporter();

            importer.Import(filePath, destination);

            Assert.That(destination.Children, Is.Empty);
        }
    }
}
