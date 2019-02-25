import * as React from "react";
import * as moment from "moment";
import Text from "../Common/Text";
import { withStyles, createStyles, WithStyles } from "@material-ui/core/styles";

interface DayRenderer {
	type: string;
	render: (day: moment.Moment) => JSX.Element;
}

interface CalendarMonthParams extends WithStyles<typeof styles> {
	Month: moment.Moment;
	DayRenderers: DayRenderer[];
}

export class CalendarMonth extends React.Component<CalendarMonthParams, {}> {
	public render() {
		return (
			<div className={this.props.classes.days}>
				{this.renderDays()}
			</div>
		);
	}

	private renderDays() {
		let firstDay = moment(this.props.Month.format("YYYY-MM-01"));
		let lastDay = firstDay.clone().add(1, "month").subtract(1, "day");

		let days = [];
		for(let dayOfMonth = 1; dayOfMonth <= lastDay.date(); dayOfMonth++)
		{
			days.push(this.renderDay(dayOfMonth));
		}

		return days;
	}

	private renderDay(dayOfMonth: number) {
		var dayOfMonthString = dayOfMonth.toString().length == 1 ? "0"+dayOfMonth.toString():dayOfMonth.toString(); 
		var date = moment(this.props.Month.format("YYYY-MM-") + dayOfMonthString);

		return (
			<div className={this.props.classes.dayItems} key={dayOfMonth}>
				<div className={this.props.classes.dateLabel}>
					<Text className={this.props.classes.dayOfMonth} Text={date.format("DD")} />
					<Text className={this.props.classes.month} Text={date.format("MMM")} />
				</div>

				<div>
					{this.props.DayRenderers.map((renderer) => <div key={renderer.type}>{renderer.render(date)}</div>)}
				</div>

				<Text className={this.props.classes.dayOfWeek} Text={date.format("dddd")} />
			</div>
		);
	}
}

const styles = createStyles({
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
	dayItems: {
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
	dateLabel: {
		padding: "0 10px",
	},
	dayOfMonth: {
		fontSize: "26px",
		color: "#E8E8E8",
	},
	month: {
		fontSize: "20px",
		color: "#696969",
	},
});

export default withStyles(styles)(CalendarMonth);
