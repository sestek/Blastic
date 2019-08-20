using Blastic.Services.Dialog;
using Blastic.Services.Dialog.FileFilters;
using Blastic.Services.Settings;
using Forge.Forms;
using Forge.Forms.Annotations;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Blastic.UserInterface.Settings
{
	[AddINotifyPropertyChangedInterface]
	[Form(Mode = DefaultFields.None)]
	public class FileBrowserSetting : Setting<string>, IActionHandler
	{
		// TODO: This class should be deleted when there is a way available to add attributes easily.

		private readonly IDialogService _dialogService;
		private readonly IFileDialogFilter _filter;

		public bool IsFolderPicker { get; set; }
		public bool IsSaveFilePicker { get; set; }

		[Field(
			Name = "{Binding " + nameof(Label) + "}",
			Icon = "{Binding " + nameof(Icon) + "}")]
		[Action("select", "", Placement = Placement.Inline, Icon = PackIconKind.FolderOpen)]
		public new string SettingValue { get; set; }

		public FileBrowserSetting(
			ISettingsService settingsService,
			IDialogService dialogService,
			IFileDialogFilter filter,
			string key,
			string defaultValue)
			:
			base(settingsService, key, defaultValue)
		{
			_dialogService = dialogService;
			_filter = filter;
		}

		public void HandleAction(IActionContext actionContext)
		{
			void SetIfNotEmpty(string path)
			{
				if (!string.IsNullOrEmpty(path))
				{
					SettingValue = path;
				}
			}

			FileDialogOptions options = new FileDialogOptions(_filter, initialDirectory: SettingValue);

			if (IsFolderPicker)
			{
				string folderPath = _dialogService.ShowSelectFolderDialog(options);
				SetIfNotEmpty(folderPath);

				return;
			}

			string filePath = IsSaveFilePicker
				? _dialogService.ShowSaveFileDialog(options)
				: _dialogService.ShowOpenFileDialog(options);

			SetIfNotEmpty(filePath);
		}
	}
}