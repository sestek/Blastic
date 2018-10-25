using Bindables;

namespace Yaver.Host.Wpf.Controls
{
	public partial class BusyIndicator
	{
		[DependencyProperty]
		public bool IsBusy { get; set; }

		public BusyIndicator()
		{
			InitializeComponent();
		}
	}
}