using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Episodes
{
	[Route("api/tvshows/episodes")]
	public class RecentlyAddedEpisodesRequestController : ControllerBase
	{
		public RecentlyAddedEpisodesRequestController(IEpisodeStore episodeStore = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
		}

		[HttpGet(nameof(RecentlyAdded))]
		[ProducesResponseType(200, Type = typeof(RecentlyAddedEpisodesResponse))]
		public ActionResult<RecentlyAddedEpisodesResponse> RecentlyAdded()
		{
			return new RecentlyAddedEpisodesResponse
				{
					Episodes = _episodeStore.FindNewEpisodes(),
				};
		}

		private readonly IEpisodeStore _episodeStore;
	}
}
