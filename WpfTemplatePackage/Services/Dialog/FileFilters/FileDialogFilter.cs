using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTemplate.Services.Dialog.FileFilters
{
	public class FileDialogFilter : List<string>, IFileDialogFilter
	{
		public IEnumerable<string> Extensions => this;
		public string Explanation { get; }

		public FileDialogFilter(string explanation, params string[] extensions)
		{
			Explanation = explanation;
			AddRange(extensions);
		}

		public string GetFileDialogRepresentation()
		{
			if (!this.Any())
			{
				throw new ArgumentException("No file extension is defined.");
			}

			StringBuilder builder = new StringBuilder();

			builder.Append(Explanation);

			builder.Append(" (");
			builder.Append(string.Join(", *.", this.Select(e => "*." + e)));
			builder.Append(")");

			builder.Append("|");
			builder.Append(string.Join(";", this.Select(e => "*." + e)));

			return builder.ToString();
		}
	}
}