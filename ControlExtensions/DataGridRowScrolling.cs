using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Bindables;

namespace Yaver.Host.Wpf.ControlExtensions
{
    public class DataGridScrolling
    {
        [AttachedProperty(OnPropertyChanged = nameof(OnEnableHorizontalScrollingWithShiftKeyChanged))]
        public static bool EnableHorizontalScrollingWithShiftKey { get; set; }

        public static bool GetEnableHorizontalScrollingWithShiftKey(DependencyObject o) => throw new WillBeImplementedByBindablesException();
        public static void SetEnableHorizontalScrollingWithShiftKey(DependencyObject o, bool value) { }

        private static void OnEnableHorizontalScrollingWithShiftKeyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = dp as DataGrid;

            if (dataGrid == null)
            {
                return;
            }

            bool wasHandling = (bool)(e.OldValue);
            bool needToHandle = (bool)(e.NewValue);

            if (wasHandling)
            {
                dataGrid.PreviewMouseWheel -= OnPreviewMouseWheel;
            }

            if (needToHandle)
            {
                dataGrid.PreviewMouseWheel += OnPreviewMouseWheel;
            }
        }

        private static void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            ScrollViewer scrollViewer = GetScrollViewer(dataGrid);

            if (Keyboard.Modifiers != ModifierKeys.Shift)
            {
                return;
            }

            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
            e.Handled = true;
        }

        private static ScrollViewer GetScrollViewer(UIElement element)
        {
            if (element == null) return null;

            ScrollViewer result = null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is ScrollViewer scrollViewer)
                {
                    result = scrollViewer;
                    break;
                }

                result = GetScrollViewer(VisualTreeHelper.GetChild(element, i) as UIElement);
            }
            return result;
        }
    }
}