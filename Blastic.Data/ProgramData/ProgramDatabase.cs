using Microsoft.Extensions.Logging;
using Blastic.Data.ProgramData.Migrations;
using Blastic.Data.ProgramData.Tables;

namespace Blastic.Data.ProgramData
{
	public class ProgramDatabase : DatabaseBase<ProgramDatabaseMigrationBase>
	{
		public SettingsTable SettingsTable { get; }

		public ProgramDatabase(
			ConnectionFactory connectionFactory,
			ILogger<ProgramDatabase> logger)
			:
			base(connectionFactory, logger)
		{
			SettingsTable = new SettingsTable(connectionFactory);
		}
	}
}