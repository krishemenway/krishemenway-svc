using KrisHemenway.Common;
using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Shows
{
	[Route("api/tvshows/shows")]
	public class ShowController : Controller
	{
		[HttpGet(nameof(All))]
		[ProducesResponseType(200, Type = typeof(AllShowsResponse))]
		public IActionResult All()
		{
			return Json(new AllShowsRequestHandler().HandleRequest());
		}

		[HttpGet(nameof(Create))]
		[ProducesResponseType(200, Type = typeof(Result))]
		public IActionResult Create([FromQuery]CreateShowRequest createShowRequest)
		{
			return Ok(new CreateShowRequestHandler().HandleRequest(createShowRequest));
		}

		[HttpPost(nameof(RefreshShow))]
		[ProducesResponseType(200, Type = typeof(Result))]
		public IActionResult RefreshShow([FromQuery]RefreshShowRequest request)
		{
			return Ok(new RefreshShowRequestHandler().HandleRequest(request));
		}
	}
}
