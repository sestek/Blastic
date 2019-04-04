using System.Windows;
using Caliburn.Micro;

namespace WpfTemplate.Controls
{
	public partial class LabeledTextBox
	{
		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			nameof(TextProperty).Replace("Property", ""),
			typeof(string),
			typeof(LabeledTextBox),
			new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static LabeledTextBox()
		{
			ConventionManager.AddElementConvention<LabeledTextBox>(
				TextProperty,
				nameof(Text),
				nameof(DataContextChanged));
		}

		public LabeledTextBox()
		{
			InitializeComponent();
		}
	}
}