using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Blastic.ControlExtensions
{
	public static class ScrollViewerExtensions
	{
		[AttachedProperty(OnPropertyChanged = nameof(AutoScrollPropertyChanged))]
		public static bool AutoScroll { get; set; }
		public static bool GetAutoScroll(DependencyObject obj) { throw new WillBeImplementedByBindablesException(); }
		public static void SetAutoScroll(DependencyObject obj, bool value) { }

		public static void AutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			if (!(d is ScrollViewer scrollViewer))
			{
				return;
			}

			if ((bool)args.NewValue)
			{
				scrollViewer.ScrollChanged += OnScrollChanged;
				scrollViewer.ScrollToEnd();
			}
			else
			{
				scrollViewer.ScrollChanged -= OnScrollChanged;
			}
		}

		private static void OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			// Only scroll to bottom when the extent changed. Otherwise you can't scroll up.
			if (e.ExtentHeightChange == 0)
			{
				return;
			}

			ScrollViewer scrollViewer = sender as ScrollViewer;
			scrollViewer?.ScrollToBottom();
		}
	}
}