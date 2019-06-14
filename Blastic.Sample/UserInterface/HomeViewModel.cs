using Blastic.Caliburn;
using Blastic.Common;
using Blastic.Execution;
using Blastic.UserInterface.TabbedMain;

namespace Blastic.Sample.UserInterface
{
	public class HomeViewModel : ScreenBase, IMainTab
	{
		public Order Order { get; }
		public bool IsFixed => true;

		public HomeViewModel(ExecutionContextFactory executionContextFactory) : base(executionContextFactory)
		{
			Order = new Order(1);
		}
	}
}