using System;

namespace KrisHemenway.Common
{
	public class UniqueId
	{
		public UniqueId(Guid uniqueId)
		{
			_uniqueId = uniqueId;
		}

		public static implicit operator Guid(UniqueId uniqueId)
		{
			return uniqueId._uniqueId;
		}

		public static implicit operator UniqueId(Guid guid)
		{
			return new UniqueId(guid);
		}

		public static bool operator ==(UniqueId uniqueIdOne, UniqueId uniqueIdTwo)
		{
			return uniqueIdOne.Equals(uniqueIdTwo);
		}

		public static bool operator !=(UniqueId uniqueIdOne, UniqueId uniqueIdTwo)
		{
			return !(uniqueIdOne == uniqueIdTwo);
		}
		
		public override bool Equals(object obj)
		{
			return _uniqueId.Equals(obj);
		}

		public override int GetHashCode()
		{
			return _uniqueId.GetHashCode();
		}

		public override string ToString()
		{
			return _uniqueId.ToString();
		}

		public readonly Guid _uniqueId;
	}
}
