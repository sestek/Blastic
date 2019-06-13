using Blastic.Caliburn.Reactive;
using Blastic.Execution;

namespace Blastic.Caliburn
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