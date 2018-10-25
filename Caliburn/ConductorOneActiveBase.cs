using System;
using Caliburn.Micro;
using Yaver.Host.Wpf.Execution;

namespace Yaver.Host.Wpf.Caliburn
{
	public class ConductorOneActiveBase<T> : Conductor<T>.Collection.OneActive, IHasExecutionContext where T : class 
	{
		public ExecutionContext ExecutionContext { get; }
		
		public ConductorOneActiveBase(ExecutionContextFactory executionContextFactory, Action<Exception> errorHandler = null)
		{
			ExecutionContext = executionContextFactory.Create(GetType(), errorHandler);
		}
	}
}