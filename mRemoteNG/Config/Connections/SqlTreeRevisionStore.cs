using System;
using System.Data;
using System.Data.Common;
using mRemoteNG.Config.DatabaseConnectors;

namespace mRemoteNG.Config.Connections;

/// <summary>Revision gate for schema 3.4 shared trees. The table is provisioned during coordinated cutover.</summary>
internal sealed class SqlTreeRevisionStore
{
    internal const int WriterProtocol = 1;

    public long ReadRevision(IDatabaseConnector connector, DbTransaction transaction, bool lockForWrite = false)
    {
        string query = connector.GetType().Name.Contains("MySql", StringComparison.Ordinal)
            ? "SELECT Revision, MinimumWriterProtocol FROM tblTreeRevision WHERE Id=1" + (lockForWrite ? " FOR UPDATE" : "")
            : "SELECT Revision, MinimumWriterProtocol FROM tblTreeRevision WITH (HOLDLOCK" + (lockForWrite ? ", UPDLOCK" : "") + ") WHERE Id=1";
        using DbCommand command = connector.DbCommand(query);
        command.Transaction = transaction;
        using DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow);
        if (!reader.Read())
            throw new InvalidOperationException("Shared SQL tree revision is not provisioned; writes are disabled until schema 3.1 cutover.");
        long revision = Convert.ToInt64(reader["Revision"], System.Globalization.CultureInfo.InvariantCulture);
        int minimumProtocol = Convert.ToInt32(reader["MinimumWriterProtocol"], System.Globalization.CultureInfo.InvariantCulture);
        if (revision < 0 || minimumProtocol > WriterProtocol)
            throw new InvalidOperationException("Shared SQL tree requires a newer writer protocol.");
        return revision;
    }

    public void AdvanceRevision(IDatabaseConnector connector, DbTransaction transaction, long expectedRevision)
    {
        long nextRevision = checked(expectedRevision + 1);
        using DbCommand command = connector.DbCommand("UPDATE tblTreeRevision SET Revision=@next WHERE Id=1 AND Revision=@expected AND MinimumWriterProtocol<=@protocol");
        command.Transaction = transaction;
        AddParameter(command, "@next", nextRevision);
        AddParameter(command, "@expected", expectedRevision);
        AddParameter(command, "@protocol", WriterProtocol);
        if (command.ExecuteNonQuery() != 1)
            throw new InvalidOperationException("Shared SQL tree changed since it was loaded. The draft was retained.");
    }

    private static void AddParameter(DbCommand command, string name, object value)
    {
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }
}
