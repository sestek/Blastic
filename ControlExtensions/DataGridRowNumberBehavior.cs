using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Bindables;

namespace Yaver.Host.Wpf.ControlExtensions
{
    public class DataGridRowNumberBehavior
    {
        [AttachedProperty(OnPropertyChanged = nameof(OnDisplayRowNumberChanged))]
        public static bool DisplayRowNumber { get; set; }

	    public static bool GetDisplayRowNumber(DependencyObject obj) => throw new WillBeImplementedByBindablesException();
	    public static void SetDisplayRowNumber(DependencyObject obj, bool newValue) { }

        [AttachedProperty]
        public static int RowNumberOffset { get; set; }

        public static int GetRowNumberOffset(DependencyObject obj) => throw new WillBeImplementedByBindablesException();
        public static void SetRowNumberOffset(DependencyObject obj, bool newValue) { }

        private static void OnDisplayRowNumberChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = target as DataGrid;

            if (dataGrid == null)
            {
                return;
            }

	        if (!(bool) e.NewValue)
	        {
		        return;
	        }

	        void LoadedRowHandler(object sender, DataGridRowEventArgs ea)
	        {
		        if (GetDisplayRowNumber(dataGrid) == false)
		        {
			        dataGrid.LoadingRow -= LoadedRowHandler;
			        return;
		        }

		        int rowNumberOffset = GetRowNumberOffset(dataGrid);

		        ea.Row.Header = ea.Row.GetIndex() + rowNumberOffset + 1;
	        }

	        dataGrid.LoadingRow += LoadedRowHandler;

	        void ItemsChangedHandler(object sender, ItemsChangedEventArgs ea)
	        {
		        if (GetDisplayRowNumber(dataGrid) == false)
		        {
			        dataGrid.ItemContainerGenerator.ItemsChanged -= ItemsChangedHandler;
			        return;
		        }
		        GetVisualChildCollection<DataGridRow>(dataGrid).ForEach(d => d.Header = d.GetIndex());
	        }

	        dataGrid.ItemContainerGenerator.ItemsChanged += ItemsChangedHandler;
        }

        private static List<T> GetVisualChildCollection<T>(object parent) where T : Visual
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : Visual
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                
	            if (child is T variable)
                {
                    visualCollection.Add(variable);
                }

	            GetVisualChildCollection(child, visualCollection);
            }
        }
    }
}