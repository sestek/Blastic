﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Blastic.Converters
{
	public class VisibilityAndConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			foreach (object value in values)
			{
				if ((Visibility)value == Visibility.Collapsed || (Visibility)value == Visibility.Hidden)
				{
					return Visibility.Collapsed;
				}
			}

			return Visibility.Visible;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}