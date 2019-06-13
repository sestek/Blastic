using System.Collections.Generic;

namespace Blastic.Services.Dialog.FileFilters
{
	public interface IFileDialogFilter
	{
		IEnumerable<string> Extensions { get; }
		string GetFileDialogRepresentation();
	}
}