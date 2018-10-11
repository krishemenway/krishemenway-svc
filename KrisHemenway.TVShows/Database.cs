using Npgsql;
using System.Data;

namespace KrisHemenway.TVShows
{
	public class Database
	{
		public static IDbConnection CreateConnection()
		{
			var connection = new NpgsqlConnection($"Host={Program.Settings.Host};Username={Program.Settings.User};Password={Program.Settings.Password};Database={Program.Settings.DatabaseName}");
			connection.Open();
			return connection;
		}
	}
}