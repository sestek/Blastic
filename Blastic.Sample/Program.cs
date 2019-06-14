using System;
using Blastic.Data;
using Blastic.Initialization;
using Blastic.UserInterface.Main;
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
				.AddProgramDatabase(DatabaseProvider.SQLite, "Data Source=Settings.sqlite;")
				.AddSettings()
				.Run<MainViewModel>();
		}
	}
}