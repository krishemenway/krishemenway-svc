import * as React from "react";
import { createUseStyles } from "react-jss";

interface TextProps {
	Text: string;
	style?: React.CSSProperties;
	className?: string;
}

const Text: React.FC<TextProps> = (props) => {
	const classes = useStyles();
	return (
		<span className={`${classes.container} ${props.className}`} style={props.style}>
			{props.Text}
		</span>
	);
}

const useStyles = createUseStyles({
	container: {
		fontFamily: "'Segoe UI', Frutiger, 'Frutiger Linotype', 'Dejavu Sans', 'Helvetica Neue', Arial, sans-serif",
		display: "inline-block",
		color: "inherit",
		background: "inherit",
		fontWeight: "inherit",
		lineHeight: "1",
	},
});

export default Text;