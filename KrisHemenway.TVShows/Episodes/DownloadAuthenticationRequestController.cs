using KrisHemenway.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Episodes
{
	[Route("api/tvshows/episodes")]
	public class DownloadAuthenticationRequestController : ControllerBase
	{
		public DownloadAuthenticationRequestController(
			ISettings settings = null,
			ISession session = null)
		{
			_settings = settings ?? Program.Settings;
			_session = session ?? HttpContext.Session;
		}

		[HttpPost(nameof(Authenticate))]
		[ProducesResponseType(200, Type = typeof(Result))]
		public ActionResult<Result> Authenticate([FromBody] DownloadAuthenticationRequest request)
		{
			if (request.Password != _settings.DownloadPassword)
			{
				return Result.Failure("Invalid Password");
			}

			DownloadAuthenticationRequiredAttribute.SetAuthenticated(_session);

			return Result.Successful;
		}

		private readonly ISettings _settings;
		private readonly ISession _session;
	}
}
