using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Bindables;
using Microsoft.Xaml.Behaviors;

namespace WpfTemplate.Caliburn
{
	public class KeyTrigger : TriggerBase<UIElement>
	{
		[DependencyProperty]
		public Key Key { get; set; }

		[DependencyProperty]
		public ModifierKeys Modifiers { get; set; }
		
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.KeyDown += OnAssociatedObjectKeyDown;
			
			if (AssociatedObject is ButtonBase buttonBase)
			{
				buttonBase.Click += OnAssociatedObjectClick;
			}
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.KeyDown -= OnAssociatedObjectKeyDown;
			
			if (AssociatedObject is ButtonBase buttonBase)
			{
				buttonBase.Click -= OnAssociatedObjectClick;
			}
		}

		private void OnAssociatedObjectClick(object sender, RoutedEventArgs e)
		{
			InvokeActions(e);
		}

		private void OnAssociatedObjectKeyDown(object sender, KeyEventArgs e)
		{
			if ((e.Key == Key) && (Keyboard.Modifiers == GetActualModifiers(e.Key, Modifiers)))
			{
				InvokeActions(e);
			}
		}

		private static ModifierKeys GetActualModifiers(Key key, ModifierKeys modifiers)
		{
			switch (key)
			{
				case Key.LeftCtrl:
				case Key.RightCtrl:
					modifiers |= ModifierKeys.Control;
					break;

				case Key.LeftAlt:
				case Key.RightAlt:
					modifiers |= ModifierKeys.Alt;
					break;

				case Key.LeftShift:
				case Key.RightShift:
					modifiers |= ModifierKeys.Shift;
					break;
			}

			return modifiers;
		}
	}
}