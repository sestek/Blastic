using System.ComponentModel.Composition;
using WpfTemplate.Caliburn;
using WpfTemplate.Execution;
using WpfTemplate.UserInterface.Main;

namespace WpfTemplate.UserInterface.Home
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