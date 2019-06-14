using System;
using System.Collections.Generic;

namespace Blastic.Common
{

	public class Order : IEquatable<Order>, IComparable<Order>, IComparable
	{
		private readonly List<int> _numbers;

		public IReadOnlyList<int> Numbers => _numbers;

		public Order(params int[] numbers)
		{
			_numbers = new List<int>(numbers);
		}

		public bool Equals(Order other)
		{
			return CompareTo(other) == 0;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Order)obj);
		}

		public override int GetHashCode()
		{
			return _numbers.GetHashCode();
		}

		public static bool operator ==(Order left, Order right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Order left, Order right)
		{
			return !Equals(left, right);
		}

		public int CompareTo(Order other)
		{
			int maxCount = _numbers.Count < other._numbers.Count
				? _numbers.Count
				: other._numbers.Count;

			for (int i = 0; i < maxCount; i++)
			{
				int left = i < _numbers.Count
					? _numbers[i]
					: 0;

				int right = i < other._numbers.Count
					? other._numbers[i]
					: 0;

				if (left < right)
				{
					return -1;
				}

				if (left > right)
				{
					return 1;
				}
			}

			return 0;
		}

		public int CompareTo(object obj)
		{
			if (ReferenceEquals(null, obj)) return 1;
			if (ReferenceEquals(this, obj)) return 0;

			return obj is Order other
				? CompareTo(other)
				: throw new ArgumentException($"Object must be of type {nameof(Order)}");
		}

		public static bool operator <(Order left, Order right)
		{
			return Comparer<Order>.Default.Compare(left, right) < 0;
		}

		public static bool operator >(Order left, Order right)
		{
			return Comparer<Order>.Default.Compare(left, right) > 0;
		}

		public static bool operator <=(Order left, Order right)
		{
			return Comparer<Order>.Default.Compare(left, right) <= 0;
		}

		public static bool operator >=(Order left, Order right)
		{
			return Comparer<Order>.Default.Compare(left, right) >= 0;
		}
	}
}