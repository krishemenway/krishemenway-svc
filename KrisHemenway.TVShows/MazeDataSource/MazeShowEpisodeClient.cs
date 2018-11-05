using KrisHemenway.Common;
using KrisHemenway.TVShows.Episodes;
using KrisHemenway.TVShows.Shows;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace KrisHemenway.TVShows
{
	public interface IMazeDataSource
	{
		Result<IReadOnlyList<IEpisode>> FindEpisodes(IShow show);
	}

	public class MazeShowEpisodeClient : IMazeDataSource
	{
		public Result<IReadOnlyList<IEpisode>> FindEpisodes(IShow show)
		{
			if(!show.MazeId.HasValue)
			{
				return Result<IReadOnlyList<IEpisode>>.Failure($"Failed to fetch episode data for show {show.Name} since because it is missing a MazeId");
			}

			try
			{
				return Result<IReadOnlyList<IEpisode>>.Successful(FindEpisodesFromMazeApi(show));
			}
			catch(Exception exception)
			{
				Log.Error($"Failed to fetch episode data for show {show.MazeId.Value}", exception);
				return Result<IReadOnlyList<IEpisode>>.Failure($"Failed to fetch episode data for show {show.MazeId.Value}");
			}
		}

		private IReadOnlyList<IEpisode> FindEpisodesFromMazeApi(IShow show)
		{
			var request = WebRequest.Create($"http://api.tvmaze.com/shows/{show.MazeId}/episodes");
			var response = request.GetResponseAsync();

			response.Wait();
			
			using (var streamReader = new StreamReader(response.Result.GetResponseStream()))
			{
				return JsonConvert.DeserializeObject<IReadOnlyList<MazeShowEpisode>>(streamReader.ReadToEnd())
					.Select(episode => CreateEpisode(episode, show))
					.ToList();
			}
		}

		private Episode CreateEpisode(MazeShowEpisode episode, IShow show)
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