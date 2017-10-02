
namespace KrisHemenway.CommonCore
{
	public class Credentials
	{
		public string User { get; set; }
		public string Password { get; set; }

		public static Credentials Create(string user, string password)
		{
			return new Credentials
			{
				User = user,
				Password = password
			};
		}

		public override string ToString()
		{
			return User;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 23 + User.GetHashCode();
				hash = hash * 23 + Password.GetHashCode();
				return hash;
			}
		}

		public override bool Equals(object obj)
		{
			var typedObj = obj as Credentials;
			return typedObj != null && User == typedObj.User && Password == typedObj.Password;
		}
	}
}
