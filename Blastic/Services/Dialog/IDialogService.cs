using System.Windows;

namespace Blastic.Services.Dialog
{
	public interface IDialogService
	{
		bool? ShowDialog<T>(object viewModel) where T : Window;

		string ShowOpenFileDialog(FileDialogOptions options = default);
		string ShowSaveFileDialog(FileDialogOptions options = default);

		string ShowSelectFolderDialog(FileDialogOptions options = default);
	}
}