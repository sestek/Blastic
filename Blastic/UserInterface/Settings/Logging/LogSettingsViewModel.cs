using PropertyChanged;
using Blastic.Execution;
using Blastic.Services.Settings;

namespace Blastic.UserInterface.Settings.Logging
{
	[AddINotifyPropertyChangedInterface]
	public class LogSettingsViewModel : SettingsSectionViewModel
	{
		public override string SectionName => "Logs";
		
		public OpenWindowOnErrorSetting OpenWindowOnError { get; }
		
		public LogSettingsViewModel(
			ExecutionContextFactory executionContextFactory,
			ISettingsService settingsService)
			:
			base(executionContextFactory)
		{
			OpenWindowOnError = new OpenWindowOnErrorSetting(settingsService);
		}
	}
}