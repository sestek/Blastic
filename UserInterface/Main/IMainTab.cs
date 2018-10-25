using Caliburn.Micro;

namespace Yaver.Host.Wpf.UserInterface.Main
{
	public interface IMainTab : IScreen
	{
		int Order { get; }
	}
}