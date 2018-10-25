namespace Yaver.Host.Wpf.Services.FileFilter
{
	public class FileDialogFilters
	{
		public static readonly IFileDialogFilter PdfFile = new FileDialogFilter("Pdf Files", "pdf");
		public static readonly IFileDialogFilter ZipFile = new FileDialogFilter("Zip Files", "zip");
	}
}