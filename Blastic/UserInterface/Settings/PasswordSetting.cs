using PropertyChanged;
using Blastic.Services.Settings;
using Forge.Forms.Annotations;

namespace Blastic.UserInterface.Settings
{
	[AddINotifyPropertyChangedInterface]
	[Form(Mode = DefaultFields.None)]
	public class PasswordSetting<T> : Setting<T>
	{
		// TODO: This class should be deleted when there is a way available to add attributes easily.

		[Field(
			Name = "{Binding " + nameof(Label) + "}",
			Icon = "{Binding " + nameof(Icon) + "}")]
		[Password]
		public new T SettingValue { get; set; }

		public PasswordSetting(
			ISettingsService settingsService,
			string key,
			T defaultValue)
			:
			base(settingsService, key, defaultValue)
		{
		}
	}
}