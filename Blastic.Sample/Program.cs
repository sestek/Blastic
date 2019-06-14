using System;
using Autofac;
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
				.AddProgramDatabase(DatabaseProvider.SQLite, "Data Source=Settings.sqlite;")
				.AddSettingsService()
				.RegisterViewAssembly(typeof(Program).Assembly)
				.Configure(x => x.AddJsonFile("AppSettings.json"))
				.Configure(builder =>
				{
					builder
						.RegisterType<HomeViewModel>()
						.SingleInstance()
						.As<IMainTab>();
				})
				.AddSettingsWindow()
				.Run<TabbedMainViewModel>();
		}
	}
}