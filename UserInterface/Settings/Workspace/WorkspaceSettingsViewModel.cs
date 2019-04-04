using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using PropertyChanged;
using WpfTemplate.Diagnostics;
using WpfTemplate.Execution;
using WpfTemplate.Services.Settings;

namespace WpfTemplate.UserInterface.Settings.Workspace
{
	[SingleInstance(AsImplementedInterface = true)]
	[AddINotifyPropertyChangedInterface]
	public class WorkspaceSettingsViewModel : SettingsSectionViewModel, IDataErrorInfo
	{
		private const string WorkspaceFolderKey = "WorkspaceFolder";

		public const string WorkspaceFolderHelp = "Workspace folder contains the internal resources and the projects " +
		                                          "you have created. You can select an empty folder initially. " +
		                                          "Then, you can fetch the internal resources if you have version control " +
		                                          "credentials. Otherwise, you have to put internal resources " +
		                                          "to the workspace manually.";

		private readonly ISettingsService _settingsService;

		public override string SectionName => "Workspace";
		public string Path { get; set; }

		public WorkspaceSettingsViewModel(
			ExecutionContextFactory executionContextFactory,
			ISettingsService settingsService)
			:
			base(executionContextFactory)
		{
			_settingsService = settingsService;
		}

		public override IEnumerable<DiagnosticMessage> GetDiagnosticMessages()
		{
			string error = this[nameof(Path)];

			if (string.IsNullOrEmpty(error))
			{
				return Enumerable.Empty<DiagnosticMessage>();
			}

			return new[] { new DiagnosticMessage(Severity.Error, error) };
		}

		public override async Task Save(CancellationToken cancellationToken)
		{
			await _settingsService.Put(WorkspaceFolderKey, Path, cancellationToken);
		}

		public override async Task ReadSettings(CancellationToken cancellationToken)
		{
			if (!await _settingsService.Contains(WorkspaceFolderKey, cancellationToken))
			{
				await _settingsService.Put(WorkspaceFolderKey, "", cancellationToken);
			}

			Path = await _settingsService.Get(WorkspaceFolderKey, cancellationToken);
		}

		public string this[string name]
		{
			get
			{
				if (name != nameof(Path))
				{
					return null;
				}

				if (Directory.Exists(Path))
				{
					return "";
				}

				return "Workspace directory does not exist.";
			}
		}

		public string Error => null;
	}
}