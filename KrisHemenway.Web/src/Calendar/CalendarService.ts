import { EpisodesInMonthResponse } from "Episodes/EpisodesInMonthResponse";
import { Observable } from "@residualeffect/reactor";
import * as moment from "moment";
import { Episode } from "Episodes/Episode";

export class CalendarService {
	constructor() {
		this.EpisodesInMonthByMonthString = new Map<string, Observable<Dictionary<Episode[]>>>();
		this.IsLoadingByMonthKey = new Map<string, Observable<boolean>>();
		this.ViewingMonth = new Observable<moment.Moment>(moment().local());
	}

	public FindOrCreateEpisodesForMonth(month: moment.Moment) : Observable<Dictionary<Episode[]>> {
		const monthKey = month.format("YYYY/MM");
		this.MaybeInitializeObservablesForMonthKey(monthKey);
		return this.ForceFetchEpisodesByDayObservable(monthKey);
	}

	public FindOrCreateCurrentMonthsIsLoading(month: moment.Moment) : Observable<boolean> {
		const monthKey = month.format("YYYY/MM");
		this.MaybeInitializeObservablesForMonthKey(monthKey);
		return this.ForceFetchIsLoadingObservable(monthKey);
	}

	private MaybeInitializeObservablesForMonthKey(monthKey: string) : void {
		let shouldLoad = false;

		if (!this.EpisodesInMonthByMonthString.has(monthKey)) {
			this.EpisodesInMonthByMonthString.set(monthKey, new Observable<Dictionary<Episode[]>>({}));
			shouldLoad = true;
		}

		if (!this.IsLoadingByMonthKey.has(monthKey)) {
			this.IsLoadingByMonthKey.set(monthKey, new Observable<boolean>(true));
			shouldLoad = true;
		}

		if (shouldLoad) {
			jQuery.getJSON(`/api/tvshows/episodes/calendar/${monthKey}`, (response: EpisodesInMonthResponse) => this.HandleEpisodesInMonthResponse(monthKey, response));
		}
	}

	private HandleEpisodesInMonthResponse(monthKey: string, response: EpisodesInMonthResponse) : void {
		var episodesPerDay : Dictionary<Episode[]> = {};

		response.EpisodesInMonth.forEach((episode) => {
			var airDate = moment(episode.AirDate);

			if(episodesPerDay[airDate.format("YYYY-MM-DD")] == null)
			{
				episodesPerDay[airDate.format("YYYY-MM-DD")] = [];
			}

			episodesPerDay[airDate.format("YYYY-MM-DD")].push(episode);
		});

		this.ForceFetchEpisodesByDayObservable(monthKey).Value = episodesPerDay;
		this.ForceFetchIsLoadingObservable(monthKey).Value = false;
	}

	private ForceFetchIsLoadingObservable(monthKey: string) {
		return this.IsLoadingByMonthKey.get(monthKey) as Observable<boolean>;
	}

	private ForceFetchEpisodesByDayObservable(monthKey: string) {
		return this.EpisodesInMonthByMonthString.get(monthKey) as Observable<Dictionary<Episode[]>>;
	}

	public EpisodesInMonthByMonthString: Map<string, Observable<Dictionary<Episode[]>>>;
	public IsLoadingByMonthKey: Map<string, Observable<boolean>>;
	public ViewingMonth: Observable<moment.Moment>;

	public static get Instance(): CalendarService {
		if (CalendarService._instance === undefined) {
			CalendarService._instance = new CalendarService();
		}

		return CalendarService._instance;
	}

	private static _instance: CalendarService;
}