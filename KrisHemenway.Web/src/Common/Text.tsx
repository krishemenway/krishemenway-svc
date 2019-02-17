import * as React from "react";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

interface TextProps extends WithStyles<typeof styles> {
	Text: string;
	style?: React.CSSProperties;
	className?: string;
}

class Text extends React.Component<TextProps, {}> {
	public render() {
		return <span className={`${this.props.classes.container} ${this.props.className}`} style={this.props.style}>{this.props.Text}</span>;
	}
}

const styles = (_: Theme) => createStyles({
	container: {
		fontFamily: "'Segoe UI', Frutiger, 'Frutiger Linotype', 'Dejavu Sans', 'Helvetica Neue', Arial, sans-serif",
		display: "inline-block",
		color: "inherit",
		background: "inherit",
		fontWeight: "inherit",
		lineHeight: "1",
	},
});

export default withStyles(styles)(Text);