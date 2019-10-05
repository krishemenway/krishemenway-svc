namespace KrisHemenway.Common
{
	public class Result
	{
		public bool Success { get; private set; }
		public string ErrorMessage { get; private set; }

		public static Result Failure(string error)
		{
			return new Result { Success = false, ErrorMessage = error };
		}

		public static readonly Result Successful = new Result { Success = true, ErrorMessage = null };
	}
}
