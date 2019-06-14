using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using Blastic.Caliburn;

namespace Blastic.Initialization
{
	public class Bootstrapper : BootstrapperBase
	{
		private readonly IContainer _container;
		private readonly Type _mainViewModelType;
		private readonly IEnumerable<Assembly> _viewAssemblies;

		public Bootstrapper(
			IContainer container,
			Type mainViewModelType,
			IEnumerable<Assembly> viewAssemblies)
		{
			_container = container;
			_mainViewModelType = mainViewModelType;
			_viewAssemblies = viewAssemblies;
		}

		protected override void Configure()
		{
			CaliburnMicroInitializer.Initialize();
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

		protected override IEnumerable<Assembly> SelectAssemblies()
		{
			return _viewAssemblies;
		}

		protected override async void OnStartup(object sender, StartupEventArgs e)
		{
			await DisplayRootViewForAsync(_mainViewModelType);
		}
	}
}