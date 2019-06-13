using System.Collections.Generic;

namespace WpfTemplate.Services.Dialog.FileFilters
{
	public interface IFileDialogFilter
	{
		IEnumerable<string> Extensions { get; }
		string GetFileDialogRepresentation();
	}
}