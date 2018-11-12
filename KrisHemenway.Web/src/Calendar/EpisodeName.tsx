import * as React from "react";
import { Episode } from "../Episodes/Episode";

interface EpisodeParams {
	Episode: Episode;
}

export class EpisodeName extends React.Component<EpisodeParams, {}> {
	public render() {
		return (
			<div className="episode-name">
				<span className="show-name gray-69">{this.props.Episode.ShowName}</span>
				<span className="episode-identity">{this.props.Episode.Season + "x" + this.props.Episode.EpisodeInSeason}</span>
				<span className="episode-name gray-69">{this.props.Episode.Title}</span>
			</div>
		);
	}
}
