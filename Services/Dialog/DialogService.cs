using System;
using System.ComponentModel.Composition;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Yaver.Host.Wpf.Services.FileFilter;

namespace Yaver.Host.Wpf.Services.Dialog
{
	[Export(typeof(IDialogService))]
	public class DialogService : IDialogService
	{
		public bool? ShowDialog<T>(object viewModel) where T : Window
		{
			Window dialog = (Window) Activator.CreateInstance(typeof(T));
			dialog.DataContext = viewModel;
			return dialog.ShowDialog();
		}

		public string ShowOpenFileDialog(IFileDialogFilter filter)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = filter.GetFileDialogRepresentation(),
				Multiselect = false
			};

			if (openFileDialog.ShowDialog() == true)
			{
				return openFileDialog.FileName;
			}

			return "";
		}

		public string ShowSaveFileDialog(IFileDialogFilter filter)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				Filter = filter.GetFileDialogRepresentation(),
				AddExtension = true
			};

			if (saveFileDialog.ShowDialog() == true)
			{
				return saveFileDialog.FileName;
			}

			return "";
		}

		public string ShowSelectFolderDialog()
		{
			CommonFileDialog folderBrowserDialog = new CommonOpenFileDialog
			{
				IsFolderPicker = true
			};

			if (folderBrowserDialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				return folderBrowserDialog.FileName;
			}

			return "";
		}
	}
}