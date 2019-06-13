using WpfTemplate.Caliburn.Reactive;
using WpfTemplate.Execution;

namespace WpfTemplate.Caliburn
{
	public class ConductorAllActiveBase<T> : ReactiveConductor<T>.Collection.AllActive, IHasExecutionContext where T : class 
	{
		public ExecutionContextFactory ExecutionContextFactory { get; }
		public ExecutionContext ExecutionContext { get; }
		
		public ConductorAllActiveBase(ExecutionContextFactory executionContextFactory)
		{
			ExecutionContextFactory = executionContextFactory;
			ExecutionContext = executionContextFactory.Create();
		}
	}
}