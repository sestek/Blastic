using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace WpfTemplate.DataLayer
{
	public class DataReader : IDisposable
	{
		public const string ListSeparator = ";";

		private readonly DbDataReader _dataReader;

		public bool HasRows => _dataReader.HasRows;

		public DataReader(DbDataReader dataReader)
		{
			_dataReader = dataReader;
		}

		public bool Read()
		{
			return _dataReader.Read();
		}

		public T Get<T>(string name)
		{
			return SafeCast<T>(_dataReader[name]);
		}

		public List<T> GetEnumList<T>(string name)
		{
			return SafeCastEnumList<T>(_dataReader[name]);
		}

		public T Get<T>(int index)
		{
			return SafeCast<T>(_dataReader[index]);
		}

		internal static T SafeCast<T>(object value)
		{
			if (value == null)
			{
				return default;
			}

			if (value == DBNull.Value)
			{
				return default;
			}

			if ((typeof(T) == typeof(int) || typeof(T).IsEnum) && value is long l)
			{
				return (T)(object)(int)l;
			}

			if (typeof(T) == typeof(int) && value is decimal d)
			{
				return (T)(object)(int)d;
			}

			if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
			{
				return (T)(object)DateTime.FromFileTimeUtc((long)value);
			}

			return (T)value;
		}

		private static List<T> SafeCastEnumList<T>(object value)
		{
			if (value == DBNull.Value)
			{
				return null;
			}

			if (!IsListOfEnums(typeof(List<T>)))
			{
				throw new ArgumentException("Return type should be a list of enums.");
			}

			string valueAsString = (string) value;
			string[] tokens = valueAsString.Split(new[] { ListSeparator }, StringSplitOptions.RemoveEmptyEntries);

			List<T> result = new List<T>();

			foreach (string token in tokens)
			{
				int valueAsInt = int.Parse(token);
				T enumValue = (T)(object)valueAsInt;

				result.Add(enumValue);
			}

			return result;
		}

		public void Dispose()
		{
			_dataReader.Dispose();
		}

		public static bool IsListOfEnums(object value)
		{
			return IsListOfEnums(value.GetType());
		}

		public static bool IsListOfEnums(Type type)
		{
			return
				type.IsGenericType
				&& type.GetGenericTypeDefinition() == typeof(List<>)
				&& type.GetGenericArguments().All(x => x.IsEnum);
		}
	}
}