using Autofac;
using Blastic.Execution;
using Blastic.Initialization.Steps;
using Blastic.Services.Dialog;
using Blastic.UserInterface.Logs;
using Blastic.UserInterface.Settings;
using Blastic.UserInterface.Settings.Logging;
using Blastic.UserInterface.TabbedMain;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Blastic.Initialization.Extensions
{
	public static class ApplicationExtensions
	{
		public static BlasticApplication RegisterMainTab<T>(this BlasticApplication application) where T : IMainTab
		{
			return application.Configure(builder =>
			{
				builder.RegisterType<T>()
					.SingleInstance()
					.As<IMainTab>();
			});
		}

		public static BlasticApplication AddLogsWindow(this BlasticApplication application)
		{
			return application.Configure(builder =>
			{
				builder
					.RegisterType<LogsViewModel>()
					.SingleInstance();

				builder
					.RegisterType<LogSink>()
					.SingleInstance();

				builder
					.RegisterType<LogSettingsViewModel>()
					.SingleInstance()
					.AsImplementedInterfaces()
					.AsSelf();
			});
		}

		public static BlasticApplication AddSettingsWindow(this BlasticApplication application)
		{
			return application.Configure(builder =>
			{
				builder
					.RegisterType<SettingsViewModel>()
					.SingleInstance();

				builder.RegisterType<ReadSettingsStep>()
					.SingleInstance()
					.As<IInitializationStep>();
			});
		}

		internal static BlasticApplication AddDefaults(this BlasticApplication application)
		{
			return application
				.AddLogging()
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

		private static BlasticApplication AddDefaultServices(this BlasticApplication application)
		{
			return application.Configure(builder =>
			{
				builder
					.RegisterType<ExecutionContextFactory>()
					.SingleInstance();

				builder
					.RegisterType<DialogService>()
					.As<IDialogService>()
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