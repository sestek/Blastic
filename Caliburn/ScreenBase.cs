using System;
using Caliburn.Micro;
using WpfTemplate.Execution;

namespace WpfTemplate.Caliburn
{
	public class ScreenBase : Screen, IHasExecutionContext
	{
		public ExecutionContext ExecutionContext { get; }

		public ScreenBase(ExecutionContextFactory executionContextFactory, Action<Exception> errorHandler = null)
		{
			ExecutionContext = executionContextFactory.Create(GetType(), errorHandler);
		}
	}
}