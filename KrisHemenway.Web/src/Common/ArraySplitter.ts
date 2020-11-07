export function SplitArrayIntoChunks<T>(fullSetOfItems: T[], maxSizeOfChunk: number): T[][] {
	if (fullSetOfItems.length <= maxSizeOfChunk) {
		return [ fullSetOfItems ];
	}

	const chunksOfItems: T[][] = [];

	for(let i = 0; i < fullSetOfItems.length; i = i + maxSizeOfChunk) {
		const arrayToAdd = fullSetOfItems.slice(i, i + maxSizeOfChunk);

		if (arrayToAdd.length > 0) {
			chunksOfItems.push(arrayToAdd);
		}
	}

	return chunksOfItems;
}
