using KrisHemenway.TVShows.Episodes;
using KrisHemenway.TVShows.Shows;
using Serilog;
using System;
using System.Linq;

namespace KrisHemenway.TVShows.Jobs
{
	public interface IRefreshShowTask
	{
		void Refresh(IShow show);
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

		public void Refresh(IShow show)
		{
			if (!show.MazeId.HasValue)
			{
				return;
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
