using Npgsql;
using System.Data;

namespace KrisHemenway.TVShows
{
	public class Database
	{
		public static IDbConnection CreateConnection()
		{
			var connection = new NpgsqlConnection($"Host={Program.Settings.DatabaseHost};Username={Program.Settings.DatabaseUser};Password={Program.Settings.DatabasePassword};Database={Program.Settings.DatabaseName};Port={Program.Settings.DatabasePort}");
			connection.Open();
			return connection;
		}
	}
}