import * as React from "react";
import { createUseStyles } from "react-jss";
import { Episode } from "Episodes/Episode";
import Text from "Common/Text";

interface EpisodeParams {
	Episode: Episode;
	ShowDownload: boolean;
	className?: string;
	style?: React.CSSProperties;
}

const EpisodeName: React.FC<EpisodeParams> = (props) => {
	const classes = useStyles();
	return (
		<div className={props.className}>
			<Text className={classes.showName} Text={props.Episode.ShowName} />
			<Text className={classes.episodeIdentity} Text={`${props.Episode.Season}x${props.Episode.EpisodeInSeason}`} />
			<Text className={classes.episodeTitle} Text={props.Episode.Title} />
		</div>
	);
}

const useStyles = createUseStyles({
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
		color: "#F0F0F0",
	},
});

export default EpisodeName;