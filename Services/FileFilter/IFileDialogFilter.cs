using System.Collections.Generic;

namespace WpfTemplate.Services.FileFilter
{
	public interface IFileDialogFilter
	{
		IEnumerable<string> Extensions { get; }
		string GetFileDialogRepresentation();
	}
}