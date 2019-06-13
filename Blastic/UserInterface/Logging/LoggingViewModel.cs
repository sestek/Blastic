using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Autofac;
using Caliburn.Micro;
using PropertyChanged;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Blastic.UserInterface.Settings.Logging;

namespace Blastic.UserInterface.Logging
{
	[SingleInstance]
	[AddINotifyPropertyChangedInterface]
	public sealed class LoggingViewModel : ILogEventSink, IViewAware
	{
		private readonly IWindowManager _windowManager;
		private readonly LogSettingsViewModel _logSettingsViewModel;

		private Window _activeWindow;

		public LogEventLevel MinimumLogLevel { get; set; }

		public IObservableCollection<Log> Logs { get; }
		public IObservableCollection<LogEventLevel> LogLevels { get; }
		
		public LoggingViewModel(
			IWindowManager windowManager,
			LogSettingsViewModel logSettingsViewModel)
		{
			_windowManager = windowManager;
			_logSettingsViewModel = logSettingsViewModel;

			Serilog.Log.Logger = new LoggerConfiguration()
				.WriteTo.Logger(Serilog.Log.Logger)
				.WriteTo.Sink(this)
				.CreateLogger();

			Logs = new BindableCollection<Log>();
			LogLevels = new BindableCollection<LogEventLevel>
			{
				LogEventLevel.Fatal,
				LogEventLevel.Error,
				LogEventLevel.Warning,
				LogEventLevel.Information,
				LogEventLevel.Debug,
				LogEventLevel.Verbose,
			};

			MinimumLogLevel = LogEventLevel.Information;
		}

		public async Task Show()
		{
			if (_activeWindow != null && PresentationSource.FromVisual(_activeWindow) != null)
			{
				_activeWindow.Activate();
				return;
			}

			dynamic settings = new ExpandoObject();
			settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;

			await _windowManager.ShowWindowAsync(this, null, settings);
		}

		public void Clear()
		{
			Logs.Clear();
		}

		public async void Emit(LogEvent logEvent)
		{
			IReadOnlyDictionary<string, LogEventPropertyValue> properties = logEvent.Properties;

			string source = "";

			if (properties.TryGetValue("SourceContext", out LogEventPropertyValue sourceValue))
			{
				source = sourceValue.ToString();
			}

			Log log = new Log
			{
				Date = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
				Level = logEvent.Level,
				Source = source,
				Message = logEvent.RenderMessage()
			};

			Logs.Add(log);

			if (logEvent.Level >= LogEventLevel.Error && _logSettingsViewModel.OpenWindowOnError.Value)
			{
				await Show();
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