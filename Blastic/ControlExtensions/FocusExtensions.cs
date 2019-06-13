using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Bindables;
using Caliburn.Micro;
using ControlzEx;

namespace Blastic.ControlExtensions
{
	public static class FocusExtensions
	{
		[AttachedProperty(OnPropertyChanged = nameof(OnIsFocusedChanged))]
		public static bool IsFocused { get; set; }
		public static bool GetIsFocused(DependencyObject obj) { throw new WillBeImplementedByBindablesException(); }
		public static void SetIsFocused(DependencyObject obj, bool value) { }

		public static void OnIsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement control = (FrameworkElement)d;

			void GotFocus(object sender, RoutedEventArgs args)
			{
				SetIsFocused((FrameworkElement)sender, true);
			}

			void LostFocus(object sender, RoutedEventArgs args)
			{
				SetIsFocused((FrameworkElement)sender, false);
			}

			void IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
			{
				FrameworkElement element = (FrameworkElement)sender;
				if (element.IsVisible && GetIsFocused(element))
				{
					element.IsVisibleChanged -= IsVisibleChanged;
					element.Focus();
				}
			}

			if (e.OldValue == null)
			{
				control.GotFocus += GotFocus;
				control.LostFocus += LostFocus;
			}

			if (!control.IsVisible)
			{
				control.IsVisibleChanged += IsVisibleChanged;
			}

			if ((bool)e.NewValue)
			{
				control.Focus();
			}
		}

		public static void SetFocus(this IViewAware screen, string property)
		{
			if (!(screen.GetView() is DependencyObject view))
			{
				return;
			}

			FrameworkElement control = FindChild(view, property);

			if (control == null)
			{
				return;
			}

			KeyboardNavigationEx.Focus(control);
		}

		private static FrameworkElement FindChild(DependencyObject parent, string childName)
		{
			if (parent == null || string.IsNullOrWhiteSpace(childName))
			{
				return null;
			}

			FrameworkElement foundChild = null;

			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

			for (int i = 0; i < childrenCount; i++)
			{
				if (!(VisualTreeHelper.GetChild(parent, i) is FrameworkElement child))
				{
					continue;
				}

				BindingExpression bindingExpression = GetBindingExpression(child);

				if (child.Name == childName)
				{
					foundChild = child;
					break;
				}

				if (bindingExpression != null && bindingExpression.ResolvedSourcePropertyName == childName)
				{
					foundChild = child;
					break;
				}

				foundChild = FindChild(child, childName);

				if (foundChild == null)
				{
					continue;
				}

				if (foundChild.Name == childName)
				{
					break;
				}

				BindingExpression foundChildBindingExpression = GetBindingExpression(foundChild);

				if (foundChildBindingExpression != null &&
					foundChildBindingExpression.ResolvedSourcePropertyName == childName)
				{
					break;
				}
			}

			return foundChild;
		}

		private static BindingExpression GetBindingExpression(FrameworkElement control)
		{
			if (control == null)
			{
				return null;
			}

			ElementConvention convention = ConventionManager.GetElementConvention(control.GetType());
			DependencyProperty bindableProperty = convention?.GetBindableProperty(control);

			return bindableProperty != null
				? control.GetBindingExpression(bindableProperty)
				: null;
		}
	}
}