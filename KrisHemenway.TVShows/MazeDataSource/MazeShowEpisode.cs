using System;

namespace KrisHemenway.TVShows
{
	public class MazeShowEpisode
	{
		public int Id { get; set; }

		public string Url { get; set; }
		public string Name { get; set; }

		public int Season { get; set; }
		public int Number { get; set; }

		public int Runtime { get; set; }

		public MazeShowEpisodeImages Image { get; set; }
		public string Summary { get; set; }

		public DateTime? AirDate { get; set; }
		public DateTime? Airstamp { get; set; }
		public TimeSpan? AirTime { get; set; }

	}
}