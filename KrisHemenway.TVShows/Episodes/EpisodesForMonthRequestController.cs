using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KrisHemenway.TVShows.Episodes
{
	[Route("api/tvshows/episodes")]
	public class EpisodesForMonthRequestController : ControllerBase
	{
		public EpisodesForMonthRequestController(IEpisodeStore episodeStore = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
		}

		[HttpGet("calendar/{year}/{month}")]
		[ProducesResponseType(200, Type = typeof(EpisodesForMonthResponse))]
		public ActionResult<EpisodesForMonthResponse> EpisodesForMonth([FromRoute] EpisodesForMonthRequest request)
		{
			var beginningOfMonth = new DateTime(request.Year, request.Month, 1);
			var endOfMonth = beginningOfMonth.AddMonths(1).AddDays(-1);

			return new EpisodesForMonthResponse
				{
					ShowDownload = DownloadAuthenticationRequiredAttribute.HasAuthenticated(HttpContext.Session),
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
