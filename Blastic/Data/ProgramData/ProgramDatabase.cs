using Autofac;
using Microsoft.Extensions.Logging;
using Blastic.Data.ProgramData.Migrations;
using Blastic.Data.ProgramData.Tables;
using Blastic.DataLayer;

namespace Blastic.Data.ProgramData
{
	[SingleInstance]
	public class ProgramDatabase : DatabaseBase<ProgramDataMigrationBase>
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