using System.Windows;
using Bindables;
using GongSolutions.Wpf.DragDrop;
using DragDrop = GongSolutions.Wpf.DragDrop.DragDrop;

namespace Blastic.ControlExtensions
{
	public static class DragDropExtensions
	{
		[AttachedProperty(OnPropertyChanged = nameof(OnLimitInsideContainerChanged))]
		public static bool LimitInsideContainer { get; set; }
		public static bool GetLimitInsideContainer(DependencyObject obj) { throw new WillBeImplementedByBindablesException(); }
		public static void SetLimitInsideContainer(DependencyObject obj, bool value) { }

		private static void OnLimitInsideContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is UIElement uiElement))
			{
				return;
			}

			if (e.NewValue is true)
			{
				DropDropHandler dropDropHandler = new DropDropHandler();
				DragDrop.SetDropHandler(uiElement, dropDropHandler);
			}
			else
			{
				DragDrop.SetDropHandler(uiElement, null);
			}
		}

		private class DropDropHandler : DefaultDropHandler
		{
			public override void DragOver(IDropInfo dropInfo)
			{
				base.DragOver(dropInfo);

				if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
				{
					return;
				}

				dropInfo.Effects = DragDropEffects.None;
				dropInfo.DropTargetAdorner = null;
			}

			public override void Drop(IDropInfo dropInfo)
			{
				if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
				{
					return;
				}

				base.Drop(dropInfo);
			}
		}
	}
}