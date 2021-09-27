using KrisHemenway.Common;
using KrisHemenway.TVShows.Episodes;
using KrisHemenway.TVShows.MazeDataSource;
using KrisHemenway.TVShows.Shows;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KrisHemenway.TVShows.Jobs
{
	public interface IRefreshShowTask
	{
		Task<Result> Refresh(IShow show);
	}

	public class RefreshShowTask : IRefreshShowTask
	{
		public RefreshShowTask(
			IEpisodeStore episodeStore = null,
			IMazeDataSource mazeShowEpisodeClient = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
			_mazeShowEpisodeClient = mazeShowEpisodeClient ?? new MazeShowEpisodeClient();
		}

		public async Task<Result> Refresh(IShow show)
		{
			if (!show.MazeId.HasValue)
			{
				return Result.Failure($"Could not refresh show {show.Name} because it was missing a maze id");
			}

			var episodesResult = await _mazeShowEpisodeClient.FindEpisodes(show);

			if (!episodesResult.Success)
			{
				return Result.Failure(episodesResult.ErrorMessage);
			}

			CreateOrUpdateEpisodes(show, episodesResult);

			return Result.Successful;
		}

		private void CreateOrUpdateEpisodes(IShow show, Result<System.Collections.Generic.IReadOnlyList<IEpisode>> episodesResult)
		{
			foreach (var episode in episodesResult.Data)
			{
				CreateOrUpdateEpisode(show, episode);
			}
		}

		private void CreateOrUpdateEpisode(IShow show, IEpisode episode)
		{
			var existingEpisode = show.Episodes.SingleOrDefault(x => x.Season == episode.Season && x.EpisodeInSeason == episode.EpisodeInSeason);

			if (existingEpisode == null)
			{
				CreateEpisode(episode);
			}
			else if (ShouldUpdateEpisode(existingEpisode, episode))
			{
				UpdateEpisode(episode);
			}
		}

		private void CreateEpisode(IEpisode episode)
		{
			Log.Information("Creating Episode: {Episode}", episode);
			_episodeStore.SaveEpisode(episode);
		}

		private void UpdateEpisode(IEpisode episode)
		{
			Log.Information("Updating Episode: {Episode}", episode);
			_episodeStore.UpdateEpisode(episode);
		}

		private static bool ShouldUpdateEpisode(IEpisode exisingEpisode, IEpisode newEpisode)
		{
			if (newEpisode.AirDate == default(DateTime) || newEpisode.AirDate == DateTime.MinValue || newEpisode.AirDate == DateTime.MaxValue)
			{
				return false;
			}

			if (exisingEpisode.Title != newEpisode.Title || exisingEpisode.AirDate != newEpisode.AirDate)
			{
				return true;
			}

			return false;
		}

		private readonly IEpisodeStore _episodeStore;
		private readonly IMazeDataSource _mazeShowEpisodeClient;
	}
}
