using System;

namespace KrisHemenway.TVShows.Episodes
{
	public interface IEpisode
	{
		Guid EpisodeId { get; }

		Guid ShowId { get; }
		string ShowName { get; }

		string Title { get; }
		int Season { get; }
		int EpisodeInSeason { get; }
		int EpisodeInShow { get; }

		string VideoPath { get; }
		bool HasEpisode { get; }

		DateTime? AirDate { get; }
		DateTime LastModified { get; }
		DateTime Created { get; }
	}

	public class Episode : IEpisode
	{
		public Guid EpisodeId { get; set; }

		public Guid ShowId { get; set; }
		public string ShowName { get; set; }

		public string Title { get; set; }
		public int Season { get; set; }
		public int EpisodeInSeason { get; set; }
		public int EpisodeInShow { get; set; }

		public string VideoPath { get; set; }
		public bool HasEpisode => !string.IsNullOrWhiteSpace(VideoPath);

		public DateTime? AirDate { get; set; }
		public DateTime LastModified { get; set; }
		public DateTime Created { get; set; }

		public override string ToString()
		{
			return $"Id: {EpisodeId}; ShowId: {ShowId}; Show: {ShowName}; Season: {Season}; EP: {EpisodeInSeason}";
		}
	}
}