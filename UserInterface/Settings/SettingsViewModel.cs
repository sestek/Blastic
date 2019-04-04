using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using PropertyChanged;
using WpfTemplate.Caliburn;
using WpfTemplate.Diagnostics;
using WpfTemplate.Execution;

namespace WpfTemplate.UserInterface.Settings
{
	[SingleInstance]
	[AddINotifyPropertyChangedInterface]
	public sealed class SettingsViewModel : ScreenBase
	{
		private Window _activeWindow;

		public IObservableCollection<ISettingsSectionViewModel> Items { get; set; }
		public IObservableCollection<DiagnosticMessage> DiagnosticMessages { get; set; }

		public bool IsDiagnosticMessagesVisible { get; set; }

		public SettingsViewModel(
			ExecutionContextFactory executionContextFactory,
			IEnumerable<ISettingsSectionViewModel> sections)
			:
			base(executionContextFactory)
		{
			Items = new BindableCollection<ISettingsSectionViewModel>(sections);
			DiagnosticMessages = new BindableCollection<DiagnosticMessage>();

			DisplayName = "Settings";
		}

		protected override async void OnActivate()
		{
			await ReadSettings();
		}

		public async Task ReadSettings()
		{
			async Task ReadSettings(CancellationToken cancellationToken)
			{
				foreach (ISettingsSectionViewModel item in Items)
				{
					await item.ReadSettings(cancellationToken);
				}
			}

			await ExecutionContext.Execute(ReadSettings);
		}

		public async void Save()
		{
			DiagnosticMessages.Clear();

			foreach (ISettingsSectionViewModel item in Items)
			{
				IEnumerable<DiagnosticMessage> diagnosticMessages = item.GetDiagnosticMessages();
				DiagnosticMessages.AddRange(diagnosticMessages);
			}

			if (DiagnosticMessages.Any(x => x.Severity == Severity.Error))
			{
				ShowDiagnosticMessages();
				return;
			}

			async Task Save(CancellationToken cancellationToken)
			{
				foreach (ISettingsSectionViewModel item in Items)
				{
					await item.Save(cancellationToken);
				}
			}

			await ExecutionContext.Execute(Save);

			TryClose();
		}

		private void ShowDiagnosticMessages()
		{
			IsDiagnosticMessagesVisible = true;
		}

		public void HideDiagnosticMessages()
		{
			IsDiagnosticMessagesVisible = false;
		}

		public void Cancel()
		{
			TryClose();
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

			ExecutionContext.WindowManager.ShowWindow(this, null, settings);
		}

		protected override void OnViewAttached(object view, object context)
		{
			_activeWindow = view as Window;
		}

		public override object GetView(object context = null)
		{
			return _activeWindow;
		}
	}
}