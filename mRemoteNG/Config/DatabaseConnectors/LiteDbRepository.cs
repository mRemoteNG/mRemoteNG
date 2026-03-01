using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiteDB;

namespace mRemoteNG.Config.DatabaseConnectors
{
    /// <summary>
    /// LiteDB-based generic document repository. Provides an embedded, server-less
    /// NoSQL storage option for new beta integrations. No external database server
    /// or installation is required — the database is stored as a single local file.
    /// </summary>
    /// <typeparam name="T">The document entity type. The type should have an <c>Id</c> property
    /// (of any type) which LiteDB automatically maps to the internal <c>_id</c> field.</typeparam>
    public class LiteDbRepository<T> : ILiteDbRepository<T>, IDisposable
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<T> _collection;
        private bool _disposed;

        /// <summary>
        /// Initialises a new repository backed by a LiteDB database.
        /// </summary>
        /// <param name="connectionString">
        /// LiteDB connection string, e.g. <c>"Filename=mydata.db"</c> or <c>":memory:"</c>
        /// for an in-process, in-memory database.
        /// </param>
        /// <param name="collectionName">Name of the LiteDB collection to use.</param>
        public LiteDbRepository(string connectionString, string collectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentNullException(nameof(collectionName));

            _database = new LiteDatabase(connectionString);
            _collection = _database.GetCollection<T>(collectionName);
        }

        /// <inheritdoc/>
        public void Insert(T item)
        {
            _collection.Insert(item);
        }

        /// <inheritdoc/>
        public IEnumerable<T> FindAll()
        {
            return _collection.FindAll();
        }

        /// <inheritdoc/>
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _collection.Find(predicate);
        }

        /// <inheritdoc/>
        public bool Update(T item)
        {
            return _collection.Update(item);
        }

        /// <inheritdoc/>
        public int DeleteMany(Expression<Func<T, bool>> predicate)
        {
            return _collection.DeleteMany(predicate);
        }

        /// <inheritdoc/>
        public int Count()
        {
            return _collection.Count();
        }

        /// <summary>Disposes the underlying LiteDB database connection.</summary>
        public void Dispose()
        {
            if (_disposed) return;
            _database.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
