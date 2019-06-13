using Blastic.Services.Settings;

namespace Blastic.UserInterface.Settings.Logging
{
	public class OpenWindowOnErrorSetting : Setting<bool>
	{
		public OpenWindowOnErrorSetting(ISettingsService settingsService)
			:
			base(settingsService, "Log.OpenLogsWindowOnError", false)
		{
			Label = "Open logs window on error";
			Help  = "Open the logs window whenever an error log is printed.";
		}
	}
}