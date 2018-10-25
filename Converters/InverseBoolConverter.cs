using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Yaver.Host.Wpf.Converters
{
	public class InverseBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return DependencyProperty.UnsetValue;
			}

			return !(bool) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return DependencyProperty.UnsetValue;
			}

			return !(bool)value;
		}
	}
}