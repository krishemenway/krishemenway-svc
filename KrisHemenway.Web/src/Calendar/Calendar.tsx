import * as React from "react";
import * as reactDom from "react-dom";
import * as moment from "moment";
import { Episode } from "../Episodes/Episode";
import { EpisodesInMonthResponse } from "./EpisodesInMonthResponse";
import { SeriesCalendar } from "./SeriesCalendar";

export interface CalendarState {
	ShowDownload: boolean;
	CurrentMonth: moment.Moment;
	EpisodesPerDay: HashTable<Array<Episode>>;
}

interface CalendarProps { }

export class Calendar extends React.Component<CalendarProps, CalendarState> {
	constructor(props: CalendarProps) {
		super(props);

		this.state = {
			ShowDownload: false,
			CurrentMonth: moment().local(),
			EpisodesPerDay: {}
		};
	}

	public componentDidMount() {
		this.loadEpisodesForMonth(this.state.CurrentMonth);
		this.preloadBorderMonths();
	}

	public render() {
		return (
			<SeriesCalendar CalendarState={this.state} OnChangeMonth={this.onChangeMonth} />
		)
	}

	private preloadBorderMonths = () : void => {
		this.loadEpisodesForMonth(this.state.CurrentMonth.clone().subtract(1, "month"));
		this.loadEpisodesForMonth(this.state.CurrentMonth.clone().add(1, 'month'));
	}

	private loadEpisodesForMonth = (date: moment.Moment) : void => {
		if (this.state.EpisodesPerDay[date.format("YYYY-MM-01")] == null) {
			this.state.EpisodesPerDay[date.format("YYYY-MM-01")] = [];
			$.getJSON("/api/tvshows/episodes/calendar/" + date.format("YYYY/MM"), this.onReceivedCalendarData);
		}
	}

	private onReceivedCalendarData = (data: EpisodesInMonthResponse, status: string, request: XMLHttpRequest) : void => {
		var episodesPerDay : HashTable<Array<Episode>> = this.state.EpisodesPerDay;

		data.EpisodesInMonth.forEach((episode) => {
			var airDate = moment(episode.AirDate);

			if(episodesPerDay[airDate.format("YYYY-MM-DD")] == null)
			{
				episodesPerDay[airDate.format("YYYY-MM-DD")] = [];
			}

			episodesPerDay[airDate.format("YYYY-MM-DD")].push(episode);
		});

		this.setState({ EpisodesPerDay: episodesPerDay, ShowDownload: data.ShowDownload });
	}
	
	private onChangeMonth = (newMonth: moment.Moment) : void => {
		this.setState({ CurrentMonth: newMonth });
		this.preloadBorderMonths();
	}
}

(window as any).initialize = () => {
	reactDom.render(<Calendar />, document.getElementById('app'));
}
