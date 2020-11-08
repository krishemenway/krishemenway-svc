import * as React from "react";
import * as reactDom from "react-dom";
import { createUseStyles } from "react-jss";
import {default as AppBackground} from "Common/AppBackground.png";
import Text from "Common/Text";
import DoneIcon from "Common/DoneIcon";
import SortByAlphaIcon from "Common/SortByAlphaIcon";
import { useObservable } from "Common/UseObservable";
import { MissingEpisodesService, MissingEpisodesForShow, SortFuncType } from "MissingEpisodes/MissingEpisodesService";
import { belowWidth } from "Common/AppStyles";

const MissingEpisodesList : React.FC<{}> = () => {
	const classes = useMissingEpisodeStyles();
	const shows = useObservable(MissingEpisodesService.Instance.FilteredAndSortedShows);
	const hideCompletedShows = useObservable(MissingEpisodesService.Instance.HideCompletedShows);
	const sortFunc = useObservable(MissingEpisodesService.Instance.SortFunc);

	React.useEffect(() => { MissingEpisodesService.Instance.LoadShows() }, []);

	return (
		<div className={classes.app}>
			<Text className={classes.title} Text="Completion Status" />

			<div className={classes.listControls}>
				<Text className={classes.sortByText} Text="Show: " />

				<button
					className={`${classes.showCompleteButton} ${!hideCompletedShows && classes.showCompleteButtonToggled}`}
					onClick={() => { MissingEpisodesService.Instance.HideCompletedShows.Value = !hideCompletedShows; }}>

					{hideCompletedShows ? "-" : "+"} Complete
				</button>

				<Text className={classes.sortByText} Text="Sort By: " />

				<button className={`${classes.sortByButton} ${sortFunc === SortFuncType.Alphabetical ? classes.isSortingByToggle : ""}`} onClick={() => { MissingEpisodesService.Instance.SortFunc.Value = SortFuncType.Alphabetical; }}>
					<Text Text="Alpha" className={classes.sortButtonText} />
					<SortByAlphaIcon className={classes.sortButtonIcon} />
				</button>

				<button className={`${classes.sortByButton} ${sortFunc === SortFuncType.Percentage ? classes.isSortingByToggle : ""}`} onClick={() => { MissingEpisodesService.Instance.SortFunc.Value = SortFuncType.Percentage; }}>
					<Text Text="Percentage" className={classes.sortButtonText} />
					<Text Text="%" className={classes.sortButtonIcon} />
				</button>
			</div>

			<div>
				{shows.map((show) => <MissingEpisodesForShow show={show} />)}
			</div>
		</div>
	);
};

const MissingEpisodesForShow : React.FC<{show: MissingEpisodesForShow}> = (props) => {
	const completionPercentage = 100 - props.show.MissingEpisodesPercentage.Value;
	const classes = useMissingEpisodeStyles();
	const expandedShow = useObservable(MissingEpisodesService.Instance.ExpandedShow);

	return (
		<div style={{marginBottom: "8px"}}>
			<button
				disabled={props.show.MissingEpisodesPercentage.Value === 0}
				className={classes.showSectionHeader}
				onClick={() => MissingEpisodesService.Instance.ExpandedShow.Value = expandedShow !== props.show ? props.show : null}>

				<div className={classes.showName}>
					<Text Text={props.show.Name} style={{marginRight: "8px"}} />
					{props.show.MissingEpisodesPercentage.Value === 0 ? <DoneIcon className={classes.doneIcon} /> : ""}
				</div>

				<Text className={classes.showPercentageText} Text={completionPercentage.toFixed(2) + "%"} />
				<span className={classes.showPercentageBar} style={{width: completionPercentage.toFixed(0) + "%", background: "#101010"}}> </span>
			</button>

			{expandedShow === props.show && expandedShow.MissingEpisodesPercentage.Value > 0 && (
				<div className={classes.missingEpisodesContainer}>
					{props.show.MissingEpisodes.map((episode) => (
						<Text className={classes.missingEpisode} Text={`${episode.Season}e${episode.EpisodeInSeason} ${episode.Title}`} />
					))}
				</div>
			)}
		</div>
	);
};

const useMissingEpisodeStyles = createUseStyles({
	app: {
		padding: "8px",

		[belowWidth(720)]: {
			padding: "8px 0",
		},
	},
	title: {
		color: "#E8E8E8",
		fontSize: "32px",
		padding: "0 16px",
		marginBottom: "40px",

		[belowWidth(720)]: {
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

		[belowWidth(720)]: {
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

		[belowWidth(720)]: {
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

		[belowWidth(720)]: {
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

		[belowWidth(720)]: {
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

		[belowWidth(720)]: {
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

		[belowWidth(720)]: {
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

(window as any).initialize = (element: HTMLElement) => {
	reactDom.render(<MissingEpisodesList />, element);
	document.getElementsByTagName("body")[0].style.background = `url('${AppBackground}') #010101`;
}
