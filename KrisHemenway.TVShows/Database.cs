﻿using Npgsql;
using StronglyTyped.GuidIds.Dapper;
using System.Data;

namespace KrisHemenway.TVShows
{
	public class Database
	{
		static Database()
		{
			DapperIdRegistrar.RegisterAll();
		}

		public static IDbConnection CreateConnection()
		{
			var connection = new NpgsqlConnection($"Host={Program.Settings.DatabaseHost};Username={Program.Settings.DatabaseUser};Password={Program.Settings.DatabasePassword};Database={Program.Settings.DatabaseName};Port={Program.Settings.DatabasePort}");
			connection.Open();
			return connection;
		}
	}
}