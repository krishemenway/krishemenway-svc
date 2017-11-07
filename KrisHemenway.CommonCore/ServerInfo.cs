
namespace KrisHemenway.Common
{
	public class ServerInfo
	{
		public string Host { get; set; }
		public int Port { get; set; }

		public static ServerInfo Create(string host, int port)
		{
			return new ServerInfo
			{
				Host = host,
				Port = port
			};
		}

		public override string ToString()
		{
			return $"{Host}:{Port}";
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 23 + Host.GetHashCode();
				hash = hash * 23 + Port.GetHashCode();
				return hash;
			}
		}

		public override bool Equals(object obj)
		{
			var typedObj = obj as ServerInfo;
			return typedObj != null && this == typedObj;
		}

		public static bool operator ==(ServerInfo a, ServerInfo b)
		{
			if (ReferenceEquals(a, b)) // if both are null, or both are same instance
			{
				return true;
			}

			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}

			return a.Host == b.Host && a.Port == b.Port;
		}

		public static bool operator !=(ServerInfo a, ServerInfo b)
		{
			return !(a == b);
		}
	}
}
