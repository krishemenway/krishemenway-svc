using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.TVShowsCore.Episodes
{
	public interface IEpisodeStore
	{
		void SaveEpisode(Episode episode);
		void UpdateEpisode(Episode episode);

		IReadOnlyList<Episode> FindNewEpisodes();
		IReadOnlyList<Episode> FindEpisodesAiring(DateTime onDate);
		IReadOnlyList<Episode> FindEpisodesAiring(DateTime start, DateTime end);
	}

	class EpisodeStore : IEpisodeStore
	{
		public IReadOnlyList<Episode> FindEpisodesAiring(DateTime onDate)
		{
			return FindEpisodesAiring(onDate, onDate);
		}

		public IReadOnlyList<Episode> FindEpisodesAiring(DateTime start, DateTime end)
		{
			const string sql = @"
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

		public IReadOnlyList<Episode> FindNewEpisodes()
		{
			const string sql = @"
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

		public void SaveEpisode(Episode episode)
		{
			const string sql = @"
				INSERT INTO episodes 
					(title, season, episode_number, episode_in_season, airdate, series_id, created_at, updated_at)
				VALUES
					(@Title, @Season, @EpisodeNumber, @EpisodeInSeason, @AirDate, @SeriesId, current_timestamp, current_timestamp)";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(sql, new { episode.Title, episode.Season, episode.EpisodeNumber, episode.EpisodeInSeason, episode.AirDate, episode.SeriesId });
			}
		}

		public void UpdateEpisode(Episode episode)
		{
			const string sql = @"
				UPDATE episodes
				SET 
					title = @Title,
					airdate = @AirDate,
					updated_at = current_timestamp
				WHERE 
					series_id = @SeriesId
					AND season = @Season
					AND episode_in_season = @EpisodeInSeason";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(sql, new { episode.SeriesId, episode.Season, episode.EpisodeInSeason, episode.Title, episode.AirDate });
			}
		}
	}
}
