using System;
using System.Threading;
using System.Threading.Tasks;
using Blastic.Caliburn;
using Blastic.Common;
using Blastic.Execution;
using Blastic.UserInterface.TabbedMain;
using Forge.Forms;

namespace Blastic.Sample.UserInterface
{
	public class HomeViewModel : ScreenBase, IMainTab
	{
		public Order Order { get; }
		public bool IsFixed => true;

		public string Text { get; set; }

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

		public async Task Test()
		{
			await ExecutionContext.Execute(x =>
			{
				throw new Exception("Tadaa!");
			});

			await Show.Dialog().For(new Prompt<string>
			{
				Message = "Test message",
				Title = "Test title"
			});
		}
	}
}