using Dapper;
using KrisHemenway.TVShows.Episodes;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.TVShows.Seriess
{
	public interface ISeriesStore
	{
		IReadOnlyList<IShow> FindAll();
		IShow Create(CreateSeriesRequest request);
	}

	internal class ShowStore : ISeriesStore
	{
		public IReadOnlyList<IShow> FindAll()
		{
			const string seriesSql = @"
				SELECT
					s.id,
					s.name,
					s.rage_id as rageid,
					s.maze_id as mazeid,
					s.created_at as created,
					s.updated_at as lastmodified,
					s.path
				FROM series s";

			const string episodesSql = @"
				SELECT
					e.id,
					e.title,
					e.season,
					e.episode_number as episodeinseries,
					e.episode_in_season as episodeinseason,
					e.airdate,
					s.id as seriesid,
					s.name as series,
					e.rage_url as rageurl,
					e.created_at as created,
					e.updated_at as lastmodified,
					e.path
				FROM episodes e
				INNER JOIN series s
					on e.series_id = s.id";

			using (var dbConnection = Database.CreateConnection())
			{
				var seriesById = dbConnection.Query<Show>(seriesSql).ToDictionary(x => x.Id, x => x);
				var allEpisodes = dbConnection.Query<Episode>(episodesSql).ToList();

				foreach (var episode in allEpisodes)
				{
					seriesById[episode.SeriesId].Episodes.Add(episode);
				}

				return seriesById.Select(x => x.Value).ToList();
			}
		}

		public IShow FindOrNull(string name)
		{
			const string seriesSql = @"
				SELECT
					s.id,
					s.name,
					s.rage_id as rageid,
					s.maze_id as mazeid,
					s.created_at as created,
					s.updated_at as lastmodified,
					s.path
				FROM series s
				WHERE s.name = @Name";

			const string episodesSql = @"
				SELECT
					e.id,
					e.title,
					e.season,
					e.episode_number as episodeinseries,
					e.episode_in_season as episodeinseason,
					e.airdate,
					s.id as seriesid,
					s.name as series,
					e.rage_url as rageurl,
					e.created_at as created,
					e.updated_at as lastmodified,
					e.path
				FROM episodes e
				INNER JOIN series s
					on e.series_id = s.id
				WHERE s.id = @SeriesId";

			using (var dbConnection = Database.CreateConnection())
			{
				var series = dbConnection.QueryFirstOrDefault<Show>(seriesSql, new { name });

				if (series == null)
				{
					return null;
				}

				series.Episodes = dbConnection.Query<Episode>(episodesSql, new { SeriesId = series.Id }).ToList();
				return series;
			}
		}

		public IShow Create(CreateSeriesRequest request)
		{
			const string sql = @"
				INSERT INTO series (id, name, rage_id, maze_id, path)
				VALUES (default, @name, @rageid, @mazeid, @path)
				RETURNING id";

			using (var dbConnection = Database.CreateConnection())
			{
				return new Show
				{
					Id = dbConnection.Query<int>(sql, request).First(),
					Name = request.Name,
					MazeId = request.MazeId,
					Path = request.Path,
					RageId = request.RageId
				};
			}

		}
	}

	public class CreateSeriesRequest
	{
		public int? MazeId { get; set; }
		public int? RageId { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
	}
}