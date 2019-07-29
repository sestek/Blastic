using System.Windows;
using Caliburn.Micro;

namespace Blastic.Controls
{
	public partial class LabeledPasswordBox
	{
		public string Password
		{
			get => (string)GetValue(PasswordProperty);
			set => SetValue(PasswordProperty, value);
		}
		public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
			nameof(PasswordProperty).Replace("Property", ""),
			typeof(string),
			typeof(LabeledPasswordBox),
			new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static LabeledPasswordBox()
		{
			ConventionManager.AddElementConvention<LabeledPasswordBox>(
				PasswordProperty,
				nameof(Password),
				nameof(DataContextChanged));
		}

		public LabeledPasswordBox()
		{
			InitializeComponent();
		}
	}
}