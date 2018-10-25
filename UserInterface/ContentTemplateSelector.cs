using System.Windows;
using System.Windows.Controls;
using Yaver.Host.Wpf.UserInterface.Home;
using Yaver.Host.Wpf.UserInterface.Login;

namespace Yaver.Host.Wpf.UserInterface
{
	public class ContentTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (!(container is FrameworkElement element))
			{
				return base.SelectTemplate(item, container);
			}

			switch (item)
			{
				case LoginViewModel _:
					return element.FindResource("LoginDataTemplate") as DataTemplate;
				case HomeViewModel _:
					return element.FindResource("HomeDataTemplate") as DataTemplate;
				default:
					return base.SelectTemplate(item, container);
			}
		}
	}
}