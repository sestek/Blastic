using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Blastic.Controls.Help
{
	public class HelpView : ContentControl
	{
		static HelpView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HelpView), new FrameworkPropertyMetadata(typeof(HelpView)));
		}

		[DependencyProperty]
		public object HelpContent { get; set; }

		[DependencyProperty]
		public Thickness HelpIconMargin { get; set; } = new Thickness(8, 0, 0, 0);
	}
}