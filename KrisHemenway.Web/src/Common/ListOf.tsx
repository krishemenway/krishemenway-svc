import * as React from "react";

interface ListPropsOf<TItem> {
	items: TItem[];
	renderItem: (item: TItem) => JSX.Element;

	key: string;
	style?: React.CSSProperties;
	className?: string;
}

export default function ListOf<TItem>(props: ListPropsOf<TItem>): JSX.Element {
	if (!props.items || props.items.length === 0) {
		return <></>;
	}

	return (
		<div key={props.key} className={props.className} style={props.style}>
			{props.items.map((item) => props.renderItem(item))}
		</div>
	);
}
