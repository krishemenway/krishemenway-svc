using Microsoft.AspNetCore.Mvc;
using System;

namespace KrisHemenway.TVShows.Episodes
{
	[Route("api/tvshows/episodes")]
	public class UpcomingEpisodesRequestController : ControllerBase
	{
		public UpcomingEpisodesRequestController(
			IEpisodeStore episodeStore = null,
			Func<DateTime> getCurrentDateFunc = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
			_getCurrentDateFunc = getCurrentDateFunc ?? (() => DateTime.Today);
		}

		[HttpGet(nameof(Upcoming))]
		[ProducesResponseType(200, Type = typeof(UpcomingEpisodesResponse))]
		public ActionResult<UpcomingEpisodesResponse> Upcoming()
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
