using Autofac;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using Blastic.Services.Dialog;

namespace Blastic.Execution
{
	[SingleInstance]
	public class ExecutionContextFactory
	{
		private readonly ILogger<ExecutionContext> _logger;
		private readonly IDialogService _dialogService;
		private readonly IWindowManager _windowManager;
		private readonly IEventAggregator _eventAggregator;
		private readonly ISnackbarMessageQueue _messageQueue;

		public ExecutionContextFactory(
			ILogger<ExecutionContext> logger,
			IDialogService dialogService,
			IWindowManager windowManager,
			IEventAggregator eventAggregator,
			ISnackbarMessageQueue messageQueue)
		{
			_logger = logger;
			_dialogService = dialogService;
			_windowManager = windowManager;
			_eventAggregator = eventAggregator;
			_messageQueue = messageQueue;
		}

		public ExecutionContext Create()
		{
			return new ExecutionContext(
				_logger,
				_dialogService,
				_windowManager,
				_eventAggregator,
				_messageQueue);
		}
	}
}