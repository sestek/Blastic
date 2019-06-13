using Autofac;
using Blastic.Caliburn;
using Blastic.Execution;
using Blastic.UserInterface.Main;

namespace Blastic.UserInterface.Home
{
	[SingleInstance]
	public sealed class HomeViewModel : ScreenBase, IMainTab
	{
		public int Order => 0;
		
		public HomeViewModel(
			ExecutionContextFactory executionContextFactory)
			:
			base(executionContextFactory)
		{
			DisplayName = "Home";
		}
	}
}