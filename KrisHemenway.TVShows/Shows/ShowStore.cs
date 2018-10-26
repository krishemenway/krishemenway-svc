using Dapper;
using KrisHemenway.TVShows.Episodes;
using System;
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
					s.show_id as showid,
					s.name,
					s.maze_id as mazeid,
					s.created_at as created,
					s.updated_at as lastmodified,
					s.path
				FROM show s";

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
					s.show_id as showid,
					s.name,
					s.maze_id as mazeid,
					s.created_at as created,
					s.updated_at as lastmodified,
					s.path
				FROM show s
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
				INSERT INTO show (show_id, name, maze_id, path)
				VALUES (default, @name, @mazeid, @path)
				RETURNING show_id";

			using (var dbConnection = Database.CreateConnection())
			{
				return new Show
				{
					ShowId = dbConnection.Query<Guid>(sql, request).Single(),
					Name = request.Name,
					MazeId = request.MazeId,
					Path = request.Path
				};
			}
		}

		private readonly IEpisodeStore _episodeStore;
	}
}