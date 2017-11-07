using KrisHemenway.TVShows.Episodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace KrisHemenway.TVShows.Episodes
{
	public interface IEpisodeIdentifier
	{
		IEpisode Identify(string filename);
	}

	public class EpisodeIdentifier : IEpisodeIdentifier
	{
		public EpisodeIdentifier(
			OnFoundMultipleShow onFoundMultipleShow,
			IShowIdentifier showIdentifier = null,
			IEpisodeDataRepository episodeDataRepository = null)
		{
			_showIdentifier = showIdentifier ?? new ShowIdentifier(onFoundMultipleShow);
			_episodeDataRepository = episodeDataRepository ?? new EpisodeDataRepository();
		}

		public IEpisode Identify(string filename)
		{
			return Identify(new VideoFile(new FileInfo(filename)));
		}

		public IEpisode Identify(IVideoFile file)
		{
			var rememberedEpisode = _episodeDataRepository.FindPreviouslyIdentifiedEpisode(file.FullPath);

			if (rememberedEpisode != null)
			{
				return rememberedEpisode;
			}

			var collectedEpisode = CollectEpisodeDataFromFileInformation(file);
			return RetrieveEpisode(collectedEpisode);
		}

		private CollectedEpisode CollectEpisodeDataFromFileInformation(IVideoFile videoFile)
		{
			var fileDirectories = videoFile.GetAllParentDirectories();
			fileDirectories.Add(videoFile.FileName);

			var collectedEpisode = new CollectedEpisode
			{
				VideoFile = videoFile,
				Show = _showIdentifier.Identify(videoFile),
				Season = FindSeason(fileDirectories)
			};

			if (collectedEpisode.Season.HasValue)
			{
				collectedEpisode.EpisodeNumberInSeason = FindEpisodeNumberInSeason(videoFile.FileName);
			}

			if (collectedEpisode.Show == null)
			{
				throw new ShowNotIdentifiedException(collectedEpisode.VideoFile);
			}

			return collectedEpisode;
		}

		private IEpisode RetrieveEpisode(CollectedEpisode collectedEpisodeData)
		{
			var episode = _episodeDataRepository.RetrieveEpisode(collectedEpisodeData);
			episode.Filename = collectedEpisodeData.VideoFile.FullPath;
			episode.Show.AddEpisode(episode);
			return episode;
		}

		private static int? FindSeason(IEnumerable<string> directories)
		{
			var regexesToTry = new string[]
			{
				@"[sS]([0-9]{1,2})",
				@"Season *([0-9]{1,2})",
				@"([0-9]{1,2})[xXeE]",
				@"([0-9]{1,2})[xXeE\.]",
				@"\[([0-9]+)\.[0-9]+\]",
				@"Season *([0-9]+)"
			};

			return TryMatchRegexes(directories, regexesToTry);
		}

		private static int? FindEpisodeNumberInSeason(string filename)
		{
			var episodeNumberInSeasonRegexes = new string[]
			{
				@"[xXeE]([0-9]{1,2})",
				@"\[[0-9]+\.([0-9]+)\]",
				@"Episode ([0-9]+)",
				@"([0-9]+) -"
			};

			return TryMatchRegexes(filename, episodeNumberInSeasonRegexes);
		}

		private static int? TryMatchRegexes(IEnumerable<string> inputTexts, IEnumerable<string> regexesToTry)
		{
			foreach (var inputText in inputTexts)
			{
				var result = TryMatchRegexes(inputText, regexesToTry);

				if (result.HasValue)
				{
					return result.Value;
				}
			}

			return null;
		}

		private static int? TryMatchRegexes(string inputText, IEnumerable<string> regexesToTry)
		{
			foreach (var regex in regexesToTry)
			{
				var matchResult = Regex.Match(inputText, regex);

				if (matchResult.Success)
				{
					return Convert.ToInt32(matchResult.Groups[1].Value);
				}
			}

			return null;
		}

		private readonly IEpisodeDataRepository _episodeDataRepository;
		private readonly IShowIdentifier _showIdentifier;
	}
}