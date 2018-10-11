using Npgsql;
using System.Data;

namespace KrisHemenway.TomatoRouter
{
	public class Database
	{
		public static IDbConnection CreateConnection()
		{
			var connection = new NpgsqlConnection($"Host={Program.Settings.Host};Username={Program.Settings.DatabaseUser};Password={Program.Settings.DatabasePassword};Database={Program.Settings.DatabaseName}");
			connection.Open();
			return connection;
		}
	}
}