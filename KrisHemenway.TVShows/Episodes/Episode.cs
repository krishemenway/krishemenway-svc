using System;

namespace KrisHemenway.TVShows.Episodes
{
	public interface IEpisode
	{
		int Id { get; }
		string Title { get; }

		string ShowName { get; }
		int ShowId { get; }
		int EpisodeNumber { get; }

		int Season { get; }
		int EpisodeInSeason { get; }

		string VideoPath { get; }

		DateTime? AirDate { get; }
		DateTime LastModified { get; }
		DateTime Created { get; }
	}

	public class Episode : IEpisode
	{
		public int Id { get; set; }
		public string Title { get; set; }

		public string ShowName { get; set; }
		public int ShowId { get; set; }
		public int EpisodeNumber { get; set; }

		public int Season { get; set; }
		public int EpisodeInSeason { get; set; }

		public string VideoPath { get; set; }

		public DateTime? AirDate { get; set; }
		public DateTime LastModified { get; set; }
		public DateTime Created { get; set; }

		public override string ToString()
		{
			return $"Id: {Id}; ShowId: {ShowId}; Show: {ShowName}; Season: {Season}; EP: {EpisodeInSeason}";
		}
	}
}