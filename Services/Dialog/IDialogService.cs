using System.Windows;
using Yaver.Host.Wpf.Services.FileFilter;

namespace Yaver.Host.Wpf.Services.Dialog
{
	public interface IDialogService
	{
		bool? ShowDialog<T>(object viewModel) where T : Window;

		string ShowOpenFileDialog(IFileDialogFilter filter);
		string ShowSaveFileDialog(IFileDialogFilter filter);

		string ShowSelectFolderDialog();
	}
}