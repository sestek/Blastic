using System;
using System.Threading;
using System.Threading.Tasks;
using WpfTemplate.DataLayer;
using WpfTemplate.DataLayer.ProviderSpecific;

namespace WpfTemplate.Data.ProgramData.Migrations._10._4
{
	public class CreateSettingsTable : ProgramDataMigrationBase
	{
		public override Version Version { get; } = new Version(10, 4, 0);
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