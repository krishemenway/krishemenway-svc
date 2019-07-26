using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Shows
{
	[Route("api/tvshows/shows")]
	public class AllShowsRequestController : ControllerBase
	{
		public AllShowsRequestController(IShowStore showStore = null)
		{
			_showStore = showStore ?? new ShowStore();
		}

		[HttpGet(nameof(All))]
		[ProducesResponseType(200, Type = typeof(AllShowsResponse))]
		public ActionResult<AllShowsResponse> All()
		{
			return new AllShowsResponse
				{
					Shows = _showStore.FindAll(),
				};
		}

		private readonly IShowStore _showStore;
	}
}
