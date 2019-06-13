using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfTemplate.Converters
{
	public class IntToByteSizeConverter : IValueConverter
	{
		private const long OneKb = 1024;
		private const long OneMb = OneKb * 1024;
		private const long OneGb = OneMb * 1024;
		private const long OneTb = OneGb * 1024;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is long l)
			{
				return ToPrettySize(l);
			}

			if (value is int i)
			{
				return ToPrettySize(i);
			}

			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public static string ToPrettySize(long value, int decimalPlaces = 2)
		{
			double asTb = Math.Round((double)value / OneTb, decimalPlaces);
			double asGb = Math.Round((double)value / OneGb, decimalPlaces);
			double asMb = Math.Round((double)value / OneMb, decimalPlaces);
			double asKb = Math.Round((double)value / OneKb, decimalPlaces);

			string chosenValue
				= asTb > 1 ? $"{asTb}Tb"
				: asGb > 1 ? $"{asGb}Gb"
				: asMb > 1 ? $"{asMb}Mb"
				: asKb > 1 ? $"{asKb}Kb"
				: $"{Math.Round((double) value, decimalPlaces)}B";

			return chosenValue;
		}
	}
}