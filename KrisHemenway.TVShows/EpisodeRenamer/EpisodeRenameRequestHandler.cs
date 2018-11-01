﻿using KrisHemenway.Common;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.TVShows.EpisodeRenamer
{
	public class EpisodeRenameRequestHandler
	{
		public EpisodeRenameRequestHandler(
			IVideoFileScanner videoFileScanner = null,
			IEpisodeIdentifier episodeIdentifier = null,
			IEpisodeRenamer episodeRenamer = null)
		{
			_videoFileScanner = videoFileScanner ?? new VideoFileScanner();
			_episodeIdentifier = episodeIdentifier ?? new EpisodeIdentifier();
			_episodeRenamer = episodeRenamer ?? new EpisodeRenamer();
		}

		public Result<IReadOnlyList<EpisodeRenameStatus>> HandleRequest(EpisodeRenameRequest request)
		{
			var videoScannerResult = _videoFileScanner.Scan(request.Path.Replace("/", "\\"));

			if (!videoScannerResult.Success)
			{
				return Result<IReadOnlyList<EpisodeRenameStatus>>.Failure(videoScannerResult.ErrorMessage);
			}

			return Result<IReadOnlyList<EpisodeRenameStatus>>.Successful(IdentifyAndRenameVideos(videoScannerResult.Data, request.IsTest).ToList());
		}

		private IEnumerable<EpisodeRenameStatus> IdentifyAndRenameVideos(IReadOnlyList<IVideoFile> videoFiles, bool isTest)
		{
			foreach (var video in videoFiles)
			{
				var identificationResult = _episodeIdentifier.Identify(video);

				if (!identificationResult.Success)
				{
					yield return new EpisodeRenameStatus { FilePath = video.FullPath, Result = Result.Failure(identificationResult.ErrorMessage) };
				}
				else
				{
					yield return new EpisodeRenameStatus { FilePath = video.FullPath, Result = _episodeRenamer.Rename(video, identificationResult.Data, isTest) };
				}
			}
		}

		private readonly IVideoFileScanner _videoFileScanner;
		private readonly IEpisodeIdentifier _episodeIdentifier;
		private readonly IEpisodeRenamer _episodeRenamer;
	}

	public class EpisodeRenameStatus
	{
		public string FilePath { get; set; }
		public Result Result { get; set; }
	}
}
