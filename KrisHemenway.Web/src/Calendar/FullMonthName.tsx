import * as React from "react";
import * as moment from "moment";
import Text from "../Common/Text";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

interface MonthParams extends WithStyles<typeof styles> {
	Month: moment.Moment;
}

export class FullMonthName extends React.Component<MonthParams, {}> {
	public render() {
		return (
			<div>
				<Text className={this.props.classes.monthNavigation} Text={this.props.Month.format("MMMM")} />
				<Text className={this.props.classes.monthNavigation} Text={this.props.Month.format("YYYY")} />
			</div>
		);
	}
}

const styles = (_: Theme) => createStyles({
	monthNavigation: {
		textAlign: "center",
		fontWeight: "bold",
		color: "#F0F0F0",
		display: "block",
	},
});

export default withStyles(styles)(FullMonthName);
