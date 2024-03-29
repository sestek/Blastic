using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Blastic.Data.Migrations;
using Blastic.Data.Tables;
using Microsoft.Extensions.Logging;

namespace Blastic.Data
{
	public abstract class DatabaseBase<T> where T : MigrationBase
	{
		protected ConnectionFactory ConnectionFactory { get; }
		protected ILogger<DatabaseBase<T>> Logger { get; }

		protected DatabaseInformationTable DatabaseInformationTable { get; }

		protected DatabaseBase(ConnectionFactory connectionFactory, ILogger<DatabaseBase<T>> logger)
		{
			ConnectionFactory = connectionFactory;
			Logger = logger;

			DatabaseInformationTable = new DatabaseInformationTable(connectionFactory);
		}

		public TransactionScope CreateTransactionScope()
		{
			return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
		}

		public async Task<bool> IsMigrationAvailable(CancellationToken cancellationToken)
		{
			using (Connection connection = ConnectionFactory.CreateConnection())
			{
				Version currentVersion = await DatabaseInformationTable.GetVersion(connection, cancellationToken);
				Version newVersion = GetMigrations()
					.Select(Activator.CreateInstance)
					.Cast<MigrationBase>()
					.Max(x => x.Version);

				return currentVersion != newVersion;
			}
		}

		public async Task MigrateAsync(CancellationToken cancellationToken, Version targetVersion = null)
		{
			using (TransactionScope transactionScope = CreateTransactionScope())
			using (Connection connection = ConnectionFactory.CreateConnection())
			using (Logger.BeginScope("Applying migrations."))
			{
				Version currentVersion = await DatabaseInformationTable.GetVersion(connection, cancellationToken);
				Version newVersion = await MigrateAsync(connection, currentVersion, targetVersion, cancellationToken);

				if (currentVersion == newVersion)
				{
					transactionScope.Complete();
					return;
				}

				await DatabaseInformationTable.SetVersion(connection, newVersion, cancellationToken);
				transactionScope.Complete();

				Logger.LogInformation("Finished migrations. New version: {0}", newVersion);
			}
		}

		private async Task<Version> MigrateAsync(
			Connection connection,
			Version currentVersion,
			Version targetVersion,
			CancellationToken cancellationToken)
		{
			IEnumerable<Type> allMigrations = GetMigrations();

			IGrouping<Version, MigrationBase>[] migrations = allMigrations
				.Select(Activator.CreateInstance)
				.Cast<MigrationBase>()
				.GroupBy(x => x.Version)
				.OrderBy(x => x.Key)
				.ToArray();

			currentVersion = currentVersion ?? new Version(0, 0, 0);
			targetVersion = targetVersion ?? migrations.Max(x => x.Key);

			if (currentVersion == targetVersion)
			{
				return targetVersion;
			}

			Logger.LogInformation("Current version: {0}. Target version: {1}", currentVersion, targetVersion);

			foreach (IGrouping<Version, MigrationBase> migrationGroup in migrations)
			{
				foreach (MigrationBase migration in migrationGroup.OrderBy(x => x.Order))
				{
					await migration.MigrateAsync(connection, currentVersion, targetVersion, cancellationToken);
				}
			}

			return migrations
				.Where(x => x.Key <= targetVersion)
				.Max(x => x.Key);
		}

		private IEnumerable<Type> GetMigrations()
		{
			Assembly assembly = Assembly.GetAssembly(GetType());

			IEnumerable<Type> genericMigrationTypes = assembly.GetTypes()
				.Where(x => !x.IsAbstract)
				.Where(x => x.BaseType == typeof(MigrationBase));

			IEnumerable<Type> migrationTypes = assembly.GetTypes()
				.Where(x => !x.IsAbstract)
				.Where(x => x.IsSubclassOf(typeof(T)));

			return genericMigrationTypes.Concat(migrationTypes);
		}
	}
}