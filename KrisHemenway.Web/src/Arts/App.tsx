import * as React from "react";
import * as reactDom from "react-dom";
import { createUseStyles } from "react-jss";
import Text from "Common/Text";
import {default as AppBackground} from "Common/AppBackground.png";
import { ArtGroup } from "Arts/ArtGroup";
import { useLayout, useMargin, useText, useTextColor } from "Common/AppStyles";
import { ArtService } from "Arts/ArtService";

const App : React.FC<{}> = () => {
	const [classes, layout, margin, text, textColors] = [useStyles(), useLayout(), useMargin(), useText(), useTextColor()];
	const allArtByCategory = ArtService.Instance.AllArtByCategory;

	return (
		<div className={classes.app}>
			<div className={`${classes.pageWrapper} ${layout.blockCenter}`}>
				<Text Text="Art" className={`${text.font28} ${text.toUpper} ${text.bold} ${margin.top} ${margin.bottomDouble} ${text.center} ${layout.block} ${textColors.grayc8}`} />

				{Object.keys(allArtByCategory).map((category) => <ArtGroup title={category} arts={allArtByCategory[category]} />)}
			</div>
		</div>
	);
};

const useStyles = createUseStyles({
	app: {
		position: "absolute",
		top: 0,
		right: 0,
		bottom: 0,
		left: 0,

		"& *": {
			boxSizing: "border-box",
		},
	},
	pageWrapper: {
		maxWidth: "1024px",
		position: "relative",
	},
});

(window as any).initialize = () => {
	reactDom.render(<App />, document.getElementById('app'));
	document.getElementsByTagName("body")[0].style.background = `url('${AppBackground}') #010101`;
};
