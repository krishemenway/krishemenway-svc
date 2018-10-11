import * as React from 'react';
import * as moment from 'moment';

interface MonthParams {
	Month: moment.Moment;
}

export class FullMonthName extends React.Component<MonthParams, {}> {
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
