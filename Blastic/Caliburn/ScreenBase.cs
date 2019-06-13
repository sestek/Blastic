using Blastic.Caliburn.Reactive;
using Blastic.Execution;

namespace Blastic.Caliburn
{
	public class ScreenBase : ReactiveScreen, IHasExecutionContext
	{
		public ExecutionContext ExecutionContext { get; }
		public ExecutionContextFactory ExecutionContextFactory { get; }

		public ScreenBase(ExecutionContextFactory executionContextFactory)
		{
			ExecutionContextFactory = executionContextFactory;
			ExecutionContext = executionContextFactory.Create();
		}
	}
}