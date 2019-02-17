import * as React from "react";
import FullMonthName from "./FullMonthName";
import { CalendarState } from "./Calendar";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";
import moment = require("moment");

interface MonthNavigationParams extends WithStyles<typeof styles> {
	CalendarState: CalendarState;
	OnChangeMonth: (month: moment.Moment) => void;
}

export class MonthNavigation extends React.Component<MonthNavigationParams, {}> {
	public render() {
		return (
			<div className={this.props.classes.monthNavigation}>
				<button className={this.props.classes.clickableMonth} onClick={() => this.props.OnChangeMonth(this.props.CalendarState.CurrentMonth.clone().subtract(1, "month"))}>
					<FullMonthName Month={this.props.CalendarState.CurrentMonth.clone().subtract(1, "month")} />
					<div className={this.props.classes.clickMonthUnderline} />
				</button>

				<div className={this.props.classes.currentMonth}>
					<FullMonthName Month={this.props.CalendarState.CurrentMonth} />
				</div>

				<button className={this.props.classes.clickableMonth} onClick={() => this.props.OnChangeMonth(this.props.CalendarState.CurrentMonth.clone().add(1, "month"))}>
					<FullMonthName Month={this.props.CalendarState.CurrentMonth.clone().add(1, "month")} />
					<div className={this.props.classes.clickMonthUnderline} />
				</button>
			</div>
		);
	}
}

const styles = (theme: Theme) => createStyles({
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
});

export default withStyles(styles)(MonthNavigation);
