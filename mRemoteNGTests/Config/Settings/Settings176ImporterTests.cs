using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using mRemoteNG.Config.Settings;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Settings
{
    [TestFixture]
    public class Settings176ImporterTests
    {
        private string _root;

        [SetUp]
        public void Setup()
        {
            _root = Path.Combine(Path.GetTempPath(), $"mrng176_{Guid.NewGuid():N}");
            Directory.CreateDirectory(_root);
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                Directory.Delete(_root, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        [Test]
        public void EachValueLandsInTheClassThatHoldsItNow()
        {
            string file = WriteUserConfig(Setting("ThemeName", "My own theme"), Setting("SingleInstance", "True"));
            ThemeLikeSettings theme = new();
            StartupLikeSettings startup = new();

            int imported = Settings176Importer.Import(file, [theme, startup], _root);

            Assert.That(theme.ThemeName, Is.EqualTo("My own theme"));
            Assert.That(startup.SingleInstance, Is.True);
            Assert.That(imported, Is.EqualTo(2));
        }

        [Test]
        public void AnOptionNowInTwoClassesIsSetInBoth()
        {
            string file = WriteUserConfig(Setting("AutoSaveEveryMinutes", "5"));
            ThemeLikeSettings theme = new();
            StartupLikeSettings startup = new();

            Settings176Importer.Import(file, [theme, startup], _root);

            Assert.That(theme.AutoSaveEveryMinutes, Is.EqualTo(5));
            Assert.That(startup.AutoSaveEveryMinutes, Is.EqualTo(5));
        }

        [Test]
        public void ValuesAreReadAsTheirTypes()
        {
            string file = WriteUserConfig(Setting("MainFormLocation", "120, 80"), Setting("SingleInstance", "True"));
            StartupLikeSettings startup = new();

            Settings176Importer.Import(file, [startup], _root);

            Assert.That(startup.MainFormLocation, Is.EqualTo(new Point(120, 80)));
            Assert.That(startup.SingleInstance, Is.True);
        }

        [Test]
        public void AValueThatCannotBeReadKeepsTheDefault()
        {
            string file = WriteUserConfig(Setting("AutoSaveEveryMinutes", "every now and then"));
            ThemeLikeSettings theme = new();

            int imported = Settings176Importer.Import(file, [theme], _root);

            Assert.That(theme.AutoSaveEveryMinutes, Is.EqualTo(10));
            Assert.That(imported, Is.Zero);
        }

        [Test]
        public void OptionsThatNoLongerExistAreSkipped()
        {
            string file = WriteUserConfig(Setting("AutomaticallyGetSessionInfo", "True"), Setting("ThemeName", "darcula"));
            ThemeLikeSettings theme = new();

            int imported = Settings176Importer.Import(file, [theme], _root);

            Assert.That(imported, Is.EqualTo(1));
        }

        [Test]
        public void TheUpgradeFlagsAreNotImported()
        {
            string file = WriteUserConfig(Setting("DoUpgrade", "False"), Setting("FirstStart", "False"));
            StartupLikeSettings startup = new();

            int imported = Settings176Importer.Import(file, [startup], _root);

            Assert.That(startup.DoUpgrade, Is.True);
            Assert.That(startup.FirstStart, Is.True);
            Assert.That(imported, Is.Zero);
        }

        [Test]
        public void TheLogPath176WroteForItselfIsNotImported()
        {
            string defaultLog = Path.Combine(_root, "mRemoteNG", "mRemoteNG.log");
            string file = WriteUserConfig(Setting("LogFilePath", defaultLog));
            StartupLikeSettings startup = new();

            Settings176Importer.Import(file, [startup], _root);

            Assert.That(startup.LogFilePath, Is.Empty);
        }

        [Test]
        public void ALogPathChosenElsewhereIsImported()
        {
            string file = WriteUserConfig(Setting("LogFilePath", @"D:\logs\mremoteng.log"));
            StartupLikeSettings startup = new();

            Settings176Importer.Import(file, [startup], _root);

            Assert.That(startup.LogFilePath, Is.EqualTo(@"D:\logs\mremoteng.log"));
        }

        [TestCase("vs2015dark", "vs2015darkNG")]
        [TestCase("darcula", "darculaNG")]
        [TestCase("vs2015light", "vs2015lightNG")]
        [TestCase("My own theme", "My own theme")]
        public void TheThemesOf176AreFoundUnderTheirNewNames(string oldName, string newName)
        {
            string file = WriteUserConfig(Setting("ThemeName", oldName));
            ThemeLikeSettings theme = new();

            Settings176Importer.Import(file, [theme], _root);

            Assert.That(theme.ThemeName, Is.EqualTo(newName));
        }

        [Test]
        public void AFileWithoutThe176SectionImportsNothing()
        {
            string file = Path.Combine(_root, "user.config");
            File.WriteAllText(file, "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration><userSettings>"
                + "<mRemoteNG.Properties.Settings>" + Setting("ThemeName", "darcula") + "</mRemoteNG.Properties.Settings>"
                + "</userSettings></configuration>");
            ThemeLikeSettings theme = new();

            Assert.That(Settings176Importer.Import(file, [theme], _root), Is.Zero);
            Assert.That(theme.ThemeName, Is.EqualTo("vs2015light"));
        }

        [Test]
        public void TheNewestVersionIsFoundAcrossInstallFolders()
        {
            WriteVersion("mRemoteNG.exe_Url_aaaa", "1.76.11.40527");
            string newest = WriteVersion("mRemoteNG.exe_Url_bbbb", "1.76.20.24615");
            WriteVersion("mRemoteNG.exe_Url_aaaa", "1.75.7012.16814");

            Assert.That(Settings176Importer.FindNewestUserConfig(_root), Is.EqualTo(newest));
        }

        [Test]
        public void NothingIsFoundWithout176()
        {
            Assert.That(Settings176Importer.FindNewestUserConfig(_root), Is.Null);

            Directory.CreateDirectory(Path.Combine(_root, "mRemoteNG", "mRemoteNG.exe_Url_aaaa", "1.76.20.24615"));
            Assert.That(Settings176Importer.FindNewestUserConfig(_root), Is.Null);
        }

        private string WriteUserConfig(params string[] settings)
        {
            string file = Path.Combine(_root, "user.config");
            File.WriteAllText(file, "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration><userSettings>"
                + "<mRemoteNG.Settings>" + string.Concat(settings) + "</mRemoteNG.Settings>"
                + "</userSettings></configuration>");
            return file;
        }

        private string WriteVersion(string appFolder, string version)
        {
            string folder = Path.Combine(_root, "mRemoteNG", appFolder, version);
            Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, "user.config");
            File.WriteAllText(file, "<configuration />");
            return file;
        }

        private static string Setting(string name, string value) =>
            $"<setting name=\"{name}\" serializeAs=\"String\"><value>{value}</value></setting>";

        private sealed class ThemeLikeSettings : ApplicationSettingsBase
        {
            [UserScopedSetting, DefaultSettingValue("vs2015light")]
            public string ThemeName
            {
                get => (string)this[nameof(ThemeName)];
                set => this[nameof(ThemeName)] = value;
            }

            [UserScopedSetting, DefaultSettingValue("10")]
            public int AutoSaveEveryMinutes
            {
                get => (int)this[nameof(AutoSaveEveryMinutes)];
                set => this[nameof(AutoSaveEveryMinutes)] = value;
            }
        }

        private sealed class StartupLikeSettings : ApplicationSettingsBase
        {
            [UserScopedSetting, DefaultSettingValue("False")]
            public bool SingleInstance
            {
                get => (bool)this[nameof(SingleInstance)];
                set => this[nameof(SingleInstance)] = value;
            }

            [UserScopedSetting, DefaultSettingValue("0")]
            public int AutoSaveEveryMinutes
            {
                get => (int)this[nameof(AutoSaveEveryMinutes)];
                set => this[nameof(AutoSaveEveryMinutes)] = value;
            }

            [UserScopedSetting, DefaultSettingValue("0, 0")]
            public Point MainFormLocation
            {
                get => (Point)this[nameof(MainFormLocation)];
                set => this[nameof(MainFormLocation)] = value;
            }

            [UserScopedSetting, DefaultSettingValue("")]
            public string LogFilePath
            {
                get => (string)this[nameof(LogFilePath)];
                set => this[nameof(LogFilePath)] = value;
            }

            [UserScopedSetting, DefaultSettingValue("True")]
            public bool DoUpgrade
            {
                get => (bool)this[nameof(DoUpgrade)];
                set => this[nameof(DoUpgrade)] = value;
            }

            [UserScopedSetting, DefaultSettingValue("True")]
            public bool FirstStart
            {
                get => (bool)this[nameof(FirstStart)];
                set => this[nameof(FirstStart)] = value;
            }
        }
    }
}
