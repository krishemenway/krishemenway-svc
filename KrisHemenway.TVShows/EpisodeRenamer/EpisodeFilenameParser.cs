using KrisHemenway.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KrisHemenway.TVShows.EpisodeRenamer
{
	public interface IEpisodeFilenameParser
	{
		Result<ParsedEpisode> TryParseVideoFile(IVideoFile videoFile);
	}

	public class EpisodeFilenameParser : IEpisodeFilenameParser
	{
		public EpisodeFilenameParser(IShowIdentifier showIdentifier = null)
		{
			_showIdentifier = showIdentifier ?? new ShowIdentifier();
		}

		public Result<ParsedEpisode> TryParseVideoFile(IVideoFile videoFile)
		{
			if (!FindSeason(videoFile.GetAllParentDirectories().Union(new[] { videoFile.FileName }), out var season))
			{
				return Result<ParsedEpisode>.Failure("Could not find season for video");
			}

			if (!FindEpisodeNumberInSeason(videoFile.FileName, out var episodeNumberInSeason))
			{
				return Result<ParsedEpisode>.Failure("Could not find episode number for video");
			}

			if (!_showIdentifier.TryIdentifyVideo(videoFile, out var show))
			{
				return Result<ParsedEpisode>.Failure("Could not find show for video");
			}

			var parsedEpisode = new ParsedEpisode
				{
					Show = show,
					Season = season,
					EpisodeNumberInSeason = episodeNumberInSeason,
				};

			return Result<ParsedEpisode>.Successful(parsedEpisode);
		}

		private static bool FindSeason(IEnumerable<string> searchParts, out int season)
		{
			if (TryMatchRegexes(searchParts, FindSeasonRegularExpressions, out season))
			{
				return true;
			}

			return false;
		}

		private static bool FindEpisodeNumberInSeason(string filename, out int episodeNumberInSeason)
		{
			if (TryMatchRegexes(filename, FindEpisodeInSeasonRegularExpressions, out episodeNumberInSeason))
			{
				return true;
			}

			return false;
		}

		private static bool TryMatchRegexes(IEnumerable<string> inputTexts, IEnumerable<string> regexesToTry, out int result)
		{
			result = 0;

			foreach (var inputText in inputTexts)
			{
				if (TryMatchRegexes(inputText, regexesToTry, out result))
				{
					return true;
				}
			}

			return false;
		}

		private static bool TryMatchRegexes(string inputText, IEnumerable<string> regexesToTry, out int result)
		{
			result = 0;

			foreach (var regex in regexesToTry)
			{
				var matchResult = Regex.Match(inputText, regex);

				if (matchResult.Success)
				{
					result = Convert.ToInt32(matchResult.Groups[1].Value);
					return true;
				}
			}

			return false;
		}

		private static readonly IReadOnlyList<string> FindEpisodeInSeasonRegularExpressions = new List<string>
			{
				@"[xXeE]([0-9]{1,2})",
				@"\[[0-9]+\.([0-9]+)\]",
				@"Episode ([0-9]+)",
				@"([0-9]+) -"
			};

		private static readonly IReadOnlyList<string> FindSeasonRegularExpressions = new List<string>
			{
				@"[sS]([0-9]{1,2})",
				@"Season *([0-9]{1,2})",
				@"([0-9]{1,2})[xXeE]",
				@"([0-9]{1,2})[xXeE\.]",
				@"\[([0-9]+)\.[0-9]+\]",
				@"Season *([0-9]+)"
			};

		private readonly IShowIdentifier _showIdentifier;
	}
}
