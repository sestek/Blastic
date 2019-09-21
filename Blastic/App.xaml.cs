using System;
using System.Windows;

namespace Blastic
{
	public class App : Application
	{
		public App()
		{
			ResourceDictionary applicationResources = new ResourceDictionary
			{
				Source = new Uri(
					"/Blastic;component/Themes/ApplicationResources.xaml",
					UriKind.RelativeOrAbsolute)
			};

			Resources.MergedDictionaries.Add(applicationResources);
		}
	}
}