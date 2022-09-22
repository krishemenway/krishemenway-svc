using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using KrisHemenway.TVShows.Shows;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;

namespace KrisHemenway.TVShows.Episodes
{
	[ApiController]
	[Route("api/tvshows")]
	public class EpisodesCalendarController : ControllerBase
	{
		public EpisodesCalendarController(
			IShowStore showStore = null)
		{
			_showStore = showStore ?? new ShowStore();
		}

		[HttpGet("episodes/calendar.ics")]
		public FileContentResult Calendar()
		{
			var calendar = new Calendar();

			foreach (var episode in _showStore.FindAll().SelectMany((s) => s.Episodes))
			{
				if (!episode.AirDate.HasValue)
				{
					continue;
				}

				var calendarEvent = new CalendarEvent
				{
					Uid = $"{episode.ShowName}-{episode.Season}-{episode.EpisodeInSeason}",

					Created = new CalDateTime(episode.Created),
					LastModified = new CalDateTime(episode.LastModified),

					Summary = episode.Formatted,

					Start = new CalDateTime(episode.AirDate.Value),
					End = new CalDateTime(episode.AirDate.Value),
					IsAllDay = true,
				};

				calendar.Events.Add(calendarEvent);
			}

			var serializedCalendar = new CalendarSerializer().SerializeToString(calendar);
			var fileContents = Encoding.UTF8.GetBytes(serializedCalendar);

			return File(fileContents, ContentType, FileName);
		}

		private readonly IShowStore _showStore;

		private const string ContentType = "text/calendar";
		private const string FileName = "calendar.ics";
	}
}
