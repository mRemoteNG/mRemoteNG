using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace mRemoteNG.Config.DatabaseConnectors
{
    /// <summary>
    /// Generic repository interface for LiteDB embedded document storage.
    /// Provides a server-less, file-based NoSQL alternative to SQL connectors
    /// for new beta integrations that do not require an external database server.
    /// </summary>
    /// <typeparam name="T">The document entity type.</typeparam>
    public interface ILiteDbRepository<T>
    {
        /// <summary>Inserts a new document into the collection.</summary>
        void Insert(T item);

        /// <summary>Returns all documents in the collection.</summary>
        IEnumerable<T> FindAll();

        /// <summary>Returns documents matching the given predicate.</summary>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>Updates an existing document. Returns true if the document was found and updated.</summary>
        bool Update(T item);

        /// <summary>Deletes all documents matching the given predicate. Returns the number of deleted documents.</summary>
        int DeleteMany(Expression<Func<T, bool>> predicate);

        /// <summary>Returns the total number of documents in the collection.</summary>
        int Count();
    }
}
