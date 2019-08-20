using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using PropertyChanged;
using Blastic.Caliburn;
using Blastic.Execution;
using Blastic.Initialization;
using Blastic.Initialization.Steps;
using Blastic.UserInterface.Events;
using Blastic.UserInterface.Logs;
using Blastic.UserInterface.Settings;

namespace Blastic.UserInterface.TabbedMain
{
	[AddINotifyPropertyChangedInterface]
	public sealed class TabbedMainViewModel
		:
		ConductorOneActiveBase<object>,
		IHandle<OpenTabEvent>,
		IHandle<OpenLogsEvent>
	{
		private readonly List<IInitializationStep> _initializationSteps;

		public ProductInformation ProductInformation { get; }
		public LogsViewModel LogsViewModel { get; }
		public SettingsViewModel SettingsViewModel { get; }

		public int FixedHeaderCount { get; }

		public TabbedMainViewModel(
			ExecutionContextFactory executionContextFactory,
			IEnumerable<IMainTab> mainTabs,
			IEnumerable<IInitializationStep> initializationSteps,
			ProductInformation productInformation = null,
			LogsViewModel logsViewModel = null,
			SettingsViewModel settingsViewModel = null)
			:
			base(executionContextFactory)
		{
			ProductInformation = productInformation;
			LogsViewModel = logsViewModel;
			SettingsViewModel = settingsViewModel;

			_initializationSteps = initializationSteps
				.OrderBy(x => x.Order)
				.ToList();

			List<IMainTab> tabs = mainTabs
				.OrderBy(x => x.Order)
				.ToList();

			FixedHeaderCount = tabs.Count(x => x.IsFixed);

			Items.AddRange(tabs);
			ActiveItem = Items.FirstOrDefault();

			ExecutionContext.EventAggregator.SubscribeOnPublishedThread(this);

			if (ProductInformation != null)
			{
				DisplayName = $"{ProductInformation.ProgramName} - {ProductInformation.Version}";
			}
		}

		protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
		{
			foreach (IInitializationStep initializationStep in _initializationSteps)
			{
				if (!await initializationStep.ShouldExecute(cancellationToken))
				{
					continue;
				}

				await ExecutionContext.Execute(
					initializationStep.Execute,
					initializationStep.Description,
					initializationStep.SuccessMessage,
					initializationStep.FailureMessage,
					initializationStep.ShowBusyIndicator,
					rethrowUnhandledException: true,
					initializationStep.IsCancellationSupported,
					cancellationToken);
			}
		}

		public async Task ShowLogs()
		{
			if (LogsViewModel != null)
			{
				await LogsViewModel.Show();
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

		public async Task HandleAsync(OpenLogsEvent message, CancellationToken cancellationToken)
		{
			await ShowLogs();
		}
	}
}