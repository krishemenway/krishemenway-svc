using System;

namespace KrisHemenway.TVShows.Episodes
{
	public class UpcomingEpisodesRequestHandler
	{
		public UpcomingEpisodesRequestHandler(
			IEpisodeStore episodeStore = null,
			Func<DateTime> getCurrentDateFunc = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
			_getCurrentDateFunc = getCurrentDateFunc ?? (() => DateTime.Today);
		}

		public UpcomingEpisodesResponse HandleRequest()
		{
			return new UpcomingEpisodesResponse
				{
					Episodes = _episodeStore.FindEpisodesAiring(_getCurrentDateFunc(), _getCurrentDateFunc().AddDays(3))
				};
		}

		private readonly IEpisodeStore _episodeStore;
		private readonly Func<DateTime> _getCurrentDateFunc;
	}
}
