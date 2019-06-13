using System.Collections.Generic;

namespace Blastic.DataLayer.Tables
{
	public abstract class TableBase
	{
		public const string ListSeparator = ";";

		public ConnectionFactory ConnectionFactory { get; }

		protected TableBase(ConnectionFactory connectionFactory)
		{
			ConnectionFactory = connectionFactory;
		}

		protected void AddParameterIfChanged<T>(Command command, string columnName, T newValue)
		{
			if (EqualityComparer<T>.Default.Equals(newValue, default(T)))
			{
				return;
			}

			command.CommandText += $" {columnName}=@{columnName},";
			command.AddParameterWithValue($"@{columnName}", newValue);
		}
	}
}