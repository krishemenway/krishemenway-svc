import * as React from "react";
import * as reactDom from "react-dom";
import { createUseStyles } from "react-jss";
import Text from "Common/Text";
import {default as AppBackground} from "Common/AppBackground.png";
import { ArtGroup } from "Arts/ArtGroup";
import { belowWidth, useBackground, useEvents, useLayout, useMargin, usePadding, useText, useTextColor } from "Common/AppStyles";
import { ArtService } from "Arts/ArtService";
import Modal from "Common/Modal";
import { useObservable } from "Common/UseObservable";
import CloseIcon from "Common/CloseIcon";
import DownloadIcon from "Common/DownloadIcon";
import { FileDownloader } from "Common/FileDownloader";

const App : React.FC<{}> = () => {
	const [classes, layout, margin, text, textColors, background, events, padding] = [useStyles(), useLayout(), useMargin(), useText(), useTextColor(), useBackground(), useEvents(), usePadding()];
	const allArtByCategory = ArtService.Instance.AllArtByCategory;
	const selectedArtMetadata = useObservable(ArtService.Instance.SelectedArtMetadata);
	const onClosed = () => { ArtService.Instance.SelectedArtMetadata.Value = null; };

	return (
		<div className={classes.app}>
			<div className={`${classes.pageWrapper} ${layout.blockCenter}`}>
				<Text Text="Art" className={`${text.font28} ${text.toUpper} ${text.bold} ${margin.top} ${margin.bottomDouble} ${text.center} ${layout.block} ${textColors.grayc8}`} />

				{Object.keys(allArtByCategory).map((category) => <ArtGroup title={category} arts={allArtByCategory[category]} />)}

				<Modal Open={selectedArtMetadata !== null} className={classes.fullscreenArtModal} onClosed={onClosed}>
					<div className={`${text.right} ${margin.vertical}`}>
						<button className={`${events.clickable} ${padding.all} ${margin.right} ${textColors.white} ${text.font24}`} onClick={() => FileDownloader.DownloadFile(selectedArtMetadata?.FullPath ?? "", `${selectedArtMetadata?.Title}${selectedArtMetadata?.FullPath.slice(selectedArtMetadata?.FullPath.lastIndexOf("."))}`)} aria-label="download art">
							<DownloadIcon />
						</button>

						<button className={`${events.clickable} ${padding.all} ${margin.right} ${textColors.white} ${text.font24}`} onClick={onClosed} aria-label="close modal">
							<CloseIcon />
						</button>
					</div>
					<div className={classes.fullscreenArt} style={{ backgroundImage: `url('${selectedArtMetadata?.FullPath ?? ""}')` }}></div>
				</Modal>
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
	fullscreenArtModal: {
		width: "calc(100% - 100px)",
		height: "calc(100% - 100px)",
		backgroundColor: "#202020",
		[belowWidth(800)]: {
			width: "calc(100% - 20px)",
			height: "calc(100% - 60px)",
		},
	},
	fullscreenArt: {
		width: "90%",
		height: "90%",
		backgroundSize: "contain",
		backgroundRepeat: "no-repeat",
		backgroundPosition: "center center",
		margin: "0 auto",
		[belowWidth(800)]: {
			backgroundPosition: "top center",
		}
	},
});

(window as any).initialize = () => {
	reactDom.render(<App />, document.getElementById('app'));
	document.getElementsByTagName("body")[0].style.background = `url('${AppBackground}') #010101`;
};
