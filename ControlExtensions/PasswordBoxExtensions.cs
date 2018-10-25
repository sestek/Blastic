using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Yaver.Host.Wpf.ControlExtensions
{
	public class PasswordBoxExtensions
	{
		[AttachedProperty(Options = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public static string BoundPassword { get; set; }

		[AttachedProperty(OnPropertyChanged = nameof(OnBindPasswordChanged))]
		public static bool BindPassword { get; set; }

		public static void SetBoundPassword(DependencyObject o, string value)
		{
		}

		public static void SetBindPassword(DependencyObject o, bool value)
		{
		}

		private static void OnBindPasswordChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (!(dependencyObject is PasswordBox passwordBox))
			{
				return;
			}

			bool wasBound = (bool)(e.OldValue);
			bool needToBind = (bool)(e.NewValue);

			if (wasBound)
			{
				passwordBox.PasswordChanged -= HandlePasswordChanged;
			}

			if (needToBind)
			{
				passwordBox.PasswordChanged += HandlePasswordChanged;
			}
		}

		private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)sender;
			SetBoundPassword(passwordBox, passwordBox.Password);
		}
	}
}