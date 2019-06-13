using System;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace WpfTemplate.Initialization
{
	public class EntryPoint
	{
		private static App _application;
		private static Bootstrapper _bootstrapper;

		public static void StartApplication()
		{
			IConfiguration configuration = new ConfigurationBuilder()
				.AddJsonFile("AppSettings.json")
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Enrich.FromLogContext()
				.WriteTo.File("Logs/Log.txt", rollingInterval: RollingInterval.Day)
				.CreateLogger();

			SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
			
			try
			{
				_application = new App();
				_bootstrapper = new Bootstrapper(configuration);
				_bootstrapper.Initialize();

				_application.DispatcherUnhandledException += (sender, args) =>
				{
					Log.Error(args.Exception, "Unhandled exception.");
					args.Handled = true;
				};

				_application.Run();
			}
			catch (Exception exception)
			{
				Log.Error(exception, "Unhandled exception.");
				throw;
			}
		}
	}
}