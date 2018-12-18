using Caliburn.Micro;
using WpfTemplate.Execution;

namespace WpfTemplate.Caliburn
{
	public class ScreenBase : Screen, IHasExecutionContext
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