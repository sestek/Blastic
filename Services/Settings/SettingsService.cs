using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using WpfTemplate.Data.ProgramData;

namespace WpfTemplate.Services.Settings
{
	[SingleInstance(AsImplementedInterface = true)]
	public class SettingsService : ISettingsService
	{
		private readonly ProgramDatabase _database;

		public SettingsService(ProgramDatabase database)
		{
			_database = database;
		}

		public async Task<bool> Contains(string key, CancellationToken cancellationToken)
		{
			return await _database.SettingsTable.Contains(key, cancellationToken);
		}

		public async Task<T> Get<T>(string key, T defaultValue, CancellationToken cancellationToken)
		{
			string serializedData = await _database.SettingsTable.Get(key, cancellationToken);

			if (serializedData == null)
			{
				return defaultValue;
			}

			return JsonConvert.DeserializeObject<T>(serializedData);
		}

		public async Task Put<T>(string key, T value, CancellationToken cancellationToken)
		{
			string serializedData = JsonConvert.SerializeObject(value);
			await _database.SettingsTable.Put(key, serializedData, cancellationToken);
		}

		public async Task Delete(string key, CancellationToken cancellationToken)
		{
			await _database.SettingsTable.Delete(key, cancellationToken);
		}
	}
}