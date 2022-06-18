export class SlotWinningPattern {
	constructor(earnsPoints: number, requiredSymbols: Dictionary<number>) {
		this.EarnsPoints = earnsPoints;
		this.RequiredSymbols = requiredSymbols;
	}

	public EarnsPoints: number;
	public RequiredSymbols: Dictionary<number>;
}
