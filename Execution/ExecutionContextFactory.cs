using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using Yaver.Host.Wpf.Services.Dialog;

namespace Yaver.Host.Wpf.Execution
{
	[Export]
	public class ExecutionContextFactory
	{
		private readonly ILoggerFactory _loggerFactory;
		private readonly IDialogService _dialogService;
		private readonly IWindowManager _windowManager;
		private readonly ISnackbarMessageQueue _messageQueue;

		[ImportingConstructor]
		public ExecutionContextFactory(
			ILoggerFactory loggerFactory,
			IDialogService dialogService,
			IWindowManager windowManager,
			ISnackbarMessageQueue messageQueue)
		{
			_loggerFactory = loggerFactory;
			_dialogService = dialogService;
			_windowManager = windowManager;
			_messageQueue = messageQueue;
		}

		public ExecutionContext Create(Type ownerType, Action<Exception> errorHandler)
		{
			return new ExecutionContext(
				_loggerFactory,
				_dialogService,
				_windowManager,
				_messageQueue,
				ownerType,
				errorHandler);
		}
	}
}