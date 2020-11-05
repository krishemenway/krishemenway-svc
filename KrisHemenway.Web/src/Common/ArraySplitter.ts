export function SplitArrayIntoChunks<T>(fullSetOfItems: T[], maxSizeOfChunk: number): T[][] {
	if (fullSetOfItems.length < 5) {
		return [ fullSetOfItems ];
	}

	const chunksOfItems: T[][] = [];

	for(let i = 0; i < fullSetOfItems.length; i = i + 4) {
		const arrayToAdd = fullSetOfItems.slice(i, i + 4);

		if (arrayToAdd.length > 0) {
			chunksOfItems.push(arrayToAdd);
		}
	}

	return chunksOfItems;
}
