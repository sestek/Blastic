﻿using Autofac;
using Blastic.Data.Initialization.Steps;
using Blastic.Data.ProgramData;
using Blastic.Data.Services;
using Blastic.Initialization;
using Blastic.Initialization.Steps;
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

				builder
					.RegisterType<ConnectionFactory>()
					.SingleInstance();

				builder
					.RegisterType<ProgramDatabase>()
					.SingleInstance();

				builder
					.RegisterType<MigrateProgramDatabaseStep>()
					.SingleInstance()
					.As<IInitializationStep>();
			});
		}

		public static BlasticApplication AddSettingsService(this BlasticApplication application)
		{
			return application.Configure(builder =>
			{
				builder.RegisterType<SettingsService>()
					.SingleInstance()
					.As<ISettingsService>();
			});
		}
	}
}