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

		protected override async Task OnActivateAsync(CancellationToken cancellationToken)
		{
			await ReadSettings(cancellationToken);
		}
		
		public async Task ReadSettings(CancellationToken cancellationToken)
		{
			async Task ReadSettings(CancellationToken token)
			{
				foreach (ISettingsSectionViewModel item in Items)
				{
					await item.ReadSettings(token);
				}
			}

			await ExecutionContext.Execute(ReadSettings, customCancellationToken: cancellationToken);
		}

		public async Task Save()
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

			await TryCloseAsync();
		}

		private void ShowDiagnosticMessages()
		{
			IsDiagnosticMessagesVisible = true;
		}

		public void HideDiagnosticMessages()
		{
			IsDiagnosticMessagesVisible = false;
		}

		public async Task Cancel()
		{
			await TryCloseAsync();
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

			await ExecutionContext.WindowManager.ShowWindowAsync(this, null, settings);
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