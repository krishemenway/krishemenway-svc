import * as React from "react";
import * as reactDom from 'react-dom';
import * as moment from "moment";
import { Episode, EpisodesInMonthResponse } from "./EpisodesInMonthResponse";
import { SeriesCalendar } from "./SeriesCalendar";

export interface CalendarState {
	CurrentMonth: moment.Moment;
	EpisodesPerDay: HashTable<Array<Episode>>;
}

interface GlobalAppState {
	CalendarState: CalendarState;
}

export class Calendar extends React.Component<{}, GlobalAppState> {
	constructor() {
		super();

		this.state = {
			CalendarState: {
				CurrentMonth: moment().local(),
				EpisodesPerDay: {}
			}
		};
	}

	public componentDidMount() {
		this.loadEpisodesForMonth(this.state.CalendarState.CurrentMonth);
		this.preloadBorderMonths();
	}

	public render() {
		return (
			<SeriesCalendar CalendarState={this.state.CalendarState} OnChangeMonth={this.onChangeMonth} />
		)
	}

	private preloadBorderMonths = () : void => {
		this.loadEpisodesForMonth(this.state.CalendarState.CurrentMonth.clone().subtract(1, "month"));
		this.loadEpisodesForMonth(this.state.CalendarState.CurrentMonth.clone().add(1, 'month'));
	}

	private loadEpisodesForMonth = (date: moment.Moment) : void => {
		if(this.state.CalendarState.EpisodesPerDay[date.format("YYYY-MM-01")] == null)
		{
			this.state.CalendarState.EpisodesPerDay[date.format("YYYY-MM-01")] = [];
			$.getJSON("/api/tvshows/episodes/calendar/" + date.format("YYYY/MM"), this.onReceivedCalendarData);
		}
	}

	private onReceivedCalendarData = (data: EpisodesInMonthResponse, status: string, request: XMLHttpRequest) : void => {
		var episodesPerDay : HashTable<Array<Episode>> = this.state.CalendarState.EpisodesPerDay;

		data.EpisodesInMonth.forEach((episode) => {
			var airDate = moment(episode.AirDate);

			if(episodesPerDay[airDate.format("YYYY-MM-DD")] == null)
			{
				episodesPerDay[airDate.format("YYYY-MM-DD")] = [];
			}

			episodesPerDay[airDate.format("YYYY-MM-DD")].push(episode);
		});

		this.setState({
			CalendarState: {
				CurrentMonth: this.state.CalendarState.CurrentMonth,
				EpisodesPerDay: episodesPerDay
			}
		});
	}
	
	private onChangeMonth = (newMonth: moment.Moment) : void => {
		this.setState({
			CalendarState: { 
				CurrentMonth: newMonth,
				EpisodesPerDay: this.state.CalendarState.EpisodesPerDay
			}
		});

		this.preloadBorderMonths();
	}
}

(window as any).initialize = () => {
	reactDom.render(<Calendar />, document.getElementById('app'));
}
