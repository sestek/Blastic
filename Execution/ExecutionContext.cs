using System;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using PropertyChanged;
using WpfTemplate.Services.Dialog;

namespace WpfTemplate.Execution
{
	[AddINotifyPropertyChangedInterface]
	public class ExecutionContext
	{
		private readonly ILogger _logger;
		private readonly Action<Exception> _errorHandler;

		public IDialogService DialogService { get; }
		public IWindowManager WindowManager { get; }
		public ISnackbarMessageQueue MessageQueue { get; }

		public bool IsBusy { get; set; }
		public string ProgressMessage { get; set; }
		public CancellationTokenSource CancellationTokenSource { get; private set; }

		public ExecutionContext(
			ILoggerFactory loggerFactory,
			IDialogService dialogService,
			IWindowManager windowManager,
			ISnackbarMessageQueue messageQueue,
			Type ownerType,
			Action<Exception> errorHandler)
		{
			DialogService = dialogService;
			WindowManager = windowManager;
			MessageQueue = messageQueue;
			_errorHandler = errorHandler;
			_logger = loggerFactory.CreateLogger(ownerType);

			CancellationTokenSource = new CancellationTokenSource();
		}

		public async Task Execute(
			Func<CancellationToken, Task> function,
			string progressMessage,
			string successMessage = "Operation succeeded.",
			string failMessage = "Operation failed.")
		{
			try
			{
				IsBusy = true;
				ProgressMessage = progressMessage;

				if (CancellationTokenSource.IsCancellationRequested)
				{
					CancellationTokenSource?.Dispose();
					CancellationTokenSource = new CancellationTokenSource();
				}

				await function(CancellationTokenSource.Token);

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
				_logger.LogError(exception, failMessage);
				_errorHandler?.Invoke(exception);

				if (!string.IsNullOrEmpty(failMessage))
				{
					MessageQueue.Enqueue(failMessage);
				}
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}