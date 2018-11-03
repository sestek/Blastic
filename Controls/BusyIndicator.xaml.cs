using Bindables;
using Yaver.Host.Wpf.Execution;

namespace Yaver.Host.Wpf.Controls
{
	public partial class BusyIndicator
	{
		[DependencyProperty]
		public ExecutionContext ExecutionContext { get; set; }

		public BusyIndicator()
		{
			InitializeComponent();
		}

		public void Cancel()
		{
			ExecutionContext?.CancellationTokenSource?.Cancel();
		}
	}
}