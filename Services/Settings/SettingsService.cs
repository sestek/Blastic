using System.Threading;
using System.Threading.Tasks;
using Autofac;
using WpfTemplate.Data.ProgramData;

namespace WpfTemplate.Services.Settings
{
	[SingleInstance]
	public class SettingsService : ISettingsService
	{
		private readonly ProgramDatabase _database;

		public SettingsService(ProgramDatabase database)
		{
			_database = database;
		}

		public async Task<bool> Contains(string key, CancellationToken cancellationToken = default)
		{
			return await _database.SettingsTable.Contains(key, cancellationToken);
		}

		public async Task<string> Get(string key, CancellationToken cancellationToken = default)
		{
			return await _database.SettingsTable.Get(key, cancellationToken);
		}

		public async Task Put(string key, string value, CancellationToken cancellationToken = default)
		{
			await _database.SettingsTable.Put(key, value, cancellationToken);
		}

		public async Task Delete(string key, CancellationToken cancellationToken = default)
		{
			await _database.SettingsTable.Delete(key, cancellationToken);
		}
	}
}