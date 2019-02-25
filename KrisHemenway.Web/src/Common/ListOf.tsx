import * as React from "react";
import { withStyles, createStyles, Theme, WithStyles } from "@material-ui/core/styles";

interface ListPropsOf<TItem> extends WithStyles<typeof styles> {
	items: TItem[];
	renderItem: (item: TItem) => JSX.Element;

	key: string;
	style?: React.CSSProperties;
	className?: string;
}

class ListOf<TItem> extends React.Component<ListPropsOf<TItem>, {}> {
	public render() {
		return <div key={this.props.key} className={this.props.className} style={this.props.style}>{this.renderItems()}</div>;
	}
	
	private renderItems() {
		if (!this.props.items || this.props.items.length === 0) {
			return "";
		}

		return this.props.items.map((item) => this.props.renderItem(item));
	}
}

const styles = (_: Theme) => createStyles({ });
export default function<TItem>() {
	return withStyles(styles)((props: ListPropsOf<TItem>) => (<ListOf<TItem> {...props} />));
}
