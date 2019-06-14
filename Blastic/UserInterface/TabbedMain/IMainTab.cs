using Blastic.Common;
using Caliburn.Micro;

namespace Blastic.UserInterface.TabbedMain
{
	public interface IMainTab : IScreen
	{
		Order Order { get; }
		bool IsFixed { get; }
	}
}