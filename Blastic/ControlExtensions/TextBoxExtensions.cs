using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace Blastic.ControlExtensions
{
	public static class TextBoxExtensions
	{
		public static void SetCaretIndexToEnd(this IViewAware screen, string property)
		{
			if (!(screen.GetView() is DependencyObject view))
			{
				return;
			}

			FrameworkElement control = VisualTreeExtensions.FindChild(view, property);

			if (control == null)
			{
				return;
			}

			TextBox textBox = VisualTreeExtensions.FindChild<TextBox>(control);

			if (textBox == null)
			{
				return;
			}

			textBox.CaretIndex = textBox.Text.Length;
		}
	}
}