using System.Threading;
using System.Threading.Tasks;
using Blastic.Common;

namespace Blastic.Initialization.Steps
{
	public interface IInitializationStep
	{
		Order Order { get; }

		string Description { get; }
		string SuccessMessage { get; }
		string FailureMessage { get; }

		bool IsCancellationSupported { get; }
		bool ShowBusyIndicator { get; }

		Task Initialize(CancellationToken cancellationToken);
	}
}