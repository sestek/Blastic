using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using PropertyChanged;
using Blastic.Caliburn;
using Blastic.Execution;
using Blastic.UserInterface.Events;
using Blastic.UserInterface.Logging;
using Blastic.UserInterface.Settings;

namespace Blastic.UserInterface.Main
{
	[AddINotifyPropertyChangedInterface]
	public class MainViewModel : ConductorOneActiveBase<object>, IHandle<OpenTabEvent>
	{
		private readonly LoggingViewModel _loggingViewModel;
		private readonly SettingsViewModel _settingsViewModel;

		public MainViewModel(
			ExecutionContextFactory executionContextFactory,
			LoggingViewModel loggingViewModel,
			SettingsViewModel settingsViewModel,
			IEnumerable<IMainTab> mainTabs)
			:
			base(executionContextFactory)
		{
			_loggingViewModel = loggingViewModel;
			_settingsViewModel = settingsViewModel;

			Items.AddRange(mainTabs.OrderBy(x => x.Order));
			ActiveItem = Items.FirstOrDefault();

			ExecutionContext.EventAggregator.SubscribeOnPublishedThread(this);
		}

		protected override async Task OnActivateAsync(CancellationToken cancellationToken)
		{
			async Task Migrate(CancellationToken token)
			{
				//await _database.MigrateAsync(token);
			}

			async Task ReadSettings(CancellationToken token)
			{
				await _settingsViewModel.ReadSettings(cancellationToken);
			}

			await ExecutionContext.Execute(
				Migrate,
				"Migrating database...",
				failMessage: "Cannot migrate database. Program might behave incorrectly.",
				customCancellationToken: cancellationToken);

			await ExecutionContext.Execute(
				ReadSettings,
				"Reading settings...",
				failMessage: "Cannot read settings. Program might behave incorrectly.",
				customCancellationToken: cancellationToken);
		}

		public async Task ShowLogs()
		{
			await _loggingViewModel.Show();
		}

		public async Task ShowSettings()
		{
			await _settingsViewModel.Show();
		}
		
		public async Task HandleAsync(OpenTabEvent message, CancellationToken cancellationToken)
		{
			object tab = message.ViewModel;

			if (!Items.Contains(tab))
			{
				Items.Add(tab);
			}

			await ActivateItemAsync(tab, cancellationToken);
		}
	}
}