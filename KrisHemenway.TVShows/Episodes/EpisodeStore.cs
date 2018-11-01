using Dapper;
using KrisHemenway.TVShows.Shows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.TVShows.Episodes
{
	public interface IEpisodeStore
	{
		void SaveEpisode(IEpisode episode);
		void UpdateEpisode(IEpisode episode);
		void UpdatePath(IEpisode episode, string path);

		IReadOnlyDictionary<IShow, IReadOnlyList<IEpisode>> FindEpisodes(params IShow[] shows);

		IReadOnlyList<IEpisode> FindNewEpisodes();
		IReadOnlyList<IEpisode> FindEpisodesAiring(DateTime onDate);
		IReadOnlyList<IEpisode> FindEpisodesAiring(DateTime start, DateTime end);
	}

	class EpisodeStore : IEpisodeStore
	{
		public IReadOnlyList<IEpisode> FindEpisodesAiring(DateTime onDate)
		{
			return FindEpisodesAiring(onDate, onDate);
		}

		public IReadOnlyList<IEpisode> FindEpisodesAiring(DateTime start, DateTime end)
		{
			const string sql = @"
				SELECT
					e.episode_id,
					e.title,
					e.season,
					e.episode_in_show as episodeinshow,
					e.episode_in_season as episodeinseason,
					e.airdate,
					e.path as videopath,
					s.show_id as showid,
					s.name as showname,
					e.created_at as created,
					e.updated_at as lastmodified,
					e.path
				FROM episode e
				INNER JOIN show s
					on s.show_id = e.show_id
				WHERE
					airdate >= @Start
					AND airdate <= @End
				ORDER BY
					s.show_id ASC,
					e.season ASC,
					e.episode_in_season ASC";

			using (var dbConnection = Database.CreateConnection())
			{
				return dbConnection.Query<Episode>(sql, new { Start = start, End = end }).ToList();
			}
		}

		public IReadOnlyList<IEpisode> FindNewEpisodes()
		{
			const string sql = @"
				SELECT
					e.episode_id,
					e.title,
					e.season,
					e.episode_in_show as episodeinshow,
					e.episode_in_season as episodeinseason,
					e.airdate,
					e.path as videopath,
					s.show_id as showid,
					s.name as showname,
					e.created_at as created,
					e.updated_at as lastmodified,
					e.path
				FROM episode e
				INNER JOIN show s
					on s.show_id = e.show_id
				WHERE
					e.created_at >= (now() AT TIME ZONE 'UTC') - INTERVAL '24 hours'
					AND e.created_at <= now() AT TIME ZONE 'UTC'
				ORDER BY
					s.show_id ASC,
					e.season ASC,
					e.episode_in_season ASC";

			using (var dbConnection = Database.CreateConnection())
			{
				return dbConnection.Query<Episode>(sql).ToList();
			}
		}

		public IReadOnlyDictionary<IShow, IReadOnlyList<IEpisode>> FindEpisodes(params IShow[] shows)
		{
			const string sql = @"
				SELECT
					e.episode_id,
					e.title,
					e.season,
					e.episode_in_show as episodeinshow,
					e.episode_in_season as episodeinseason,
					e.airdate,
					e.path as videopath,
					s.show_id as showid,
					s.name as showname,
					e.created_at as created,
					e.updated_at as lastmodified,
					e.path
				FROM episode e
				INNER JOIN show s
					on s.show_id = e.show_id
				WHERE
					e.show_id = ANY(@showids)
				ORDER BY
					s.show_id ASC,
					e.season ASC,
					e.episode_in_season ASC";

			using (var dbConnection = Database.CreateConnection())
			{
				var sqlParams = new
					{
						ShowIds = shows.Select(x => x.ShowId).ToList()
					};

				var showsById = shows.ToDictionary(show => show.ShowId, show => show);
				var episodesByShow = shows.ToDictionary(show => show, show => new List<IEpisode>());

				foreach(var episode in dbConnection.Query<Episode>(sql, sqlParams))
				{
					episodesByShow[showsById[episode.ShowId]].Add(episode);
				}

				return episodesByShow.ToDictionary(x => x.Key, x => (IReadOnlyList<IEpisode>)x.Value);
			}
		}

		public void SaveEpisode(IEpisode episode)
		{
			const string sql = @"
				INSERT INTO episode
					(title, season, episode_in_show, episode_in_season, airdate, show_id, created_at, updated_at)
				VALUES
					(@Title, @Season, @EpisodeInShow, @EpisodeInSeason, @AirDate, @ShowId, current_timestamp, current_timestamp)";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(sql, new { episode.Title, episode.Season, episode.EpisodeInShow, episode.EpisodeInSeason, episode.AirDate, episode.ShowId });
			}
		}

		public void UpdateEpisode(IEpisode episode)
		{
			const string sql = @"
				UPDATE episode
				SET 
					title = @Title,
					airdate = @AirDate,
					updated_at = current_timestamp
				WHERE 
					show_id = @ShowId
					AND season = @Season
					AND episode_in_season = @EpisodeInSeason";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(sql, new { episode.ShowId, episode.Season, episode.EpisodeInSeason, episode.Title, episode.AirDate });
			}
		}

		public void UpdatePath(IEpisode episode, string path)
		{
			const string sql = @"
				UPDATE episode
				SET 
					path = @Path,
					updated_at = current_timestamp
				WHERE 
					show_id = @ShowId
					AND season = @Season
					AND episode_in_season = @EpisodeInSeason";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(sql, new { episode.ShowId, episode.Season, episode.EpisodeInSeason, path });
			}
		}
	}
}
