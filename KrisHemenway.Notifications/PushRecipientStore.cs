using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace KrisHemenway.Notifications
{
	public interface IPushRecipientStore
	{
		List<PushRecipient> FindPushRecipients();
		void AddPushRecipient(string deviceToken);
	}

	public class PushRecipientStore : IPushRecipientStore
	{
		public List<PushRecipient> FindPushRecipients()
		{
			const string FindActivePushRecipientsSql = @"
				SELECT
					device_token as devicetoken
				FROM
					public.push_recipient
				WHERE
					deactivated_time IS NULL";

			using (var dbConnection = Database.CreateConnection())
			{
				return dbConnection.Query<PushRecipient>(FindActivePushRecipientsSql).ToList();
			}
		}

		public void AddPushRecipient(string deviceToken)
		{
			const string InsertPushRecipientSql = @"
				INSERT INTO public.push_recipient 
				(device_token)
				SELECT @DeviceToken
				WHERE NOT EXISTS (
					SELECT 1 
					FROM push.push_recipient
					WHERE device_token = @DeviceToken
				)";

			using (var dbConnection = Database.CreateConnection())
			{
				dbConnection.Execute(InsertPushRecipientSql, new { DeviceToken = deviceToken });
			}
		}
	}

	public class PushRecipient
	{
		public string DeviceToken { get; set; }
	}
}