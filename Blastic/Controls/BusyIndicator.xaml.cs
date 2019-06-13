using Bindables;
using Blastic.Execution;

namespace Blastic.Controls
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