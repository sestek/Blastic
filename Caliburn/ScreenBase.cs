using System;
using Caliburn.Micro;
using Yaver.Host.Wpf.Execution;

namespace Yaver.Host.Wpf.Caliburn
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