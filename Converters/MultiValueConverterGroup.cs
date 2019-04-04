using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfTemplate.Converters
{
	public class MultiValueConverterGroup : List<object>, IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			object result = DependencyProperty.UnsetValue;

			object converterInput = values;
			object[] multiConverterInput = values;

			for (int i = 0; i < Count; i++)
			{
				object converter = this[i];
				object nextConverter = i < Count - 1 ? this[i + 1] : null;

				switch (converter)
				{
					case IMultiValueConverter multiValueConverter:
						result = multiValueConverter.Convert(multiConverterInput, targetType, parameter, culture);
						break;
					case IValueConverter valueConverter:
						result = valueConverter.Convert(converterInput, targetType, parameter, culture);
						break;
					default:
						throw new ArgumentException(nameof(converter));
				}
					
				switch (nextConverter)
				{
					case IMultiValueConverter _ when result is object[] array:
						multiConverterInput = array;
						break;
					case IMultiValueConverter _:
						multiConverterInput = new[] { result };
						break;
					case IValueConverter _:
						converterInput = result;
						break;
				}
			}

			return result;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}