using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace KrisHemenway.Notification
{
	public static class Database
	{
		public static IDbConnection CreateConnection()
		{
			var connection = new NpgsqlConnection($"Host={Host};Username={User};Password={Password};Database={DatabaseName};Port={Port}");
			connection.Open();
			return connection;
		}

		private static string Host => Program.Configuration.GetValue<string>("NotificationsDatabaseHost");
		private static int Port => Program.Configuration.GetValue<int>("NotificationsDatabasePort");

		private static string User => Program.Configuration.GetValue<string>("NotificationsDatabaseUser");
		private static string Password => Program.Configuration.GetValue<string>("NotificationsDatabasePassword");

		private static string DatabaseName => Program.Configuration.GetValue<string>("NotificationsDatabaseName");
	}
}