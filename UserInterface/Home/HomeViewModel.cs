using System.ComponentModel.Composition;
using Yaver.Host.Wpf.Caliburn;
using Yaver.Host.Wpf.Execution;
using Yaver.Host.Wpf.UserInterface.Main;

namespace Yaver.Host.Wpf.UserInterface.Home
{
	[Export]
	[Export(typeof(IMainTab))]
	public sealed class HomeViewModel : ScreenBase, IMainTab
	{
		public int Order => 0;

		[ImportingConstructor]
		public HomeViewModel(ExecutionContextFactory executionContextFactory) : base(executionContextFactory)
		{
			DisplayName = "Home";
		}
	}
}