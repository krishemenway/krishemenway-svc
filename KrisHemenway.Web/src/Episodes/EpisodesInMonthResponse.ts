import { Episode } from "./Episode";

export interface EpisodesInMonthResponse {
	EpisodesInMonth: Array<Episode>;
	ShowDownload: boolean;
}
