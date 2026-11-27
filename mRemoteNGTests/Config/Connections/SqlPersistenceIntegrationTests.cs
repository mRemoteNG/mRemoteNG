using System;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
using mRemoteNG.Connection;
using mRemoteNG.Properties;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Connections
{
    [TestFixture("mssql")]
    [TestFixture("mysql")]
    [Category("DisposableSql")]
    [NonParallelizable]
    public class SqlPersistenceIntegrationTests(string backend)
    {
        private string _host;
        private string _password;
        private string _database;
        private string _savedType;
        private string _savedDatabase;
        private bool _savedReadOnly;
        private const string OldPassword = "Synthetic-tree-password-1";
        private const string NewPassword = "Synthetic-tree-password-2";

        [OneTimeSetUp]
        public void RequireDisposableProfile()
        {
            if (Environment.GetEnvironmentVariable("MREMOTENG_DISPOSABLE_SQL") != "1")
                Assert.Ignore("Run Tools/Test-SqlPersistence.ps1 to provision disposable databases.");
            _host = Environment.GetEnvironmentVariable("MREMOTENG_TEST_" + backend.ToUpperInvariant());
            _password = Environment.GetEnvironmentVariable("MREMOTENG_TEST_SQL_PASSWORD");
            Assert.That(_host, Does.Match(@"^127\.0\.0\.1:[0-9]+$"), "Only explicit loopback test endpoints are allowed.");
            Assert.That(_password, Is.Not.Null.And.Not.Empty);
            Exception lastError = null;
            for (int attempt = 0; attempt < 60; attempt++)
            {
                try
                {
                    using IDatabaseConnector connector = Connector(AdminDatabase);
                    DbConnectionStringBuilder settings = new() { ConnectionString = connector.DbConnection().ConnectionString };
                    settings["Connection Timeout"] = 2;
                    connector.DbConnection().ConnectionString = settings.ConnectionString;
                    connector.Connect();
                    return;
                }
                catch (DbException error)
                {
                    lastError = error;
                    Thread.Sleep(1000);
                }
            }
            Assert.Fail("Disposable " + backend + " did not become ready: " + lastError?.GetType().Name);
        }

        private string AdminDatabase => backend == "mssql" ? "master" : "mysql";

        private IDatabaseConnector Connector(string database = null)
        {
            return backend == "mssql"
                ? new MSSqlDatabaseConnector(_host, database ?? _database, "sa", _password)
                : new MySqlDatabaseConnector(_host, database ?? _database, "root", _password);
        }

        [SetUp]
        public void CreateDatabase()
        {
            _savedType = OptionsDBsPage.Default.SQLServerType;
            _savedDatabase = OptionsDBsPage.Default.SQLDatabaseName;
            _savedReadOnly = OptionsDBsPage.Default.SQLReadOnly;
            _database = "mremoteng_test_" + Guid.NewGuid().ToString("N");
            OptionsDBsPage.Default.SQLServerType = backend;
            OptionsDBsPage.Default.SQLDatabaseName = "unused_saved_setting";
            OptionsDBsPage.Default.SQLReadOnly = false;
            Execute("CREATE DATABASE " + _database, AdminDatabase);
            ConnectionTreeModel seed = Loader().Load();
            RootNodeInfo root = Root(seed);
            root.Name = "Original root";
            root.PasswordString = OldPassword;
            root.AddChild(new ConnectionInfo { Name = "Original connection", Password = "Synthetic-host-secret" });
            Saver().Save(seed);
        }

        [TearDown]
        public void DropDatabase()
        {
            try
            {
                Assert.That(_database, Does.Match("^mremoteng_test_[a-f0-9]{32}$"));
                // Pooled connections can otherwise prevent SQL Server DROP DATABASE.
                Microsoft.Data.SqlClient.SqlConnection.ClearAllPools();
                MySql.Data.MySqlClient.MySqlConnection.ClearAllPools();
                Execute("DROP DATABASE " + _database, AdminDatabase);
            }
            finally
            {
                OptionsDBsPage.Default.SQLServerType = _savedType;
                OptionsDBsPage.Default.SQLDatabaseName = _savedDatabase;
                OptionsDBsPage.Default.SQLReadOnly = _savedReadOnly;
            }
        }

        [Test]
        public void Save_TwoWritersAtOneRevision_OneCommitsAndOtherRetainsDraft()
        {
            ConnectionTreeModel first = Loader().Load();
            ConnectionTreeModel second = Loader().Load();
            long? revision = first.SqlTreeRevision;
            first.GetRecursiveChildList().Single().Name = "First draft";
            second.GetRecursiveChildList().Single().Name = "Second draft";
            using Barrier start = new(2);
            Task<Exception> Write(ConnectionTreeModel model) => Task.Run(() =>
            {
                start.SignalAndWait();
                try { Saver().Save(model); return null; }
                catch (Exception error) { return error; }
            });
            Task<Exception>[] writes = [Write(first), Write(second)];
            Task.WaitAll(writes);
            Assert.That(writes.Count(t => t.Result == null), Is.EqualTo(1));
            Assert.That(writes.Single(t => t.Result != null).Result,
                Is.TypeOf<InvalidOperationException>().And.Message.Contains("draft was retained"));
            ConnectionTreeModel loser = writes[0].Result == null ? second : first;
            ConnectionTreeModel winner = ReferenceEquals(loser, first) ? second : first;
            Assert.That(loser.SqlTreeRevision, Is.EqualTo(revision));
            Assert.That(loser.GetRecursiveChildList().Single().Name,
                Is.EqualTo(ReferenceEquals(loser, first) ? "First draft" : "Second draft"));
            Assert.That(Loader().Load().GetRecursiveChildList().Single().Name,
                Is.EqualTo(winner.GetRecursiveChildList().Single().Name));
            Assert.That(winner.SqlTreeRevision, Is.EqualTo(revision + 1));
        }

        [Test]
        public void Save_FailureAfterRowsAndUpdateMarker_RollsBackEntireSnapshot()
        {
            ConnectionTreeModel draft = Loader().Load();
            SqlConnectionListMetaData before = Metadata();
            DateTime marker = UpdateMarker();
            long? revision = draft.SqlTreeRevision;
            Execute(backend == "mssql"
                ? "CREATE TRIGGER RejectRevision ON dbo.tblTreeRevision AFTER UPDATE AS THROW 51000, 'Synthetic late failure', 1;"
                : "CREATE TRIGGER RejectRevision BEFORE UPDATE ON tblTreeRevision FOR EACH ROW SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Synthetic late failure'");
            Root(draft).Name = "Rejected root";
            Root(draft).PasswordString = NewPassword;
            draft.GetRecursiveChildList().Single().Name = "Rejected connection";
            Assert.That(() => Saver().Save(draft),
                Throws.InstanceOf<DbException>().With.Message.Contains("Synthetic late failure"));
            SqlConnectionListMetaData after = Metadata();
            Assert.Multiple(() =>
            {
                Assert.That(after.Name, Is.EqualTo(before.Name));
                Assert.That(after.Protected, Is.EqualTo(before.Protected));
                Assert.That(after.ConfVersion, Is.EqualTo(before.ConfVersion));
                Assert.That(UpdateMarker(), Is.EqualTo(marker));
                Assert.That(draft.SqlTreeRevision, Is.EqualTo(revision));
            });
            ConnectionTreeModel persisted = Loader().Load();
            Assert.That(persisted.SqlTreeRevision, Is.EqualTo(revision));
            Assert.That(persisted.GetRecursiveChildList().Single().Name, Is.EqualTo("Original connection"));
            Assert.That(persisted.GetRecursiveChildList().Single().Password, Is.EqualTo("Synthetic-host-secret"));
        }

        [Test]
        public void Load_PasswordChangesDuringAuthentication_ReturnsOneConsistentSnapshot()
        {
            ConnectionTreeModel writer = Loader().Load();
            long? revision = writer.SqlTreeRevision;
            bool prompted = false;
            ConnectionTreeModel loaded = Loader(() =>
            {
                Assert.That(prompted, Is.False, "Authentication must use the captured metadata.");
                prompted = true;
                Root(writer).PasswordString = NewPassword;
                writer.GetRecursiveChildList().Single().Name = "New generation";
                writer.GetRecursiveChildList().Single().Password = "New synthetic secret";
                Saver().Save(writer);
                return OldPassword.ConvertToSecureString();
            }).Load();
            Assert.That(prompted, Is.True);
            Assert.That(loaded.SqlTreeRevision, Is.EqualTo(revision));
            Assert.That(Root(loaded).Name, Is.EqualTo("Original root"));
            Assert.That(loaded.GetRecursiveChildList().Single().Name, Is.EqualTo("Original connection"));
            Assert.That(loaded.GetRecursiveChildList().Single().Password, Is.EqualTo("Synthetic-host-secret"));
            Assert.That(Loader(() => NewPassword.ConvertToSecureString()).Load().GetRecursiveChildList().Single().Name,
                Is.EqualTo("New generation"));
        }

        [TestCase(2, 1L)]
        [TestCase(1, -1L)]
        public void Save_UnsupportedProtocolOrInvalidRevision_RejectsWithoutWriting(int protocol, long revision)
        {
            ConnectionTreeModel draft = Loader().Load();
            SqlConnectionListMetaData before = Metadata();
            DateTime marker = UpdateMarker();
            Execute($"UPDATE tblTreeRevision SET MinimumWriterProtocol={protocol}, Revision={revision}");
            Root(draft).Name = "Rejected root";
            Assert.Throws<InvalidOperationException>(() => Saver().Save(draft));
            Assert.That(Metadata().Protected, Is.EqualTo(before.Protected));
            Assert.That(Metadata().Name, Is.EqualTo(before.Name));
            Assert.That(UpdateMarker(), Is.EqualTo(marker));
        }

        [Test]
        public void Save_RevisionOverflow_RollsBackWrites()
        {
            ConnectionTreeModel draft = Loader().Load();
            SqlConnectionListMetaData before = Metadata();
            DateTime marker = UpdateMarker();
            Execute("UPDATE tblTreeRevision SET Revision=9223372036854775807");
            draft.SqlTreeRevision = long.MaxValue;
            Root(draft).Name = "Rejected root";
            draft.GetRecursiveChildList().Single().Name = "Rejected row";
            Assert.Throws<OverflowException>(() => Saver().Save(draft));
            Assert.That(Metadata().Protected, Is.EqualTo(before.Protected));
            Assert.That(Metadata().Name, Is.EqualTo(before.Name));
            Assert.That(UpdateMarker(), Is.EqualTo(marker));
            ConnectionTreeModel persisted = Loader().Load();
            Assert.That(persisted.SqlTreeRevision, Is.EqualTo(long.MaxValue));
            Assert.That(persisted.GetRecursiveChildList().Single().Name, Is.EqualTo("Original connection"));
        }

        [Test]
        public void Synchronize_AfterSaveConflict_DoesNotReplaceDraft()
        {
            ConnectionTreeModel draft = Loader().Load();
            ConnectionTreeModel writer = Loader().Load();
            draft.GetRecursiveChildList().Single().Name = "Unsaved local draft";
            Saver().Save(writer);
            Assert.Throws<InvalidOperationException>(() => Saver().Save(draft));
            int reloads = 0;
            using RemoteConnectionsSyncronizer synchronizer = new(Checker(), () => reloads++, () => draft);
            ConnectionsUpdateAvailableEventArgs update = null;
            synchronizer.ConnectionsUpdateAvailable += (_, args) => update = args;
            Assert.That(synchronizer.IsUpdateAvailable(), Is.True, "The remote update must actually be detected.");
            synchronizer.Disable();
            Assert.That(update, Is.Not.Null);
            Assert.That(update.Handled, Is.False);
            Assert.That(reloads, Is.Zero);
            Assert.That(draft.GetRecursiveChildList().Single().Name, Is.EqualTo("Unsaved local draft"));
            draft = Loader().Load();
            Assert.That(synchronizer.IsUpdateAvailable(), Is.True);
            synchronizer.Disable();
            Assert.That(reloads, Is.EqualTo(1), "Explicitly loading a fresh model allows synchronization again.");
        }

        private SqlConnectionsLoader Loader(Func<Optional<SecureString>> authenticate = null) =>
            new(new LocalConnectionPropertiesXmlSerializer(), new InMemoryStringDataProvider(),
                () => Connector(), authenticate ?? (() => OldPassword.ConvertToSecureString()));

        private SqlConnectionsSaver Saver() => new(new SaveFilter(true) { SavePassword = true, SaveUsername = true,
            SaveDomain = true, SaveInheritance = true, SaveCredentialId = true },
            new LocalConnectionPropertiesXmlSerializer(), new InMemoryStringDataProvider(), () => Connector());

        private SqlConnectionsUpdateChecker Checker() => new(() => Connector(), () => DateTime.MinValue);

        private DateTime UpdateMarker()
        {
            using SqlConnectionsUpdateChecker checker = Checker();
            DateTime? marker = null;
            checker.ConnectionsUpdateAvailable += (_, args) => marker = args.UpdateTime;
            Assert.That(checker.IsUpdateAvailable(), Is.True);
            Assert.That(marker, Is.Not.Null);
            return marker.Value;
        }

        private SqlConnectionListMetaData Metadata()
        {
            using IDatabaseConnector connector = Connector();
            return new SqlDatabaseMetaDataRetriever().GetDatabaseMetaData(connector);
        }

        private static RootNodeInfo Root(ConnectionTreeModel model) => model.RootNodes.OfType<RootNodeInfo>().Single();

        private void Execute(string sql, string database = null)
        {
            using IDatabaseConnector connector = Connector(database);
            connector.Connect();
            using DbCommand command = connector.DbCommand(sql);
            command.ExecuteNonQuery();
        }
    }
}
