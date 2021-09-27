using KrisHemenway.TVShows.Episodes;
using KrisHemenway.TVShows.Shows;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace KrisHemenway.TVShows.MazeDataSource
{
	public interface IMazeDataSource
	{
		Task<Result<IReadOnlyList<IEpisode>>> FindEpisodes(IShow show);
	}

	public class MazeShowEpisodeClient : IMazeDataSource
	{
		public async Task<Result<IReadOnlyList<IEpisode>>> FindEpisodes(IShow show)
		{
			if(!show.MazeId.HasValue)
			{
				return Result<IReadOnlyList<IEpisode>>.Failure($"Failed to fetch episode data for show {show.Name} since because it is missing a MazeId");
			}

			try
			{
				return Result<IReadOnlyList<IEpisode>>.Successful(await FindEpisodesFromMazeApi(show));
			}
			catch(Exception exception)
			{
				Log.Error("Failed to fetch episode data for show {MazeId}: {@Exception}", show.MazeId.Value, exception);
				return Result<IReadOnlyList<IEpisode>>.Failure($"Failed to fetch episode data for show {show.MazeId.Value}");
			}
		}

		private async Task<IReadOnlyList<IEpisode>> FindEpisodesFromMazeApi(IShow show)
		{
			var request = WebRequest.Create($"http://api.tvmaze.com/shows/{show.MazeId}/episodes");

			using (var response = await request.GetResponseAsync())
			using (var streamReader = new StreamReader(response.GetResponseStream()))
			{
				var responseString = streamReader.ReadToEnd();
				var mazeEpisodes = JsonSerializer.Deserialize<IReadOnlyList<MazeShowEpisode>>(responseString);

				return mazeEpisodes.Select(episode => CreateEpisode(episode, show)).ToList();
			}
		}

		private static Episode CreateEpisode(MazeShowEpisode episode, IShow show)
		{
			return new Episode
				{
					Title = episode.Name,
					AirDate = episode.AirDate,
					Season = episode.Season,
					EpisodeInSeason = episode.Number,
					ShowName = show.Name,
					ShowId = show.ShowId
				};
		}
	}
}