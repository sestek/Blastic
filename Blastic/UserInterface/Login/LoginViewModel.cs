using PropertyChanged;
using Blastic.Caliburn;
using Blastic.Execution;

namespace Blastic.UserInterface.Login
{
	[AddINotifyPropertyChangedInterface]
	public class LoginViewModel : ScreenBase
	{
		public LoginViewModel(ExecutionContextFactory executionContextFactory) : base(executionContextFactory)
		{
		}
	}
}