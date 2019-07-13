using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using Blastic.Caliburn;
using Blastic.Common;
using Blastic.Execution;
using Blastic.UserInterface.TabbedMain;

namespace Blastic.Sample.UserInterface
{
	public class HomeViewModel : ScreenBase, IMainTab
	{
		public Order Order { get; }
		public bool IsFixed => true;

		public string Text { get; set; }
		public AdaptiveCard Card { get; set; }

		public HomeViewModel(ExecutionContextFactory executionContextFactory) : base(executionContextFactory)
		{
			Order = new Order(1);
		}

		protected override Task OnInitializeAsync(CancellationToken cancellationToken)
		{
			Text = "Initialized";
			return Task.CompletedTask;
		}

		protected override Task OnActivateAsync(CancellationToken cancellationToken)
		{
			Text = "Activated";
			return Task.CompletedTask;
		}

		public void Test()
		{
			Card = AdaptiveCard.FromJson(File.ReadAllText(@"C:\Users\yusuf.gunaydin\Desktop\temp\card.json")).Card;
		}
	}
}