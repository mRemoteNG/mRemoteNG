using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Config.Settings
{
    public class DockPanelLayoutSerializer : ISerializer<DockPanel, string>
    {
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