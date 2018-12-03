using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using WpfTemplate.Caliburn;
using WpfTemplate.UserInterface.Main;

namespace WpfTemplate.Initialization
{
	public class Bootstrapper : BootstrapperBase
	{
		private readonly ILoggerFactory _loggerFactory;
		private CompositionContainer _container;

		public Bootstrapper(ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory;
		}
		
		protected override void Configure()
		{
			CaliburnMicroInitializer.Initialize();

			AggregateCatalog aggregateCatalog = new AggregateCatalog();
			DirectoryCatalog dllCatalog = new DirectoryCatalog("./", "*.dll");
			DirectoryCatalog exeCatalog = new DirectoryCatalog("./", "*.exe");

			aggregateCatalog.Catalogs.Add(dllCatalog);
			aggregateCatalog.Catalogs.Add(exeCatalog);

			AssemblySource.Instance.AddRange(
				aggregateCatalog.Parts
					.Select(part => ReflectionModelServices.GetPartType(part).Value.Assembly)
					.Where(assembly => !AssemblySource.Instance.Contains(assembly)));

			_container = new CompositionContainer(aggregateCatalog);

			CompositionBatch batch = new CompositionBatch();

			batch.AddExportedValue(_loggerFactory);
			batch.AddExportedValue<IWindowManager>(new WindowManager());
			batch.AddExportedValue<IEventAggregator>(new EventAggregator());
			batch.AddExportedValue<ISnackbarMessageQueue>(new SnackbarMessageQueue());
			batch.AddExportedValue(aggregateCatalog);

			_container.Compose(batch);
		}

		protected override object GetInstance(Type serviceType, string key)
		{
			string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
			List<object> exports = _container.GetExportedValues<object>(contract).ToList();

			if (exports.Any())
			{
				return exports.First();
			}

			throw new Exception($"Could not locate any instances of contract {contract}.");
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
		}

		protected override void BuildUp(object instance)
		{
			_container.SatisfyImportsOnce(instance);
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<MainViewModel>();
		}
	}
}