using System.Windows;
using WpfTemplate.Services.FileFilter;

namespace WpfTemplate.Services.Dialog
{
	public interface IDialogService
	{
		bool? ShowDialog<T>(object viewModel) where T : Window;

		string ShowOpenFileDialog(IFileDialogFilter filter);
		string ShowSaveFileDialog(IFileDialogFilter filter);

		string ShowSelectFolderDialog();
	}
}