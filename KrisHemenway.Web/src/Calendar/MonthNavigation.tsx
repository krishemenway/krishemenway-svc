import * as React from "react";
import * as moment from "moment";
import { CalendarService } from "Calendar/CalendarService";
import { useObservable } from "Common/UseObservable";
import Text from "Common/Text";
import { createUseStyles } from "react-jss";
import { belowWidth } from "Common/AppStyles";

const MonthNavigation: React.FC = () => {
	const classes = useStyles();
	const viewingMonth = useObservable(CalendarService.Instance.ViewingMonth);
	const previousMonth = viewingMonth.clone().subtract(1, 'month');
	const nextMonth = viewingMonth.clone().add(1, 'month');
	
	return (
		<div className={classes.monthNavigationContainer}>
			<SwitchMonthButton Month={previousMonth} />

			<div className={classes.currentMonth}>
				<FullMonthName Month={viewingMonth} />
			</div>

			<SwitchMonthButton Month={nextMonth} />
		</div>
	)
};

const SwitchMonthButton: React.FC<{Month: moment.Moment}> = (props) => {
	const classes = useStyles();

	return (
		<button className={classes.clickableMonth} onClick={() => CalendarService.Instance.ChangeMonth(props.Month)}>
			<FullMonthName Month={props.Month} />
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

const useStyles = createUseStyles({
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
		[belowWidth(768)]: {
			fontSize: "24px",
		},
	},
	clickableMonth: {
		fontSize: "28px",
		padding: "10px 0",
		flexGrow: 1,
		flexBasis: 0,
		[belowWidth(768)]: {
			fontSize: "16px",
		},
	},
	monthText: {
		textAlign: "center",
		fontWeight: "bold",
		color: "#F0F0F0",
		display: "block",
	},
});

export default MonthNavigation;
