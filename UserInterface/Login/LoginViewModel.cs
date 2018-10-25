using System.ComponentModel.Composition;
using PropertyChanged;
using Yaver.Host.Wpf.Caliburn;
using Yaver.Host.Wpf.Execution;

namespace Yaver.Host.Wpf.UserInterface.Login
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