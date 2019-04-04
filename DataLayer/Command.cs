using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WpfTemplate.DataLayer.ProviderSpecific;

namespace WpfTemplate.DataLayer
{
	public class Command : IDisposable
	{
		private readonly DbCommand _command;
		private readonly Connection _connection;
		private readonly ILogger _logger;

		public string CommandText
		{
			get => _command.CommandText;
			set => _command.CommandText = value;
		}

		public Command(DbCommand command, Connection connection, ILogger logger)
		{
			_command = command;
			_connection = connection;
			_logger = logger;

			// In Azure Db connections the 30 seconds timeout occurs frequently.
			_command.CommandTimeout = (int)TimeSpan.FromHours(1).TotalSeconds;
		}

		public void AddParameterWithValue(string name, object value)
		{
			DbParameter parameter = _command.CreateParameter();
			SetParameter(parameter, value);
			
			parameter.ParameterName = name;
			_command.Parameters.Add(parameter);
		}

		public void AddParameter(string name)
		{
			AddParameterWithValue(name, null);
		}

		public void SetParameter(string name, object value)
		{
			DbParameter parameter = _command.Parameters[name];
			SetParameter(parameter, value);
		}

		public void SetParameter(int index, object value)
		{
			DbParameter parameter = _command.Parameters[index];
			SetParameter(parameter, value);
		}

		public void SetParameter(DbParameter parameter, object value)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}

			if (value is DateTime dateTime)
			{
				value = dateTime.ToFileTimeUtc();
			}

			if (DataReader.IsListOfEnums(value))
			{
				IEnumerable<object> list = ((IList) value).Cast<object>();
				value = string.Join(DataReader.ListSeparator, list.Select(x => (int) x));
			}

			parameter.Value = value;
			FixParameterScale(parameter);
		}

		public void ClearParameters()
		{
			_command.Parameters.Clear();
		}

		public void RemoveExcessParameters(int requiredParameterCount)
		{
			for (int i = _command.Parameters.Count - 1; i >= requiredParameterCount; i--)
			{
				_command.Parameters.RemoveAt(i);
			}
		}

		private void FixParameterScale(DbParameter parameter)
		{
			if (parameter.Value is DateTime)
			{
				// We set the scale property so that Azure connection does not throw datetime precision error.
				((IDbDataParameter)parameter).Scale = 3;
			}
		}

		public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
		{
			return await ExecuteAndLogException(async () => await _command.ExecuteNonQueryAsync(cancellationToken));
		}

		public async Task<T> ExecuteScalarAsync<T>(CancellationToken cancellationToken)
		{
			object result = await ExecuteAndLogException(async () => await _command.ExecuteScalarAsync(cancellationToken));
			return DataReader.SafeCast<T>(result);
		}

		public async Task<DataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
		{
			DbDataReader reader = await ExecuteAndLogException(async () => await _command.ExecuteReaderAsync(cancellationToken));
			return new DataReader(reader);
		}

		public void Dispose()
		{
			_command.Dispose();
		}

		private async Task<T> ExecuteAndLogException<T>(Func<Task<T>> function)
		{
			try
			{
				FixProviderSpecificQuery();
				return await function();
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Error while executing database command.");
				throw;
			}
		}

		private void FixProviderSpecificQuery()
		{
			string providerSpecificCommandText = _command.CommandText;

			foreach (KeyValuePair<string, string> pair in Placeholders.ProviderSpecificQueries[_connection.Provider])
			{
				string key = pair.Key;
				string value = pair.Value;

				providerSpecificCommandText = providerSpecificCommandText.Replace(key, value);
			}

			_command.CommandText = providerSpecificCommandText;
		}
	}
}