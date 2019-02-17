import * as React from "react";
import * as moment from "moment";
import Text from "../Common/Text";
import EpisodeName from "../Episodes/EpisodeName";
import { CalendarState } from "./Calendar";
import { Episode } from "../Episodes/Episode";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

interface SeriesCalendarParams extends WithStyles<typeof styles> {
	CalendarState: CalendarState;
	OnChangeMonth: Function;
}

export class SeriesCalendar extends React.Component<SeriesCalendarParams, {}> {
	public render() {
		return (
			<div className={this.props.classes.days}>
				{this.renderDays()}
			</div>
		);
	}

	private renderDays() {
		let firstDay = moment(this.props.CalendarState.CurrentMonth.format("YYYY-MM-01"));
		let lastDay = firstDay.clone().add(1, "month").subtract(1, "day");

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
			<div className={this.props.classes.dayEpisodeListings} key={dayOfMonth}>
				<div className={this.props.classes.listingDate}>
					<Text className={this.props.classes.listingDayOfMonth} Text={date.format("DD")} />
					<Text className={this.props.classes.listingMonth} Text={date.format("MMM")} />
				</div>

				<div className={this.props.classes.episodes}>{renderedEpisodes}</div>
				<Text className={this.props.classes.dayOfWeek} Text={date.format("dddd")} />
			</div>
		);
	}

	private renderEpisodes(episodes: Episode[]) {
		return episodes.map((episode, index) => {
			return (
				<div className={this.props.classes.episodeNames} key={index}>
					<EpisodeName Episode={episode} ShowDownload={this.props.CalendarState.IsAuthenticated} />
				</div>
			);
		}, this);
	}
}

const styles = (theme: Theme) => createStyles({
	dayOfWeek: {
		fontSize: "24px",
		fontWeight: "bold",
		position: "absolute",
		zIndex: 0,
		bottom: "3px",
		right: "3px",
		letterSpacing: "2px",
		textTransform: "uppercase",
		color: "#1a1a1a",
		cursor: "default",
	},
	days: { },
	dayEpisodeListings: {
		border: "1px solid #383838",
		borderWidth: "1px 0 0 0",
		position: "relative",
		padding: "10px 0",
		display: "flex",
		flexDirection: "row",
		flexWrap: "nowrap",
	
		"&:hover": {
			backgroundColor: "rgba(0,0,0,.2)",
			borderColor: "#191919",
		},
	},
	listingDate: {
		padding: "0 10px",
	},
	listingDayOfMonth: {
		fontSize: "26px",
		color: "#E8E8E8",
	},
	listingMonth: {
		fontSize: "20px",
		color: "#696969",
	},
	episodes: {
		flexBasis: "auto",
		zIndex: 1,
	},
	episodeNames: {
		margin: "5px 0",

		"&:first-child": {
			marginTop: "0",
		},

		"&:last-child": {
			marginBottom: "0",
		},
	},
});

export default withStyles(styles)(SeriesCalendar);
