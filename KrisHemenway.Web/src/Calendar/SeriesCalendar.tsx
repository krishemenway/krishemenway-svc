import * as React from 'react';
import * as moment from 'moment';
import { FullMonthName } from './FullMonthName';
import { CalendarState } from './Calendar';
import { EpisodeName } from './EpisodeName';
import { Episode } from './EpisodesInMonthResponse';

interface SeriesCalendarParams {
	CalendarState: CalendarState;
	OnChangeMonth: Function;
}

export class SeriesCalendar extends React.Component<SeriesCalendarParams, {}> {
	public render() {
		return (
			<div className="calendar">
				<div className="months flex-row-container">
					<button className="previous-month flex-even-distribution font-28 phone-font-16 padding-vertical" onClick={() => this.clickMonth(this.props.CalendarState.CurrentMonth.clone().subtract(1, 'month'))}>
						<FullMonthName Month={this.props.CalendarState.CurrentMonth.clone().subtract(1, 'month')} />
					</button>

					<div className="current-month flex-even-distribution font-34 phone-font-24 padding-vertical">
						<FullMonthName Month={this.props.CalendarState.CurrentMonth} />
					</div>

					<button className="next-month flex-even-distribution font-28 phone-font-16 padding-vertical" onClick={() => this.clickMonth(this.props.CalendarState.CurrentMonth.clone().add(1, 'month'))}>
						<FullMonthName Month={this.props.CalendarState.CurrentMonth.clone().add(1, 'month')} />
					</button>
				</div>

				<div className="days">{this.renderDays()}</div>
			</div>
		);
	}
	
	private clickMonth(month: moment.Moment) {
		this.props.OnChangeMonth(month);
	}
	
	private renderDays() {
		let firstDay = moment(this.props.CalendarState.CurrentMonth.format("YYYY-MM-01"));
		let lastDay = firstDay.clone().add(1, 'month').subtract(1, 'day');

		let days = [];
		for(let i = 1; i <= lastDay.date(); i++) 
		{
			days.push(this.renderDay(i));
		}

		return days;
	}
	
	private renderDay(dayOfMonth: number) {
		var dayOfMonthString = dayOfMonth.toString().length == 1 ? "0"+dayOfMonth.toString():dayOfMonth.toString(); 
		var date = moment(this.props.CalendarState.CurrentMonth.format("YYYY-MM-") + dayOfMonthString);
		var episodes = this.props.CalendarState.EpisodesPerDay[date.format("YYYY-MM-DD")];
		var renderedEpisodes = episodes != null && episodes.length > 0 ? this.renderEpisodes(episodes) : null;

		return (
			<div className="day-episode-listings padding-vertical flex-row-container" key={dayOfMonth}>
				<div className="listing-date padding-horizontal">
					<span className="day font-26">{date.format("DD")}</span>
					<span className="month font-20 gray-69">{date.format("MMM")}</span>
				</div>

				<div className="episode-listings">
					{renderedEpisodes}
				</div>

				<div className="day-of-week font-24 bold">{date.format("dddd")}</div>
			</div>
		);
	}

	private renderEpisodes(episodes: Array<Episode>) {
		return episodes.map((episode, index) => {
			return (
				<div className="margin-vertical-half no-bookend-margin-vertical" key={index}>
					<EpisodeName Episode={episode} />
				</div>
			);
		}, this);
	}
}
