using System;
using Blastic.Data;
using Blastic.Initialization;
using Blastic.Initialization.Extensions;
using Blastic.Sample.UserInterface;
using Blastic.UserInterface.TabbedMain;
using Microsoft.Extensions.Configuration;

namespace Blastic.Sample
{
	public class Program
	{
		[STAThread]
		public static void Main()
		{
			new BlasticApplication()
				.Configure(x => x.AddJsonFile("AppSettings.json"))
				.RegisterViewAssembly<Program>()
				.RegisterMainTab<HomeViewModel>()
				.AddLogsWindow()
				.AddSettingsWindow()
				.AddSettingsService()
				.AddProgramDatabase(DatabaseProvider.SQLite, "Data Source=Settings.sqlite;")
				.Run<TabbedMainViewModel>();
		}
	}
}