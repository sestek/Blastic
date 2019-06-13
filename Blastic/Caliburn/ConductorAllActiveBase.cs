using Blastic.Caliburn.Reactive;
using Blastic.Execution;

namespace Blastic.Caliburn
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