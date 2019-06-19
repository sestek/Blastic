using Blastic.Execution;
using Blastic.Services.Settings;
using Blastic.UserInterface.Settings;
using PropertyChanged;

namespace Blastic.UserInterface.Logs.Settings
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