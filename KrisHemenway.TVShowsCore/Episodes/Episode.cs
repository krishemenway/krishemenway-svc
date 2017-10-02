using System;

namespace KrisHemenway.TVShowsCore.Episodes
{
	public class Episode
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