using KrisHemenway.TVShows.Episodes;
using KrisHemenway.TVShows.Shows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace KrisHemenway.TVShows
{
	public interface IMazeDataSource
	{
		List<IEpisode> FindEpisodes(IShow show);
	}

	public class MazeDataSource : IMazeDataSource
	{
		public List<IEpisode> FindEpisodes(IShow show)
		{
			var episodes = new List<IEpisode>();

			if(!show.MazeId.HasValue)
			{
				return episodes;
			}

			try
			{
				episodes.AddRange(FindEpisodesFromMazeApi(show));
			}
			catch(Exception exception)
			{
				throw new Exception($"Failed to fetch episode data for show {show.MazeId.Value}", exception);
			}

			return episodes;
		}

		private IReadOnlyList<Episode> FindEpisodesFromMazeApi(IShow show)
		{
			var request = WebRequest.Create($"http://api.tvmaze.com/shows/{show.MazeId}/episodes");
			var response = request.GetResponseAsync();

			response.Wait();
			
			using (var streamReader = new StreamReader(response.Result.GetResponseStream()))
			{
				return JsonConvert.DeserializeObject<IReadOnlyList<MazeTVEpisode>>(streamReader.ReadToEnd())
					.Select(episode => CreateEpisode(episode, show))
					.ToList();
			}
		}

		private Episode CreateEpisode(MazeTVEpisode episode, IShow show)
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

		public DateTime? AirDate { get; set; }
		public DateTime? Airstamp { get; set; }
		public TimeSpan? AirTime { get; set; }

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