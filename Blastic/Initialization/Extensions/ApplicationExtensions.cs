using Autofac;
using Blastic.Execution;
using Blastic.UserInterface.Settings;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Blastic.Initialization.Extensions
{
	public static class ApplicationExtensions
	{
		internal static BlasticApplication AddDefaults(this BlasticApplication application)
		{
			return application
				.AddLogging()
				.AddSettings()
				.AddDefaultServices();
		}

		private static BlasticApplication AddLogging(this BlasticApplication application, LogLevel minimumLogLevel = LogLevel.Trace)
		{
			return application.Configure(x =>
			{
				x.AddLogging(builder =>
				{
					builder.SetMinimumLevel(minimumLogLevel);
					builder.AddSerilog();
				});
			});
		}

		private static BlasticApplication AddSettings(this BlasticApplication application)
		{
			return application.Configure(builder =>
			{
				builder
					.RegisterType<SettingsViewModel>()
					.SingleInstance();
				
				builder
					.RegisterAssemblyTypes(typeof(ISettingsSectionViewModel).Assembly)
					.AsImplementedInterfaces();
			});
		}

		private static BlasticApplication AddDefaultServices(this BlasticApplication application)
		{
			return application.Configure(builder =>
			{
				builder
					.RegisterType<ExecutionContextFactory>()
					.SingleInstance();

				builder
					.RegisterType<WindowManager>()
					.As<IWindowManager>()
					.SingleInstance();

				builder
					.RegisterType<EventAggregator>()
					.As<IEventAggregator>()
					.SingleInstance();

				builder
					.RegisterType<SnackbarMessageQueue>()
					.As<ISnackbarMessageQueue>()
					.SingleInstance();
			});
		}
	}
}