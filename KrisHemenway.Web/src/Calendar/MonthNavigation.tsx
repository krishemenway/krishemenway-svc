import * as React from "react";
import * as moment from "moment";
import { makeStyles, Theme } from "@material-ui/core/styles";
import { CalendarService } from "Calendar/CalendarService";
import { useObservable } from "Common/UseObservable";
import { ObservableCalendarMonth } from "Calendar/ObservableCalendarMonth";
import Text from "Common/Text";

const MonthNavigation: React.FC = () => {
	const classes = useStyles();
	const viewingMonth = useObservable(CalendarService.Instance.ViewingMonth);
	const previousMonth = CalendarService.Instance.FindObservableCalendarMonth(viewingMonth.Date.clone().subtract(1, 'month'));
	const nextMonth = CalendarService.Instance.FindObservableCalendarMonth(viewingMonth.Date.clone().add(1, 'month'));
	
	return (
		<div className={classes.monthNavigationContainer}>
			<SwitchMonthButton Month={previousMonth} />

			<div className={classes.currentMonth}>
				<FullMonthName Month={viewingMonth.Date} />
			</div>

			<SwitchMonthButton Month={nextMonth} />
		</div>
	)
};

const SwitchMonthButton: React.FC<{Month: ObservableCalendarMonth}> = (props) => {
	const classes = useStyles();

	return (
		<button className={classes.clickableMonth} onClick={() => CalendarService.Instance.ChangeMonth(props.Month.Date)}>
			<FullMonthName Month={props.Month.Date} />
			<div className={classes.clickMonthUnderline} />
		</button>
	);
}

const FullMonthName: React.FC<{Month: moment.Moment}> = (props) => {
	const classes = useStyles();

	return (
		<div>
			<Text className={classes.monthText} Text={props.Month.format("MMMM")} />
			<Text className={classes.monthText} Text={props.Month.format("YYYY")} />
		</div>
	);
};

const useStyles = makeStyles(theme => ({
	monthNavigationContainer: {
		display: "flex",
		flexDirection: "row",
		flexWrap: "nowrap",
	},
	clickMonthUnderline: {
		border: "1px solid #585858",
		borderWidth: "0 0 1px 0",
		width: "0",
		margin: "10px auto 0 auto",
	
		"-webkit-transition": "all 300ms",
		transition: "all 300ms",

		"button:hover &": {
			width: "40%",
			opacity: 1,
		},
	},
	currentMonth: {
		cursor: "default",
		fontSize: "34px",
		padding: "10px 0 20px 0",
		flexGrow: 1,
		flexBasis: 0,
		[theme.breakpoints.down(768)]: {
			fontSize: "24px",
		},
	},
	clickableMonth: {
		fontSize: "28px",
		padding: "10px 0",
		flexGrow: 1,
		flexBasis: 0,
		[theme.breakpoints.down(768)]: {
			fontSize: "16px",
		},
	},
	monthText: {
		textAlign: "center",
		fontWeight: "bold",
		color: "#F0F0F0",
		display: "block",
	},
}));

export default MonthNavigation;
