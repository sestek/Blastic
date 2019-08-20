using Blastic.Execution;
using Blastic.Services.Dialog;
using Blastic.Services.Dialog.FileFilters;
using Blastic.Services.Settings;
using Blastic.UserInterface.Settings;
using PropertyChanged;

namespace Blastic.UserInterface.Logs.Settings
{
	[AddINotifyPropertyChangedInterface]
	public class LogSettingsViewModel : SettingsSectionViewModel
	{
		public override string SectionName => "Logs";
		
		public OpenWindowOnErrorSetting OpenWindowOnErrorSetting { get; }
		
		public LogSettingsViewModel(
			ExecutionContextFactory executionContextFactory,
			ISettingsService settingsService,
			IDialogService dialogService)
			:
			base(executionContextFactory, settingsService)
		{
			OpenWindowOnErrorSetting = new OpenWindowOnErrorSetting(settingsService);

			RegisterForUI(OpenWindowOnErrorSetting);
		}
	}
}