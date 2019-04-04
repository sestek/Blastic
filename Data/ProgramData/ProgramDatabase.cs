using Autofac;
using Microsoft.Extensions.Logging;
using WpfTemplate.Data.ProgramData.Migrations;
using WpfTemplate.Data.ProgramData.Tables;
using WpfTemplate.DataLayer;

namespace WpfTemplate.Data.ProgramData
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