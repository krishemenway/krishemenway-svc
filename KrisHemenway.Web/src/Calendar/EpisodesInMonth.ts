import * as moment from "moment";
import { Episode } from "Episodes/Episode";

export interface EpisodesInMonth {
	readonly Date: moment.Moment;
	readonly EpisodesInMonth: Dictionary<Episode[]>;
}
