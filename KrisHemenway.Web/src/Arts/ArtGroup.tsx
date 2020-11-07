import * as React from "react";
import Text from "Common/Text";
import ArtPreview from "Arts/ArtPreview";
import { ArtMetadata } from "Arts/ArtService";
import { useLayout, useMargin, usePadding, useText, useTextColor } from "Common/AppStyles";
import { SplitArrayIntoChunks } from "Common/ArraySplitter";
import { useMediaQueries } from "Common/UseMediaQuery";

export const ArtGroup: React.FC<{ title: string; arts: ArtMetadata[] }> = (props) => {
	const [textColor, margin, layout, text, padding] = [useTextColor(), useMargin(), useLayout(), useText(), usePadding()];
	const columnCount = useMediaQueries<number>(['(max-width: 800px)'], [2], 4);
	console.log(columnCount);

	return (
		<div className={`${margin.bottom}`}>
			<div className={`${margin.bottomHalf}`}><Text Text={props.title} className={`${textColor.white} ${text.font20}`} /></div>
			<div className={`${layout.horzRule} ${margin.bottomHalf}`} />

			{SplitArrayIntoChunks(props.arts, columnCount).map((artChunk) => (
				<div className={`${margin.bottomDouble} ${layout.flexRow}`}>
					{artChunk.map((art) => <ArtPreview art={art} className={`${layout.flexEvenDistribution} ${padding.horizontal}`} />)}
				</div>
			))}
		</div>
	);
};
