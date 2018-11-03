using System;
using System.Linq;

namespace KrisHemenway.TVShows.Episodes
{
	public class EpisodesForMonthRequestHandler
	{
		public EpisodesForMonthRequestHandler(IEpisodeStore episodeStore = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
		}

		public EpisodesForMonthResponse HandleRequest(EpisodesForMonthRequest request)
		{
			var beginningOfMonth = new DateTime(request.Year, request.Month, 1);
			var endOfMonth = beginningOfMonth.AddMonths(1).AddDays(-1);

			return new EpisodesForMonthResponse
				{
					EpisodesInMonth = _episodeStore.FindEpisodesAiring(beginningOfMonth, endOfMonth)
						.OrderBy(x => x.AirDate)
						.ThenBy(x => x.ShowId)
						.ThenBy(x => x.Season)
						.ThenBy(x => x.EpisodeInSeason)
						.ToList(),
				};
		}

		private readonly IEpisodeStore _episodeStore;
	}
}
