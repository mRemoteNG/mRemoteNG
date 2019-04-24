using mRemoteNG.Config.Serializers;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Config.Settings
{
    public class DockPanelLayoutSerializer : ISerializer<DockPanel, string>
    {
        public Version Version { get; } = new Version(1, 0);

        public string Serialize(DockPanel dockPanel)
        {
            if (dockPanel == null)
                throw new ArgumentNullException(nameof(dockPanel));

            XDocument xdoc;
            using (var memoryStream = new MemoryStream())
            {
                dockPanel.SaveAsXml(memoryStream, Encoding.UTF8);
                memoryStream.Position = 0;
                xdoc = XDocument.Load(memoryStream, LoadOptions.SetBaseUri);
            }

            return $"{xdoc.Declaration}{Environment.NewLine}{xdoc}";
        }
    }
}