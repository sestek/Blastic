using System.Collections.Generic;
using System.Linq;

namespace WpfTemplate.Services.Dialog.FileFilters
{
	public class FileDialogFilterCollection : List<FileDialogFilter>, IFileDialogFilter
	{
		public IEnumerable<string> Extensions => this.SelectMany(filter => filter.Extensions);

		public string GetFileDialogRepresentation()
		{
			return string.Join("|", this.Select(filter => filter.GetFileDialogRepresentation()));
		}
	}
}