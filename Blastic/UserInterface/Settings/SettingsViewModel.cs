using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using PropertyChanged;
using Blastic.Caliburn;
using Blastic.Diagnostics;
using Blastic.Execution;

namespace Blastic.UserInterface.Settings
{
	[AddINotifyPropertyChangedInterface]
	public sealed class SettingsViewModel : ScreenBase
	{
		private bool _hasReadSettings;
		private Window _activeWindow;

		public IObservableCollection<ISettingsSectionViewModel> Items { get; set; }
		public IObservableCollection<DiagnosticMessage> DiagnosticMessages { get; set; }

		public TaskCompletionSource<bool> ShowDiagnosticMessagesTaskCompletionSource { get; set; }
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

		protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
		{
			await ReadSettings(cancellationToken);
		}
		
		public async Task ReadSettings(CancellationToken cancellationToken)
		{
			if (_hasReadSettings)
			{
				return;
			}

			async Task ReadSettings(CancellationToken token)
			{
				foreach (ISettingsSectionViewModel item in Items)
				{
					await ((IScreen)item).ActivateAsync(cancellationToken);
					await item.ReadSettings(token);
				}
			}

			await ExecutionContext.Execute(ReadSettings, customCancellationToken: cancellationToken);

			_hasReadSettings = true;
		}

		public async void Save()
		{
			async Task Check(CancellationToken cancellationToken)
			{
				DiagnosticMessages.Clear();

				foreach (ISettingsSectionViewModel item in Items)
				{
					IEnumerable<DiagnosticMessage> diagnosticMessages = await item.GetDiagnosticMessages(cancellationToken);
					DiagnosticMessages.AddRange(diagnosticMessages);
				}
			}

			await ExecutionContext.Execute(Check, "Validating settings.");

			if (DiagnosticMessages.Any(x => x.Severity >= Severity.Warning))
			{
				bool shouldContinue = await ShowDiagnosticMessages();

				if (!shouldContinue)
				{
					return;
				}
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

		private Task<bool> ShowDiagnosticMessages()
		{
			ShowDiagnosticMessagesTaskCompletionSource = new TaskCompletionSource<bool>();
			IsDiagnosticMessagesVisible = true;

			return ShowDiagnosticMessagesTaskCompletionSource.Task;
		}

		public void HideDiagnosticMessages()
		{
			IsDiagnosticMessagesVisible = false;
			ShowDiagnosticMessagesTaskCompletionSource.SetResult(false);
		}

		public void HideDiagnosticMessagesIgnoreErrors()
		{
			IsDiagnosticMessagesVisible = false;
			ShowDiagnosticMessagesTaskCompletionSource.SetResult(true);
		}

		public async Task Cancel()
		{
			await TryCloseAsync();

			foreach (ISettingsSectionViewModel item in Items)
			{
				item.Revert();
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