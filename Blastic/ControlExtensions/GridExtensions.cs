using System;
using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Blastic.ControlExtensions
{
	public class GridExtensions
	{
		[AttachedProperty(OnPropertyChanged = nameof(OnRowDefinitionsChanged))]
		public static string RowDefinitions { get; set; }

		public static void SetRowDefinitions(DependencyObject obj, string value)
		{
		}

		private static void OnRowDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Grid targetGrid = d as Grid;
			string rows = e.NewValue as string;

			if (targetGrid == null || rows == null)
			{
				return;
			}

			targetGrid.RowDefinitions.Clear();
			string[] rowDefinitions = rows.Split(',');

			foreach (string rowDefinition in rowDefinitions)
			{
				if (rowDefinition.Trim() == "")
				{
					targetGrid.RowDefinitions.Add(new RowDefinition());
				}
				else
				{
					targetGrid.RowDefinitions.Add(new RowDefinition
					{
						Height = ParseLength(rowDefinition)
					});
				}
			}
		}

		[AttachedProperty(OnPropertyChanged = nameof(OnColumnDefinitionsChanged))]
		public static string ColumnDefinitions { get; set; }

		public static void SetColumnDefinitions(DependencyObject obj, string value)
		{
		}

		private static void OnColumnDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Grid targetGrid = d as Grid;
			string columns = e.NewValue as string;

			if (targetGrid == null || columns == null)
			{
				return;
			}

			targetGrid.ColumnDefinitions.Clear();
			string[] columnDefinitions = columns.Split(',');

			foreach (string columnDefinition in columnDefinitions)
			{
				string[] tokens = columnDefinition.Trim().Split(':');

				if (tokens.Length < 1 || tokens.Length > 2)
				{
					throw new ArgumentException($"Invalid column definition: {columnDefinition}");
				}

				string size = tokens[0];
				string group = tokens.Length > 1 ? tokens[1] : null;

				if (size.Trim() == "")
				{
					targetGrid.ColumnDefinitions.Add(new ColumnDefinition
					{
						SharedSizeGroup = group
					});
				}
				else
				{
					targetGrid.ColumnDefinitions.Add(new ColumnDefinition
					{
						Width = ParseLength(size),
						SharedSizeGroup = group
					});
				}
			}
		}

		private static GridLength ParseLength(string length)
		{
			length = length.Trim();

			if (length.ToLowerInvariant().Equals("auto"))
			{
				return new GridLength(0, GridUnitType.Auto);
			}
			if (length.Contains("*"))
			{
				length = length.Replace("*", "");

				if (string.IsNullOrEmpty(length))
				{
					length = "1";
				}

				return new GridLength(double.Parse(length), GridUnitType.Star);
			}

			return new GridLength(double.Parse(length), GridUnitType.Pixel);
		}
	}
}