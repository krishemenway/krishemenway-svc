import * as React from 'react';
import * as reactDom from 'react-dom';
import * as moment from 'moment';

interface HashTable<T> {
	[key: string]: T;
}

interface Episode
{
	Id: number;
	Title: string;
	Series: string;
	SeriesId: number;
	EpisodeNumber: number;
	Season: number;
	EpisodeInSeason: number;
	AirDate: string;
}

interface EpisodesInMonthResponse
{
	EpisodesInMonth: Array<Episode>;
}

interface GlobalAppState
{
	CalendarState: CalendarState;
}

interface CalendarState
{
	CurrentMonth: moment.Moment;
	EpisodesPerDay: HashTable<Array<Episode>>;
}

interface EpisodeParams
{
	Episode: Episode;
}

interface MonthParams
{
	Month: moment.Moment;
}

interface SeriesCalendarParams
{
	CalendarState: CalendarState;
	OnChangeMonth: Function;
}

class FullMonthName extends React.Component<MonthParams, {}>
{
	constructor(params: MonthParams) {
		super(params);
	}
	
	public render() {
		return (
			<div className="month-navigation">
				<div className="month text-center bold">{this.props.Month.format("MMMM")}</div>
				<div className="year text-center bold">{this.props.Month.format("YYYY")}</div>
				<div className="click-indicator"></div>
			</div>
		);
	}
}

class EpisodeName extends React.Component<EpisodeParams, {}>
{
	constructor() {
		super();
	}
	
	public render() {
		return (
			<div className="episode-name">
				<span className="series-name gray-69">{this.props.Episode.Series}</span>
				<span className="episode-identity">{this.props.Episode.Season + "x" + this.props.Episode.EpisodeInSeason}</span>
				<span className="episode-name gray-69">{this.props.Episode.Title}</span>
			</div>
		);
	}
}

class SeriesCalendar extends React.Component<SeriesCalendarParams, {}>
{
	constructor() {
		super();
	}
	
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

export class App extends React.Component<{}, GlobalAppState>
{
	constructor() {
		super();
		var currentTime = moment().local();
		
		this.state = {
			CalendarState: {
				CurrentMonth: currentTime,
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
		this.loadEpisodesForMonth(this.state.CalendarState.CurrentMonth.clone().subtract(1, 'month'));
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

reactDom.render(<App />, document.getElementById('app'));