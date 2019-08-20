using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blastic.Initialization.Extensions;
using Blastic.UserInterface.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Log = Serilog.Log;

namespace Blastic.Initialization
{
	public class BlasticApplication
	{
		private readonly ConfigurationBuilder _configurationBuilder;
		private readonly ContainerBuilder _containerBuilder;
		private readonly ServiceCollection _serviceCollection;

		private readonly HashSet<Assembly> _viewAssemblies;

		private readonly App _application;

		public BlasticApplication()
		{
			_configurationBuilder = new ConfigurationBuilder();
			_containerBuilder = new ContainerBuilder();
			_serviceCollection = new ServiceCollection();

			_viewAssemblies = new HashSet<Assembly>();

			_application = new App();

			this.AddDefaults();
		}

		public BlasticApplication OnUnhandledException(Func<DispatcherUnhandledExceptionEventArgs, Task> func)
		{
			_application.DispatcherUnhandledException += async (sender, args) =>
			{
				await func(args);
			};

			return this;
		}

		public BlasticApplication Configure(Action<ConfigurationBuilder> action)
		{
			action(_configurationBuilder);
			return this;
		}

		public BlasticApplication Configure(Action<ContainerBuilder> action)
		{
			action(_containerBuilder);
			return this;
		}

		public BlasticApplication Configure(Action<IServiceCollection> action)
		{
			action(_serviceCollection);
			return this;
		}

		public BlasticApplication RegisterViewAssembly<T>()
		{
			_viewAssemblies.Add(typeof(T).Assembly);
			return this;
		}

		public void Run<TMainViewModel>()
		{
			IConfiguration configuration = _configurationBuilder.Build();

			LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration);

			RegisterViewAssembly<BlasticApplication>();

			Configure(x => x.RegisterInstance(configuration));
			Configure(x => x.RegisterType<TMainViewModel>());

			_containerBuilder.Populate(_serviceCollection);
			IContainer container = _containerBuilder.Build();

			LogSink logSink = container.ResolveOptional<LogSink>();

			if (logSink != null)
			{
				loggerConfiguration.WriteTo.Sink(logSink);
			}

			Log.Logger = loggerConfiguration.CreateLogger();

			Bootstrapper bootstrapper = new Bootstrapper(
				container,
				typeof(TMainViewModel),
				_viewAssemblies);

			SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
			
			try
			{
				bootstrapper.Initialize();
				_application.Run();
			}
			catch (Exception exception)
			{
				Log.Error(exception, exception.Message);
				throw;
			}
		}
	}
}