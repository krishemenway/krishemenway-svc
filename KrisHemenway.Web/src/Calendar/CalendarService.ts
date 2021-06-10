import { Observable } from "@residualeffect/reactor";
import * as moment from "moment";
import { EpisodesInMonth } from "Calendar/EpisodesInMonth";
import { Loadable } from "Common/Loadable";
import { Http } from "Common/Http";
import { EpisodesInMonthResponse } from "Episodes/EpisodesInMonthResponse";
import { Episode } from "Episodes/Episode";

export class CalendarService {
	constructor() {
		this.MonthOfEpisodes = {};
		this.ViewingMonth = new Observable(moment());
	}

	public ChangeMonth(date: moment.Moment): void {
		this.ViewingMonth.Value = date;
		this.AddBoundaryMonthOfEpisodes();
	}

	public FindOrCreateMonthOfEpisodes(dayInMonth: moment.Moment): Loadable<EpisodesInMonth> {
		const monthKey = dayInMonth.format("YYYY-MM");

		if (this.MonthOfEpisodes[monthKey] === undefined) {
			this.MonthOfEpisodes[monthKey] = new Loadable();
			this.TryLoadEpisodesInMonth(dayInMonth, this.MonthOfEpisodes[monthKey]);
		}

		return this.MonthOfEpisodes[monthKey];
	}

	private CreateMonthOfEpisodes(date: moment.Moment, response: EpisodesInMonthResponse) : EpisodesInMonth {
		return {
			Date: date,
			EpisodesInMonth: response.EpisodesInMonth.reduce((episodesPerDay, episode) => {
				var airDate = moment(episode.AirDate);
	
				if(episodesPerDay[airDate.format("YYYY-MM-DD")] == null)
				{
					episodesPerDay[airDate.format("YYYY-MM-DD")] = [];
				}
	
				episodesPerDay[airDate.format("YYYY-MM-DD")].push(episode);
	
				return episodesPerDay;
			}, {} as Dictionary<Episode[]>),
		};
	}

	private AddBoundaryMonthOfEpisodes() {
		this.FindOrCreateMonthOfEpisodes(this.ViewingMonth.Value.clone().add(1, 'month'));
		this.FindOrCreateMonthOfEpisodes(this.ViewingMonth.Value.clone().subtract(1, 'month'));
	}

	private TryLoadEpisodesInMonth(dayInMonth: moment.Moment, loadable: Loadable<EpisodesInMonth>) {
		Http.get<EpisodesInMonthResponse, EpisodesInMonth>(`/api/tvshows/episodes/calendar/${dayInMonth.format("YYYY/MM")}`, loadable, (response) => this.CreateMonthOfEpisodes(dayInMonth, response));
	}

	public MonthOfEpisodes: Dictionary<Loadable<EpisodesInMonth>>;
	public ViewingMonth: Observable<moment.Moment>;

	public static get Instance(): CalendarService {
		if (CalendarService._instance === undefined) {
			CalendarService._instance = new CalendarService();
		}

		return CalendarService._instance;
	}

	private static _instance: CalendarService;
}