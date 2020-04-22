import * as moment from "moment";
import { Observable } from "@residualeffect/reactor";
import { Episode } from "Episodes/Episode";
import { EpisodesInMonthResponse } from "Episodes/EpisodesInMonthResponse";


export class ObservableCalendarMonth {
	constructor(date: moment.Moment) {
		this.Date = date;
		this.EpisodesInMonth = new Observable({});
		this.IsLoading = new Observable(false);

		this.TryLoadEpisodesInMonth();
	}

	public TryLoadEpisodesInMonth() {
		this.IsLoading.Value = true;
		fetch(`/api/tvshows/episodes/calendar/${this.Date.format("YYYY/MM")}`)
			.then((response: Response) => response.json())
			.then((response: EpisodesInMonthResponse) => { this.HandleEpisodesInMonthResponse(response); this.IsLoading.Value = false; })
	}

	private HandleEpisodesInMonthResponse(response: EpisodesInMonthResponse) : void {
		var episodesPerDay : Dictionary<Episode[]> = {};

		response.EpisodesInMonth.forEach((episode) => {
			var airDate = moment(episode.AirDate);

			if(episodesPerDay[airDate.format("YYYY-MM-DD")] == null)
			{
				episodesPerDay[airDate.format("YYYY-MM-DD")] = [];
			}

			episodesPerDay[airDate.format("YYYY-MM-DD")].push(episode);
		});

		this.EpisodesInMonth.Value = episodesPerDay;
		this.IsLoading.Value = false;
	}

	public readonly Date: moment.Moment;
	public readonly EpisodesInMonth: Observable<Dictionary<Episode[]>>;
	public readonly IsLoading: Observable<boolean>;
}