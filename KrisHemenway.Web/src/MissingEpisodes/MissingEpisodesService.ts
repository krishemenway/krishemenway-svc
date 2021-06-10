import { Computed, Observable } from "@residualeffect/reactor";
import { Percentage } from "../Common/Percentage";
import { Episode } from "Episodes/Episode";
import { Loadable } from "Common/Loadable";
import { Http } from "Common/Http";

export class MissingEpisodesService {
	constructor() {
		this.LoadableShows = new Loadable<MissingEpisodesViewModel>();
	}

	public LoadShows() {
		Http.get<MissingEpisodesForShowResponse, MissingEpisodesViewModel>("/api/tvshows/episodes/missing", this.LoadableShows, (response) => new MissingEpisodesViewModel(response));
	}

	public LoadableShows: Loadable<MissingEpisodesViewModel>;

	public static get Instance(): MissingEpisodesService {
		if (MissingEpisodesService._instance === undefined) {
			MissingEpisodesService._instance = new MissingEpisodesService();
		}

		return MissingEpisodesService._instance;
	}

	private static _instance: MissingEpisodesService;
}

export enum SortFuncType {
	Alphabetical,
	Percentage,
}

export interface MissingEpisodesForShow {
	Name: string;
	MissingEpisodes: Episode[];
	MissingEpisodesPercentage: Percentage;
}

export interface MissingEpisodesForShowResponse {
	AllShows: MissingEpisodesForShow[];
	TotalMissingEpisodesPercentage: Percentage;
}

export class MissingEpisodesViewModel {
	constructor(response: MissingEpisodesForShowResponse) {
		this.MissingEpisodesForShowResponse = response;

		this.SortFunc = new Observable<SortFuncType>(SortFuncType.Percentage);
		this.HideCompletedShows = new Observable<boolean>(false);
		this.ExpandedShow = new Observable<MissingEpisodesForShow|null>(null);
		this.FilteredAndSortedShows = new Computed<MissingEpisodesForShow[]>(() => this.FilterAndSortShows());
	}

	private FilterAndSortShows(): MissingEpisodesForShow[] {
		const sortFunc = this.FindSortFunc();
		const filterFunc = this.FindFilterFunc();

		return this.MissingEpisodesForShowResponse.AllShows.filter(filterFunc).sort(sortFunc);
	}

	private FindFilterFunc() : ((show: MissingEpisodesForShow) => boolean) {
		if (this.HideCompletedShows.Value) {
			return (show) => show.MissingEpisodesPercentage.Value > 0;
		}

		return () => true;
	}

	private FindSortFunc() : ((a: MissingEpisodesForShow, b: MissingEpisodesForShow) => number) {
		if (this.SortFunc.Value === SortFuncType.Alphabetical) {
			return (a, b) => a.Name.localeCompare(b.Name);
		} else {
			return (a, b) => a.MissingEpisodesPercentage.Value - b.MissingEpisodesPercentage.Value;
		}
	}

	public SortFunc: Observable<SortFuncType>;
	public HideCompletedShows: Observable<boolean>;
	public ExpandedShow: Observable<MissingEpisodesForShow|null>;
	public FilteredAndSortedShows: Computed<MissingEpisodesForShow[]>;

	private MissingEpisodesForShowResponse: MissingEpisodesForShowResponse;
}
