using Autofac;
using WpfTemplate.Caliburn;
using WpfTemplate.Execution;
using WpfTemplate.UserInterface.Main;

namespace WpfTemplate.UserInterface.Home
{
	[SingleInstance]
	public sealed class HomeViewModel : ScreenBase, IMainTab
	{
		public int Order => 0;
		
		public HomeViewModel(ExecutionContext executionContext) : base(executionContext)
		{
			DisplayName = "Home";
		}
	}
}