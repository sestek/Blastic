using PropertyChanged;

namespace WpfTemplate.Controls.Selectable
{
	[AddINotifyPropertyChangedInterface]
	public class SelectableViewModel<T>
	{
		public T Value { get; }
		public bool IsSelected { get; set; }

		public SelectableViewModel(T value, bool isSelected = false)
		{
			Value = value;
			IsSelected = isSelected;
		}
	}
}