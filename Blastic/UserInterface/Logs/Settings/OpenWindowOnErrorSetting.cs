using Blastic.Services.Settings;
using Blastic.UserInterface.Settings;

namespace Blastic.UserInterface.Logs.Settings
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