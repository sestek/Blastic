using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Blastic.DataLayer.Context
{
	public class AmbientContext<T> where T : class
	{
		private static readonly ConditionalWeakTable<string, T> ContextTable;

		// ReSharper disable once StaticMemberInGenericType
		// We really want to create a static for each generic type.
		private static readonly AsyncLocal<string> Storage = new AsyncLocal<string>();

		static AmbientContext()
		{
			ContextTable = new ConditionalWeakTable<string, T>();
		}
		
		public AmbientContext()
		{
			string crossReferenceKey = Storage.Value;

			if (crossReferenceKey != null)
			{
				return;
			}

			crossReferenceKey = Guid.NewGuid().ToString("N");
			Storage.Value = crossReferenceKey;
		}
		
		public T Get()
		{
			string crossReferenceKey = Storage.Value;
			ContextTable.TryGetValue(crossReferenceKey, out T value);

			return value;
		}
		
		public void Save(T value)
		{
			string crossReferenceKey = Storage.Value;

			if (ContextTable.TryGetValue(crossReferenceKey, out _))
			{
				ContextTable.Remove(crossReferenceKey);
			}

			ContextTable.Add(crossReferenceKey, value);
		}
	}
}