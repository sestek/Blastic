using System;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Yaver.Host.Wpf.Initialization
{
	public class EntryPoint
	{
		private static App _application;
		private static Bootstrapper _bootstrapper;

		[STAThread]
		public static void Main()
		{
			StartApplication();
		}

		private static void StartApplication()
		{
			SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

			LogManager.LoadConfiguration("NLog.config");

			NLogLoggerFactory loggerFactory = new NLogLoggerFactory();
			ILogger logger = loggerFactory.CreateLogger(typeof(EntryPoint));

			try
			{
				_application = new App();
				_bootstrapper = new Bootstrapper(loggerFactory);
				_bootstrapper.Initialize();

				_application.DispatcherUnhandledException += (sender, args) =>
				{
					logger.LogError(args.Exception, "Unhandled exception.");
					args.Handled = true;
				};

				_application.Run();
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Unhandled exception.");
				throw;
			}
			finally
			{
				LogManager.Shutdown();
			}
		}
	}
}