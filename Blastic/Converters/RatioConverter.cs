using System;
using System.Globalization;
using System.Windows.Data;

namespace Blastic.Converters
{
	public class RatioConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length != 2)
			{
				throw new ArgumentException(nameof(values));
			}

			double first = GetValue(values[0]);
			double second = GetValue(values[1]);

			return first / second;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private double GetValue(object value)
		{
			switch (value)
			{
				case int i:
					return i;
				case long l:
					return l;
				case float f:
					return f;
				case double d:
					return d;
			}

			throw new ArgumentException(nameof(value));
		}
	}
}