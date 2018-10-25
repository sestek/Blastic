using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Bindables;

namespace Yaver.Host.Wpf.Caliburn
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
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.KeyDown -= OnAssociatedObjectKeyDown;
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