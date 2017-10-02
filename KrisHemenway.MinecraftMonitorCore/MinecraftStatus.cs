using System;

namespace KrisHemenway.MinecraftMonitorCore
{
	public class MinecraftStatus
	{
		public bool Available { get; set; }
		public int PlayersOnline { get; set; }
		public int MaxPlayers { get; set; }
		public string MessageOfTheDay { get; set; }
		public DateTime StatusTime { get; set; }

		public override bool Equals(object obj)
		{
			var typedObj = obj as MinecraftStatus;
			return typedObj != null
				&& typedObj.Available == Available
				&& typedObj.MaxPlayers == MaxPlayers
				&& typedObj.MessageOfTheDay == MessageOfTheDay
				&& typedObj.PlayersOnline == PlayersOnline;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 23 + PlayersOnline.GetHashCode();
				hash = hash * 23 + MaxPlayers.GetHashCode();
				hash = hash * 23 + MessageOfTheDay.GetHashCode();
				return hash;
			}
		}

		public static readonly MinecraftStatus Default = new MinecraftStatus { PlayersOnline = 0, Available = false };
	}
}