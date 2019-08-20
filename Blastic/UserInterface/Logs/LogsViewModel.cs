using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Blastic.UserInterface.Logs.Settings;
using Caliburn.Micro;
using PropertyChanged;
using Serilog.Events;

namespace Blastic.UserInterface.Logs
{
	[AddINotifyPropertyChangedInterface]
	public sealed class LogsViewModel : IViewAware
	{
		private readonly IWindowManager _windowManager;
		private readonly LogSettingsViewModel _logSettingsViewModel;

		private Window _activeWindow;

		public LogEventLevel MinimumLogLevel { get; set; }

		public LogSink LogSink { get; }
		public IObservableCollection<LogEventLevel> LogLevels { get; }
		
		public LogsViewModel(
			IWindowManager windowManager,
			LogSettingsViewModel logSettingsViewModel,
			LogSink logSink)
		{
			_windowManager = windowManager;
			_logSettingsViewModel = logSettingsViewModel;
			LogSink = logSink;
			
			LogLevels = new BindableCollection<LogEventLevel>
			{
				LogEventLevel.Fatal,
				LogEventLevel.Error,
				LogEventLevel.Warning,
				LogEventLevel.Information,
				LogEventLevel.Debug
			};

			MinimumLogLevel = LogEventLevel.Debug;

			LogSink.Logs.CollectionChanged += LogsChanged;
		}

		private async void LogsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			bool? hasErrorLog = e.NewItems?.Cast<Log>().Any(x => x.Level >= LogEventLevel.Error);

			if (hasErrorLog == true && _logSettingsViewModel.OpenWindowOnErrorSetting.Value)
			{
				await Show();
			}
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
			LogSink.Logs.Clear();
		}

		// Will be called by Fody.
		private void OnMinimumLogLevelChanged()
		{
			ICollectionView collectionView = CollectionViewSource.GetDefaultView(LogSink.Logs);
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