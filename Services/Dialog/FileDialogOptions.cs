using System.Windows;
using WpfTemplate.Services.Dialog.FileFilters;

namespace WpfTemplate.Services.Dialog
{
	public class FileDialogOptions
	{
		public IFileDialogFilter Filter { get; }
		public Window Owner { get; }

		public string InitialDirectory { get; }
		public bool IsMultiSelect { get; }

		public FileDialogOptions(
			IFileDialogFilter filter = default,
			Window owner = default,
			string initialDirectory = default,
			bool isMultiSelect = default)
		{
			Filter = filter;
			Owner = owner;
			InitialDirectory = initialDirectory;
			IsMultiSelect = isMultiSelect;
		}
	}
}