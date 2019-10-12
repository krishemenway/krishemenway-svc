using Dapper;
using KrisHemenway.TVShows.Episodes;
using StronglyTyped.GuidIds;
using StronglyTyped.GuidIds.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.TVShows.Shows
{
	public interface IShowStore
	{
		IReadOnlyList<IShow> FindAll();
		IShow Create(CreateShowRequest request);

		bool TryFindByName(string name, out IShow show);
		bool TryFindByPath(IReadOnlyList<string> paths, out IShow show);
	}

	internal class ShowStore : IShowStore
	{
		static ShowStore()
		{
			TypeHandlerForIdOf<Show>.Register();
		}

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
				if (_cachedShows != null && _cachedShows.Any())
				{
					return _cachedShows;
				}

				var shows = dbConnection.Query<Show>(sql).ToList();
				var episodesByShow = _episodeStore.FindEpisodes(shows.ToArray());

				foreach (var show in shows)
				{
					show.Episodes = episodesByShow[show];
				}

				_cachedShows = shows;
				return shows;
			}
		}

		public bool TryFindByPath(IReadOnlyList<string> paths, out IShow show)
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
				WHERE lower(s.path) = ANY(@Paths)";

			using (var dbConnection = Database.CreateConnection())
			{
				var writableShow = dbConnection.QueryFirstOrDefault<Show>(sql, new { Paths = paths.Select(x => x.ToLower()).ToList() });
				show = writableShow;

				if (show == null)
				{
					return false;
				}

				writableShow.Episodes = _episodeStore.FindEpisodes(show)[show];
				return show != null;
			}
		}

		public bool TryFindByName(string name, out IShow show)
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
				var writableShow = dbConnection.QueryFirstOrDefault<Show>(sql, new { name });
				show = writableShow;

				if (show == null)
				{
					return false;
				}

				writableShow.Episodes = _episodeStore.FindEpisodes(show)[show];
				return show != null;
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
						ShowId = dbConnection.Query<Id<Show>>(sql, request).Single(),
						Name = request.Name,
						MazeId = request.MazeId,
						Path = request.Path,
					};
			}
		}

		[ThreadStatic]
		private static IReadOnlyList<IShow> _cachedShows;

		private readonly IEpisodeStore _episodeStore;
	}
}