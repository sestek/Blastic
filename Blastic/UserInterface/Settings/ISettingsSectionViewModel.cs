using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blastic.Diagnostics;

namespace Blastic.UserInterface.Settings
{
	public interface ISettingsSectionViewModel
	{
		string SectionName { get; }
		
		Task<IEnumerable<DiagnosticMessage>> GetDiagnosticMessages(CancellationToken cancellationToken);

		Task Save(CancellationToken cancellationToken);
		Task ReadSettings(CancellationToken cancellationToken);
		void Revert();
	}
}