using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KrisHemenway.TVShows.Episodes
{
	public class DownloadAuthenticationRequiredAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!HasAuthenticated(context.HttpContext.Session))
			{
				context.Result = new NotFoundResult();
			}
		}

		internal static bool HasAuthenticated(ISession session)
		{
			return session.GetString("HasAuthenticatedForDownload") == "true";
		}

		internal static void SetAuthenticated(ISession session)
		{
			session.SetString("HasAuthenticatedForDownload", "true");
		}
	}
}
