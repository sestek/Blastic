using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Blastic.Data.Migrations
{
	public abstract class MigrationBase
	{
		public abstract Version Version { get; }
		public abstract int Order { get; }

		public async Task MigrateAsync(
			Connection connection,
			Version currentVersion,
			Version targetVersion,
			CancellationToken cancellationToken)
		{
			Debug.Assert(connection != null);
			Debug.Assert(currentVersion != null);
			Debug.Assert(targetVersion != null);

			if (currentVersion == targetVersion)
			{
				return;
			}

			if (currentVersion < targetVersion && Version <= targetVersion)
			{
				await MigrateUpAsync(connection, cancellationToken);
			}

			if (currentVersion > targetVersion && Version >= targetVersion)
			{
				await MigrateDownAsync(connection, cancellationToken);
			}
		}

		protected abstract Task MigrateUpAsync(Connection connection, CancellationToken cancellationToken);
		protected abstract Task MigrateDownAsync(Connection connection, CancellationToken cancellationToken);
	}
}