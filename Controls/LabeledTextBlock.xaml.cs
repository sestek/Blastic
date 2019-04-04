using Bindables;

namespace WpfTemplate.Controls
{
	public partial class LabeledTextBlock
	{
		[DependencyProperty]
		public string Text { get; set; }

		public LabeledTextBlock()
		{
			InitializeComponent();
		}
	}
}