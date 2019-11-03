import * as React from "react";
import * as reactDom from "react-dom";
import ProjectLink from "./ProjectLink";
import Text from "../Common/Text";
import * as AppBackground from "../Common/AppBackground.png";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

interface AppProps extends WithStyles<typeof styles> { }

class App extends React.Component<AppProps, {}> {
	public render() {
		return (
			<div className={this.props.classes.homeApp}>
				<div className={this.props.classes.projects}>
					<Text Text="Projects" className={this.props.classes.projectsTitle} />

					<ProjectLink
						Title="TV Show Release Calendar"
						Description="complete calendar listing of all the TV Shows being tracked"
						Location="/Calendar.html"
					/>

					<ProjectLink
						Title="Sloshy Dosh Man Killing Floor 2 Stats"
						Description="tracking the stats of local killing floor 2 server"
						Location="https://www.sloshydoshman.com"
					/>

					<ProjectLink
						Title="Automated Typing Center for America"
						Description="what is happening right now."
						Location="/im-busy"
					/>

					<ProjectLink
						Title="Supermarket Ninja (mobile)"
						Description="swipin food away at the cash register!"
						Location="/supermarketninja"
					/>

					<ProjectLink
						Title="TV Show Completion"
						Description="Completion for TV Shows"
						Location="/MissingEpisodes.html"
					/>
				</div>
			</div>
		)
	}
}

const styles = (theme: Theme) => createStyles({
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

const AppWithStyles = withStyles(styles)(App);

(window as any).initialize = () => {
	reactDom.render(<AppWithStyles />, document.getElementById('app'));
	document.getElementsByTagName("body")[0].style.background = `url('${AppBackground}') #010101`;
}
