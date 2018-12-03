using Caliburn.Micro;

namespace WpfTemplate.UserInterface.Main
{
	public interface IMainTab : IScreen
	{
		int Order { get; }
	}
}