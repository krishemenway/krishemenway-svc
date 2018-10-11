export interface EpisodesInMonthResponse {
	EpisodesInMonth: Array<Episode>;
}

export interface Episode {
	Id: number;
	Title: string;
	Series: string;
	SeriesId: number;
	EpisodeNumber: number;
	Season: number;
	EpisodeInSeason: number;
	AirDate: string;
}
