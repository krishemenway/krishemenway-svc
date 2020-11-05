import * as React from "react";
import Text from "Common/Text";
import ArtPreview from "Arts/ArtPreview";
import { ArtMetadata } from "Arts/ArtService";
import { useLayout, useMargin, useText, useTextColor } from "Common/AppStyles";
import { SplitArrayIntoChunks } from "Common/ArraySplitter";

export const ArtGroup: React.FC<{ title: string; arts: ArtMetadata[] }> = (props) => {
	const [textColor, margin, layout, text] = [useTextColor(), useMargin(), useLayout(), useText()];

	return (
		<div className={`${margin.bottom}`}>
			<div className={`${margin.bottomHalf}`}><Text Text={props.title} className={`${textColor.white} ${text.font20}`} /></div>
			<div className={`${layout.horzRule} ${margin.bottomHalf}`} />

			{SplitArrayIntoChunks(props.arts, 4).map((artChunk) => (
				<div className={`${margin.bottomDouble} ${layout.flexRow}`}>
					{artChunk.map((art) => <ArtPreview art={art} className={`${layout.flexEvenDistribution}`} />)}
				</div>
			))}
		</div>
	);
};
