﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using PropertyChanged;
using Yaver.Host.Wpf.Caliburn;
using Yaver.Host.Wpf.Execution;
using Yaver.Host.Wpf.UserInterface.Logging;
using Yaver.Host.Wpf.UserInterface.Settings;

namespace Yaver.Host.Wpf.UserInterface.Main
{
	[Export]
	[AddINotifyPropertyChangedInterface]
	public class MainViewModel : ConductorOneActiveBase<IMainTab>
	{
		private readonly LoggingViewModel _loggingViewModel;
		private readonly SettingsViewModel _settingsViewModel;

		[ImportingConstructor]
		public MainViewModel(
			ExecutionContextFactory executionContextFactory,
			LoggingViewModel loggingViewModel,
			SettingsViewModel settingsViewModel,
			[ImportMany] IEnumerable<IMainTab> mainTabs)
			:
			base(executionContextFactory)
		{
			_loggingViewModel = loggingViewModel;
			_settingsViewModel = settingsViewModel;

			Items.AddRange(mainTabs.OrderBy(x => x.Order));
			ActiveItem = Items.FirstOrDefault();
		}

		public void ShowLogs()
		{
			_loggingViewModel.Show();
		}

		public void ShowSettings()
		{
			_settingsViewModel.Show();
		}
	}
}