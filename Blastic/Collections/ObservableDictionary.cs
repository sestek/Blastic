using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Blastic.Collections
{
	public class ObservableDictionary<TKey, TValue>
		:
		IDictionary<TKey, TValue>,
		INotifyCollectionChanged,
		INotifyPropertyChanged
	{
		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		protected IDictionary<TKey, TValue> Dictionary { get; private set; }

		public ICollection<TKey> Keys => Dictionary.Keys;
		public ICollection<TValue> Values => Dictionary.Values;

		public int Count => Dictionary.Count;
		public bool IsReadOnly => Dictionary.IsReadOnly;

		public ObservableDictionary()
		{
			Dictionary = new Dictionary<TKey, TValue>();
		}

		public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
		{
			Dictionary = new Dictionary<TKey, TValue>(dictionary);
		}

		public ObservableDictionary(IEqualityComparer<TKey> comparer)
		{
			Dictionary = new Dictionary<TKey, TValue>(comparer);
		}

		public ObservableDictionary(int capacity)
		{
			Dictionary = new Dictionary<TKey, TValue>(capacity);
		}

		public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			Dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
		}

		public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			Dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
		}

		public void Add(TKey key, TValue value)
		{
			Insert(key, value, true);
		}

		public bool ContainsKey(TKey key)
		{
			return Dictionary.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			bool removed = Dictionary.Remove(key);

			if (removed)
			{
				OnCollectionChanged();
			}

			return removed;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return Dictionary.TryGetValue(key, out value);
		}

		public TValue this[TKey key]
		{
			get => Dictionary[key];
			set => Insert(key, value, false);
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Insert(item.Key, item.Value, true);
		}

		public void Clear()
		{
			if (Dictionary.Count <= 0)
			{
				return;
			}

			Dictionary.Clear();
			OnCollectionChanged();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return Dictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			Dictionary.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return Remove(item.Key);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return Dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Dictionary).GetEnumerator();
		}

		public void AddRange(IDictionary<TKey, TValue> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException(nameof(items));
			}

			if (items.Count <= 0)
			{
				return;
			}

			if (Dictionary.Count > 0)
			{
				if (items.Keys.Any(k => Dictionary.ContainsKey(k)))
				{
					throw new ArgumentException("An item with the same key has already been added.");
				}

				foreach (KeyValuePair<TKey, TValue> item in items)
				{
					Dictionary.Add(item);
				}
			}
			else
			{
				Dictionary = new Dictionary<TKey, TValue>(items);
			}

			OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray());
		}

		private void Insert(TKey key, TValue value, bool add)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (Dictionary.TryGetValue(key, out TValue item))
			{
				if (add)
				{
					throw new ArgumentException("An item with the same key has already been added.");
				}

				if (Equals(item, value))
				{
					return;
				}

				Dictionary[key] = value;

				OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, item));
			}
			else
			{
				Dictionary[key] = value;

				OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
			}
		}

		private void OnPropertyChanged()
		{
			OnPropertyChanged(nameof(Count));
			OnPropertyChanged("Item[]");
			OnPropertyChanged(nameof(Keys));
			OnPropertyChanged(nameof(Values));
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void OnCollectionChanged()
		{
			OnPropertyChanged();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void OnCollectionChanged(
			NotifyCollectionChangedAction action,
			KeyValuePair<TKey, TValue> changedItem)
		{
			OnPropertyChanged();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
		}

		private void OnCollectionChanged(
			NotifyCollectionChangedAction action,
			KeyValuePair<TKey, TValue> newItem,
			KeyValuePair<TKey, TValue> oldItem)
		{
			OnPropertyChanged();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
		}

		private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
		{
			OnPropertyChanged();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItems));
		}
	}
}