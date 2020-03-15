import * as React from "react";
import * as moment from "moment";
import Text from "Common/Text";
import { makeStyles } from "@material-ui/core/styles";

interface DayRenderer {
	type: string;
	render: (day: moment.Moment) => JSX.Element;
}

interface CalendarMonthParams {
	Month: moment.Moment;
	DayRenderers: DayRenderer[];
}

function CreateDatesInMonth(date: moment.Moment) {
	const firstDay = moment(date.format("YYYY-MM-01"));
	const lastDay = firstDay.clone().add(1, "month").subtract(1, "day");

	const yearAndMonthPart = firstDay.format("YYYY-MM-");
	const days = [];

	for(let dayOfMonth = 1; dayOfMonth <= lastDay.date(); dayOfMonth++)
	{
		days.push(moment(yearAndMonthPart + dayOfMonth));
	}

	return days;
}

const CalendarMonth: React.FC<CalendarMonthParams> = (props) => {
	const dates = CreateDatesInMonth(props.Month);
	return <div>{dates.map((date) => <CalendarDay Date={date} DayRenderers={props.DayRenderers} />)}</div>;
}

interface CalendarDayProps {
	Date: moment.Moment;
	DayRenderers: DayRenderer[];
}

const CalendarDay: React.FC<CalendarDayProps> = (props) => {
	const classes = useCalendarDayStyles();

	return (
		<div className={classes.dayItems} key={props.Date.toString()}>
			<div className={classes.dateLabel}>
				<Text className={classes.dayOfMonth} Text={props.Date.format("DD")} />
				<Text className={classes.month} Text={props.Date.format("MMM")} />
			</div>

			<div>
				{props.DayRenderers.map((renderer) => <div key={renderer.type}>{renderer.render(props.Date)}</div>)}
			</div>

			<Text className={classes.dayOfWeek} Text={props.Date.format("dddd")} />
		</div>
	);
}

const useCalendarDayStyles = makeStyles({
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

export default CalendarMonth;
