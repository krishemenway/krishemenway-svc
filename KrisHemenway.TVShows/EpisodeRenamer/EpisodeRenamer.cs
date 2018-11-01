using KrisHemenway.Common;
using KrisHemenway.TVShows.Episodes;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace KrisHemenway.TVShows.EpisodeRenamer
{
	public interface IEpisodeRenamer
	{
		Result Rename(IVideoFile file, IEpisode episode, bool isTest);
	}

	public class EpisodeRenamer : IEpisodeRenamer
	{
		public EpisodeRenamer(
			IReadOnlyDictionary<string, MagicFieldAction> magicFieldActions = null,
			ISettings settings = null,
			IEpisodeStore episodeStore = null)
		{
			_settings = settings ?? Program.Settings;
			_episodeStore = episodeStore ?? new EpisodeStore();
			MagicFields = magicFieldActions ?? new Dictionary<string, MagicFieldAction>
				{
					{"%r", (episode) => episode.ShowName},
					{"%s", (episode) => episode.Season.ToString(CultureInfo.InvariantCulture).PadLeft(_settings.RenamePadNumbers, '0')},
					{"%f", (episode) => episode.EpisodeInSeason.ToString(CultureInfo.InvariantCulture).PadLeft(_settings.RenamePadNumbers, '0')},
					{"%t", (episode) => episode.Title},
					{"%d", (episode) => BuildDate(episode.AirDate, _settings.RenameDateFormat)},
					{"%e", (episode) => episode.EpisodeInShow.ToString(CultureInfo.InvariantCulture).PadLeft(_settings.RenamePadNumbers, '0')}
				};
		}

		/// <summary>
		/// Generate a string based on a formula. %r for Show name, %s for Season Number, %e for the overall episode
		/// number, %f for the episode number in the season, %p for the production number, %t for the title, and %d for the 
		/// air date
		/// </summary>
		/// <param name="file">file to be renamed</param>
		/// <param name="episode">episode data to use to rename file</param>
		/// <param name="settings">options for renaming the file</param>
		/// <returns></returns>
		public Result Rename(IVideoFile file, IEpisode episode, bool isTest)
		{
			var newFilename = CreateFilename(episode);
			var newFilePath = string.Format("{0}\\{1}{2}", file.DirectoryPath, newFilename, file.FileExtension);

			if(!file.FullPath.Equals(newFilePath, StringComparison.CurrentCultureIgnoreCase) && !isTest)
			{
				var result = TryMoveFile(file.FullPath, newFilePath);

				if (result.Success)
				{
					_episodeStore.UpdatePath(episode, newFilePath);
				}
			}
			else if (!episode.HasEpisode)
			{
				_episodeStore.UpdatePath(episode, file.FullPath);
			}

			return Result.Successful;
		}

		private Result TryMoveFile(string fromFilePath, string toFilePath)
		{
			try
			{
				File.Move(fromFilePath, toFilePath);
				Log.Information("Renamed {FromPath} to {ToPath} @ {CompletedTime}", fromFilePath, toFilePath, DateTimeOffset.Now);
				return Result.Successful;
			}
			catch (IOException exception)
			{
				if (!exception.Message.Contains("used by another process"))
				{
					return Result.Failure(exception.Message);
				}

				return Result.Failure("Video file is currently locked, cannot move");
			}
			catch (Exception exception)
			{
				return Result.Failure(exception.Message);
			}
		}

		protected static string CleanFilename(string filename)
		{
			var workingFilename = filename;

			workingFilename = workingFilename.Replace("*", "#");
			workingFilename = workingFilename.Replace("/", ",");
			workingFilename = Regex.Replace(workingFilename, @"[\\:?""<>|]", string.Empty);
			workingFilename = workingFilename.Replace("  ", " ");

			return workingFilename;
		}

		private string CreateFilename(IEpisode episode)
		{
			return CleanFilename(MagicFields.Aggregate(_settings.RenameFormat, (current, magicField) => current.Replace(magicField.Key, magicField.Value.Invoke(episode))));
		}

		private static string BuildDate(DateTime? date, string dateFormula)
		{
			return date.HasValue ? date.Value.ToString(dateFormula) : string.Empty;
		}

		private readonly IReadOnlyDictionary<string, MagicFieldAction> MagicFields;
		private readonly ISettings _settings;
		private readonly IEpisodeStore _episodeStore;
	}

	public delegate bool PromptForRenameAction(string fromFilename, string toFilename);
	public delegate string MagicFieldAction(IEpisode episode);
}
