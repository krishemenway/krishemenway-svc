export interface EpisodesInMonthResponse {
	EpisodesInMonth: Array<Episode>;
}

export interface Episode {
	Id: number;
	Title: string;
	ShowName: string;
	ShowId: number;
	EpisodeNumber: number;
	Season: number;
	EpisodeInSeason: number;
	AirDate: string;
}
