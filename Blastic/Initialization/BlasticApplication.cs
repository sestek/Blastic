using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blastic.Initialization.Extensions;
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

		public BlasticApplication RegisterViewAssembly(Assembly assembly)
		{
			_viewAssemblies.Add(assembly);
			return this;
		}

		public void Run<TMainViewModel>()
		{
			IConfiguration configuration = _configurationBuilder.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			RegisterViewAssembly(typeof(BlasticApplication).Assembly);

			Configure(x => x.RegisterInstance(configuration));
			Configure(x => x.RegisterType<TMainViewModel>());

			_containerBuilder.Populate(_serviceCollection);
			IContainer container = _containerBuilder.Build();

			Bootstrapper bootstrapper = new Bootstrapper(
				container,
				typeof(TMainViewModel),
				_viewAssemblies);

			SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
			
			try
			{
				bootstrapper.Initialize();

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