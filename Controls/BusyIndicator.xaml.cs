using Bindables;
using WpfTemplate.Execution;

namespace WpfTemplate.Controls
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