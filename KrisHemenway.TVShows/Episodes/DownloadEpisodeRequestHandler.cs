using KrisHemenway.Common;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KrisHemenway.TVShows.Episodes
{
	public class DownloadEpisodeRequestHandler
	{
		public DownloadEpisodeRequestHandler(
			IEpisodeStore episodeStore = null,
			FileExtensionContentTypeProvider fileExtensionContentTypeProvider = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
			_fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? new FileExtensionContentTypeProvider(ContentTypeMappings);
		}

		public Result<DownloadEpisodeResponse> HandleRequest(DownloadEpisodeRequest request)
		{
			var episode = _episodeStore.FindEpisodes(request.EpisodeId).Single();
			var fileInfo = new FileInfo(episode.VideoPath);

			if (!_fileExtensionContentTypeProvider.TryGetContentType(fileInfo.FullName, out var contentType))
			{
				Log.Error("Could not find content type for file: {VideoPath}", episode.VideoPath);
				return Result<DownloadEpisodeResponse>.Failure("Cannot download file");
			}

			var response = new DownloadEpisodeResponse
				{
					ContentType = contentType,
					FileStream = fileInfo.OpenRead(),
					FileName = fileInfo.Name,
				};

			return Result<DownloadEpisodeResponse>.Successful(response);
		}

		private static readonly Dictionary<string, string> ContentTypeMappings = new Dictionary<string, string>
			{
				{ ".mkv", "video/x-matroska" },
			};

		private readonly IEpisodeStore _episodeStore;
		private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
	}
}
