import * as React from "react";
import { createUseStyles } from "react-jss";
import Text from "Common/Text";
import { belowWidth } from "Common/AppStyles";

interface ProjectLinkProps {
	Title: string;
	Location: string;
	Description: string;
}

const ProjectLink: React.FC<ProjectLinkProps> = (props) => {
	const classes = useStyles();
	return (
		<a className={classes.projectItem} href={props.Location} title={props.Title}>
			<Text Text={props.Title} className={classes.projectTitle} />
			<Text Text={props.Description} className={classes.projectDescription} />
		</a>
	);
}

const useStyles = createUseStyles({
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

		[belowWidth(768)]: {
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

export default ProjectLink;
