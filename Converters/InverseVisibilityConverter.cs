using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfTemplate.Converters
{
	public class InverseVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility visibility)
			{
				return visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
			}
			
			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}