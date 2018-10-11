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
					e.id,
					e.title,
					e.season,
					e.episode_number as episodenumber,
					e.episode_in_season as episodeinseason,
					e.airdate,
					e.video_path as videopath,
					s.id as showid,
					s.name as showname,
					e.created_at as created,
					e.updated_at as lastmodified,
					e.path
				FROM episodes e
				INNER JOIN series s
					on s.id = e.series_id
				WHERE
					airdate >= @Start
					AND airdate <= @End
				ORDER BY s.id ASC, e.season ASC, e.episode_in_season ASC";

			using (var dbConnection = Database.CreateConnection())
			{
				return dbConnection.Query<Episode>(sql, new { Start = start, End = end }).ToList();
			}
		}

		public IReadOnlyList<IEpisode> FindNewEpisodes()
		{
			const string sql = @"
				SELECT
					e.id,
					e.title,
					e.season,
					e.episode_number as episodenumber,
					e.episode_in_season as episodeinseason,
					e.airdate,
					e.video_path as videopath,
					s.id as showid,
					s.name as showname,
					e.created_at as created,
					e.updated_at as lastmodified,
					e.path
				FROM episodes e
				INNER JOIN series s
					on s.id = e.series_id
				WHERE
					e.created_at >= (now() AT TIME ZONE 'UTC') - INTERVAL '24 hours'
					AND e.created_at <= now() AT TIME ZONE 'UTC'
				ORDER BY s.id ASC, e.season ASC, e.episode_in_season ASC";

			using (var dbConnection = Database.CreateConnection())
			{
				return dbConnection.Query<Episode>(sql).ToList();
			}
		}

		public IReadOnlyDictionary<IShow, IReadOnlyList<IEpisode>> FindEpisodes(params IShow[] shows)
		{
			const string sql = @"
				SELECT
					e.id,
					e.title,
					e.season,
					e.episode_number as episodenumber,
					e.episode_in_season as episodeinseason,
					e.airdate,
					e.video_path as videopath,
					s.id as showid,
					s.name as showname,
					e.created_at as created,
					e.updated_at as lastmodified,
					e.path
				FROM episodes e
				INNER JOIN series s
					on s.id = e.series_id
				WHERE
					e.series_id IN @showids
				ORDER BY s.id ASC, e.season ASC, e.episode_in_season ASC";

			using (var dbConnection = Database.CreateConnection())
			{
				var sqlParams = new
					{
						ShowIds = shows.Select(x => x.Id).ToList()
					};

				var showsById = shows.ToDictionary(x => x.Id, x => x);
				var episodesByShow = shows.ToDictionary(x => x, x => new List<IEpisode>());

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
				INSERT INTO episodes 
					(title, season, episode_number, episode_in_season, airdate, series_id, created_at, updated_at)
				VALUES
					(@Title, @Season, @EpisodeNumber, @EpisodeInSeason, @AirDate, @ShowId, current_timestamp, current_timestamp)";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(sql, new { episode.Title, episode.Season, episode.EpisodeNumber, episode.EpisodeInSeason, episode.AirDate, episode.ShowId });
			}
		}

		public void UpdateEpisode(IEpisode episode)
		{
			const string sql = @"
				UPDATE episodes
				SET 
					title = @Title,
					airdate = @AirDate,
					updated_at = current_timestamp
				WHERE 
					series_id = @ShowId
					AND season = @Season
					AND episode_in_season = @EpisodeInSeason";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(sql, new { episode.ShowId, episode.Season, episode.EpisodeInSeason, episode.Title, episode.AirDate });
			}
		}
	}
}
