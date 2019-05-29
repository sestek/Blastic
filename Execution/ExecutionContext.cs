using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using PropertyChanged;
using WpfTemplate.Services.Dialog;

namespace WpfTemplate.Execution
{
	[InstancePerDependency]
	[AddINotifyPropertyChangedInterface]
	public class ExecutionContext
	{
		private readonly ILogger _logger;

		public IDialogService DialogService { get; }
		public IWindowManager WindowManager { get; }
		public IEventAggregator EventAggregator { get; }
		public ISnackbarMessageQueue MessageQueue { get; }

		public bool IsBusy { get; set; }
		public string ProgressMessage { get; set; }
		public CancellationTokenSource CancellationTokenSource { get; private set; }

		public ExecutionContext(
			ILogger<ExecutionContext> logger,
			IDialogService dialogService,
			IWindowManager windowManager,
			IEventAggregator eventAggregator,
			ISnackbarMessageQueue messageQueue)
		{
			DialogService = dialogService;
			WindowManager = windowManager;
			EventAggregator = eventAggregator;
			MessageQueue = messageQueue;
			_logger = logger;

			CancellationTokenSource = new CancellationTokenSource();
		}

		public async Task Execute(
			Func<CancellationToken, Task> function,
			string progressMessage = "",
			string successMessage = "",
			string failMessage = "",
			bool showProgress = true,
			CancellationToken? customCancellationToken = null)
		{
			try
			{
				if (showProgress)
				{
					IsBusy = true;
					ProgressMessage = progressMessage;
				}

				if (CancellationTokenSource.IsCancellationRequested)
				{
					CancellationTokenSource?.Dispose();
					CancellationTokenSource = new CancellationTokenSource();
				}

				CancellationToken cancellationToken = customCancellationToken ?? CancellationTokenSource.Token;

				await function(cancellationToken);

				if (!string.IsNullOrEmpty(successMessage))
				{
					MessageQueue.Enqueue(successMessage);
				}
			}
			catch (TaskCanceledException)
			{
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "");

				if (!string.IsNullOrEmpty(failMessage))
				{
					MessageQueue.Enqueue(failMessage);
				}
			}
			finally
			{
				if (showProgress)
				{
					IsBusy = false;
				}
			}
		}
	}
}