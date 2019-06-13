using System;
using System.Windows;
using Autofac;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Blastic.Services.Dialog
{
	[SingleInstance]
	public class DialogService : IDialogService
	{
		public bool? ShowDialog<T>(object viewModel) where T : Window
		{
			Window dialog = (Window) Activator.CreateInstance(typeof(T));
			dialog.DataContext = viewModel;

			return dialog.ShowDialog();
		}

		public string ShowOpenFileDialog(FileDialogOptions options)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = options?.Filter?.GetFileDialogRepresentation() ?? "",
				Multiselect = options?.IsMultiSelect ?? false,
				InitialDirectory = options?.InitialDirectory ?? ""
			};

			bool? result = options?.Owner == null
				? openFileDialog.ShowDialog()
				: openFileDialog.ShowDialog(options.Owner);

			if (result == true)
			{
				return openFileDialog.FileName;
			}

			return "";
		}

		public string ShowSaveFileDialog(FileDialogOptions options)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				Filter = options?.Filter?.GetFileDialogRepresentation() ?? "",
				InitialDirectory = options?.InitialDirectory ?? "",
				AddExtension = true
			};

			bool? result = options?.Owner == null
				? saveFileDialog.ShowDialog()
				: saveFileDialog.ShowDialog(options.Owner);

			if (result == true)
			{
				return saveFileDialog.FileName;
			}

			return "";
		}

		public string ShowSelectFolderDialog(FileDialogOptions options)
		{
			CommonFileDialog folderBrowserDialog = new CommonOpenFileDialog
			{
				IsFolderPicker = true,
				InitialDirectory = options?.InitialDirectory ?? ""
			};

			CommonFileDialogResult result = options?.Owner == null
				? folderBrowserDialog.ShowDialog()
				: folderBrowserDialog.ShowDialog(options.Owner);

			if (result == CommonFileDialogResult.Ok)
			{
				return folderBrowserDialog.FileName;
			}

			return "";
		}
	}
}