using KrisHemenway.Common;
using KrisHemenway.TVShows.Episodes;
using KrisHemenway.TVShows.Shows;
using Serilog;
using System;
using System.Linq;

namespace KrisHemenway.TVShows.Jobs
{
	public interface IRefreshShowTask
	{
		Result Refresh(IShow show);
	}

	public class RefreshShowTask : IRefreshShowTask
	{
		public RefreshShowTask(
			IEpisodeStore episodeStore = null,
			IMazeDataSource mazeDataSource = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
			_mazeDataSource = mazeDataSource ?? new MazeDataSource();
		}

		public Result Refresh(IShow show)
		{
			if (!show.MazeId.HasValue)
			{
				return Result.Failure($"Could not refresh show {show.Name} because it was missing a maze id");
			}

			foreach (var episode in _mazeDataSource.FindEpisodes(show))
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

			return Result.Successful;
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
		private readonly IMazeDataSource _mazeDataSource;
	}
}
