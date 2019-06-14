using Autofac;
using Blastic.Execution;
using Blastic.Services.Dialog;
using Blastic.UserInterface.Logging;
using Blastic.UserInterface.Settings;
using Blastic.UserInterface.Settings.Logging;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Blastic.Initialization.Extensions
{
	public static class ApplicationExtensions
	{
		public static BlasticApplication AddLoggingWindow(this BlasticApplication application)
		{
			return application.Configure(x =>
			{
				x.RegisterType<LogSettingsViewModel>();
				x.RegisterType<LoggingViewModel>();
			});
		}

		public static BlasticApplication AddSettingsWindow(this BlasticApplication application)
		{
			return application.Configure(builder =>
			{
				builder
					.RegisterType<SettingsViewModel>()
					.SingleInstance();
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