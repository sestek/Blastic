using Caliburn.Micro;
using WpfTemplate.Execution;

namespace WpfTemplate.Caliburn
{
	public class ConductorOneActiveBase<T> : Conductor<T>.Collection.OneActive, IHasExecutionContext where T : class 
	{
		public ExecutionContext ExecutionContext { get; }
		
		public ConductorOneActiveBase(ExecutionContext executionContext)
		{
			ExecutionContext = executionContext;
		}
	}
}