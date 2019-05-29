using WpfTemplate.Caliburn.Reactive;
using WpfTemplate.Execution;

namespace WpfTemplate.Caliburn
{
	public class ConductorOneActiveBase<T> : ReactiveConductor<T>.Collection.OneActive, IHasExecutionContext where T : class 
	{
		public ExecutionContextFactory ExecutionContextFactory { get; }
		public ExecutionContext ExecutionContext { get; }
		
		public ConductorOneActiveBase(ExecutionContextFactory executionContextFactory)
		{
			ExecutionContextFactory = executionContextFactory;
			ExecutionContext = executionContextFactory.Create();
		}
	}
}