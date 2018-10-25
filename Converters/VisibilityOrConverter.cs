using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Yaver.Host.Wpf.Converters
{
	public class VisibilityOrConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			foreach (object value in values)
			{
				if ((Visibility)value == Visibility.Visible)
				{
					return Visibility.Visible;
				}
			}

			return Visibility.Collapsed;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}