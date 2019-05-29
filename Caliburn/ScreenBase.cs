using WpfTemplate.Caliburn.Reactive;
using WpfTemplate.Execution;

namespace WpfTemplate.Caliburn
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