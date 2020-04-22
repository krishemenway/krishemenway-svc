import { Observable } from "@residualeffect/reactor";
import * as moment from "moment";
import { ObservableCalendarMonth } from "Calendar/ObservableCalendarMonth";

export class CalendarService {
	constructor() {
		this.CalendarMonths = {};
		this.ViewingMonth = new Observable<ObservableCalendarMonth>(this.TryAddObservableCalendarMonth(moment()));
		this.AddBoundaryObservableCalendarMonths();
	}

	public ChangeMonth(date: moment.Moment) {
		this.ViewingMonth.Value = this.CalendarMonths[date.format("YYYY-MM")];
		this.AddBoundaryObservableCalendarMonths();
	}

	public FindObservableCalendarMonth(date: moment.Moment) {
		return this.CalendarMonths[date.format("YYYY-MM")];
	}

	private AddBoundaryObservableCalendarMonths() {
		this.TryAddObservableCalendarMonth(this.ViewingMonth.Value.Date.clone().add(1, 'month'));
		this.TryAddObservableCalendarMonth(this.ViewingMonth.Value.Date.clone().subtract(1, 'month'));
	}

	private TryAddObservableCalendarMonth(date: moment.Moment): ObservableCalendarMonth {
		const monthKey = date.format("YYYY-MM");

		if (this.CalendarMonths[monthKey] === undefined) {
			const observableCalendarMonth = new ObservableCalendarMonth(date);
			this.CalendarMonths[monthKey] = observableCalendarMonth;
		}

		return this.CalendarMonths[monthKey];
	}

	public CalendarMonths: Dictionary<ObservableCalendarMonth>;
	public ViewingMonth: Observable<ObservableCalendarMonth>;

	public static get Instance(): CalendarService {
		if (CalendarService._instance === undefined) {
			CalendarService._instance = new CalendarService();
		}

		return CalendarService._instance;
	}

	private static _instance: CalendarService;
}