using System;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Config.Settings
{
    [SupportedOSPlatform("windows")]
    public class DockPanelLayoutSaver
    {
        private readonly ISerializer<DockPanel, string> _dockPanelSerializer;
        private readonly IDataProvider<string> _dataProvider;

        public DockPanelLayoutSaver(ISerializer<DockPanel, string> dockPanelSerializer,
                                    IDataProvider<string> dataProvider)
        {
            ArgumentNullException.ThrowIfNull(dockPanelSerializer);
            ArgumentNullException.ThrowIfNull(dataProvider);
            _dockPanelSerializer = dockPanelSerializer;
            _dataProvider = dataProvider;
        }

        public void Save()
        {
            try
            {
                if (Directory.Exists(SettingsFileInfo.SettingsPath) == false)
                {
                    Directory.CreateDirectory(SettingsFileInfo.SettingsPath);
                }

                string serializedLayout = _dockPanelSerializer.Serialize(FrmMain.Default.pnlDock);
                _dataProvider.Save(serializedLayout);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SavePanelsToXML failed", ex);
            }
        }

        public void SaveLayout(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Layout name cannot be empty", nameof(name));

            try
            {
                string layoutsDir = Path.Combine(SettingsFileInfo.SettingsPath, "Layouts");
                if (!Directory.Exists(layoutsDir))
                {
                    Directory.CreateDirectory(layoutsDir);
                }

                string filePath = Path.Combine(layoutsDir, name + ".xml");
                string serializedLayout = _dockPanelSerializer.Serialize(FrmMain.Default.pnlDock);
                File.WriteAllText(filePath, serializedLayout);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to save layout '{name}'", ex);
            }
        }

        public static void DeleteLayout(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Layout name cannot be empty", nameof(name));

            try
            {
                string layoutsDir = Path.Combine(SettingsFileInfo.SettingsPath, "Layouts");
                string filePath = Path.Combine(layoutsDir, name + ".xml");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to delete layout '{name}'", ex);
            }
        }
    }
}