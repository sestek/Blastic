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

namespace Blastic.UserInterface.TabbedMain
{
	[AddINotifyPropertyChangedInterface]
	public class TabbedMainViewModel : ConductorOneActiveBase<object>, IHandle<OpenTabEvent>
	{
		public LoggingViewModel LoggingViewModel { get; }
		public SettingsViewModel SettingsViewModel { get; }

		public int FixedHeaderCount { get; }

		public TabbedMainViewModel(
			ExecutionContextFactory executionContextFactory,
			IEnumerable<IMainTab> mainTabs,
			LoggingViewModel loggingViewModel = null,
			SettingsViewModel settingsViewModel = null)
			:
			base(executionContextFactory)
		{
			LoggingViewModel = loggingViewModel;
			SettingsViewModel = settingsViewModel;

			List<IMainTab> tabs = mainTabs
				.OrderBy(x => x.Order)
				.ToList();

			FixedHeaderCount = tabs.Count(x => x.IsFixed);

			Items.AddRange(tabs);
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
				await SettingsViewModel.ReadSettings(cancellationToken);
			}

			await ExecutionContext.Execute(
				Migrate,
				"Migrating database...",
				failMessage: "Cannot migrate database. Program might behave incorrectly.",
				customCancellationToken: cancellationToken);

			if (SettingsViewModel != null)
			{
				await ExecutionContext.Execute(
				ReadSettings,
				"Reading settings...",
				failMessage: "Cannot read settings. Program might behave incorrectly.",
				customCancellationToken: cancellationToken);
			}
		}

		public async Task ShowLogs()
		{
			if (LoggingViewModel != null)
			{
				await LoggingViewModel.Show();
			}
		}

		public async Task ShowSettings()
		{
			if (SettingsViewModel != null)
			{
				await SettingsViewModel.Show();
			}
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