namespace Blastic.DataLayer
{
	public class DatabaseConfiguration
	{
		public DatabaseProvider DatabaseProvider { get; }
		public string ConnectionString { get; }

		public DatabaseConfiguration(DatabaseProvider databaseProvider, string connectionString)
		{
			DatabaseProvider = databaseProvider;
			ConnectionString = connectionString;
		}
	}
}