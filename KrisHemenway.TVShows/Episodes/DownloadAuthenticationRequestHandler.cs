using KrisHemenway.Common;
using Microsoft.AspNetCore.Http;

namespace KrisHemenway.TVShows.Episodes
{
	public interface IDownloadAuthenticationRequestHandler
	{
		Result HandleRequest(DownloadAuthenticationRequest request, ISession session);
	}

	public class DownloadAuthenticationRequestHandler : IDownloadAuthenticationRequestHandler
	{
		public DownloadAuthenticationRequestHandler(ISettings settings = null)
		{
			_settings = settings ?? Program.Settings;
		}

		public Result HandleRequest(DownloadAuthenticationRequest request, ISession session)
		{
			if (request.Password != _settings.DownloadPassword)
			{
				return Result.Failure("Invalid Password");
			}

			DownloadAuthenticationRequiredAttribute.SetAuthenticated(session);
			return Result.Successful;
		}

		private readonly ISettings _settings;
	}
}
