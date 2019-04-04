using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bindables;
using WpfTemplate.ControlExtensions;

namespace WpfTemplate.Controls
{
	public class LabeledControl : ContentControl
	{
		static LabeledControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(LabeledControl),
				new FrameworkPropertyMetadata(typeof(LabeledControl)));
		}

		[DependencyProperty]
		public string Label { get; set; }

		public object ButtonContent
		{
			get => GetValue(ButtonContentProperty);
			set => SetValue(ButtonContentProperty, value);
		}
		public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(
			nameof(ButtonContentProperty).Replace("Property", ""),
			typeof(object),
			typeof(LabeledControl),
			new PropertyMetadata(OnButtonContentChanged));

		private static void OnButtonContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((LabeledControl)d).MaintainLogicalTree(e.OldValue, e.NewValue);
		}

		public LabeledControl()
		{
			GotFocus += (sender, args) =>
			{
				FocusExtensions.SetIsFocused(this, true);
				args.Handled = true;
			};
		}

		private void MaintainLogicalTree(object oldContent, object newContent)
		{
			if (oldContent != null)
			{
				RemoveLogicalChild(oldContent);
			}

			if (newContent != null)
			{
				AddLogicalChild(newContent);
			}
		}

		protected override System.Collections.IEnumerator LogicalChildren
		{
			get
			{
				List<object> children = new List<object>();

				if (ButtonContent != null)
				{
					children.Add(ButtonContent);
				}

				if (Content != null)
				{
					children.Add(Content);
				}
				
				if (!children.Any())
				{
					return base.LogicalChildren;
				}
				
				return children.GetEnumerator();
			}
		}
	}
}