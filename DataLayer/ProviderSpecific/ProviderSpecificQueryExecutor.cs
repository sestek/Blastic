using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WpfTemplate.DataLayer.ProviderSpecific
{
	public class ProviderSpecificQueryExecutor
	{
		// This threshold tries to ensure that all databases execute the multiple inserts successfully.
		// In SQLite there is a parameter count limit of 999. Other database providers have maximum
		// limit greater than this.
		private const int Threshold = 999;

		public static async Task ExecuteWithParameterThrottlingAsync<T>(
			Connection connection,
			string baseCommandText,
			string loopCommandText,
			IReadOnlyCollection<T> enumerateOn,
			Action<Command> addBaseParameters,
			Action<Command, int> addLoopParameters,
			Action<Command> setBaseParameters,
			Action<Command, T, int> setLoopParameters,
			Func<Command, Task> executeCommand,
			Action<Command> finalizeCommand)
		{
			using (Command command = connection.CreateCommand())
			{
				int baseParameterCount = baseCommandText.Count(x => x == '@');
				int loopParameterCount = loopCommandText.Count(x => x == '@');

				int commitThreshold = Threshold - baseParameterCount;
				commitThreshold /= Math.Max(loopParameterCount, 1);

				int counter = 0;
				int lastCounter = 0;
				
				addBaseParameters(command);

				for (int i = 0; i < commitThreshold; i++)
				{
					if (i >= enumerateOn.Count)
					{
						break;
					}

					addLoopParameters(command, i);
				}

				setBaseParameters(command);

				async Task Commit()
				{
					// ReSharper disable AccessToDisposedClosure
					// ReSharper disable AccessToModifiedClosure
					if (counter != lastCounter)
					{
						command.CommandText = baseCommandText;

						for (int i = 0; i < counter; i++)
						{
							command.CommandText += loopCommandText.Replace("?", i.ToString());
						}

						finalizeCommand(command);
					}

					await executeCommand(command);

					setBaseParameters(command);

					lastCounter = counter;
					counter = 0;
					// ReSharper restore AccessToModifiedClosure
					// ReSharper restore AccessToDisposedClosure
				}

				foreach (T entry in enumerateOn)
				{
					if (counter == commitThreshold)
					{
						await Commit();
					}

					int parameterIndex = baseParameterCount + loopParameterCount * counter;

					setLoopParameters(command, entry, parameterIndex);

					counter++;
				}

				if (counter > 0)
				{
					int requiredParameterCount = baseParameterCount + counter * loopParameterCount;

					command.RemoveExcessParameters(requiredParameterCount);
					await Commit();
				}
			}
		}

		public static async Task<DataReader> ExecuteWithPaginationAsync(
			Connection connection,
			Command command,
			int skipParameter,
			int takeParameter,
			CancellationToken cancellationToken)
		{
			command.CommandText += Placeholders.ProviderSpecificQueries[connection.Provider][Placeholders.PaginationKey];

			command.CommandText = command.CommandText.Replace(
				Placeholders.PaginationOffsetPlaceholder,
				skipParameter.ToString());

			command.CommandText = command.CommandText.Replace(
				Placeholders.PaginationLimitPlaceholder,
				takeParameter.ToString());

			return await command.ExecuteReaderAsync(cancellationToken);
		}

		public static async Task DropIndexAsync(
			Connection connection,
			string tableName,
			string indexName,
			CancellationToken cancellationToken)
		{
			string commandText = Placeholders.ProviderSpecificQueries[connection.Provider][Placeholders.DropIndexKey];

			commandText = commandText.Replace(Placeholders.DropIndexTableNamePlaceholder, tableName);
			commandText = commandText.Replace(Placeholders.DropIndexIndexNamePlaceholder, indexName);

			await ExecuteNonQueryAsync(connection, commandText, cancellationToken);
		}

		public static async Task<bool> DoesTableExistAsync(
			Connection connection,
			string tableName,
			CancellationToken cancellationToken)
		{
			string commandText = Placeholders.ProviderSpecificQueries[connection.Provider][Placeholders.TableExistsKey];

			commandText = commandText.Replace(Placeholders.TableExistsTableNamePlaceholder, tableName);

			using (Command command = connection.CreateCommand())
			{
				command.CommandText = commandText;
				int count = await command.ExecuteScalarAsync<int>(cancellationToken);

				return count > 0;
			}
		}

		public static async Task AlterCollationAsync(
			Connection connection,
			string tableName,
			string columnName,
			string dataType,
			CancellationToken cancellationToken)
		{
			string commandText = Placeholders.ProviderSpecificQueries[connection.Provider][Placeholders.AlterCollationKey];

			commandText = commandText.Replace(Placeholders.AlterCollationTableNamePlaceholder, tableName);
			commandText = commandText.Replace(Placeholders.AlterCollationColumnNamePlaceholder, columnName);
			commandText = commandText.Replace(Placeholders.AlterCollationDataTypePlaceholder, dataType);

			await ExecuteNonQueryAsync(connection, commandText, cancellationToken);
		}

		public static async Task<int> ExecuteAndGetInsertedRowIdAsync(
			Connection connection,
			Command command,
			string tableName,
			CancellationToken cancellationToken)
		{
			command.CommandText = $"{command.CommandText.TrimEnd(';', ' ')};{Placeholders.ProviderSpecificQueries[connection.Provider][Placeholders.InsertedRowIdKey]}";
			command.CommandText = command.CommandText.Replace(
				Placeholders.InsertedRowIdTableNamePlaceholder,
				tableName);

			return await command.ExecuteScalarAsync<int>(cancellationToken);
		}

		private static async Task ExecuteNonQueryAsync(
			Connection connection,
			string commandText,
			CancellationToken cancellationToken)
		{
			using (Command command = connection.CreateCommand())
			{
				command.CommandText = commandText;
				await command.ExecuteNonQueryAsync(cancellationToken);
			}
		}
	}
}