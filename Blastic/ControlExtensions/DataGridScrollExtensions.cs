using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Blastic.ControlExtensions
{
	public static class DataGridScrollExtensions
	{
		[AttachedProperty(OnPropertyChanged = nameof(OnPropertyChanged))]
		public static bool TrackSelectedItem { get; set; }
		public static bool GetTrackSelectedItem(DependencyObject obj) { throw new WillBeImplementedByBindablesException(); }
		public static void SetTrackSelectedItem(DependencyObject obj, bool value) { }

		public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is DataGrid dataGrid))
			{
				return;
			}

			bool newValue = (bool)e.NewValue;

			if (newValue)
			{
				dataGrid.SelectionChanged += SelectionChanged;
			}
		}

		private static void SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)sender;

			if (dataGrid.SelectedItem == null)
			{
				return;
			}

			dataGrid.UpdateLayout();
			dataGrid.ScrollIntoView(dataGrid.SelectedItem);
		}
	}
}