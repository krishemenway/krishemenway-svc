import { Observable } from "@residualeffect/reactor";
import { SlotReel } from "./SlotReel";
import { SlotWinningPattern } from "./SlotWinningPattern";

export class SlotMachine {
	constructor() {
		this.CostPerSpin = -5;
		this.Reels = Array.from({ length: 3 } , () => new SlotReel());
		this.AllSymbols = ["LuckySeven", "Banana", "Lemon", "Cherry"];
		this.WinningPatterns = [
			new SlotWinningPattern(100, { "LuckySeven": 3 }),
			new SlotWinningPattern(80, { "Banana": 1, "Cherry": 1, "Lemon": 1 }),
			new SlotWinningPattern(50, { "Cherry": 3 }),
			new SlotWinningPattern(10, { "Banana": 2 }),
			new SlotWinningPattern(5, { "Lemon": 2 }),
		];

		this.SpinWinFlag = new Observable(null);
		this.Score = new Observable(this.LoadScoreFromStorage());
		this.PreLockSpin = new Observable(true);
		this.MostRecentWonPattern = new Observable(null);
	}

	public SetupPreLockSpin(): void {
		this.PreLockSpin.Value = true;

		this.Reels.forEach((r) => {
			r.IsLocked.Value = false;
		});
	}

	public PickRandomSymbol(): string {
		return this.AllSymbols[Math.floor(Math.random() * this.AllSymbols.length)];
	}

	public Spin(): void {
		const preLockSpin = this.PreLockSpin.Value;

		if (preLockSpin) {
			this.UpdateScore(this.CostPerSpin);
			this.SpinWinFlag.Value = null;
			this.MostRecentWonPattern.Value = null;
		}
	
		const reelsToSpin = this.Reels.filter((reel) => !reel.IsLocked.Value);

		const allReelPromises = reelsToSpin.map((reel, index) => {
			const promise = new Promise<string>((resolveSymbol) => {
				window.setTimeout(() => { resolveSymbol(this.PickRandomSymbol()); }, index * 1000 + 2000);
			});

			reel.CurrentSymbol.Start(() => promise);
			return promise;
		});
	
		Promise.all(allReelPromises).then(() => {
			const emptySymbols = this.AllSymbols.reduce((all, symbol) => { all[symbol] = 0; return all; }, {} as Dictionary<number>);
			const currentSymbols = this.Reels.reduce((all, current) => { all[current.CurrentSymbol.Data.Value.ReceivedData ?? ""]++; return all; }, emptySymbols);

			const matchingPattern = this.WinningPatterns.find((pattern) => {
				return Object.keys(pattern.RequiredSymbols).every((key) => {
					const reelsRequiredForKey = pattern.RequiredSymbols[key];
					return currentSymbols[key] >= reelsRequiredForKey;
				});
			});
	
			if (matchingPattern !== undefined) {
				this.SetupPreLockSpin();
				this.UpdateScore(matchingPattern.EarnsPoints);
				this.MostRecentWonPattern.Value = matchingPattern;
				this.SpinWinFlag.Value = true;
			} else if (!this.PreLockSpin.Value) {
				this.SetupPreLockSpin();
				this.SpinWinFlag.Value = false;
			} else {
				this.PreLockSpin.Value = false;
			}
		});
	}

	public UpdateScore(pointDifference: number): void {
		this.Score.Value = this.Score.Value + pointDifference;
		window.localStorage.setItem("SlotScore", this.Score.Value.toString());
	}

	private LoadScoreFromStorage(): number {
		return parseInt(window.localStorage.getItem("SlotScore") ?? SlotMachine.DefaultScore.toString(), 10);
	}

	public CostPerSpin: number;
	public Reels: SlotReel[];

	public SpinWinFlag: Observable<boolean|null>;
	public Score: Observable<number>;
	public PreLockSpin: Observable<boolean>;
	public MostRecentWonPattern: Observable<SlotWinningPattern|null>;

	public AllSymbols: string[];
	public WinningPatterns: SlotWinningPattern[];

	public static DefaultScore = 50;
}
