using Caliburn.Micro;
using WpfTemplate.Execution;

namespace WpfTemplate.Caliburn
{
	public class ScreenBase : Screen, IHasExecutionContext
	{
		public ExecutionContext ExecutionContext { get; }

		public ScreenBase(ExecutionContext executionContext)
		{
			ExecutionContext = executionContext;
		}
	}
}