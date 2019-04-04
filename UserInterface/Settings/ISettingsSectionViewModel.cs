using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WpfTemplate.Diagnostics;

namespace WpfTemplate.UserInterface.Settings
{
	public interface ISettingsSectionViewModel
	{
		string SectionName { get; }
		
		IEnumerable<DiagnosticMessage> GetDiagnosticMessages();

		Task Save(CancellationToken cancellationToken);
		Task ReadSettings(CancellationToken cancellationToken);
	}
}