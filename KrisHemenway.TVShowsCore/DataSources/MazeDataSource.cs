using KrisHemenway.TVShowsCore.Episodes;
using KrisHemenway.TVShowsCore.Seriess;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace KrisHemenway.TVShowsCore
{
	public interface IMazeDataSource
	{
		List<Episode> FindEpisodes(Series series);
	}

	public class MazeDataSource : IMazeDataSource
	{
		public MazeDataSource()
		{
			_logger = new LoggerFactory().CreateLogger<MazeDataSource>();
		}

		public List<Episode> FindEpisodes(Series series)
		{
			var episodes = new List<Episode>();

			if(!series.MazeId.HasValue)
			{
				return episodes;
			}

			try
			{
				episodes.AddRange(FindEpisodesFromMazeApi(series));
			}
			catch(Exception e)
			{
				_logger.LogError($"Failed to Read Data for Maze {series.MazeId.Value}", e);
			}

			return episodes;
		}

		private IReadOnlyList<Episode> FindEpisodesFromMazeApi(Series series)
		{
			var request = WebRequest.Create($"http://api.tvmaze.com/shows/{series.MazeId}/episodes");
			var response = request.GetResponseAsync();

			response.Wait();
			
			using (var streamReader = new StreamReader(response.Result.GetResponseStream()))
			{
				return JsonConvert.DeserializeObject<IReadOnlyList<MazeTVEpisode>>(streamReader.ReadToEnd())
					.Select(episode => CreateEpisode(episode, series))
					.ToList();
			}
		}

		private Episode CreateEpisode(MazeTVEpisode episode, Series series)
		{
			return new Episode
			{
				Title = episode.Name,
				AirDate = episode.AirDate,
				Season = episode.Season,
				EpisodeInSeason = episode.Number,
				Series = series.Name,
				SeriesId = series.Id
			};
		}

		private readonly ILogger<MazeDataSource> _logger;

	}

	public class MazeTVEpisode
	{
		public int Id { get; set; }

		public string Url { get; set; }
		public string Name { get; set; }

		public int Season { get; set; }
		public int Number { get; set; }

		public int Runtime { get; set; }

		public MazeTVEpisodeImages Image { get; set; }
		public string Summary { get; set; }

		public DateTime AirDate { get; set; }
		public DateTime Airstamp { get; set; }
		public TimeSpan AirTime { get; set; }

	}

	public class MazeTVEpisodeImages
	{
		public string Medium { get; set; }
		public string Original { get; set; }
	}

	public class MazeTVEpisodeRecord
	{
		public string number { get; set; }
		public int season { get; set; }
		public int episode { get; set; }
		public string airdate { get; set; }
		public string title { get; set; }
	}
}