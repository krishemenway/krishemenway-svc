using Dapper;
using KrisHemenway.TVShows.Episodes;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.TVShows.Shows
{
	public interface IShowStore
	{
		IReadOnlyList<IShow> FindAll();
		IShow Create(CreateShowRequest request);
	}

	internal class ShowStore : IShowStore
	{
		public ShowStore()
		{
			_episodeStore = new EpisodeStore();
		}

		public IReadOnlyList<IShow> FindAll()
		{
			const string sql = @"
				SELECT
					s.id,
					s.name,
					s.rage_id as rageid,
					s.maze_id as mazeid,
					s.created_at as created,
					s.updated_at as lastmodified,
					s.path
				FROM series s";

			using (var dbConnection = Database.CreateConnection())
			{
				var shows = dbConnection.Query<Show>(sql).ToList();
				var episodesByShow = _episodeStore.FindEpisodes(shows.ToArray());

				foreach (var show in shows)
				{
					show.Episodes = episodesByShow[show];
				}

				return shows;
			}
		}

		public IShow FindOrNull(string name)
		{
			const string sql = @"
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

			using (var dbConnection = Database.CreateConnection())
			{
				var show = dbConnection.QueryFirstOrDefault<Show>(sql, new { name });

				if (show == null)
				{
					return null;
				}

				show.Episodes = _episodeStore.FindEpisodes(show)[show];

				return show;
			}
		}

		public IShow Create(CreateShowRequest request)
		{
			const string sql = @"
				INSERT INTO series (id, name, maze_id, path)
				VALUES (default, @name, @mazeid, @path)
				RETURNING id";

			using (var dbConnection = Database.CreateConnection())
			{
				return new Show
				{
					Id = dbConnection.Query<int>(sql, request).First(),
					Name = request.Name,
					MazeId = request.MazeId,
					Path = request.Path
				};
			}
		}

		private readonly IEpisodeStore _episodeStore;
	}

	public class CreateShowRequest
	{
		public int? MazeId { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
	}
}