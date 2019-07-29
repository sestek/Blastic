using System.Threading;
using System.Threading.Tasks;
using Blastic.Common;
using Blastic.UserInterface.Settings;

namespace Blastic.Initialization.Steps
{
	public class ReadSettingsStep : IInitializationStep
	{
		public static readonly Order Order = new Order(0);

		private readonly SettingsViewModel _settingsViewModel;

		Order IInitializationStep.Order => Order;

		public string Description { get; }
		public string SuccessMessage { get; }
		public string FailureMessage { get; }

		public bool IsCancellationSupported => false;
		public bool ShowBusyIndicator => true;

		public ReadSettingsStep(SettingsViewModel settingsViewModel)
		{
			_settingsViewModel = settingsViewModel;

			Description = "Reading settings...";
			SuccessMessage = "";
			FailureMessage = "Cannot read settings. Program might behave incorrectly.";
		}

		public async Task Initialize(CancellationToken cancellationToken)
		{
			await _settingsViewModel.ReadSettings(cancellationToken);
		}
	}
}