import { Episode } from "Episodes/Episode";

export interface EpisodesInMonthResponse {
	EpisodesInMonth: Array<Episode>;
	ShowDownload: boolean;
}
