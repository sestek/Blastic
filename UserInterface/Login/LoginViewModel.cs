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
		public LoginViewModel(ExecutionContext executionContext) : base(executionContext)
		{
		}
	}
}