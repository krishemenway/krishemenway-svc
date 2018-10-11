using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.TomatoRouter
{
	public interface IDailyBandwidthStore
	{
		IReadOnlyList<DailyBandwidthUsage> Find(DateTime startDate, DateTime endDate);
		void Create(DailyBandwidthUsage bandwidthUsage);
		void Update(DailyBandwidthUsage bandwidthUsage);
	}

	public class DailyBandwidthStore : IDailyBandwidthStore
	{
		public IReadOnlyList<DailyBandwidthUsage> Find(DateTime startDate, DateTime endDate)
		{
			const string sql = @"
				SELECT
					u.bandwidth_day,
					u.total_kilobytes_downloaded,
					u.total_kilobytes_uplaoded,
				FROM daily_bandwidth_usage u";

			using (var dbConnection = Database.CreateConnection())
			{
				return dbConnection.Query<DailyBandwidthUsage>(sql).ToList();
			}
		}

		public void Create(DailyBandwidthUsage bandwidthUsage)
		{
			const string sql = @"
				INSERT INTO daily_bandwidth_usage 
					(bandwidth_day, total_kilobytes_downloaded, total_kilobytes_uplaoded, created_at, updated_at)
				VALUES
					(@Date, @TotalKilobytesDownloaded, @TotalKilobytesUploaded, current_timestamp, current_timestamp)";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(sql, new { bandwidthUsage.Date, bandwidthUsage.TotalKilobytesDownloaded, bandwidthUsage.TotalKilobytesUploaded });
			}
		}

		public void Update(DailyBandwidthUsage bandwidthUsage)
		{
			const string sql = @"
				UPDATE daily_bandwidth_usage
				SET 
					total_kilobytes_downloaded = @TotalKilobytesDownloaded,
					total_kilobytes_uplaoded = @TotalKilobytesUploaded,
					updated_at = current_timestamp
				WHERE
					bandwidth_day = @Date";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(sql, new { bandwidthUsage.Date, bandwidthUsage.TotalKilobytesDownloaded, bandwidthUsage.TotalKilobytesUploaded });
			}
		}
	}
}
