using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Yaver.Host.Wpf.ControlExtensions
{
	public class DataGridColumnSizeExtensions
	{
		[AttachedProperty(OnPropertyChanged = nameof(OnFixColumnFillSizeChanged))]
		public static bool FixColumnFillSize { get; set; }

		public static bool GetFixColumnFillSize(DependencyObject element) => throw new WillBeImplementedByBindablesException();
		public static void SetFixColumnFillSize(DependencyObject element, bool value) { }

		[AttachedProperty]
		public static INotifyCollectionChanged DataGridItemsSource { get; set; }

		public static INotifyCollectionChanged GetDataGridItemsSource(DependencyObject element) => throw new WillBeImplementedByBindablesException();
		public static void SetDataGridItemsSource(DependencyObject element, INotifyCollectionChanged value) { }

		[AttachedProperty]
		public static NotifyCollectionChangedEventHandler CollectionChangedAction { get; set; }

		public static NotifyCollectionChangedEventHandler GetCollectionChangedAction(DependencyObject element) => throw new WillBeImplementedByBindablesException();
		public static void SetCollectionChangedAction(DependencyObject element, NotifyCollectionChangedEventHandler value) { }

		private static void OnFixColumnFillSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = d as DataGrid;

			if (dataGrid == null)
			{
				return;
			}

			DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));

			if ((e.NewValue as bool?) == true)
			{
				descriptor?.AddValueChanged(dataGrid, OnItemsSourceChanged);
			}
			else
			{
				descriptor?.RemoveValueChanged(dataGrid, OnItemsSourceChanged);
			}
		}

		private static void OnItemsSourceChanged(object sender, EventArgs e)
		{
			DataGrid dataGrid = sender as DataGrid;

			if (dataGrid == null)
			{
				return;
			}

			INotifyCollectionChanged oldItemsSource = GetDataGridItemsSource(dataGrid);

			if (oldItemsSource != null)
			{
				NotifyCollectionChangedEventHandler collectionChangedAction = GetCollectionChangedAction(dataGrid);
				oldItemsSource.CollectionChanged -= collectionChangedAction;
			}

			INotifyCollectionChanged observableCollection = dataGrid.ItemsSource as INotifyCollectionChanged;

			if (observableCollection == null)
			{
				return;
			}


			void Action(object o, NotifyCollectionChangedEventArgs a)
			{
				FixColumnSizes(dataGrid);
			}

			SetCollectionChangedAction(dataGrid, Action);
			observableCollection.CollectionChanged += Action;

			FixColumnSizes(dataGrid);
			SetDataGridItemsSource(dataGrid, (INotifyCollectionChanged)dataGrid.ItemsSource);
		}

		private static void FixColumnSizes(DataGrid dataGrid)
		{
			DataGridColumn firstColumn = dataGrid?.Columns.FirstOrDefault();

			if (firstColumn == null)
			{
				return;
			}

			firstColumn.Width = 0;
			dataGrid.UpdateLayout();
			firstColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
		}
	}
}