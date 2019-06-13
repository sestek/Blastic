namespace Blastic.UserInterface.Events
{
	public class OpenTabEvent
	{
		public object ViewModel { get; }

		public OpenTabEvent(object viewModel)
		{
			ViewModel = viewModel;
		}
	}
}