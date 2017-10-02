using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace KrisHemenway.NotificationCore
{
	public static class Database
	{
		public static IDbConnection CreateConnection()
		{
			var connection = new NpgsqlConnection($"Host={Host};Username={User};Password={Password};Database={DatabaseName}");
			connection.Open();
			return connection;
		}

		private static string Host
		{
			get { return Program.Configuration.GetValue<string>("PushServiceHost"); }
		}

		private static string User
		{
			get { return Program.Configuration.GetValue<string>("PushServiceUser"); }
		}

		private static string Password
		{
			get { return Program.Configuration.GetValue<string>("PushServicePassword"); }
		}

		private const string DatabaseName = "krishemenway";
	}
}