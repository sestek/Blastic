﻿using System.Windows;
using Bindables;

namespace Blastic.Controls
{
	public partial class DualView
	{
		[DependencyProperty]
		public bool ShowFirstView { get; set; } = true;

		[DependencyProperty]
		public UIElement FirstView { get; set; }
		
		[DependencyProperty]
		public UIElement SecondView { get; set; }

		public DualView()
		{
			InitializeComponent();
		}
	}
}