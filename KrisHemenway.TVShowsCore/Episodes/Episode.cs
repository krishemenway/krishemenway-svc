using System;

namespace KrisHemenway.TVShows.Episodes
{
	public interface IEpisode
	{
		int Id { get; }
		string Title { get; }

		string Series { get; }
		int SeriesId { get; }
		int EpisodeNumber { get; }

		int Season { get; }
		int EpisodeInSeason { get; }

		DateTime? AirDate { get; }
		DateTime LastModified { get; }
		DateTime Created { get; }
	}

	public class Episode : IEpisode
	{
		public int Id { get; set; }
		public string Title { get; set; }

		public string Series { get; set; }
		public int SeriesId { get; set; }
		public int EpisodeNumber { get; set; }

		public int Season { get; set; }
		public int EpisodeInSeason { get; set; }

		public DateTime? AirDate { get; set; }
		public DateTime LastModified { get; set; }
		public DateTime Created { get; set; }

		public override string ToString()
		{
			return $"Id: {Id}; SeriesId: {SeriesId}; Series: {Series}; Season: {Season}; EP: {EpisodeInSeason}";
		}
	}
}