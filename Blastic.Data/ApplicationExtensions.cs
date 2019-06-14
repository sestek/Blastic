using Autofac;
using Blastic.Data.ProgramData;
using Blastic.Data.Services;
using Blastic.Initialization;
using Blastic.Services.Settings;

namespace Blastic.Data
{
	public static class ApplicationExtensions
	{
		public static BlasticApplication AddProgramDatabase(
			this BlasticApplication application,
			DatabaseProvider databaseProvider,
			string connectionString)
		{
			return application.Configure(builder =>
			{
				DatabaseConfiguration databaseConfiguration = new DatabaseConfiguration(databaseProvider, connectionString);

				builder.RegisterInstance(databaseConfiguration);
				builder.RegisterType<ConnectionFactory>().SingleInstance();
				builder.RegisterType<ProgramDatabase>().SingleInstance();
			});
		}

		public static BlasticApplication AddSettings(this BlasticApplication application)
		{
			return application.Configure(builder =>
			{
				builder.RegisterType<SettingsService>()
					.As<ISettingsService>()
					.SingleInstance();
			});
		}
	}
}