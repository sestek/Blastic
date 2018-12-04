using System.Collections.Generic;
using System.Linq;
using Autofac;
using PropertyChanged;
using WpfTemplate.Caliburn;
using WpfTemplate.Execution;
using WpfTemplate.UserInterface.Logging;
using WpfTemplate.UserInterface.Settings;

namespace WpfTemplate.UserInterface.Main
{
	[SingleInstance]
	[AddINotifyPropertyChangedInterface]
	public class MainViewModel : ConductorOneActiveBase<IMainTab>
	{
		private readonly LoggingViewModel _loggingViewModel;
		private readonly SettingsViewModel _settingsViewModel;
		
		public MainViewModel(
			ExecutionContext executionContext,
			LoggingViewModel loggingViewModel,
			SettingsViewModel settingsViewModel,
			IEnumerable<IMainTab> mainTabs)
			:
			base(executionContext)
		{
			_loggingViewModel = loggingViewModel;
			_settingsViewModel = settingsViewModel;

			Items.AddRange(mainTabs.OrderBy(x => x.Order));
			ActiveItem = Items.FirstOrDefault();
		}

		public void ShowLogs()
		{
			_loggingViewModel.Show();
		}

		public void ShowSettings()
		{
			_settingsViewModel.Show();
		}
	}
}