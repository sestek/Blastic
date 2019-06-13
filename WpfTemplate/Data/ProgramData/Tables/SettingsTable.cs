using System.Threading;
using System.Threading.Tasks;
using WpfTemplate.DataLayer;
using WpfTemplate.DataLayer.Tables;

namespace WpfTemplate.Data.ProgramData.Tables
{
	public class SettingsTable : TableBase
	{
		public SettingsTable(ConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		public async Task<string> Get(string key, CancellationToken cancellationToken)
		{
			using (Connection connection = ConnectionFactory.CreateConnection())
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = "SELECT * FROM Settings WHERE Key=@Key";
				command.AddParameterWithValue("@Key", key);

				using (DataReader reader = await command.ExecuteReaderAsync(cancellationToken))
				{
					if (!reader.Read())
					{
						return null;
					}

					return reader.Get<string>("Value");
				}
			}
		}

		public async Task Put(string key, string value, CancellationToken cancellationToken)
		{
			using (Connection connection = ConnectionFactory.CreateConnection())
			{
				if (await Contains(connection, key, cancellationToken))
				{
					await Update(connection, key, value, cancellationToken);
				}
				else
				{
					await Insert(connection, key, value, cancellationToken);
				}
			}
		}

		public async Task<bool> Contains(string key, CancellationToken cancellationToken)
		{
			using (Connection connection = ConnectionFactory.CreateConnection())
			{
				return await Contains(connection, key, cancellationToken);
			}
		}

		private async Task<bool> Contains(Connection connection, string key, CancellationToken cancellationToken)
		{
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = "SELECT 1 FROM Settings WHERE Key=@Key";
				command.AddParameterWithValue("@Key", key);

				using (DataReader reader = await command.ExecuteReaderAsync(cancellationToken))
				{
					return reader.HasRows;
				}
			}
		}

		public async Task Delete(string key, CancellationToken cancellationToken)
		{
			using (Connection connection = ConnectionFactory.CreateConnection())
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = @"DELETE FROM Settings WHERE Key=@Key";
				command.AddParameterWithValue("@Key", key);

				await command.ExecuteNonQueryAsync(cancellationToken);
			}
		}

		private async Task Insert(Connection connection, string key, string value, CancellationToken cancellationToken)
		{
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = @"INSERT INTO Settings (Key, Value) VALUES (@Key, @Value)";
				
				command.AddParameterWithValue("@Key", key);
				command.AddParameterWithValue("@Value", value);

				await command.ExecuteNonQueryAsync(cancellationToken);
			}
		}

		private async Task Update<T>(Connection connection, string key, T value, CancellationToken cancellationToken)
		{
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = @"UPDATE Settings SET Value=@Value WHERE Key=@Key";
				
				command.AddParameterWithValue("@Value", value);
				command.AddParameterWithValue("@Key", key);

				await command.ExecuteNonQueryAsync(cancellationToken);
			}
		}
	}
}