using System;
using System.Collections.Generic;
using System.Windows;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using WpfTemplate.Caliburn;
using WpfTemplate.DataLayer;
using WpfTemplate.UserInterface.Main;

namespace WpfTemplate.Initialization
{
	public class Bootstrapper : BootstrapperBase
	{
		private readonly IConfiguration _configuration;
		private IContainer _container;

		public Bootstrapper(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void Configure()
		{
			CaliburnMicroInitializer.Initialize();

			ServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection
				.AddLogging(builder =>
				{
					builder.SetMinimumLevel(LogLevel.Trace);
					builder.AddSerilog();
				});

			DatabaseProvider databaseProvider = (DatabaseProvider) Enum.Parse(
				typeof(DatabaseProvider),
				_configuration["Application:Database:DatabaseProvider"]);
			
			string connectionString = _configuration["Application:Database:ConnectionString"];

			DatabaseConfiguration databaseConfiguration = new DatabaseConfiguration(databaseProvider, connectionString);

			ContainerBuilder containerBuilder = new ContainerBuilder();

			containerBuilder.RegisterInstance(_configuration);
			containerBuilder.RegisterInstance(databaseConfiguration);
			containerBuilder.RegisterType<WindowManager>().As<IWindowManager>();
			containerBuilder.RegisterType<EventAggregator>().As<IEventAggregator>();
			containerBuilder.RegisterType<SnackbarMessageQueue>().As<ISnackbarMessageQueue>();
			containerBuilder.Populate(serviceCollection);
			containerBuilder.RegisterByAttributes(typeof(Bootstrapper).Assembly);

			_container = containerBuilder.Build();
		}

		protected override object GetInstance(Type serviceType, string key)
		{
			if (string.IsNullOrWhiteSpace(key) && _container.IsRegistered(serviceType))
			{
				return _container.Resolve(serviceType);
			}

			if (!string.IsNullOrWhiteSpace(key) && _container.IsRegisteredWithKey(key, serviceType))
			{
				return _container.ResolveKeyed(key, serviceType);
			}

			throw new Exception($"Could not locate any instances of contract {key ?? serviceType.Name}.");
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return _container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
		}

		protected override void BuildUp(object instance)
		{
			_container.InjectProperties(instance);
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<MainViewModel>();
		}
	}
}