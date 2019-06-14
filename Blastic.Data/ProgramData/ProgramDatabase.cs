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
			ILoggerFactory loggerFactory)
			:
			base(connectionFactory, loggerFactory)
		{
			SettingsTable = new SettingsTable(connectionFactory);
		}
	}
}