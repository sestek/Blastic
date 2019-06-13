using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfTemplate.Converters
{
	public class CountToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int count)
			{
				int toCompare = parameter as int? ?? 0;

				return count > toCompare ? Visibility.Visible : Visibility.Collapsed;
			}

			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}