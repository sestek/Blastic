using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WpfTemplate.Caliburn;
using WpfTemplate.Diagnostics;
using WpfTemplate.Execution;

namespace WpfTemplate.UserInterface.Settings
{
	public abstract class SettingsSectionViewModel : ScreenBase, ISettingsSectionViewModel
	{
		public abstract string SectionName { get; }

		protected SettingsSectionViewModel(ExecutionContextFactory executionContextFactory) : base(executionContextFactory)
		{
		}

		public virtual IEnumerable<DiagnosticMessage> GetDiagnosticMessages()
		{
			return Enumerable.Empty<DiagnosticMessage>();
		}

		public abstract Task Save(CancellationToken cancellationToken);
		public abstract Task ReadSettings(CancellationToken cancellationToken);
	}
}