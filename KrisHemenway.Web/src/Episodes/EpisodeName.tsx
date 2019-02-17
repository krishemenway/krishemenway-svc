import * as React from "react";
import { Episode } from "./Episode";
import CloudDownload from "@material-ui/icons/CloudDownload";
import Text from "../Common/Text";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

interface EpisodeParams extends WithStyles<typeof styles> {
	Episode: Episode;
	ShowDownload: boolean;
}

export class EpisodeName extends React.Component<EpisodeParams, {}> {
	public render() {
		return (
			<div>
				<Text className={this.props.classes.showName} Text={this.props.Episode.ShowName} />
				<Text className={this.props.classes.episodeIdentity} Text={`${this.props.Episode.Season}x${this.props.Episode.EpisodeInSeason}`} />
				<Text className={this.props.classes.episodeTitle} Text={this.props.Episode.Title} />
				{this.maybeRenderDownload()}
			</div>
		);
	}

	private maybeRenderDownload() {
		if (!this.props.ShowDownload || !this.props.Episode.HasEpisode) {
			return "";
		}

		return (
			<a className={this.props.classes.downloadIcon} href={this.episodeDownloadUrl()}>
				<CloudDownload width={16} />
			</a>
		);
	}

	private episodeDownloadUrl() {
		return `/api/tvshows/episodes/download?EpisodeId=${this.props.Episode.EpisodeId}`;
	}
}

const styles = (theme: Theme) => createStyles({
	showName: {
		color: "#696969",
		marginRight: "8px",
	},
	episodeIdentity: {
		color: "#E8E8E8",
		marginRight: "8px",
	},
	episodeTitle: {
		color: "#909090",
	},
	downloadIcon: {
		display: "inline-block",
		verticalAlign: "bottom",
		marginLeft: "8px",
	},
});

export default withStyles(styles)(EpisodeName);