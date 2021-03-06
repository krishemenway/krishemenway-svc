﻿using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace KrisHemenway.TVShows
{
	public interface ISettings
	{
		int WebPort { get; }
		string LogFile { get; }

		string DatabaseUser { get; }
		string DatabasePassword { get; }

		string DatabaseHost { get; }
		int DatabasePort { get; }

		string DatabaseName { get; }

		IReadOnlyList<string> RenameVideoFileExtensions { get; }
		string RenameDateFormat { get; }
		string RenameFormat { get; }

		int RenamePadNumbers { get; }

		string DownloadPassword { get; }
		bool CanDownload { get; }
	}

	public class Settings : ISettings
	{
		public Settings(IConfigurationRoot configuration)
		{
			_configuration = configuration;
		}

		public int WebPort => _configuration.GetValue<int>("WebPort");
		public string LogFile => _configuration.GetValue<string>("LogFile");

		public string DatabaseUser => _configuration.GetValue<string>("TVShowDatabaseUser");
		public string DatabasePassword => _configuration.GetValue<string>("TVShowDatabasePassword");

		public string DatabaseHost => _configuration.GetValue<string>("TVShowDatabaseHost");
		public int DatabasePort => _configuration.GetValue<int>("TVShowDatabasePort");

		public string DatabaseName => _configuration.GetValue<string>("TVShowDatabaseName");

		public IReadOnlyList<string> RenameVideoFileExtensions => _configuration.GetSection("RenameVideoFileExtensions").Get<string[]>();
		public string RenameDateFormat => _configuration.GetValue<string>("RenameDateFormat");
		public string RenameFormat => _configuration.GetValue<string>("RenameFormat");
		public int RenamePadNumbers => _configuration.GetValue<int>("RenamePadNumbers");

		public string DownloadPassword => _configuration.GetValue<string>("DownloadPassword");
		public bool CanDownload => _configuration.GetValue<bool>("CanDownload");

		private readonly IConfigurationRoot _configuration;
	}
}
