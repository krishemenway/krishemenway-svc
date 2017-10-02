using KrisHemenway.TVShowsCore.Episodes;
using KrisHemenway.TVShowsCore.Seriess;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace KrisHemenway.TVShowsCore.Jobs
{
	public interface IRefreshSeriesTask
	{
		void Refresh(Series series);
	}

	public class RefreshSeriesTask : IRefreshSeriesTask
	{
		public RefreshSeriesTask()
		{
			_episodeStore = new EpisodeStore();
			_logger = new LoggerFactory().CreateLogger<RefreshTVShowsJob>();
			_mazeDataSource = new MazeDataSource();
		}

		public void Refresh(Series series)
		{
			if (!series.MazeId.HasValue)
			{
				return;
			}

			foreach (var episode in _mazeDataSource.FindEpisodes(series))
			{
				var existingEpisode = series.Episodes.SingleOrDefault(x => x.Season == episode.Season && x.EpisodeInSeason == episode.EpisodeInSeason);

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

		private void CreateEpisode(Episode episode)
		{
			_logger.LogDebug($"Creating Episode: {episode}");
			_episodeStore.SaveEpisode(episode);
		}

		private void UpdateEpisode(Episode episode)
		{
			_logger.LogDebug($"Updating Episode: {episode}");
			_episodeStore.UpdateEpisode(episode);
		}

		private static bool ShouldUpdateEpisode(Episode exisingEpisode, Episode newEpisode)
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

		private readonly EpisodeStore _episodeStore;
		private readonly ILogger<RefreshTVShowsJob> _logger;
		private MazeDataSource _mazeDataSource;
	}
}
