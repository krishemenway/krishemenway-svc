import * as React from "react";
import * as reactDom from "react-dom";
import { createUseStyles } from "react-jss";
import ProjectLink from "Home/ProjectLink";
import Text from "Common/Text";
import {default as AppBackground} from "Common/AppBackground.png";

const App : React.FC<{}> = () => {
	const classes = useStyles();
	return (
		<div className={classes.homeApp}>
			<div className={classes.projects}>
				<Text Text="Projects" className={classes.projectsTitle} />

				<ProjectLink
					Title="Artwork"
					Description="some art I have made"
					Location="/Arts.html"
				/>

				<ProjectLink
					Title="Sloshy Dosh Man Killing Floor 2 Stats"
					Description="tracking the stats of local killing floor 2 server"
					Location="https://www.sloshydoshman.com"
				/>

				<ProjectLink
					Title="Slot Machine"
					Description="press the button. why? I dunno. could be cool?"
					Location="/Slots.html"
				/>

				<ProjectLink
					Title="Typing Center for America"
					Description="something that continuously types on screen for you"
					Location="https://im-busy.krishemenway.com"
				/>

				<ProjectLink
					Title="Supermarket Ninja (mobile)"
					Description="swipin food away at the cash register!"
					Location="/supermarketninja"
				/>

				<ProjectLink
					Title="Game Profile"
					Description="project for tracking which games you are playing with interesting statistics"
					Location="https://profile.krishemenway.com"
				/>
			</div>
		</div>
	);
}

const useStyles = createUseStyles({
	homeApp: {
		position: "absolute",
		top: 0,
		right: 0,
		bottom: 0,
		left: 0,

		"& *": {
			boxSizing: "border-box",
		},
	},
	projects: {
		maxWidth: "1024px",
		position: "relative",
		margin: "0 auto",

	},
	projectsTitle: {
		display: "block",
		fontSize: "28px",
		color: "#C8C8C8",
		textAlign: "center",
		textTransform: "uppercase",
		fontWeight: "bold",
		margin: "20px 0",
	},
});

(window as any).initialize = () => {
	reactDom.render(<App />, document.getElementById('app'));
	document.getElementsByTagName("body")[0].style.background = `url('${AppBackground}') #010101`;
}
