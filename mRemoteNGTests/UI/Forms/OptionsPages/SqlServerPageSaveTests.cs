using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.UI.Forms.OptionsPages;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    /// <summary>
    /// Saving the SQL Server page must store what the user sees, and must not re-apply
    /// unchanged settings: a bad save makes every start fail (#3249).
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class SqlServerPageSaveTests
    {
        private bool _useSqlServer;
        private string _serverType;
        private string _host;
        private string _database;
        private string _user;
        private string _pass;
        private bool _readOnly;
        private RemoteConnectionsSyncronizer _synchronizer;
        private SqlServerPage _page;

        [SetUp]
        public void Setup()
        {
            OptionsDBsPage settings = OptionsDBsPage.Default;
            _useSqlServer = settings.UseSQLServer;
            _serverType = settings.SQLServerType;
            _host = settings.SQLHost;
            _database = settings.SQLDatabaseName;
            _user = settings.SQLUser;
            _pass = settings.SQLPass;
            _readOnly = settings.SQLReadOnly;
            _synchronizer = Runtime.ConnectionsService.RemoteConnectionsSyncronizer;
        }

        [TearDown]
        public void Teardown()
        {
            _page?.Dispose();
            _page = null;

            OptionsDBsPage settings = OptionsDBsPage.Default;
            settings.UseSQLServer = _useSqlServer;
            settings.SQLServerType = _serverType;
            settings.SQLHost = _host;
            settings.SQLDatabaseName = _database;
            settings.SQLUser = _user;
            settings.SQLPass = _pass;
            settings.SQLReadOnly = _readOnly;
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer = _synchronizer;
        }

        private static void StoreSettings(bool useSqlServer, string host, string database, string user, string password)
        {
            OptionsDBsPage settings = OptionsDBsPage.Default;
            settings.UseSQLServer = useSqlServer;
            settings.SQLServerType = "mssql";
            settings.SQLHost = host;
            settings.SQLDatabaseName = database;
            settings.SQLUser = user;
            settings.SQLPass = new LegacyRijndaelCryptographyProvider().Encrypt(password, Runtime.EncryptionKey);
            settings.SQLReadOnly = false;
        }

        private SqlServerPage LoadPage()
        {
            _page = new SqlServerPage();
            _page.LoadSettings();
            return _page;
        }

        [Test]
        public void ServerAndDatabaseTypedInTheSimpleViewAreSaved()
        {
            StoreSettings(false, "", "", "", "");
            SqlServerPage page = LoadPage();

            // the simple view is what the page shows until "Advanced" is clicked
            page.mrngTextBox2.Text = "sql01.example.local";
            page.mrngTextBox1.Text = "mRemoteNG";
            page.SaveSettings();

            Assert.That(OptionsDBsPage.Default.SQLHost, Is.EqualTo("sql01.example.local"));
            Assert.That(OptionsDBsPage.Default.SQLDatabaseName, Is.EqualTo("mRemoteNG"));
        }

        [Test]
        public void ServerTypedInTheAdvancedViewIsShownInTheSimpleView()
        {
            StoreSettings(false, "", "", "", "");
            SqlServerPage page = LoadPage();

            page.txtSQLServer.Text = "sql02.example.local";

            Assert.That(page.mrngTextBox2.Text, Is.EqualTo("sql02.example.local"));
        }

        [Test]
        public void StoredSqlLoginIsNotReplacedWhenThePageLoads()
        {
            StoreSettings(false, "sql01.example.local", "mRemoteNG", "mrng_login", "secret");
            SqlServerPage page = LoadPage();

            Assert.That(page.txtSQLUsername.Text, Is.EqualTo("mrng_login"));
            page.SaveSettings();
            Assert.That(OptionsDBsPage.Default.SQLUser, Is.EqualTo("mrng_login"));
        }

        [Test]
        public void WindowsAuthenticationIsSavedAsAnEmptyUserName()
        {
            // an empty user name is what makes the connector use Windows authentication
            StoreSettings(false, "sql01.example.local", "mRemoteNG", "", "");
            SqlServerPage page = LoadPage();

            page.SaveSettings();

            Assert.That(OptionsDBsPage.Default.SQLUser, Is.Empty);
        }

        [Test]
        public void MySqlIsSavedAsTheTypeTheConnectorFactoryReads()
        {
            StoreSettings(false, "sql01.example.local", "mRemoteNG", "mrng_login", "secret");
            SqlServerPage page = LoadPage();

            ((ComboBox)page.Controls.Find("txtSQLType", true).Single()).SelectedIndex = 1;
            page.SaveSettings();

            Assert.That(OptionsDBsPage.Default.SQLServerType, Is.EqualTo("mysql"));
        }

        [Test]
        public void UseSqlServerCheckboxDoesNotCoverTheServerFields()
        {
            StoreSettings(true, "", "", "", "");
            SqlServerPage page = LoadPage();
            page.Size = new System.Drawing.Size(656, 500);
            page.CreateControl();
            page.PerformLayout();

            Control checkBox = page.Controls.Find("chkUseSQLServer", true).Single();
            Control serverBlock = page.Controls.Find("pnlServerBlock", true).Single();

            Assert.That(serverBlock.Visible, Is.True);
            Assert.That(checkBox.Bounds.IntersectsWith(serverBlock.Bounds), Is.False,
                () => $"checkbox {checkBox.Bounds} overlaps server block {serverBlock.Bounds}");
        }

        [Test]
        public void SimpleViewCanBeShownAgainAfterTheAdvancedView()
        {
            StoreSettings(true, "", "", "", "");
            SqlServerPage page = LoadPage();
            page.Size = new System.Drawing.Size(656, 500);
            page.CreateControl();

            Control expandButton = page.Controls.Find("btnExpandOptions", true).Single();
            Control testButton = page.Controls.Find("btnTestConnection", true).Single();
            Control tabs = page.Controls.Find("tabCtrlSQL", true).Single();
            Control simpleView = page.Controls.Find("tableLayoutPanel1", true).Single();
            MethodInfo click = typeof(SqlServerPage).GetMethod("btnExpandOptions_Click", BindingFlags.NonPublic | BindingFlags.Instance);

            click.Invoke(page, [expandButton, EventArgs.Empty]);
            Assert.That(tabs.Visible, Is.True);
            Assert.That(expandButton.Bounds.IntersectsWith(tabs.Bounds), Is.False, "the button back to the simple view is covered by the tabs");
            Assert.That(testButton.Bounds.IntersectsWith(tabs.Bounds), Is.False, "Test connection is covered by the tabs");

            click.Invoke(page, [expandButton, EventArgs.Empty]);
            Assert.That(tabs.Visible, Is.False);
            Assert.That(simpleView.Visible, Is.True);
            Assert.That(expandButton.Bounds.IntersectsWith(simpleView.Bounds), Is.False);
        }

        [Test]
        public void UnchangedSettingsAreNotAppliedAgain()
        {
            StoreSettings(true, "sql01.example.local", "mRemoteNG", "mrng_login", "secret");
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer = null;
            SqlServerPage page = LoadPage();

            // applying would replace the synchronizer and reload the connections from the database
            page.SaveSettings();

            Assert.That(Runtime.ConnectionsService.RemoteConnectionsSyncronizer, Is.Null);
        }
    }
}
