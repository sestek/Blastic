using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfTemplate.DataLayer.Migrations._10._4
{
	public class CreateDatabaseInformationTable : MigrationBase
	{
		public override Version Version { get; } = new Version(10, 4, 0);
		public override int Order { get; } = 0;

		protected override async Task MigrateUpAsync(Connection connection, CancellationToken cancellationToken)
		{
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = "CREATE TABLE DatabaseInformation(Version NVARCHAR(255) PRIMARY KEY)";
				await command.ExecuteNonQueryAsync(cancellationToken);

				command.CommandText = "INSERT INTO DatabaseInformation (Version) VALUES (@Version)";
				command.AddParameterWithValue("@Version", Version.ToString());

				await command.ExecuteNonQueryAsync(cancellationToken);
			}
		}

		protected override async Task MigrateDownAsync(Connection connection, CancellationToken cancellationToken)
		{
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = "DROP TABLE DatabaseInformation";
				await command.ExecuteNonQueryAsync(cancellationToken);
			}
		}
	}
}