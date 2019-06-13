using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Transactions;
using Autofac;
using Microsoft.Extensions.Logging;

namespace Blastic.DataLayer
{
	[SingleInstance]
	public class ConnectionFactory
	{
		private readonly DatabaseConfiguration _databaseConfiguration;
		private readonly ILogger _logger;

		private static readonly ConditionalWeakTable<Transaction, Tuple<DbConnection, DbTransaction>> AmbientConnectionsTable;

		static ConnectionFactory()
		{
			AmbientConnectionsTable = new ConditionalWeakTable<Transaction, Tuple<DbConnection, DbTransaction>>();
		}

		public ConnectionFactory(DatabaseConfiguration databaseConfiguration, ILoggerFactory loggerFactory)
		{
			_databaseConfiguration = databaseConfiguration;
			_logger = loggerFactory.CreateLogger<Connection>();
		}

		internal (DbConnection connection, DbTransaction transaction) CreateDbConnection()
		{
			if (Transaction.Current != null && AmbientConnectionsTable.TryGetValue(Transaction.Current, out Tuple<DbConnection, DbTransaction> tuple))
			{
				return (tuple.Item1, tuple.Item2);
			}

			DbConnection dbConnection;

			switch (_databaseConfiguration.DatabaseProvider)
			{
				case DatabaseProvider.SQLite:
					dbConnection = new SQLiteConnection(_databaseConfiguration.ConnectionString);
					break;

				case DatabaseProvider.SQLServer:
					dbConnection = new SqlConnection(_databaseConfiguration.ConnectionString);
					break;

				default:
					throw new ArgumentOutOfRangeException(
						nameof(_databaseConfiguration.DatabaseProvider),
						"Database provider is not implemented: " + _databaseConfiguration.DatabaseProvider);
			}

			dbConnection.Open();
			DbTransaction dbTransaction = null;

			if (Transaction.Current == null || _databaseConfiguration.DatabaseProvider != DatabaseProvider.SQLServer)
			{
				dbTransaction = dbConnection.BeginTransaction();
			}

			if (Transaction.Current != null)
			{
				AmbientConnectionsTable.Add(Transaction.Current, new Tuple<DbConnection, DbTransaction>(dbConnection, dbTransaction));
			}

			return (dbConnection, dbTransaction);
		}

		internal bool ShouldRegisterToTransactionScope()
		{
			if (Transaction.Current == null)
			{
				return false;
			}

			return !AmbientConnectionsTable.TryGetValue(Transaction.Current, out _);
		}

		internal void UnregisterConnection(Transaction transaction)
		{
			if (transaction == null)
			{
				return;
			}

			AmbientConnectionsTable.Remove(transaction);
		}

		internal Connection CreateConnection()
		{
			return new Connection(this, _databaseConfiguration.DatabaseProvider, _logger);
		}
	}
}