using System.ComponentModel.Composition;
using PropertyChanged;
using WpfTemplate.Caliburn;
using WpfTemplate.Execution;

namespace WpfTemplate.UserInterface.Login
{
	[Export]
	[AddINotifyPropertyChangedInterface]
	public class LoginViewModel : ScreenBase
	{
		public LoginViewModel(ExecutionContextFactory executionContextFactory) : base(executionContextFactory)
		{
		}
	}
}