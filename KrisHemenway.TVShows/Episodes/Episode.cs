using KrisHemenway.TVShows.Shows;
using StronglyTyped.GuidIds;
using System;

namespace KrisHemenway.TVShows.Episodes
{
	public interface IEpisode
	{
		Id<Episode> EpisodeId { get; }

		Id<Show> ShowId { get; }
		string ShowName { get; }

		string Title { get; }
		int Season { get; }
		int EpisodeInSeason { get; }
		int EpisodeInShow { get; }

		string VideoPath { get; }
		bool HasEpisode { get; }
		bool IsMissing { get; }

		DateTime? AirDate { get; }
		DateTime LastModified { get; }
		DateTime Created { get; }
	}

	public class Episode : IEpisode
	{
		public Id<Episode> EpisodeId { get; set; }

		public Id<Show> ShowId { get; set; }
		public string ShowName { get; set; }

		public string Title { get; set; }
		public int Season { get; set; }
		public int EpisodeInSeason { get; set; }
		public int EpisodeInShow { get; set; }

		public string VideoPath { get; set; }
		public bool HasEpisode => !string.IsNullOrWhiteSpace(VideoPath);

		public DateTime? AirDate { get; set; }
		public bool HasAired => AirDate.HasValue && AirDate.Value.Date <= DateTime.Today;

		public DateTime LastModified { get; set; }
		public DateTime Created { get; set; }

		public bool IsMissing => !HasEpisode && HasAired;

		public override string ToString()
		{
			return $"Id: {EpisodeId}; ShowId: {ShowId}; Show: {ShowName}; Season: {Season}; EP: {EpisodeInSeason}";
		}
	}
}