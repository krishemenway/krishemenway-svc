import * as React from "react";
import FullMonthName from "./FullMonthName";
import { makeStyles, Theme } from "@material-ui/core/styles";
import moment = require("moment");

interface MonthNavigationProps {
	CurrentMonth: moment.Moment;
	OnChangeMonth: (month: moment.Moment) => void;
}

const MonthNavigation: React.FC<MonthNavigationProps> = (props) => {
	const classes = useMonthNavigationStyles();
	
	return (
		<div className={classes.monthNavigation}>
			<button className={classes.clickableMonth} onClick={() => props.OnChangeMonth(props.CurrentMonth.clone().subtract(1, "month"))}>
				<FullMonthName Month={props.CurrentMonth.clone().subtract(1, "month")} />
				<div className={classes.clickMonthUnderline} />
			</button>

			<div className={classes.currentMonth}>
				<FullMonthName Month={props.CurrentMonth} />
			</div>

			<button className={classes.clickableMonth} onClick={() => props.OnChangeMonth(props.CurrentMonth.clone().add(1, "month"))}>
				<FullMonthName Month={props.CurrentMonth.clone().add(1, "month")} />
				<div className={classes.clickMonthUnderline} />
			</button>
		</div>
	)
};

const useMonthNavigationStyles = makeStyles(theme => ({
	monthNavigation: {
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
}));

export default MonthNavigation;
