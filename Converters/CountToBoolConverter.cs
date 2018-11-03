using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Yaver.Host.Wpf.Converters
{
	public class CountToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int i)
			{
				return i != 0;
			}

			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}