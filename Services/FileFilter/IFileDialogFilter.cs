using System.Collections.Generic;

namespace Yaver.Host.Wpf.Services.FileFilter
{
	public interface IFileDialogFilter
	{
		IEnumerable<string> Extensions { get; }
		string GetFileDialogRepresentation();
	}
}