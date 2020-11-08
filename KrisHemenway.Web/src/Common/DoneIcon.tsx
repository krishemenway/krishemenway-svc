import * as React from "react";

const DoneIcon: React.FC<{ color?: string; className?: string }> = (props) => {
	return (
		<svg className={props.className} style={{ fill: props.color ?? "currentColor", height: "1em", width: "1em" }} xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
			<path d="M9 16.2L4.8 12l-1.4 1.4L9 19 21 7l-1.4-1.4L9 16.2z"></path>
		</svg>
	);
}

export default DoneIcon;


