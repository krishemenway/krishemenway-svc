using System;
using System.Text.Json.Serialization;

namespace KrisHemenway.TVShows.MazeDataSource
{
	public class MazeShowEpisode
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("season")]
		public int Season { get; set; }

		[JsonPropertyName("number")]
		public int Number { get; set; }

		[JsonPropertyName("airdate")]
		public DateTime? AirDate { get; set; }
	}
}