using System.Collections.Generic;
using System.Dynamic;
using System.Windows;
using Autofac;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using PropertyChanged;
using WpfTemplate.Caliburn;
using WpfTemplate.Execution;

namespace WpfTemplate.UserInterface.Settings
{
	[SingleInstance]
	[AddINotifyPropertyChangedInterface]
	public sealed class SettingsViewModel : ScreenBase
	{
		private Window _activeWindow;

		public IEnumerable<Swatch> Swatches { get; }
		public bool IsDark { get; set; }
		
		public SettingsViewModel(ExecutionContextFactory executionContextFactory) : base(executionContextFactory)
		{
			Swatches = new SwatchesProvider().Swatches;
			DisplayName = "Settings";
		}

		// Will be called by Fody.
		private void OnIsDarkChanged()
		{
			new PaletteHelper().SetLightDark(IsDark);
		}
		
		public void ApplyPrimary(Swatch swatch)
		{
			new PaletteHelper().ReplacePrimaryColor(swatch);
		}

		public void ApplyAccent(Swatch swatch)
		{
			new PaletteHelper().ReplaceAccentColor(swatch);
		}
		
		public void Show()
		{
			if (_activeWindow != null && PresentationSource.FromVisual(_activeWindow) != null)
			{
				_activeWindow.Activate();
				return;
			}

			dynamic settings = new ExpandoObject();
			settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;

			ExecutionContext.WindowManager.ShowWindow(this, null, settings);
		}

		protected override void OnViewAttached(object view, object context)
		{
			_activeWindow = view as Window;
		}

		public override object GetView(object context = null)
		{
			return _activeWindow;
		}
	}
}