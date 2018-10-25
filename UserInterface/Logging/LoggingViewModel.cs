using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;
using NLog;
using NLog.Config;
using NLog.Targets;
using PropertyChanged;
using LogManager = NLog.LogManager;

namespace Yaver.Host.Wpf.UserInterface.Logging
{
	[Export]
	[AddINotifyPropertyChangedInterface]
	public sealed class LoggingViewModel : TargetWithLayout, IViewAware
	{
		private readonly IWindowManager _windowManager;
		private Window _activeWindow;

		public LogLevel MinimumLogLevel { get; set; }

		public IObservableCollection<Log> Logs { get; }
		public IObservableCollection<LogLevel> LogLevels { get; }

		[ImportingConstructor]
		public LoggingViewModel(IWindowManager windowManager)
		{
			_windowManager = windowManager;

			LogManager.Configuration.AddTarget("UserInterface", this);

			LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, this));
			LogManager.Configuration.Reload();

			Logs = new BindableCollection<Log>();
			LogLevels = new BindableCollection<LogLevel>
			{
				LogLevel.Error,
				LogLevel.Warn,
				LogLevel.Info,
				LogLevel.Debug
			};

			MinimumLogLevel = LogLevel.Debug;

			Layout = "${longdate} | ${level:padding=-5} | ${logger} | ${message} ${exception:format=tostring}";
		}

		public void Show()
		{
			if (_activeWindow != null && PresentationSource.FromVisual(_activeWindow) != null)
			{
				_activeWindow.Activate();
				return;
			}

			dynamic settings = new ExpandoObject();
			settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;

			_windowManager.ShowWindow(this, null, settings);
		}

		public void Clear()
		{
			Logs.Clear();
		}

		protected override void Write(LogEventInfo logEvent)
		{
			string logMessage = Layout.Render(logEvent);
			string[] tokens = logMessage.Split('|');

			Log log = new Log
			{
				Date = tokens[0],
				Level = logEvent.Level,
				Source = tokens[2],
				Message = tokens[3]
			};

			Logs.Add(log);

			if (logEvent.Level >= LogLevel.Error)
			{
				Show();
			}
		}

		// Will be called by Fody.
		private void OnMinimumLogLevelChanged()
		{
			ICollectionView collectionView = CollectionViewSource.GetDefaultView(Logs);
			collectionView.Filter = o => ((Log)o).Level >= MinimumLogLevel;
		}

		public void AttachView(object view, object context = null)
		{
			_activeWindow = view as Window;

			ViewAttached?.Invoke(this, new ViewAttachedEventArgs
			{
				View = view
			});
		}

		public object GetView(object context = null)
		{
			return _activeWindow;
		}

		public event EventHandler<ViewAttachedEventArgs> ViewAttached;
	}
}