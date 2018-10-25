using System;
using System.Globalization;
using System.Windows.Data;

namespace Yaver.Host.Wpf.Converters
{
	public class BooleanAndConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			foreach (object value in values)
			{
				if ((value as bool?) != true)
				{
					return false;
				}
			}

			return true;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}