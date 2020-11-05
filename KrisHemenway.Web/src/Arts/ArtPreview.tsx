import * as React from "react";
import Text from "Common/Text";
import { useEvents, useMargin, useText, useTextColor } from "Common/AppStyles";
import { ArtService, ArtMetadata } from "Arts/ArtService";
import { createUseStyles } from "react-jss";

const ArtPreview: React.FC<{ art: ArtMetadata; className?: string }> = (props) => {
	const [classes, textColor, events, text, margin] = [useStyles(), useTextColor(), useEvents(), useText(), useMargin()];
	return (
		<button className={`${events.clickable} ${props.className ?? ""}`} onClick={() => { ArtService.Instance.SelectedArtMetadata.Value = props.art; }}>
			<div className={`${classes.artPreview} ${margin.bottom}`} style={{ backgroundImage: `url('${props.art.ThumbnailPath}')`}} />
			<div className={`${text.center} ${margin.bottom}`}><Text Text={props.art.Title} className={`${textColor.white}`} /></div>
		</button>
	);
};

const useStyles = createUseStyles({
	artPreview: {
		backgroundRepeat: "no-repeat",
		backgroundPosition: "center",
		backgroundSize: "auto 100%",
		height: "150px",
	}
});

export default ArtPreview;
