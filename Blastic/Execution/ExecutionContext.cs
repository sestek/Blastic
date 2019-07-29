using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using PropertyChanged;
using Blastic.Services.Dialog;
using Blastic.UserInterface.Events;

namespace Blastic.Execution
{
	[AddINotifyPropertyChangedInterface]
	public class ExecutionContext
	{
		public ILogger Logger { get; }
		public IDialogService DialogService { get; }
		public IWindowManager WindowManager { get; }
		public IEventAggregator EventAggregator { get; }
		public ISnackbarMessageQueue MessageQueue { get; }

		public bool IsBusy { get; set; }
		public string ProgressMessage { get; set; }
		public IObservableCollection<string> ProgressDetails { get; }

		public bool IsCancellationSupported { get; set; }
		public CancellationTokenSource CancellationTokenSource { get; private set; }

		public ExecutionContext(
			ILogger<ExecutionContext> logger,
			IDialogService dialogService,
			IWindowManager windowManager,
			IEventAggregator eventAggregator,
			ISnackbarMessageQueue messageQueue)
		{
			Logger = logger;
			DialogService = dialogService;
			WindowManager = windowManager;
			EventAggregator = eventAggregator;
			MessageQueue = messageQueue;

			ProgressDetails = new BindableCollection<string>();
			CancellationTokenSource = new CancellationTokenSource();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public async Task Execute(
			Func<CancellationToken, Task> function,
			string progressMessage = "",
			string successMessage = "",
			string failMessage = "",
			bool showProgress = true,
			bool rethrowUnhandledException = true,
			bool isCancellationSupported = true,
			CancellationToken? customCancellationToken = null)
		{
			try
			{
				if (showProgress)
				{
					IsBusy = true;
					ProgressMessage = progressMessage;
				}

				ProgressDetails.Clear();
				IsCancellationSupported = isCancellationSupported;
				
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
				Logger.LogError(exception, exception.Message);

				MessageQueue.Enqueue(
					string.IsNullOrEmpty(failMessage)
						? exception.Message
						: failMessage,
					"Open Logs",
					() => EventAggregator.PublishOnUIThreadAsync(new OpenLogsEvent()));

				if (rethrowUnhandledException)
				{
					throw;
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