using KrisHemenway.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KrisHemenway.TVShows.Episodes
{
	[Route("api/tvshows/episodes")]
	public class DownloadEpisodeRequestController : ControllerBase
	{
		public DownloadEpisodeRequestController(
			IEpisodeStore episodeStore = null,
			FileExtensionContentTypeProvider fileExtensionContentTypeProvider = null,
			ISettings settings = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
			_fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? new FileExtensionContentTypeProvider(ContentTypeMappings);
			_settings = settings ?? Program.Settings;
		}

		[HttpGet(nameof(Download))]
		[DownloadAuthenticationRequired]
		public ActionResult<Result> Download([FromQuery] DownloadEpisodeRequest request)
		{
			if (!_settings.CanDownload)
			{
				Log.Error("Attempted to download video while disabled: {EpisodeId}", request.EpisodeId);
				return Result.Failure("Cannot download file");
			}

			var episode = _episodeStore.FindEpisodes(request.EpisodeId).Single();
			var fileInfo = new FileInfo(episode.VideoPath);

			if (!_fileExtensionContentTypeProvider.TryGetContentType(fileInfo.FullName, out var contentType))
			{
				Log.Error("Could not find content type for file: {VideoPath}", episode.VideoPath);
				return Result.Failure("Cannot download file");
			}

			return File(fileInfo.OpenRead(), contentType, fileInfo.Name);
		}

		private static readonly Dictionary<string, string> ContentTypeMappings = new Dictionary<string, string>
			{
				{ ".mkv", "video/x-matroska" },
			};

		private readonly IEpisodeStore _episodeStore;
		private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
		private readonly ISettings _settings;
	}
}
