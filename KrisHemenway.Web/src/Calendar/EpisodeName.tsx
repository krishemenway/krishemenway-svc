import * as React from "react";
import { Episode } from "../Episodes/Episode";
import { DownloadIcon } from "./DownloadIcon";

interface EpisodeParams {
	Episode: Episode;
	ShowDownload: boolean;
}

export class EpisodeName extends React.Component<EpisodeParams, {}> {
	public render() {
		return (
			<div className="episode-name">
				<span className="show-name gray-69">{this.props.Episode.ShowName}</span>
				<span className="episode-identity">{this.props.Episode.Season + "x" + this.props.Episode.EpisodeInSeason}</span>
				<span className="episode-name gray-90">{this.props.Episode.Title}</span>
				{this.maybeRenderDownload()}
			</div>
		);
	}

	private maybeRenderDownload() {
		if (!this.props.ShowDownload || !this.props.Episode.HasEpisode) {
			return "";
		}

		return (
			<a href={`/api/tvshows/episodes/download?EpisodeId=${this.props.Episode.EpisodeId}`}>
				<DownloadIcon />
			</a>
		);
	}
}
