using System;
using Blastic.Data;
using Blastic.Initialization;
using Blastic.Initialization.Extensions;
using Blastic.Sample.UserInterface;
using Blastic.UserInterface.TabbedMain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blastic.Sample
{
	public class Program
	{
		[STAThread]
		public static void Main()
		{
			new BlasticApplication()
				.Configure(x => x.AddSingleton(new ProductInformation
				{
					ProgramName = "Blastic Sample Application",
					Version = typeof(Program).Assembly.GetName().Version
				}))
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