using System.Windows;
using Bindables;
using WpfTemplate.Services.Dialog;
using WpfTemplate.Services.Dialog.FileFilters;

namespace WpfTemplate.Controls
{
	public partial class FilePicker
	{
		[DependencyProperty(Options = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public string Path { get; set; }
		
		[DependencyProperty]
		public IFileDialogFilter Filter { get; set; }

		[DependencyProperty]
		public IDialogService DialogService { get; set; }

		[DependencyProperty]
		public bool IsFolderPicker { get; set; }

		[DependencyProperty]
		public bool IsSaveFilePicker { get; set; }

		public FilePicker()
		{
			InitializeComponent();
		}

		public void SelectPath()
		{
			void SetIfNotEmpty(string path)
			{
				if (!string.IsNullOrEmpty(path))
				{
					Path = path;
				}
			}

			FileDialogOptions options = new FileDialogOptions(Filter, Window.GetWindow(this), Path);

			if (IsFolderPicker)
			{
				string folderPath = DialogService.ShowSelectFolderDialog(options);
				SetIfNotEmpty(folderPath);

				return;
			}

			string filePath = IsSaveFilePicker
				? DialogService.ShowSaveFileDialog(options)
				: DialogService.ShowOpenFileDialog(options);

			SetIfNotEmpty(filePath);
		}
	}
}