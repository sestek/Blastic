using System.Threading;
using System.Threading.Tasks;
using Blastic.Common;
using Blastic.Data.ProgramData;
using Blastic.Initialization.Steps;
using Microsoft.Extensions.Logging;

namespace Blastic.Data.Initialization.Steps
{
	public class MigrateProgramDatabaseStep : IInitializationStep
	{
		public static readonly Order Order = new Order(int.MinValue);

		private readonly ProgramDatabase _programDatabase;
		private readonly ILogger<MigrateProgramDatabaseStep> _logger;

		Order IInitializationStep.Order => Order;

		public string Description { get; }
		public string SuccessMessage { get; }
		public string FailureMessage { get; }

		public bool ShowBusyIndicator => true;

		public MigrateProgramDatabaseStep(
			ProgramDatabase programDatabase,
			ILogger<MigrateProgramDatabaseStep> logger)
		{
			_programDatabase = programDatabase;
			_logger = logger;

			Description = "Migrating database...";
			SuccessMessage = "";
			FailureMessage = "Cannot migrate database. Program might behave incorrectly.";
		}

		public async Task Initialize(CancellationToken cancellationToken)
		{
			_logger.LogDebug("Checking and applying migrations.");
			await _programDatabase.MigrateAsync(cancellationToken);
			_logger.LogDebug("Finished checking and applying migrations.");
		}
	}
}