import * as React from "react";
import Text from "../Common/Text";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

interface ProjectLinkProps extends WithStyles<typeof styles> {
	Title: string;
	Location: string;
	Description: string;
}

class ProjectLink extends React.Component<ProjectLinkProps, {}> {
	public render() {
		return (
			<a className={this.props.classes.projectItem} href={this.props.Location} title={this.props.Title}>
				<Text Text={this.props.Title} className={this.props.classes.projectTitle} />
				<Text Text={this.props.Description} className={this.props.classes.projectDescription} />
			</a>
		)
	}
}

const styles = (theme: Theme) => createStyles({
	projectItem: {
		padding: "10px",
		width: "400px",
		margin: "0 auto",
		backgroundColor: "#222222",
		display: "block",
		borderTop: "1px solid #242424",
		borderBottom: "1px solid #464646",
		textDecoration: "none",

		"&:hover": {
			backgroundColor: "#343434",
		},

		"&:first-of-type": {
			borderTop: "0px",
			borderTopLeftRadius: "6px",
			borderTopRightRadius: "6px",
		},

		"&:last-of-type": {
			borderBottom: "0px",
			borderBottomLeftRadius: "6px",
			borderBottomRightRadius: "6px",
		},

		[theme.breakpoints.down(768)]: {
			width: "auto",
			margin: "0 20px",
		}
	},
	projectTitle: {
		fontSize: "18px",
		color: "#E8E8E8",
		display: "block",
	},
	projectDescription: {
		fontSize: "12px",
		color: "#E8E8E8",
		textTransform: "lowercase",
		display: "block",

		"&:hover": {
			textDecoration: "none",
		},
	},
});

export default withStyles(styles)(ProjectLink);