using Autofac;
using PropertyChanged;
using WpfTemplate.Caliburn;
using WpfTemplate.Execution;

namespace WpfTemplate.UserInterface.Login
{
	[SingleInstance]
	[AddINotifyPropertyChangedInterface]
	public class LoginViewModel : ScreenBase
	{
		public LoginViewModel(ExecutionContextFactory executionContextFactory) : base(executionContextFactory)
		{
		}
	}
}