using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.EpisodeRenamer
{
	[Route("api/tvshows")]
	public class EpisodeRenamerController : Controller
	{
		[HttpPost("rename")]
		public IActionResult Rename([FromBody]EpisodeRenameRequest request)
		{
			return Json(new EpisodeRenameRequestHandler().HandleRequest(request));
		}
	}
}
