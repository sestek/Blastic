using Autofac;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using WpfTemplate.Services.Dialog;

namespace WpfTemplate.Execution
{
	[SingleInstance]
	public class ExecutionContextFactory
	{
		private readonly ILogger<ExecutionContext> _logger;
		private readonly IDialogService _dialogService;
		private readonly IWindowManager _windowManager;
		private readonly ISnackbarMessageQueue _messageQueue;

		public ExecutionContextFactory(
			ILogger<ExecutionContext> logger,
			IDialogService dialogService,
			IWindowManager windowManager,
			ISnackbarMessageQueue messageQueue)
		{
			_logger = logger;
			_dialogService = dialogService;
			_windowManager = windowManager;
			_messageQueue = messageQueue;
		}

		public ExecutionContext Create()
		{
			return new ExecutionContext(
				_logger,
				_dialogService,
				_windowManager,
				_messageQueue);
		}
	}
}