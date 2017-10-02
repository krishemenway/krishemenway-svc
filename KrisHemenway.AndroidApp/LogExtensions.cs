using System;

namespace KrisHemenway.AndroidApp
{
	public static class Log
	{
		public static void Error(Exception exception)
		{
			Android.Util.Log.Error("KH", $"{exception.Message}\n{exception.StackTrace}");
		}
	}
}