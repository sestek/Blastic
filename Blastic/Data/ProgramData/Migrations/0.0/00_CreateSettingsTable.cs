using System;
using System.Threading;
using System.Threading.Tasks;
using Blastic.DataLayer;
using Blastic.DataLayer.ProviderSpecific;

namespace Blastic.Data.ProgramData.Migrations._0._0
{
	public class CreateSettingsTable : ProgramDataMigrationBase
	{
		public override Version Version { get; } = new Version(0, 0, 0);
		public override int Order { get; } = 1;

		protected override async Task MigrateUpAsync(Connection connection, CancellationToken cancellationToken)
		{
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = $@"CREATE TABLE Settings (
										 	Key		{Placeholders.NVarCharMaxColumnPlaceholder} PRIMARY KEY,
										 	Value	{Placeholders.NVarCharMaxColumnPlaceholder}
										);";

				await command.ExecuteNonQueryAsync(cancellationToken);
			}
		}

		protected override async Task MigrateDownAsync(Connection connection, CancellationToken cancellationToken)
		{
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = "DROP TABLE Settings";
				await command.ExecuteNonQueryAsync(cancellationToken);
			}
		}
	}
}