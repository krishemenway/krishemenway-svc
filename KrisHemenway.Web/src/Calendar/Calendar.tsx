import * as React from "react";
import * as reactDom from "react-dom";
import * as moment from "moment";
import * as jQuery from "jquery";
import { Episode } from "../Episodes/Episode";
import { EpisodesInMonthResponse } from "../Episodes/EpisodesInMonthResponse";
import SeriesCalendar from "./SeriesCalendar";
import MonthNavigation from "./MonthNavigation";
import DownloadLogin from "./DownloadLogin";
import * as AppBackground from "../Common/AppBackground.png";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

export interface CalendarState {
	IsAuthenticated: boolean;
	CurrentMonth: moment.Moment;
	EpisodesPerDay: HashTable<Array<Episode>>;
}

interface CalendarProps extends WithStyles<typeof styles> { }

export class Calendar extends React.Component<CalendarProps, CalendarState> {
	constructor(props: CalendarProps) {
		super(props);

		this.state = {
			IsAuthenticated: false,
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
			<div className={this.props.classes.calendarApp}>
				<div className={this.props.classes.widthWrapper}>
					<MonthNavigation
						CalendarState={this.state}
						OnChangeMonth={(month) => this.setState({ CurrentMonth: month })} />

					<SeriesCalendar
						CalendarState={this.state}
					/>

					<DownloadLogin
						OnAuthenticated={() => this.setState({ IsAuthenticated: true })}
						IsAuthenticated={this.state.IsAuthenticated}
					/>
				</div>
			</div>
		);
	}

	private preloadBorderMonths = () : void => {
		this.loadEpisodesForMonth(this.state.CurrentMonth.clone().subtract(1, "month"));
		this.loadEpisodesForMonth(this.state.CurrentMonth.clone().add(1, "month"));
	}

	private loadEpisodesForMonth = (date: moment.Moment) : void => {
		if (this.state.EpisodesPerDay[date.format("YYYY-MM-01")] == null) {
			this.state.EpisodesPerDay[date.format("YYYY-MM-01")] = [];
			jQuery.getJSON("/api/tvshows/episodes/calendar/" + date.format("YYYY/MM"), this.onReceivedCalendarData);
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

		this.setState({ EpisodesPerDay: episodesPerDay, IsAuthenticated: data.ShowDownload });
	}
	
	private onChangeMonth = (newMonth: moment.Moment) : void => {
		this.setState({ CurrentMonth: newMonth });
		this.preloadBorderMonths();
	}
}

const styles = createStyles({
	calendarApp: { },
	widthWrapper: {
		margin: "0 auto",
		maxWidth: "900px",
		padding: "0 10px 100px 10px",
		background: "rgba(0,0,0,.2)",
	},
});

const CalendarWithStyle = withStyles(styles)(Calendar);

(window as any).initialize = () => {
	reactDom.render(<CalendarWithStyle />, document.getElementById("app"));
	document.getElementsByTagName("body")[0].style.background = `url('${AppBackground}') #010101`;
}
