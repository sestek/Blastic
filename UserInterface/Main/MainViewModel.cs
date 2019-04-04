using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Caliburn.Micro;
using PropertyChanged;
using WpfTemplate.Caliburn;
using WpfTemplate.Data.ProgramData;
using WpfTemplate.Execution;
using WpfTemplate.UserInterface.Events;
using WpfTemplate.UserInterface.Logging;
using WpfTemplate.UserInterface.Settings;

namespace WpfTemplate.UserInterface.Main
{
	[SingleInstance]
	[AddINotifyPropertyChangedInterface]
	public class MainViewModel : ConductorOneActiveBase<object>, IHandle<OpenTabEvent>
	{
		private readonly LoggingViewModel _loggingViewModel;
		private readonly SettingsViewModel _settingsViewModel;
		private readonly ProgramDatabase _database;

		public MainViewModel(
			ExecutionContextFactory executionContextFactory,
			LoggingViewModel loggingViewModel,
			SettingsViewModel settingsViewModel,
			ProgramDatabase database,
			IEnumerable<IMainTab> mainTabs)
			:
			base(executionContextFactory)
		{
			_loggingViewModel = loggingViewModel;
			_settingsViewModel = settingsViewModel;
			_database = database;

			Items.AddRange(mainTabs.OrderBy(x => x.Order));
			ActiveItem = Items.FirstOrDefault();

			ExecutionContext.EventAggregator.Subscribe(this);
		}

		protected override async void OnInitialize()
		{
			async Task Migrate(CancellationToken cancellationToken)
			{
				await _database.MigrateAsync(cancellationToken);
			}

			async Task ReadSettings(CancellationToken cancellationToken)
			{
				await _settingsViewModel.ReadSettings();
			}

			await ExecutionContext.Execute(
				Migrate,
				"Migrating database...",
				failMessage: "Cannot migrate database. Program might behave incorrectly.");

			await ExecutionContext.Execute(
				ReadSettings,
				"Reading settings...",
				failMessage: "Cannot read settings. Program might behave incorrectly.");
		}

		public void ShowLogs()
		{
			_loggingViewModel.Show();
		}

		public void ShowSettings()
		{
			_settingsViewModel.Show();
		}

		public void Handle(OpenTabEvent message)
		{
			object tab = message.ViewModel;

			if (!Items.Contains(tab))
			{
				Items.Add(tab);
			}
			
			ActivateItem(tab);
		}
	}
}