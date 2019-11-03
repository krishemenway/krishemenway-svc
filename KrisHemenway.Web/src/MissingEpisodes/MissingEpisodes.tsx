import * as jQuery from "jquery";
import * as React from "react";
import * as reactDom from "react-dom";
import * as AppBackground from "../Common/AppBackground.png";
import { withStyles, createStyles, WithStyles, Theme } from "@material-ui/core/styles";
import { Episode } from "../Episodes/Episode";
import Text from "../Common/Text";
import DoneIcon from "@material-ui/icons/Done";
import SortByAlphaIcon from "@material-ui/icons/SortByAlpha";

enum SortFuncType {
	Alphabetical,
	Percentage,
}

interface Percentage {
	Value: number;
	Count: number;
	Total: number;
}

interface MissingEpisodesForShow {
	Name: string;
	MissingEpisodes: Episode[];
	MissingEpisodesPercentage: Percentage;
}

interface MissingEpisodesForShowResponse {
	AllShows: MissingEpisodesForShow[];
}

export interface MissingEpisodesState {
	SortFunc: SortFuncType;
	ShowComplete: boolean;
	Shows: MissingEpisodesForShow[];
	ExpandedShow: MissingEpisodesForShow|null;
}

interface MissingEpisodesProps extends WithStyles<typeof styles> { }

export class MissingEpisodes extends React.Component<MissingEpisodesProps, MissingEpisodesState> {
	constructor(props: MissingEpisodesProps) {
		super(props);

		this.state = {
			Shows: [],
			ShowComplete: true,
			SortFunc: SortFuncType.Percentage,
			ExpandedShow: null,
		};
	}

	public componentDidMount() {
		jQuery.getJSON("/api/tvshows/episodes/missing", (response: MissingEpisodesForShowResponse) => this.HandleMissingEpisodesForShowsResponse(response));
	}

	public render() {
		return (
			<div className={this.props.classes.app}>
				<Text className={this.props.classes.title} Text="Completion Status" />

				<div className={this.props.classes.listControls}>
					<Text className={this.props.classes.sortByText} Text="Show: " />

					<button
						className={`${this.props.classes.showCompleteButton} ${this.state.ShowComplete && this.props.classes.showCompleteButtonToggled}`}
						onClick={() => this.setState({ShowComplete: !this.state.ShowComplete})}>

						{this.state.ShowComplete ? "+" : "-"} Complete
					</button>

					<Text className={this.props.classes.sortByText} Text="Sort By: " />

					<button className={`${this.props.classes.sortByButton} ${this.state.SortFunc === SortFuncType.Alphabetical ? this.props.classes.isSortingByToggle : ""}`} onClick={() => this.setState({SortFunc: SortFuncType.Alphabetical})}>
						<Text Text="Alpha" className={this.props.classes.sortButtonText} />
						<SortByAlphaIcon className={this.props.classes.sortButtonIcon} />
					</button>

					<button className={`${this.props.classes.sortByButton} ${this.state.SortFunc === SortFuncType.Percentage ? this.props.classes.isSortingByToggle : ""}`} onClick={() => this.setState({SortFunc: SortFuncType.Percentage})}>
						<Text Text="Percentage" className={this.props.classes.sortButtonText} />
						<Text Text="%" className={this.props.classes.sortButtonIcon} />
					</button>
				</div>

				<div>
					{this.state.Shows.filter(this.FindFilterFunc()).sort(this.FindSortFunc()).map((show) => this.RenderShow(show))}
				</div>
			</div>
		);
	}

	private RenderShow(show: MissingEpisodesForShow) {
		const completionPercentage = 100 - show.MissingEpisodesPercentage.Value;

		return (
			<div style={{marginBottom: "8px"}}>
				<button
					disabled={show.MissingEpisodesPercentage.Value === 0}
					className={this.props.classes.showSectionHeader}
					onClick={() => this.setState({ExpandedShow: this.state.ExpandedShow !== show ? show : null})}>

					<div className={this.props.classes.showName}>
						<Text Text={show.Name} style={{marginRight: "8px"}} />
						{show.MissingEpisodesPercentage.Value === 0 ? <DoneIcon className={this.props.classes.doneIcon} /> : ""}
					</div>

					<Text className={this.props.classes.showPercentageText} Text={completionPercentage.toFixed(2) + "%"} />
					<span className={this.props.classes.showPercentageBar} style={{width: completionPercentage.toFixed(0) + "%", background: "#101010"}}> </span>
				</button>

				{this.state.ExpandedShow === show && this.state.ExpandedShow.MissingEpisodesPercentage.Value > 0 && (
					<div className={this.props.classes.missingEpisodesContainer}>
						{show.MissingEpisodes.map((episode) => (
							<Text className={this.props.classes.missingEpisode} Text={`${episode.Season}e${episode.EpisodeInSeason} ${episode.Title}`} />
						))}
					</div>
				)}
			</div>
		);
	}

	private FindFilterFunc() : ((show: MissingEpisodesForShow) => boolean) {
		if (!this.state.ShowComplete) {
			return (show) => show.MissingEpisodesPercentage.Value > 0;
		}

		return () => true;
	}

	private FindSortFunc() : ((a: MissingEpisodesForShow, b: MissingEpisodesForShow) => number) {
		if (this.state.SortFunc === SortFuncType.Alphabetical) {
			return (a, b) => a.Name.localeCompare(b.Name);
		} else {
			return (a, b) => a.MissingEpisodesPercentage.Value - b.MissingEpisodesPercentage.Value;
		}
	}

	private HandleMissingEpisodesForShowsResponse(response: MissingEpisodesForShowResponse) {
		this.setState({ Shows: response.AllShows });
	}
}

const styles = (theme: Theme) => createStyles({
	app: {
		padding: "8px",

		[theme.breakpoints.down(720)]: {
			padding: "8px 0",
		},
	},
	title: {
		color: "#E8E8E8",
		fontSize: "32px",
		padding: "0 16px",
		marginBottom: "40px",

		[theme.breakpoints.down(720)]: {
			fontSize: "24px",
			lineHeight: "28px",
		},
	},
	listControls: {
		marginBottom: "16px",
	},
	sortByText: {
		color: "#F8F8F8",
		fontSize: "18px",
		margin: "0 16px",

		[theme.breakpoints.down(720)]: {
			fontSize: "16px",
		},
	},
	showCompleteButton: {
		color: "#F8F8F8",
		fontSize: "18px",
		padding: "4px 16px",
		background: "transparent",
		border: "1px solid #404040",
		cursor: "pointer",
		marginRight: "8px",

		"&:hover": {
			background: "rgba(255,255,255,.1)",
		},

		[theme.breakpoints.down(720)]: {
			fontSize: "16px",
		},
	},
	showCompleteButtonToggled: {
		borderColor: "#C8C8C8",
		background: "rgba(255,255,255,.05)",
	},
	sortByButton: {
		color: "#F8F8F8",
		fontSize: "18px",
		border: "1px solid #404040",
		padding: "4px 16px",
		background: "transparent",
		cursor: "pointer",
		marginRight: "8px",

		"&:hover": {
			background: "rgba(255,255,255,.1)",
		},

		[theme.breakpoints.down(720)]: {
			fontSize: "16px",
		},
	},
	isSortingByToggle: {
		borderColor: "#C8C8C8",
		background: "rgba(255,255,255,.05)",
	},
	showName: {
		color: "#F8F8F8",
		fontSize: "22px",
		position: "relative",
		zIndex: 2,
		background: "transparent",

		[theme.breakpoints.down(720)]: {
			fontSize: "18px",
		},
	},
	showPercentageText: {
		color: "#F8F8F8",
		fontSize: "18px",
		position: "absolute",
		zIndex: 2,
		right: "16px",
		background: "transparent",
		marginTop: "4px",

		[theme.breakpoints.down(720)]: {
			fontSize: "16px",
		},
	},
	showPercentageBar: {
		position: "absolute",
		left: 0,
		top: "4px",
		bottom: "4px",
		zIndex: 1,
	},
	showSectionHeader: {
		cursor: "pointer",
		position: "relative",
		padding: "8px 16px",
		background: "#020202",
		display: "flex",
		width: "100%",

		"&:disabled": {
			cursor: "default",
		},
	},
	missingEpisodesContainer: {
		background: "#020202",
		padding: "8px 0",
		display: "flex",
		flexWrap: "wrap",
	},
	missingEpisode: {
		color: "#F8F8F8",
		fontSize: "16px",
		padding: "8px 16px",
	},
	doneIcon: {
		position: "relative",
		top: "4px",
		left: "4px",
		fontSize: "inherit",
		verticalAlign: "top",
	},
	sortButtonText: {
		marginRight: "8px",

		[theme.breakpoints.down(720)]: {
			display: "none",
		}
	},
	sortButtonIcon: {
		fontSize: "16px",
		verticalAlign: "middle",
		position: "relative",

		"svg&": {
			top: "-1px",
		}
	},
});

const MissingEpisodesWithStyle = withStyles(styles)(MissingEpisodes);

(window as any).initialize = (element: HTMLElement) => {
	reactDom.render(<MissingEpisodesWithStyle />, element);
	document.getElementsByTagName("body")[0].style.background = `url('${AppBackground}') #010101`;
}
